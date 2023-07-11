using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    /// <summary>
    /// An abstract class used in a <see cref="Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix{T}" /> derived class,
    /// that pairs given objects of type T
    /// and for each pair calculates the distance between the objects that formed it.
    /// </summary>
    public abstract class DistanceMatrix
    {
        private static IDistanceMatrixStatic? _staticInstance;
        internal static IDistanceMatrixStatic? StaticInstance =>
            _staticInstance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IDistanceMatrixStatic>();


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
        /// as per actual implementation with the default EntityListGetter and DistanceCalculator methods.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentException"></exception>
        public static DistanceMatrix<T>? Create<T>() where T : MBObjectBase => StaticInstance?.Create<T>();

        /// <summary>
        /// Initializes and returns a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
        /// as per actual implementation with custom methods that will be used to get the list of analyzed objects
        /// and calculate the distances between them.
        /// </summary>
        /// <param name="customListGetter">
        /// A delegate to the method that will be used to get a list of objects of type <typeparamref name="T"/>
        /// for calculating the distances between them.
        /// </param>
        /// <param name="customDistanceCalculator">
        /// A delegate to the method that will be used to calculate the distance between two given type <typeparamref name="T"/> objects.
        /// </param>
        /// <exception cref="T:System.ArgumentException"></exception>
        public static DistanceMatrix<T>? Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) where T : MBObjectBase =>
            StaticInstance?.Create(customListGetter, customDistanceCalculator);

        /// <summary>Calculates distance between two given <see cref="Hero"/> objects.</summary>
        /// <param name="hero1">The first of the heroes to calculate distance between.</param>
        /// <param name="hero2">The second of the heroes to calculate distance between.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Hero"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        public static float? CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2)
            => StaticInstance?.CalculateDistanceBetweenHeroes(hero1, hero2);

        /// <summary>Calculates distance between two given <see cref="Clan"/> objects.</summary>
        /// <param name="clan1">First of the clans to calculate distance between.</param>
        /// <param name="clan2">Second of the clans to calculate distance between.</param>
        /// <param name="settlementOwnersPairedList">
        /// List of the distances between pairs of settlements and of the weights of the paired settlements,
        /// except that the owner clan pairs are used instead of the settlements themselves to speed up the process.
        /// </param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Clan"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated (usually when clan has no fiefs).
        /// </returns>
        /// <remarks>Calculation is based on the average distance between clans fiefs weighted by the fief type.</remarks>
        public static float? CalculateDistanceBetweenClans(Clan clan1, Clan clan2, IEnumerable<DistanceMatrixResult> settlementOwnersPairedList)
            => StaticInstance?.CalculateDistanceBetweenClans(clan1, clan2, settlementOwnersPairedList);

        /// <summary>Calculates distance between two given <see cref="Kingdom"/> objects.</summary>
        /// <param name="kingdom1">First of the kingdoms to calculate distance between.</param>
        /// <param name="kingdom2">Second of the kingdoms to calculate distance between.</param>
        /// <param name="clanDistanceMatrix">Settlement distance matrix .</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Kingdom"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between kingdoms fiefs weighted by the fief type.</remarks>
        public static float? CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Clan> clanDistanceMatrix)
            => StaticInstance?.CalculateDistanceBetweenKingdoms(kingdom1, kingdom2, clanDistanceMatrix);

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
        public static List<DistanceMatrixResult>? GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
            => StaticInstance?.GetSettlementOwnersPairedList(settlementDistanceMatrix);
    }
}