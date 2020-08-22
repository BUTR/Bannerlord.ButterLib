using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    internal sealed class DistanceMatrixStaticImplementation : DistanceMatrixStatic
    {
        public override DistanceMatrix<T> Create<T>() => new DistanceMatrixImplementation<T>();

        public override DistanceMatrix<T> Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) =>
            new DistanceMatrixImplementation<T>(customListGetter, customDistanceCalculator);

        /// <summary>Calculates distance between two given <see cref="Hero"/> objects.</summary>
        /// <param name="hero1">The first of the heroes to calculate distance between.</param>
        /// <param name="hero2">The second of the heroes to calculate distance between.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Hero"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        public override float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2)
        {
            var (mobileParty1, settlement1) = GetMapPosition(hero1);
            var (mobileParty2, settlement2) = GetMapPosition(hero2);

            return
              settlement1 != null && settlement2 != null
              ? Campaign.Current.Models.MapDistanceModel.GetDistance(settlement1, settlement2)
              : mobileParty1 != null && mobileParty2 != null
              ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty1, mobileParty2)
              : settlement1 != null && mobileParty2 != null
              ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty2, settlement1)
              : mobileParty1 != null && settlement2 != null
              ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty1, settlement2)
              : float.NaN;
        }

        /// <summary>Calculates distance between two given <see cref="Clan"/> objects.</summary>
        /// <param name="clan1">First of the clans to calculate distance between.</param>
        /// <param name="clan2">Second of the clans to calculate distance between.</param>
        /// <param name="settlementOwnersPairedList">
        /// Enumerable of the distances between pairs of settlements and of the weights of the paired settlements,
        /// except that the owner clan pairs are used instead of the settlements themselves to speed up the process.
        /// </param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Clan"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between clans fiefs weighted by the fief type.</remarks>
        public override float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, IEnumerable<(ulong Owners, float Distance, float Weight)> settlementOwnersPairedList)
        {
            var pair = clan1.Id > clan2.Id ? ElegantPairHelper.Pair(clan2.Id, clan1.Id) : ElegantPairHelper.Pair(clan1.Id, clan2.Id);
            var settlementDistances = settlementOwnersPairedList.Where(tuple => tuple.Owners == pair && !float.IsNaN(tuple.Distance))
                                                                                                 .Select(x => (x.Distance, x.Weight)).ToList();
            return GetWeightedMeanDistance(settlementDistances);
        }

        /// <summary>Calculates distance between two given <see cref="Kingdom"/> objects.</summary>
        /// <param name="kingdom1">First of the kingdoms to calculate distance between.</param>
        /// <param name="kingdom2">Second of the kingdoms to calculate distance between.</param>
        /// <param name="settlementDistanceMatrix">Settlement distance matrix .</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Kingdom"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between kingdoms fiefs weighted by the fief type.</remarks>
        public override float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Settlement> settlementDistanceMatrix)
        {
            bool Predicate(KeyValuePair<(Settlement object1, Settlement object2), float> x) =>
              !float.IsNaN(x.Value) && ((x.Key.object1.MapFaction == kingdom1 && x.Key.object2.MapFaction == kingdom2) ||
                                        (x.Key.object2.MapFaction == kingdom1 && x.Key.object1.MapFaction == kingdom2));

            static (float distance, float weight) WeightedSettlementSelector(KeyValuePair<(Settlement object1, Settlement object2), float> x) =>
              (x.Value, GetSettlementWeight(x.Key.object1) + GetSettlementWeight(x.Key.object2));

            var settlementDistances = settlementDistanceMatrix.AsTypedDictionary.Where(Predicate).Select(WeightedSettlementSelector).ToList();
            return GetWeightedMeanDistance(settlementDistances);
        }

        /// <summary>
        /// Transforms given <see cref="Settlement"/> distance matrix into list of the weighted distances
        /// between pairs of settlements, except that the owner clan pairs are used instead of the settlements themselves.
        /// </summary>
        /// <param name="settlementDistanceMatrix">Settlement distance matrix to transform into list.</param>
        /// <returns>
        /// A list of tuples holding information about pair of initial settlements owners, distance between settlements
        /// and combined settlement weight.
        /// </returns>
        /// <remarks>
        /// This method could be used to supply
        /// <see cref="DistanceMatrixImplementation{T}.CalculateDistanceBetweenClans(Clan, Clan, List{(ulong Owners, float Distance, float Weight)})"/>
        /// method with required list argument.
        /// </remarks>
        public override List<(ulong Owners, float Distance, float Weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
        {
            static (MBGUID OwnerId1, MBGUID OwnerId2, float Distance, float Weight) FirstSelector(KeyValuePair<(Settlement Object1, Settlement Object2), float> kvp) =>
              (OwnerId1: kvp.Key.Object1.OwnerClan.Id, OwnerId2: kvp.Key.Object2.OwnerClan.Id, Distance: kvp.Value, Weight: GetSettlementWeight(kvp.Key.Object1) + GetSettlementWeight(kvp.Key.Object2));

            static (ulong Owners, float Distance, float Weight) SecondSelector((MBGUID OwnerId1, MBGUID OwnerId2, float Distance, float Weight) tuple) =>
              (tuple.OwnerId1 > tuple.OwnerId2 ? ElegantPairHelper.Pair(tuple.OwnerId2, tuple.OwnerId1) : ElegantPairHelper.Pair(tuple.OwnerId1, tuple.OwnerId2), tuple.Distance, tuple.Weight);

            return settlementDistanceMatrix.AsTypedDictionary.Select(FirstSelector).Select(SecondSelector).ToList();
        }

        private static (MobileParty? mobileParty, Settlement? settlement) GetMapPosition(Hero hero) =>
            hero.IsPrisoner && hero.PartyBelongedToAsPrisoner != null
                ? hero.PartyBelongedToAsPrisoner.IsSettlement ? ((MobileParty?)null, hero.PartyBelongedToAsPrisoner.Settlement) : (hero.PartyBelongedToAsPrisoner.MobileParty, null)
                : hero.CurrentSettlement != null && !hero.IsFugitive
                    ? ((MobileParty?)null, hero.CurrentSettlement)
                    : hero.PartyBelongedTo != null ? (hero.PartyBelongedTo, (Settlement?)null) : (null, null);

        private static float GetWeightedMeanDistance(IReadOnlyCollection<(float Distance, float Weight)> settlementDistances) =>
            settlementDistances.Any(x => x.Weight > 0)
                ? (float)((settlementDistances.Sum(x => x.Distance * x.Weight) + 1.0) / (settlementDistances.Sum(x => x.Weight) + 1.0))
                : float.NaN;

        private static float GetSettlementWeight(Settlement settlement) => settlement.IsTown ? 2f : settlement.IsCastle ? 1f : settlement.IsVillage ? 0.5f : 0f;
    }
}