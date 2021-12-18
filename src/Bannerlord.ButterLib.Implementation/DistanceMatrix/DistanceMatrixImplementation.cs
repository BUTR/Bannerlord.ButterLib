using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    /// <inheritdoc/>
    internal sealed class DistanceMatrixImplementation<T> : DistanceMatrix<T> where T : MBObjectBase
    {
        //Fields
        private readonly Dictionary<ulong, float> _distanceMatrix;
        private readonly Dictionary<(T Object1, T Object2), float> _typedDistanceMatrix;
        private readonly Func<IEnumerable<T>>? _entityListGetter;
        private readonly Func<T, T, float>? _distanceCalculator;
        private Dictionary<MBGUID, MBObjectBase> _cachedMapping = new();

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

        //Public methods
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

        private T GetObject(MBGUID id) => _cachedMapping.TryGetValue(id, out var obj) && obj is T objT
            ? objT
            : throw new ArgumentException($"Id '{id}' was not found!", nameof(id));

        /// <summary>Calculates distance matrix for the <see cref="MBObjectBase"/> objects of the specified subtype <typeparamref name="T"/>.</summary>
        /// <exception cref="T:System.ArgumentException"></exception>
        private Dictionary<ulong, float> CalculateDistanceMatrix()
        {
            if (Campaign.Current?.GetCampaignBehavior<GeopoliticsBehavior>() is null)
                return new Dictionary<ulong, float>();

            if (_entityListGetter is not null && _distanceCalculator is not null)
            {
                var entities = _entityListGetter().ToList();
                _cachedMapping = entities.ToDictionary(key => key.Id, value => value as MBObjectBase);

                return entities
                    .SelectMany(_ => entities, (X, Y) => (X, Y))
                    .Where(tuple => tuple.X.Id < tuple.Y.Id)
                    .ToDictionary(
                        key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                        value => _distanceCalculator(value.X, value.Y));
            }

            if (typeof(Hero).IsAssignableFrom(typeof(T)))
            {
#if e160 || e161 || e162 || e163 || e164 || e165 || e170
                var activeHeroes = Hero.AllAliveHeroes
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
                    .Where(h => h.IsInitialized && !h.IsNotSpawned && !h.IsDisabled && !h.IsDead && !h.IsNotable).ToList();
                _cachedMapping = activeHeroes.ToDictionary(key => key.Id, value => value as MBObjectBase);

                return activeHeroes
                    .SelectMany(_ => activeHeroes, (X, Y) => (X, Y))
                    .Where(tuple => tuple.X.Id < tuple.Y.Id)
                    .ToDictionary(
                        key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                        value => CalculateDistanceBetweenHeroes(value.X, value.Y).GetValueOrDefault());
            }

            if (typeof(Settlement).IsAssignableFrom(typeof(T)))
            {
                var settlements = Settlement.All.Where(s => s.IsInitialized && (s.IsFortification || s.IsVillage)).ToList();
                _cachedMapping = settlements.ToDictionary(key => key.Id, value => value as MBObjectBase);
                return settlements
                    .SelectMany(_ => settlements, (X, Y) => (X, Y))
                    .Where(tuple => tuple.X.Id < tuple.Y.Id)
                    .ToDictionary(
                        key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                        value => Campaign.Current.Models.MapDistanceModel.GetDistance(value.X, value.Y));
            }

            if (typeof(Clan).IsAssignableFrom(typeof(T)))
            {
                var clans = Clan.All.Where(c => c.IsInitialized && c.Fiefs.Any()).ToList();
                _cachedMapping = clans.ToDictionary(key => key.Id, value => value as MBObjectBase);

                var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
                var lst = GetSettlementOwnersPairedList(settlementDistanceMatrix);

                return clans
                    .SelectMany(_ => clans, (X, Y) => (X, Y))
                    .Where(tuple => tuple.X.Id < tuple.Y.Id)
                    .ToDictionary(
                        key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                        value => CalculateDistanceBetweenClans(value.X, value.Y, lst ?? Enumerable.Empty<(ulong, float, float)>()).GetValueOrDefault());
            }

            if (typeof(Kingdom).IsAssignableFrom(typeof(T)))
            {
                var kingdoms = Kingdom.All.Where(k => k.IsInitialized && k.Fiefs.Any()).ToList();
                _cachedMapping = kingdoms.ToDictionary(key => key.Id, value => value as MBObjectBase);

                var claDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsBehavior>().ClanDistanceMatrix ?? new DistanceMatrixImplementation<Clan>();

                return kingdoms.SelectMany(_ => kingdoms, (X, Y) => (X, Y)).Where(tuple => tuple.X.Id < tuple.Y.Id)
                               .ToDictionary(key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id), value => CalculateDistanceBetweenKingdoms(value.X, value.Y, claDistanceMatrix).GetValueOrDefault());
            }

            throw new ArgumentException($"{typeof(T).FullName} is not a supported type");
        }

        private Dictionary<(T Object1, T Object2), float> GetTypedDistanceMatrix() => _distanceMatrix
            .ToDictionary(key => ElegantPairHelper.UnPairMBGUID(key.Key), value => value.Value)
            .ToDictionary(key => (GetObject(key.Key.A), GetObject(key.Key.B)), value => value.Value);
    }
}