using System;
using System.Reflection;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
    public static class AccessTools2
    {
        /// <summary>Gets the delegate for a method by searching the type and all its super types</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            MethodInfo miOriginal = Method(type, method);
            return miOriginal != null ? (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), miOriginal) : null;
        }

        /// <summary>Gets the delegate for a method by searching the type and all its super types</summary>
        /// <param name="instance">The instance where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate where TInstance : notnull
        {
            return GetDelegate<TDelegate>(instance.GetType(), method);
        }

        /// <summary>Gets the delegate for a directly declared method</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            MethodInfo miOriginal = DeclaredMethod(type, method);
            return miOriginal != null ? (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), miOriginal) : null;
        }

        /// <summary>Gets the delegate for a directly declared method</summary>
        /// <param name="instance">The instance where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate where TInstance : notnull
        {
            return GetDeclaredDelegate<TDelegate>(instance.GetType(), method);
        }
    }
}
