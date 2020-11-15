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
            return fieldInfo is null ? null : AccessTools.StaticFieldRefAccess<TField>(fieldInfo);
        }

        /// <summary>Creates an instance field reference delegate for a private type</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="type">The class/type</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`2" /> delegate</returns>
        public static FieldRef<object, TField>? FieldRefAccess<TField>(Type type, string fieldName)
        {
            var field = Field(type, fieldName);
            return field is null ? null : AccessTools.FieldRefAccess<object, TField>(field);
        }

        /// <summary>Creates an instance field reference</summary>
        /// <typeparam name="TObject">The class the field is defined in</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable field reference delegate</returns>
        public static FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(string fieldName)
        {
            var field = typeof(TObject).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            return field is null ? null : AccessTools.FieldRefAccess<TObject, TField>(field);
        }
        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? constructorInfo) where TDelegate : Delegate
        {
            if (constructorInfo is null) return null;
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

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var methodInfo = Method(type, method);
            return methodInfo is null ? null : GetDelegateObjectInstance<TDelegate>(methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type,
                                                                      string method,
                                                                      Type[]? parameters,
                                                                      Type[]? generics = null) where TDelegate : Delegate
        {
            var methodInfo = Method(type, method, parameters, generics);
            return methodInfo is null ? null : GetDelegateObjectInstance<TDelegate>(methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var methodInfo = DeclaredMethod(type, method);
            return methodInfo is null ? null : GetDelegateObjectInstance<TDelegate>(methodInfo);
        }


        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type,
                                                                              string method,
                                                                              Type[]? parameters,
                                                                              Type[]? generics = null) where TDelegate : Delegate
        {
            var methodInfo = DeclaredMethod(type, method, parameters, generics);
            return methodInfo is null ? null : GetDelegateObjectInstance<TDelegate>(methodInfo);
        }

        /// <summary>Get a delegate bound to the instance type <see cref="object"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
        {
            if (methodInfo is null) return null;

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameters = methodInfo.GetParameters().Select((t2, i) => Expression.Parameter(t2.ParameterType, $"p{i}")).ToList();

            var body = Expression.Call(
                Expression.Convert(instance, methodInfo.DeclaringType),
                methodInfo,
                parameters);

            return Expression.Lambda<TDelegate>(body, new List<ParameterExpression> { instance }.Concat(parameters)).Compile();
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var methodInfo = Method(type, method);
            return methodInfo is null ? null : GetDelegate<TDelegate>(methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/>
        /// or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type,
                                                        string method,
                                                        Type[]? parameters,
                                                        Type[]? generics = null) where TDelegate : Delegate
        {
            var methodInfo = Method(type, method, parameters, generics);
            return methodInfo is null ? null : GetDelegate<TDelegate>(methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
        {
            var methodInfo = DeclaredMethod(type, method);
            return methodInfo is null ? null : GetDelegate<TDelegate>(methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type,
                                                                string method,
                                                                Type[]? parameters,
                                                                Type[]? generics = null) where TDelegate : Delegate
        {
            var methodInfo = DeclaredMethod(type, method, parameters, generics);
            return methodInfo is null ? null : GetDelegate<TDelegate>(methodInfo);
        }

        /// <summary>Get a delegate for a method described by <paramref name="methodInfo"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
            => methodInfo is null ? null : Delegate.CreateDelegate(typeof(TDelegate), methodInfo) as TDelegate;

        /// <summary>
        /// Get a delegate for an instance method declared by <paramref name="instance"/>'s type or any of its base types.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance,
                                                                   string method) where TDelegate : Delegate where TInstance : notnull
        {
            if (instance is null) return null;
            var methodInfo = Method(instance.GetType(), method);
            return methodInfo is null ? null : GetDelegate<TDelegate, TInstance>(instance, methodInfo);
        }

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="instance"/>'s type or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance,
                                                                   string method,
                                                                   Type[]? parameters,
                                                                   Type[]? generics = null) where TDelegate : Delegate where TInstance : notnull
        {
            if (instance is null) return null;
            var methodInfo = Method(instance.GetType(), method, parameters, generics);
            return methodInfo is null ? null : GetDelegate<TDelegate, TInstance>(instance, methodInfo);
        }

        /// <summary>
        /// Get a delegate for an instance method directly declared by <paramref name="instance"/>'s type.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance,
                                                                           string method) where TDelegate : Delegate where TInstance : notnull
        {
            if (instance is null) return null;
            var methodInfo = DeclaredMethod(instance.GetType(), method);
            return methodInfo is null ? null : GetDelegate<TDelegate, TInstance>(instance, methodInfo);
        }

        /// <summary>
        /// Get a delegate for an instance method directly declared by <paramref name="instance"/>'s type.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance,
                                                                           string method,
                                                                           Type[]? parameters,
                                                                           Type[]? generics = null) where TDelegate : Delegate where TInstance : notnull
        {
            if (instance is null) return null;
            var methodInfo = DeclaredMethod(instance.GetType(), method, parameters, generics);
            return methodInfo is null ? null : GetDelegate<TDelegate, TInstance>(instance, methodInfo);
        }

        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance,
                                                                   MethodInfo? methodInfo) where TDelegate : Delegate where TInstance : notnull
            => (instance is null || methodInfo is null) ? null : GetDelegate<TDelegate>(instance, methodInfo);

        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(object instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => (instance is null || methodInfo is null) ? null : Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo.Name) as TDelegate;
    }
}