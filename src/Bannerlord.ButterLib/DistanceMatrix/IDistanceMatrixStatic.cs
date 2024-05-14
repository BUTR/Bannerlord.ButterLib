using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix;

/// <summary>
/// An abstract class, containing static members of the
/// <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> class.
/// </summary>
internal interface IDistanceMatrixStatic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
    /// as per actual implementation with the default EntityListGetter and DistanceCalculator methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException"></exception>
    DistanceMatrix<T> Create<T>() where T : MBObjectBase;

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
    DistanceMatrix<T> Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) where T : MBObjectBase;

    /// <summary>Calculates distance between two given <see cref="Hero"/> objects.</summary>
    /// <param name="hero1">The first of the heroes to calculate distance between.</param>
    /// <param name="hero2">The second of the heroes to calculate distance between.</param>
    /// <returns>
    /// A floating-point number representing the distance between two specified <see cref="Hero"/> objects
    /// or <see cref="float.NaN" /> if distance could not be calculated.
    /// </returns>
    float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2);

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
    float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, Dictionary<ulong, WeightedDistance> settlementOwnersPairedList);

    /// <summary>Calculates distance between two given <see cref="Kingdom"/> objects.</summary>
    /// <param name="kingdom1">First of the kingdoms to calculate distance between.</param>
    /// <param name="kingdom2">Second of the kingdoms to calculate distance between.</param>
    /// <param name="clanDistanceMatrix">Settlement distance matrix .</param>
    /// <returns>
    /// A floating-point number representing the distance between two specified <see cref="Kingdom"/> objects
    /// or <see cref="float.NaN" /> if distance could not be calculated.
    /// </returns>
    /// <remarks>Calculation is based on the average distance between kingdoms fiefs weighted by the fief type.</remarks>
    float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Clan> clanDistanceMatrix);

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
    /// <see cref="DistanceMatrix{T}.CalculateDistanceBetweenClans(Clan, Clan, IEnumerable{DistanceMatrixResult})"/>
    /// method with required list argument.
    /// </remarks>
    Dictionary<ulong, WeightedDistance> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix);
}

public record WeightedDistance(float Distance, float Weight);