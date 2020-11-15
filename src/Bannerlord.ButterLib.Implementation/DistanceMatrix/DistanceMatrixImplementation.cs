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
    /// <inheritdoc/>
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
        /// <inheritdoc/>
        public override Dictionary<ulong, float> AsDictionary => _distanceMatrix;

        /// <inheritdoc/>
        public override Dictionary<(T Object1, T Object2), float> AsTypedDictionary => _typedDistanceMatrix;

        //Constructors
        /// <inheritdoc/>
        public DistanceMatrixImplementation()
        {
            _entityListGetter = null;
            _distanceCalculator = null;
            _distanceMatrix = CalculateDistanceMatrix();
            _typedDistanceMatrix = GetTypedDistanceMatrix();
        }

        /// <inheritdoc/>
        public DistanceMatrixImplementation(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator)
        {
            _entityListGetter = customListGetter;
            _distanceCalculator = customDistanceCalculator;
            _distanceMatrix = CalculateDistanceMatrix();
            _typedDistanceMatrix = GetTypedDistanceMatrix();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.Implementation.DistanceMatrix.DistanceMatrixImplementation`1" /> class
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

        /// <inheritdoc/>
        public override float GetDistance(T object1, T object2) =>
            _distanceMatrix.TryGetValue(object1.Id > object2.Id ? ElegantPairHelper.Pair(object2.Id, object1.Id) : ElegantPairHelper.Pair(object1.Id, object2.Id),
                                        out var distance) ? distance : float.NaN;

        /// <inheritdoc/>
        public override void SetDistance(T object1, T object2, float distance)
        {
            _distanceMatrix[object1.Id > object2.Id ? ElegantPairHelper.Pair(object2.Id, object1.Id) : ElegantPairHelper.Pair(object1.Id, object2.Id)] = distance;
            _typedDistanceMatrix[object1.Id > object2.Id ? (object2, object1) : (object1, object2)] = distance;
        }

        //Private methods
        /// <summary>Calculates distance matrix for the <see cref="MBObjectBase"/> objects of the specified subtype <typeparamref name="T"/>.</summary>
        /// <exception cref="T:System.ArgumentException"></exception>
        private Dictionary<ulong, float> CalculateDistanceMatrix()
        {
            if (_entityListGetter is not null && _distanceCalculator is not null)
            {
                var entities = _entityListGetter().ToList();
                return entities.SelectMany(_ => entities, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                               .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => _distanceCalculator(value.X, value.Y));
            }
            if (typeof(Hero).IsAssignableFrom(typeof(T)))
            {
                var activeHeroes = Hero.All.Where(h => h.IsInitialized && !h.IsNotSpawned && !h.IsDisabled && !h.IsDead && !h.IsNotable).ToList();
                return activeHeroes.SelectMany(_ => activeHeroes, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                                   .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenHeroes(value.X, value.Y).GetValueOrDefault());
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
                            .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenClans(value.X, value.Y, lst ?? Enumerable.Empty<(ulong, float, float)>()).GetValueOrDefault());
            }
            if (typeof(Kingdom).IsAssignableFrom(typeof(T)))
            {
                var kingdoms = Kingdom.All.Where(k => k.IsInitialized && k.Fiefs.Any()).ToList();
                var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
                return kingdoms.SelectMany(_ => kingdoms, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                               .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenKingdoms(value.X, value.Y, settlementDistanceMatrix).GetValueOrDefault());
            }
            throw new ArgumentException($"{typeof(string).FullName} is not supported type");
        }

        private Dictionary<(T Object1, T Object2), float> GetTypedDistanceMatrix() =>
            _distanceMatrix.ToDictionary(key => ElegantPairHelper.UnPairMBGUID(key.Key), value => value.Value)
                           .ToDictionary(key => ((T)MBObjectManager.Instance.GetObject(key.Key.A), (T)MBObjectManager.Instance.GetObject(key.Key.B)), value => value.Value);
    }
}