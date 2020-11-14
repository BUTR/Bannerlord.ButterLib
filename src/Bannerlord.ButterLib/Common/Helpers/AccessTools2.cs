using HarmonyLib;

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
        /// <summary>Creates a static field reference delegate</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldInfo">The field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`1" /> delegate</returns>
        public static FieldRef<TField>? StaticFieldRefAccess<TField>(FieldInfo? fieldInfo)
        {
            return fieldInfo == null ? null : AccessTools.StaticFieldRefAccess<TField>(fieldInfo);
        }

        /// <summary>Creates an instance field reference delegate for a private type</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="type">The class/type</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`2" /> delegate</returns>
        public static FieldRef<object, TField>? FieldRefAccess<TField>(Type type, string fieldName)
        {
            var field = Field(type, fieldName);
            return field == null ? null : AccessTools.FieldRefAccess<object, TField>(field);
        }

        /// <summary>Creates an instance field reference</summary>
        /// <typeparam name="TObject">The class the field is defined in</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable field reference delegate</returns>
        public static FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(string fieldName)
        {
            var field = typeof(TObject).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            return field == null ? null : AccessTools.FieldRefAccess<TObject, TField>(field);
        }


        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? constructorInfo) where TDelegate : Delegate
        {
            if (constructorInfo == null) return null;
            var parameters = constructorInfo.GetParameters().Select((p, i) => Expression.Parameter(p.ParameterType, $"p{i}")).ToList();
            var newExpression = Expression.New(constructorInfo, parameters);
            return Expression.Lambda<TDelegate>(newExpression, parameters).Compile();
        }

        public static TDelegate? GetConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
        {
            var constructorInfo = Constructor(type, parameters);
            return GetDelegate<TDelegate>(constructorInfo);
        }

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
        {
            var constructorInfo = DeclaredConstructor(type, parameters);
            return GetDelegate<TDelegate>(constructorInfo);
        }


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
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
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
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
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
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, MethodInfo? methodInfo) where TDelegate : Delegate where TInstance : notnull
        {
            return GetDelegate<TDelegate>(instance, methodInfo);
        }

        public static TDelegate? GetDelegate<TDelegate>(object instance, MethodInfo? methodInfo) where TDelegate : Delegate
        {
            if (methodInfo == null) return null;
            return Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo.Name) as TDelegate;
        }
    }
}