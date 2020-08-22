using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    public abstract class DistanceMatrix
    {
        private static DistanceMatrixStatic? _staticInstance;
        internal static DistanceMatrixStatic StaticInstance =>
            _staticInstance ??= ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<DistanceMatrixStatic>();

        public static DistanceMatrix<T> Create<T>() where T : MBObjectBase => StaticInstance.Create<T>();
        public static DistanceMatrix<T> Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) where T : MBObjectBase =>
            StaticInstance.Create<T>(customListGetter, customDistanceCalculator);

        /// <summary>Calculates distance between two given <see cref="Hero"/> objects.</summary>
        /// <param name="hero1">The first of the heroes to calculate distance between.</param>
        /// <param name="hero2">The second of the heroes to calculate distance between.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Hero"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        public static float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2)
            => StaticInstance.CalculateDistanceBetweenHeroes(hero1, hero2);

        /// <summary>Calculates distance between two given <see cref="Clan"/> objects.</summary>
        /// <param name="clan1">First of the clans to calculate distance between.</param>
        /// <param name="clan2">Second of the clans to calculate distance between.</param>
        /// <param name="settlementOwnersPairedList">
        /// List of the distances between pairs of settlements and of the weights of the paired settlements,
        /// except that the owner clan pairs are used instead of the settlements themselves to speed up the process.
        /// </param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Clan"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between clans fiefs weighted by the fief type.</remarks>
        public static float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, IEnumerable<(ulong Owners, float Distance, float Weight)> settlementOwnersPairedList)
            => StaticInstance.CalculateDistanceBetweenClans(clan1, clan2, settlementOwnersPairedList);

        /// <summary>Calculates distance between two given <see cref="Kingdom"/> objects.</summary>
        /// <param name="kingdom1">First of the kingdoms to calculate distance between.</param>
        /// <param name="kingdom2">Second of the kingdoms to calculate distance between.</param>
        /// <param name="settlementDistanceMatrix">Settlement distance matrix .</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Kingdom"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between kingdoms fiefs weighted by the fief type.</remarks>
        public static float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Settlement> settlementDistanceMatrix)
            => StaticInstance.CalculateDistanceBetweenKingdoms(kingdom1, kingdom2, settlementDistanceMatrix);

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
        /// <see cref="CalculateDistanceBetweenClans"/>
        /// method with required list argument.
        /// </remarks>
        public static List<(ulong Owners, float Distance, float Weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
            => StaticInstance.GetSettlementOwnersPairedList(settlementDistanceMatrix);
    }
}