using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    /// <summary>
    /// A generic class that pairs given objects of type <typeparamref name="T"/>
    /// and for each pair calculates the distance between the objects that formed it.
    /// </summary>
    /// <typeparam name="T">The type of objects for which the distance matrix should be calculated.</typeparam>
    /// <remarks><see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> implements built-in calculation for the <see cref="Hero"/>,
    /// <see cref="Settlement"/>, <see cref="Clan"/> and <see cref="Kingdom"/> objects.
    /// For any other <see cref="MBObjectBase"/> subtypes custom EntityListGetter and DistanceCalculator methods
    /// should be provided using special constructor <see cref="DistanceMatrixImplementation{T}"/> .
    /// </remarks>
    [Serializable]
    internal sealed class DistanceMatrixImplementation<T> : DistanceMatrix<T>, ISerializable where T : MBObjectBase
    {
        //Fields
        private readonly Dictionary<ulong, float> _distanceMatrix;
        [NonSerialized]
        private readonly Dictionary<(T Object1, T Object2), float> _typedDistanceMatrix;
        [NonSerialized]
        private readonly Func<IEnumerable<T>>? _entityListGetter;
        [NonSerialized]
        private readonly Func<T, T, float>? _distanceCalculator;

        //properties
        /// <summary>Raw distance matrix representation</summary>
        /// <value>
        /// A dictionary of paired type <typeparamref name="T"/> objects
        /// represented by unique 64-bit unsigned number as key
        /// and floating-point numbers, representing distances between those objects as value.
        /// </value>
        public override Dictionary<ulong, float> AsDictionary => _distanceMatrix;

        /// <summary>Objectified distance matrix representation</summary>
        /// <value>
        /// A dictionary of paired type <typeparamref name="T"/> objects as key
        /// and floating-point numbers, representing distances between those objects as value.
        /// </value>
        public override Dictionary<(T Object1, T Object2), float> AsTypedDictionary => _typedDistanceMatrix;

        //Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
        /// with default EntityListGetter and DistanceCalculator methods.
        /// </summary>
        /// <exception cref="T:System.ArgumentException"></exception>
        public DistanceMatrixImplementation()
        {
            _entityListGetter = null;
            _distanceCalculator = null;
            _distanceMatrix = CalculateDistanceMatrix();
            _typedDistanceMatrix = GetTypedDistanceMatrix();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
        /// with custom methods that will be used to get the list of analyzed objects and calculate the distances between them.
        /// </summary>
        /// <param name="customListGetter">
        /// A delegate to the method that will be used to get a list of objects of type <typeparamref name="T"/>
        /// for calculating the distances between them.
        /// </param>
        /// <param name="customDistanceCalculator">
        /// A delegate to the method that will be used to calculate the distance between two given type <typeparamref name="T"/> objects.
        /// </param>
        /// <exception cref="T:System.ArgumentException"></exception>
        public DistanceMatrixImplementation(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator)
        {
            _entityListGetter = customListGetter;
            _distanceCalculator = customDistanceCalculator;
            _distanceMatrix = CalculateDistanceMatrix();
            _typedDistanceMatrix = GetTypedDistanceMatrix();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> class
        /// using serialized data.
        /// </summary>
        /// <remarks>Used exclusively for deserialization.</remarks>
        private DistanceMatrixImplementation(SerializationInfo info, StreamingContext context)
        {
            var type = Type.GetType(info.GetString("ObjectTypeName"))!;
            _distanceMatrix = (Dictionary<ulong, float>)info.GetValue(type.Name + "DistanceMatrix", typeof(Dictionary<ulong, float>));
            _distanceMatrix.OnDeserialization(this);
            _typedDistanceMatrix = GetTypedDistanceMatrix();
        }

        //Public methods
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ObjectTypeName", typeof(T).AssemblyQualifiedName);
            info.AddValue(typeof(T).Name + "DistanceMatrix", _distanceMatrix);
        }

        /// <summary>Gets calculated distance between specified type <typeparamref name="T"/> objects.</summary>
        /// <param name="object1">The first of the objects between which it is necessary to determine the distance.</param>
        /// <param name="object2">The second of the objects between which it is necessary to determine the distance.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="MBObjectBase"/> objects;
        /// or <see cref="float.NaN" />, if distance was not calculated or it is uncomputable.
        /// </returns>
        public override float GetDistance(T object1, T object2) =>
            _distanceMatrix.TryGetValue(object1.Id > object2.Id ? ElegantPairHelper.Pair(object2.Id, object1.Id) : ElegantPairHelper.Pair(object1.Id, object2.Id),
                                        out var distance) ? distance : float.NaN;

        //Private methods
        /// <summary>Calculates distance matrix for the <see cref="MBObjectBase"/> objects of the specified subtype <typeparamref name="T"/>.</summary>
        /// <exception cref="T:System.ArgumentException"></exception>
        private Dictionary<ulong, float> CalculateDistanceMatrix()
        {
            if (_entityListGetter != null && _distanceCalculator != null)
            {
                var entities = _entityListGetter().ToList();
                return entities.SelectMany(_ => entities, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                               .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => _distanceCalculator(value.X, value.Y));
            }
            if (typeof(Hero).IsAssignableFrom(typeof(T)))
            {
                var activeHeroes = Hero.All.Where(h => h.IsInitialized && !h.IsNotSpawned && !h.IsDisabled && !h.IsDead && !h.IsNotable).ToList();
                return activeHeroes.SelectMany(_ => activeHeroes, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                                   .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenHeroes(value.X, value.Y));
            }
            if (typeof(Settlement).IsAssignableFrom(typeof(T)))
            {
                var settlements = Settlement.All.Where(s => s.IsInitialized && (s.IsFortification || s.IsVillage)).ToList();
                return settlements.SelectMany(_ => settlements, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                                  .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => Campaign.Current.Models.MapDistanceModel.GetDistance(value.X, value.Y));
            }
            if (typeof(Clan).IsAssignableFrom(typeof(T)))
            {
                var clans = Clan.All.Where(c => c.IsInitialized && c.Fortifications.Count > 0).ToList();
                var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
                var lst = GetSettlementOwnersPairedList(settlementDistanceMatrix);

                return clans.SelectMany(_ => clans, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                            .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenClans(value.X, value.Y, lst));
            }
            if (typeof(Kingdom).IsAssignableFrom(typeof(T)))
            {
                var kingdoms = Kingdom.All.Where(k => k.IsInitialized && k.Fiefs.Any()).ToList();
                var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
                return kingdoms.SelectMany(_ => kingdoms, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                               .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenKingdoms(value.X, value.Y, settlementDistanceMatrix));
            }
            throw new ArgumentException($"{typeof(string).FullName} is not supported type");
        }

        private Dictionary<(T Object1, T Object2), float> GetTypedDistanceMatrix() =>
            _distanceMatrix.ToDictionary(key => ElegantPairHelper.UnPairMBGUID(key.Key), value => value.Value)
                           .ToDictionary(key => ((T)MBObjectManager.Instance.GetObject(key.Key.A), (T)MBObjectManager.Instance.GetObject(key.Key.B)), value => value.Value);
    }
}