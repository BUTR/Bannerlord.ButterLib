using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    /// <summary>
    /// A static class, containing accordant members of the
    /// <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> class.
    /// </summary>
    internal sealed class DistanceMatrixStaticImplementation : IDistanceMatrixStatic
    {
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

            return
              settlement1 is not null && settlement2 is not null
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
        public float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, IEnumerable<(ulong Owners, float Distance, float Weight)> settlementOwnersPairedList)
        {
            var pair = clan1.Id > clan2.Id ? ElegantPairHelper.Pair(clan2.Id, clan1.Id) : ElegantPairHelper.Pair(clan1.Id, clan2.Id);
            var settlementDistances = settlementOwnersPairedList.Where(tuple => tuple.Owners == pair && !float.IsNaN(tuple.Distance))
                                                                                                 .Select(x => (x.Distance, x.Weight)).ToList();
            return GetWeightedMeanDistance(settlementDistances);
        }

        /// <inheritdoc/>
        public float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Clan> clanDistanceMatrix)
        {
            bool Predicate(KeyValuePair<(Clan object1, Clan object2), float> x) =>
              !float.IsNaN(x.Value) && ((x.Key.object1.Kingdom == kingdom1 && x.Key.object2.Kingdom == kingdom2) ||
                                        (x.Key.object2.Kingdom == kingdom1 && x.Key.object1.Kingdom == kingdom2));

            static float ClanDistanceSelector(KeyValuePair<(Clan object1, Clan object2), float> x) => x.Value;

            return clanDistanceMatrix.AsTypedDictionary.Where(Predicate).Select(ClanDistanceSelector).Sum();
        }

        /// <inheritdoc/>
        public List<(ulong Owners, float Distance, float Weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
        {
            static (MBGUID OwnerId1, MBGUID OwnerId2, float Distance, float Weight) FirstSelector(KeyValuePair<(Settlement Object1, Settlement Object2), float> kvp) =>
              (OwnerId1: kvp.Key.Object1.OwnerClan.Id, OwnerId2: kvp.Key.Object2.OwnerClan.Id, Distance: kvp.Value, Weight: GetSettlementWeight(kvp.Key.Object1) + GetSettlementWeight(kvp.Key.Object2));

            static (ulong Owners, float Distance, float Weight) SecondSelector((MBGUID OwnerId1, MBGUID OwnerId2, float Distance, float Weight) tuple) =>
              (tuple.OwnerId1 > tuple.OwnerId2 ? ElegantPairHelper.Pair(tuple.OwnerId2, tuple.OwnerId1) : ElegantPairHelper.Pair(tuple.OwnerId1, tuple.OwnerId2), tuple.Distance, tuple.Weight);

            return settlementDistanceMatrix.AsTypedDictionary.Select(FirstSelector).Select(SecondSelector).ToList();
        }

        private static (MobileParty? mobileParty, Settlement? settlement) GetMapPosition(Hero hero) =>
            hero.IsPrisoner && hero.PartyBelongedToAsPrisoner is not null
                ? hero.PartyBelongedToAsPrisoner.IsSettlement ? ((MobileParty?)null, hero.PartyBelongedToAsPrisoner.Settlement) : (hero.PartyBelongedToAsPrisoner.MobileParty, null)
                : hero.CurrentSettlement is not null && !hero.IsFugitive
                    ? ((MobileParty?)null, hero.CurrentSettlement)
                    : hero.PartyBelongedTo is not null ? (hero.PartyBelongedTo, (Settlement?)null) : (null, null);

        private static float GetWeightedMeanDistance(IReadOnlyCollection<(float Distance, float Weight)> settlementDistances) =>
            settlementDistances.Any(x => x.Weight > 0)
                ? (float)((settlementDistances.Sum(x => x.Distance * x.Weight) + 1.0) / (settlementDistances.Sum(x => x.Weight) + 1.0))
                : float.NaN;

        private static float GetSettlementWeight(Settlement settlement) => settlement.IsTown ? 2f : settlement.IsCastle ? 1f : settlement.IsVillage ? 0.5f : 0f;
    }
}