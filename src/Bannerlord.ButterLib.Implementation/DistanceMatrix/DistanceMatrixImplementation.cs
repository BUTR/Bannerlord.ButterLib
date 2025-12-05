using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix;

/// <inheritdoc/>
internal sealed class DistanceMatrixImplementation<T> : DistanceMatrix<T> where T : MBObjectBase
{
    //Fields
    private readonly Dictionary<ulong, float> _distanceMatrix;
    private readonly Dictionary<(T Object1, T Object2), float> _typedDistanceMatrix;
    private readonly Dictionary<T, SortedSet<(T OtherObject, float Distance)>> _flatenedDictionary;

    private readonly Func<IEnumerable<T>>? _entityListGetter;
    private readonly Func<T, T, object[]?, float>? _distanceCalculator;
    private readonly object[]? _distanceCalculatorArgs;
    private Dictionary<MBGUID, MBObjectBase> _cachedMapping = new();

    //properties
    /// <inheritdoc/>
    public override Dictionary<ulong, float> AsDictionary => _distanceMatrix;
    /// <inheritdoc/>
    public override Dictionary<(T Object1, T Object2), float> AsTypedDictionary => _typedDistanceMatrix;
    /// <inheritdoc/>
    public override Dictionary<T, SortedSet<(T OtherObject, float Distance)>> AsFlatenedDictionary => _flatenedDictionary;

    //Constructors
    /// <inheritdoc/>
    public DistanceMatrixImplementation()
    {
        _entityListGetter = null;
        _distanceCalculator = null;
        _distanceCalculatorArgs = null;

        _distanceMatrix = CalculateDistanceMatrix();
        _typedDistanceMatrix = GetTypedDistanceMatrix();
        _flatenedDictionary = GetFlatenedDictionary();
    }

    /// <inheritdoc/>
    public DistanceMatrixImplementation(Func<IEnumerable<T>> customListGetter, Func<T, T, object[]?, float> customDistanceCalculator, object[]? distanceCalculatorArgs = null)
    {
        _entityListGetter = customListGetter;
        _distanceCalculator = customDistanceCalculator;
        _distanceCalculatorArgs = distanceCalculatorArgs;

        _distanceMatrix = CalculateDistanceMatrix();
        _typedDistanceMatrix = GetTypedDistanceMatrix();
        _flatenedDictionary = GetFlatenedDictionary();
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

        _flatenedDictionary[object1].RemoveWhere(x => x.OtherObject == object2);
        _flatenedDictionary[object1].Add((object2, distance));
        _flatenedDictionary[object2].RemoveWhere(x => x.OtherObject == object1);
        _flatenedDictionary[object2].Add((object1, distance));
    }

    /// <inheritdoc/>
    public override IEnumerable<(T OtherObject, float Distance)> GetNearestNeighbours(T inquiredObject, int count) => GetNearestNeighbours(inquiredObject, count, IsNotNaN());

    /// <inheritdoc/>
    public override IEnumerable<(T OtherObject, float Distance)> GetNearestNeighbours(T inquiredObject, int count, Func<(T OtherObject, float Distance), bool> searchPredicate) =>
        _flatenedDictionary.TryGetValue(inquiredObject, out var nearestNeighbors) ? nearestNeighbors.Where(searchPredicate).Take(count) : [];

    /// <inheritdoc/>
    public override IEnumerable<(T OtherObject, float Distance)> GetNearestNeighboursNormalized(T inquiredObject, int count, float scaleMin = 0f, float scaleMax = 100f) =>
        GetNearestNeighboursNormalized(inquiredObject, count, IsNotNaN(), scaleMin, scaleMax);

    /// <inheritdoc/>
    public override IEnumerable<(T OtherObject, float Distance)> GetNearestNeighboursNormalized(T inquiredObject, int count, Func<(T OtherObject, float Distance), bool> searchPredicate, float scaleMin = 0f, float scaleMax = 100f)
    {
        if (_flatenedDictionary.TryGetValue(inquiredObject, out var nearestNeighbors))
        {
            var sourceList = nearestNeighbors.Where(searchPredicate).ToList();
            GetRanges(scaleMin, scaleMax, sourceList, out var value, out var scale);
            return sourceList.Select(i => (i.OtherObject, Distance: float.IsNaN(i.Distance) ? scale.Min : value.Range == 0f ? scale.Max : (scale.Range * (i.Distance - value.Min) / value.Range) + scale.Min)).Take(count);
        }
        return [];
    }

    //Private methods

    private static Func<(T OtherObject, float Distance), bool> IsNotNaN() => x => !float.IsNaN(x.Distance);

    private static void GetRanges(float scaleMin, float scaleMax, ICollection<(T OtherObject, float Distance)> nearestNeighbors, out (float Min, float Max, float Range) value, out (float Min, float Max, float Range) scale)
    {
        var numericList = nearestNeighbors.Where(IsNotNaN()).Select(x => x.Distance).ToList();

        value = numericList.Count > 0
            ? numericList.GroupBy(g => 1).Select(g =>
            {
                float minValue = g.Min(x => x);
                float maxValue = g.Max(x => x);
                return (Min: minValue, Max: maxValue, Range: maxValue - minValue);
            }).FirstOrDefault()
            : (Min: float.NaN, Max: float.NaN, Range: float.NaN);
        scale = (Min: scaleMin, Max: scaleMax, Range: scaleMax - scaleMin);
    }

    private T GetObject(MBGUID id) => _cachedMapping.TryGetValue(id, out var obj) && obj is T objT
        ? objT
        : throw new ArgumentException($"Id '{id}' was not found!", nameof(id));

    /// <summary>Calculates distance matrix for the <see cref="MBObjectBase"/> objects of the specified subtype <typeparamref name="T"/>.</summary>
    /// <exception cref="T:System.ArgumentException"></exception>
    private Dictionary<ulong, float> CalculateDistanceMatrix()
    {
        if (_entityListGetter is not null && _distanceCalculator is not null)
        {
            var entities = _entityListGetter().ToList();
            _cachedMapping = entities.ToDictionary(key => key.Id, value => value as MBObjectBase);

            return entities
                .SelectMany(_ => entities, (X, Y) => (X, Y))
                .Where(tuple => tuple.X.Id < tuple.Y.Id)
                .ToDictionary(
                    key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                    value => _distanceCalculator(value.X, value.Y, _distanceCalculatorArgs));
        }

        if (typeof(Hero).IsAssignableFrom(typeof(T)))
        {
            var activeHeroes = Hero.AllAliveHeroes.Where(h => !h.IsNotSpawned && !h.IsDisabled && !h.IsDead && !h.IsChild && !h.IsNotable).ToList();
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
            bool considerVillages = DistanceMatrixSubSystem.Instance?.ConsiderVillages ?? true;
            var settlements = Settlement.All.Where(s => s.IsFortification || (considerVillages && s.IsVillage)).ToList();
            _cachedMapping = settlements.ToDictionary(key => key.Id, value => value as MBObjectBase);

#if v134 || v135 || v136 || v137 || v138 || v139
            return settlements
                .SelectMany(_ => settlements, (X, Y) => (X, Y))
                .Where(tuple => tuple.X.Id < tuple.Y.Id)
                .ToDictionary(
                    key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                    value => Campaign.Current.Models.MapDistanceModel.GetDistance(value.X, value.Y, false, false, MobileParty.NavigationType.Default));
#elif v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115 || v116 || v120 || v121 || v122 || v123 || v124 || v125 || v126 || v127 || v128 || v129 || v1210 || v1211 || v1212
            return settlements
                .SelectMany(_ => settlements, (X, Y) => (X, Y))
                .Where(tuple => tuple.X.Id < tuple.Y.Id)
                .ToDictionary(
                    key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                    value => Campaign.Current.Models.MapDistanceModel.GetDistance(value.X, value.Y));
#else
#error DEFINE
#endif
        }

        if (typeof(Clan).IsAssignableFrom(typeof(T)))
        {
            var clans = Clan.All.Where(c => !c.IsEliminated && !c.IsBanditFaction).ToList();
            _cachedMapping = clans.ToDictionary(key => key.Id, value => value as MBObjectBase);

            var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
            var lst = GetSettlementOwnersPairedList(settlementDistanceMatrix);

            return clans
                .SelectMany(_ => clans, (X, Y) => (X, Y))
                .Where(tuple => tuple.X.Id < tuple.Y.Id)
                .ToDictionary(
                    key => ElegantPairHelper.Pair(key.X.Id, key.Y.Id),
                    value => CalculateDistanceBetweenClans(value.X, value.Y, lst!).GetValueOrDefault());
        }

        if (typeof(Kingdom).IsAssignableFrom(typeof(T)))
        {
            var kingdoms = Kingdom.All.Where(k => !k.IsEliminated).ToList();
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

    private Dictionary<T, SortedSet<(T OtherObject, float Distance)>> GetFlatenedDictionary()
    {
        var list = _typedDistanceMatrix.ToList();
        var keyList = list.SelectMany(kvp => new[] { kvp.Key.Object1, kvp.Key.Object2 }).Distinct().ToList();

        var result = new Dictionary<T, SortedSet<(T OtherObject, float Distance)>>();
        keyList.ForEach(key =>
        {
            var valueList = list.Where(kvp => kvp.Key.Object1 == key || kvp.Key.Object2 == key).Select(kvp => (OtherObject: kvp.Key.Object1 == key ? kvp.Key.Object2 : kvp.Key.Object1, Distance: kvp.Value)).Distinct().ToList();
            SortedSet<(T OtherObject, float Distance)> valueSet = new(valueList, new TupleComparer());
            result.Add(key, valueSet);
        });
        return result;
    }

    private class TupleComparer : IComparer<(T OtherObject, float Distance)>
    {
        public int Compare((T OtherObject, float Distance) x, (T OtherObject, float Distance) y)
        {
            int distanceComparison = Comparer<float>.Default.Compare(x.Distance, y.Distance);
            return distanceComparison == 0 ? Comparer<uint>.Default.Compare(x.OtherObject.Id.InternalValue, y.OtherObject.Id.InternalValue) : distanceComparison;
        }
    }
}