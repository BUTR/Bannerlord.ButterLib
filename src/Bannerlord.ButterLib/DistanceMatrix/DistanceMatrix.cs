using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    public abstract class DistanceMatrix<T> where T : MBObjectBase
    {
        private static DistanceMatrixStatic<T>? _instance;
        private static DistanceMatrixStatic<T> Instance =>
            _instance ??= ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<DistanceMatrixStatic<T>>();

        public static DistanceMatrix<T> Create() =>
            Instance.Create();
        public static DistanceMatrix<T> Create(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) =>
            Instance.Create(customListGetter, customDistanceCalculator);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
        /// with default EntityListGetter and DistanceCalculator methods.
        /// </summary>
        /// <exception cref="T:System.ArgumentException"></exception>
        protected DistanceMatrix() { }

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
        protected DistanceMatrix(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) { }

        /// <summary>Raw distance matrix representation</summary>
        /// <value>
        /// A dictionary of paired type <typeparamref name="T"/> objects
        /// represented by unique 64-bit unsigned number as key
        /// and floating-point numbers, representing distances between those objects as value.
        /// </value>
        public abstract Dictionary<ulong, float> AsDictionary { get; }

        /// <summary>Objectified distance matrix representation</summary>
        /// <value>
        /// A dictionary of paired type <typeparamref name="T"/> objects as key
        /// and floating-point numbers, representing distances between those objects as value.
        /// </value>
        public abstract Dictionary<(T object1, T object2), float> AsTypedDictionary { get; }

        /// <summary>Gets calculated distance between specified type <typeparamref name="T"/> objects.</summary>
        /// <param name="object1">The first of the objects between which it is necessary to determine the distance.</param>
        /// <param name="object2">The second of the objects between which it is necessary to determine the distance.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="MBObjectBase"/> objects;
        /// or <see cref="float.NaN" />, if distance was not calculated or it is uncomputable.
        /// </returns>
        public abstract float GetDistance(T object1, T object2);


        /// <summary>Calculates distance between two given <see cref="Hero"/> objects.</summary>
        /// <param name="hero1">The first of the heroes to calculate distance between.</param>
        /// <param name="hero2">The second of the heroes to calculate distance between.</param>
        /// <returns>
        /// A floating-point number representing the distance between two specified <see cref="Hero"/> objects
        /// or <see cref="float.NaN" /> if distance could not be calculated.
        /// </returns>
        public static float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2)
            => Instance.CalculateDistanceBetweenHeroes(hero1, hero2);

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
        public static float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, List<(ulong owners, float distance, float weight)> settlementOwnersPairedList)
            => Instance.CalculateDistanceBetweenClans(clan1, clan2, settlementOwnersPairedList);

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
            => Instance.CalculateDistanceBetweenKingdoms(kingdom1, kingdom2, settlementDistanceMatrix);

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
        public static List<(ulong owners, float distance, float weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix)
            => Instance.GetSettlementOwnersPairedList(settlementDistanceMatrix);
    }
}