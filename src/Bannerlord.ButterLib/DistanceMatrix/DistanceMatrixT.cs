using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem; //used for the documentation refs
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    /// <summary>
    /// A generic class that pairs given objects of type <typeparamref name="T"/>
    /// and for each pair calculates the distance between the objects that formed it.
    /// </summary>
    /// <typeparam name="T">The type of objects for which the distance matrix should be calculated.</typeparam>
    /// <remarks><see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> implements built-in calculation for the <see cref="Hero"/>,
    /// <see cref="Settlement"/>, <see cref="Clan"/> and <see cref="Kingdom"/> objects.
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
        public static DistanceMatrix<T> Create() => StaticInstance.Create<T>();

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
        public static DistanceMatrix<T> Create(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) =>
            StaticInstance.Create(customListGetter, customDistanceCalculator);

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
    }
}