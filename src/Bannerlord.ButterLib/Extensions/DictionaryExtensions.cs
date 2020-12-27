using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Extensions
{
    /// <summary>Extension class for working with Dictionaries.</summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Deconstructs a <see cref="T:System.Collections.Generic.KeyValuePair`2" /> into two variables
        /// containing a key and a value respectively.
        /// </summary>
        /// <typeparam name="TKey">The type of the key used in the <see cref="T:System.Collections.Generic.KeyValuePair`2" />.</typeparam>
        /// <typeparam name="TValue">The type of the value used in the <see cref="T:System.Collections.Generic.KeyValuePair`2" />.</typeparam>
        /// <param name="tuple">An original pair of key and value.</param>
        /// <param name="key">Deconstructed key.</param>
        /// <param name="value">Deconstructed value.</param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> tuple, out TKey key, out TValue value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}