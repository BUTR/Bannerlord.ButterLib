using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
    public static class AccessTools2
    {
        /// <summary>Allows to use object as Delegate's instance type.</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var miOriginal = Method(type, method);
            return miOriginal != null ? GetDelegateObjectInstance<TDelegate>(miOriginal) : null;
        }

        /// <summary>Gets the delegate for a directly declared method</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var miOriginal = DeclaredMethod(type, method);
            return miOriginal != null ? GetDelegateObjectInstance<TDelegate>(miOriginal) : null;
        }

        /// <summary>Allows to use object as Delegate's instance type.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/></param>
        /// <returns>A delegate or null when the method is null</returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo methodInfo) where TDelegate : Delegate
        {
            if (methodInfo == null) return null;

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameters = methodInfo.GetParameters().Select((t2, i) => Expression.Parameter(t2.ParameterType, $"p{i}")).ToList();

            var body = Expression.Call(
                Expression.Convert(instance, methodInfo.DeclaringType!),
                methodInfo,
                parameters);
            return Expression.Lambda<TDelegate>(body, new List<ParameterExpression> { instance }.Concat(parameters)).Compile();
        }


        /// <summary>Gets the delegate for a method by searching the type and all its super types</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var miOriginal = Method(type, method);
            return miOriginal != null ? GetDelegate<TDelegate>(miOriginal) : null;
        }

        /// <summary>Gets the delegate for a directly declared method</summary>
        /// <param name="type">The type where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var miOriginal = DeclaredMethod(type, method);
            return miOriginal != null ? GetDelegate<TDelegate>(miOriginal) : null;
        }

        /// <summary>Gets the delegate for a method</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/></param>
        /// <returns>A delegate or null when the method is null</returns>
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo methodInfo) where TDelegate : Delegate
        {
            if (methodInfo == null) return null;
            return Delegate.CreateDelegate(typeof(TDelegate), methodInfo) as TDelegate;
        }


        /// <summary>Gets the delegate for a method by searching the type and all its super types</summary>
        /// <param name="instance">The instance where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate where TInstance : notnull
        {
            var miOriginal = Method(instance.GetType(), method);
            return miOriginal == null ? null : GetDelegate<TDelegate, TInstance>(instance, miOriginal);
        }

        /// <summary>Gets the delegate for a directly declared method</summary>
        /// <param name="instance">The instance where the method is declared</param>
        /// <param name="method">The name of the method (case sensitive)</param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate where TInstance : notnull
        {
            var miOriginal = DeclaredMethod(instance.GetType(), method);
            return miOriginal == null ? null : GetDelegate<TDelegate, TInstance>(instance, miOriginal);
        }

        /// <summary>Gets the delegate for a method</summary>
        /// <param name="instance">The instance where the method is declared</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/></param>
        /// <returns>A delegate or null when type/name is null or when the method cannot be found</returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, MethodInfo methodInfo) where TDelegate : Delegate where TInstance : notnull
        {
            return Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo.Name) as TDelegate;
        }
    }
}