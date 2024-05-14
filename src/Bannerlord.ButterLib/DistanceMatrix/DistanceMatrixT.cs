using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix;

/// <summary>
/// A generic class that pairs given objects of type <typeparamref name="T"/>
/// and for each pair calculates the distance between the objects that formed it.
/// </summary>
/// <typeparam name="T">The type of objects for which the distance matrix should be calculated.</typeparam>
/// <remarks><see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> implements built-in calculation for the <see cref="Hero"/>,
/// <see cref="TaleWorlds.CampaignSystem.Settlements.Settlement"/>, <see cref="Clan"/> and <see cref="Kingdom"/> objects.
/// For any other <see cref="MBObjectBase"/> subtypes custom EntityListGetter and DistanceCalculator methods
/// should be provided using special constructor <see cref="DistanceMatrix{T}.Create(Func{IEnumerable{T}}, Func{T, T, float})"/> .
/// </remarks>
public abstract class DistanceMatrix<T> : DistanceMatrix where T : MBObjectBase
{
    /// <summary>
    /// Initializes and returns a new instance of the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1"/> class
    /// as per actual implementation with the default EntityListGetter and DistanceCalculator methods.
    /// </summary>
    /// <exception cref="T:System.ArgumentException"></exception>
    public static DistanceMatrix<T>? Create() => StaticInstance?.Create<T>();

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
    /// <param name="distanceCalculatorArgs">Optional array of arguments that will be passed to the calculation method.</param>
    /// <exception cref="T:System.ArgumentException"></exception>
    public static DistanceMatrix<T>? Create(Func<IEnumerable<T>> customListGetter, Func<T, T, object[]?, float> customDistanceCalculator, object[]? distanceCalculatorArgs) =>
        StaticInstance?.Create(customListGetter, customDistanceCalculator, distanceCalculatorArgs);

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
    public abstract Dictionary<(T Object1, T Object2), float> AsTypedDictionary { get; }

    /// <summary>Objectified distance matrix representation for nearest neighbours processing</summary>
    /// <value>
    /// A dictionary of the <typeparamref name="T"/> object type as a key
    /// and a values are represented by a <see cref="T:System.Collections.Generic.SortedSet`1"/> containing tuples of all other objects and the corresponding distances in a form of floating point numbers.
    /// </value>
    public abstract Dictionary<T, SortedSet<(T OtherObject, float Distance)>> AsFlatenedDictionary { get; }

    /// <summary>Gets calculated distance between specified type <typeparamref name="T"/> objects.</summary>
    /// <param name="object1">The first of the objects between which it is necessary to determine the distance.</param>
    /// <param name="object2">The second of the objects between which it is necessary to determine the distance.</param>
    /// <returns>
    /// A floating-point number representing the distance between two specified <see cref="MBObjectBase"/> objects;
    /// or <see cref="float.NaN" />, if distance was not calculated or it is uncomputable.
    /// </returns>
    public abstract float GetDistance(T object1, T object2);

    /// <summary>
    /// Sets new distance value for the specified type <typeparamref name="T"/> objects.
    /// </summary>
    /// <param name="object1">The first of the objects between which it is necessary to change the predetermined distance.</param>
    /// <param name="object2">The second of the objects between which it is necessary to change the predetermined distance.</param>
    /// <param name="distance">New distance value.</param>
    public abstract void SetDistance(T object1, T object2, float distance);

    /// <summary>
    /// Search for nearest neighbours of the specified type <typeparamref name="T"/> object.
    /// </summary>
    /// <param name="inquiredObject">Object to search nearest neighbours for.</param>
    /// <param name="count">Number of neighbours to be returned.</param>
    /// <returns></returns>
    public abstract IEnumerable<(T OtherObject, float Distance)> GetNearestNeighbours(T inquiredObject, int count);

    /// <summary>
    /// Search for nearest neighbours of the specified type <typeparamref name="T"/> object
    /// using provided search predicate.
    /// </summary>
    /// <remarks>Does not automatically exclude <see cref="float.NaN"/> distances.</remarks>
    /// <param name="inquiredObject">Object to search nearest neighbours for.</param>
    /// <param name="count">Number of neighbours to be returned.</param>
    /// <param name="searchPredicate">A search predicate to filter through neighbours before returning nearest ones that qualify.</param>
    /// <returns></returns>
    public abstract IEnumerable<(T OtherObject, float Distance)> GetNearestNeighbours(T inquiredObject, int count, Func<(T OtherObject, float Distance), bool> searchPredicate);

    /// <summary>
    /// Search for nearest neighbours of the specified type <typeparamref name="T"/> object
    /// and then normalize the result to a given range based on all the ditances
    /// between specified object and other objects in the Matrix.
    /// </summary>
    /// <remarks>Automatically excludes <see cref="float.NaN"/> distances.</remarks>
    /// <param name="inquiredObject">Object to search nearest neighbours for.</param>
    /// <param name="count">Number of neighbours to be returned.</param>
    /// <param name="scaleMin">Minimum normalized value.</param>
    /// <param name="scaleMax">Maximum normalized value.</param>
    /// <returns></returns>
    public abstract IEnumerable<(T OtherObject, float Distance)> GetNearestNeighboursNormalized(T inquiredObject, int count, float scaleMin = 0f, float scaleMax = 100f);

    /// <summary>
    /// Search for nearest neighbours of the specified type <typeparamref name="T"/> object
    /// using provided search predicate. Then normalize the result to a given range based on all the ditances
    /// between specified object and other objects in the Matrix that also qualify the predicate.
    /// </summary>
    /// <remarks>Does not automatically exclude <see cref="float.NaN"/> distances.</remarks>
    /// <param name="inquiredObject">Object to search nearest neighbours for.</param>
    /// <param name="count">Number of neighbours to be returned.</param>
    /// <param name="searchPredicate">A search predicate to filter through neighbours before returning nearest ones that qualify.</param>
    /// <param name="scaleMin">Minimum normalized value.</param>
    /// <param name="scaleMax">Maximum normalized value.</param>
    /// <returns></returns>
    public abstract IEnumerable<(T OtherObject, float Distance)> GetNearestNeighboursNormalized(T inquiredObject, int count, Func<(T OtherObject, float Distance), bool> searchPredicate, float scaleMin = 0f, float scaleMax = 100f);
}