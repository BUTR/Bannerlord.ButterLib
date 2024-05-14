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

/// <summary>
/// A static class, containing accordant members of the
/// <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> class.
/// </summary>
internal sealed class DistanceMatrixStaticImplementation : IDistanceMatrixStatic
{
    private record DistanceMatrixResultUnpaired(MBGUID OwnerId1, MBGUID OwnerId2, float Distance, float Weight);

    /// <inheritdoc/>
    public DistanceMatrix<T> Create<T>() where T : MBObjectBase => new DistanceMatrixImplementation<T>();

    /// <inheritdoc/>
    public DistanceMatrix<T> Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) where T : MBObjectBase =>
        new DistanceMatrixImplementation<T>(customListGetter, customDistanceCalculator);

    /// <inheritdoc/>
    public float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2)
    {
        var (mobileParty1, settlement1) = GetMapPosition(hero1);
        var (mobileParty2, settlement2) = GetMapPosition(hero2);

        return settlement1 is not null && settlement2 is not null
            ? Campaign.Current.Models.MapDistanceModel.GetDistance(settlement1, settlement2)
            : mobileParty1 is not null && mobileParty2 is not null
                ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty1, mobileParty2)
                : settlement1 is not null && mobileParty2 is not null
                    ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty2, settlement1)
                    : mobileParty1 is not null && settlement2 is not null
                        ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty1, settlement2)
                        : float.NaN;
    }

    /// <inheritdoc/>
    public float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, Dictionary<ulong, WeightedDistance> settlementOwnersPairedList)
    {
        var pair = clan1.Id > clan2.Id ? ElegantPairHelper.Pair(clan2.Id, clan1.Id) : ElegantPairHelper.Pair(clan1.Id, clan2.Id);
        if (settlementOwnersPairedList.TryGetValue(pair, out var weightedDistance))
        {
            return (1 + weightedDistance.Distance) / (1 + weightedDistance.Weight);
        }
        return float.NaN;
    }

    /// <inheritdoc/>
    public float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Clan> clanDistanceMatrix)
    {
        bool Predicate(KeyValuePair<(Clan object1, Clan object2), float> x) =>
            !float.IsNaN(x.Value) && ((x.Key.object1.Kingdom == kingdom1 && x.Key.object2.Kingdom == kingdom2) ||
                                      (x.Key.object2.Kingdom == kingdom1 && x.Key.object1.Kingdom == kingdom2));

        static WeightedDistance WeightedSettlementSelector(KeyValuePair<(Clan object1, Clan object2), float> x) =>
            new(x.Value, x.Key.object1.Tier + x.Key.object2.Tier);

        var settlementDistances = clanDistanceMatrix.AsTypedDictionary.Where(Predicate).Select(WeightedSettlementSelector).ToList();
        return GetWeightedMeanDistance(settlementDistances);
    }

    /// <inheritdoc/>
    public Dictionary<ulong, WeightedDistance> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
    {
        static DistanceMatrixResultUnpaired FirstSelector(KeyValuePair<(Settlement Object1, Settlement Object2), float> kvp) =>
            new(OwnerId1: kvp.Key.Object1.OwnerClan.Id, OwnerId2: kvp.Key.Object2.OwnerClan.Id, Distance: kvp.Value, Weight: GetSettlementWeight(kvp.Key.Object1) + GetSettlementWeight(kvp.Key.Object2));

        static DistanceMatrixResult SecondSelector(DistanceMatrixResultUnpaired x) =>
            new(x.OwnerId1 > x.OwnerId2 ? ElegantPairHelper.Pair(x.OwnerId2, x.OwnerId1) : ElegantPairHelper.Pair(x.OwnerId1, x.OwnerId2), x.Distance, x.Weight);

        return settlementDistanceMatrix.AsTypedDictionary.Select(FirstSelector).Select(SecondSelector).GroupBy(g => g.Owners).Select(g => new DistanceMatrixResult(g.Key, g.Sum(x => x.Distance * x.Weight), g.Sum(x => x.Weight))).ToDictionary(key => key.Owners, value => new WeightedDistance(value.Distance, value.Weight));
    }

    private static (MobileParty? mobileParty, Settlement? settlement) GetMapPosition(Hero hero) =>
        hero.IsPrisoner && hero.PartyBelongedToAsPrisoner is not null
            ? hero.PartyBelongedToAsPrisoner.IsSettlement
                ? (null, hero.PartyBelongedToAsPrisoner.Settlement)
                : (hero.PartyBelongedToAsPrisoner.MobileParty, null)
            : hero.CurrentSettlement is not null && !hero.IsFugitive
                ? (null, hero.CurrentSettlement)
                : hero.PartyBelongedTo is not null
                    ? (hero.PartyBelongedTo, null)
                    : (null, null);

    private static float GetWeightedMeanDistance(IReadOnlyCollection<WeightedDistance> settlementDistances) =>
        settlementDistances.Any(x => x.Weight > 0)
            ? (float) ((settlementDistances.Sum(x => x.Distance * x.Weight) + 1.0) / (settlementDistances.Sum(x => x.Weight) + 1.0))
            : float.NaN;

    private static float GetSettlementWeight(Settlement settlement) => settlement.IsTown ? 2f : settlement.IsCastle ? 1f : settlement.IsVillage ? 0.5f : 0f;
}