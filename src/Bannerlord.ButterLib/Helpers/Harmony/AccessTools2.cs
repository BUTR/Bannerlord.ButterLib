using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using static HarmonyLib.AccessTools;

// ReSharper disable once CheckNamespace
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
            => fieldInfo is null ? null : AccessTools.StaticFieldRefAccess<TField>(fieldInfo);

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
            => GetDelegate<TDelegate>(Constructor(type, parameters));

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredConstructor(type, parameters));

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
            => GetDelegateObjectInstance<TDelegate>(Method(type, method));

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
            => GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                     Type type,
                                                                     string method,
                                                                     Type[]? parameters = null,
                                                                     Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics))) is not null;

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
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method));

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
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                             Type type,
                                                                             string method,
                                                                             Type[]? parameters = null,
                                                                             Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

        /// <summary>Get a delegate bound to the instance type <see cref="object"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
        {
            if (methodInfo is null || methodInfo.DeclaringType is null) return null;

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
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate => GetDelegate<TDelegate>(Method(type, method));

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
            => GetDelegate<TDelegate>(Method(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/>
        /// or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                       Type type,
                                                       string method,
                                                       Type[]? parameters = null,
                                                       Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(Method(type, method, parameters, generics))) is not null;

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
            => GetDelegate<TDelegate>(DeclaredMethod(type, method));

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
            => GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                               Type type,
                                                               string method,
                                                               Type[]? parameters = null,
                                                               Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

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
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method));

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
                                                                   Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for an instance method named <paramref name="method"/>,
        /// declared by <paramref name="instance"/>'s type or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                  TInstance instance,
                                                                  string method,
                                                                  Type[]? parameters = null,
                                                                  Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics))) is not null;

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
                                                                           string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method));

        /// <summary>
        /// Get a delegate for an instance method named <paramref name="method"/>, directly declared by <paramref name="instance"/>'s type.
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
                                                                           Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for an instance method named <paramref name="method"/>,
        /// directly declared by <paramref name="instance"/>'s type.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                          TInstance instance,
                                                                          string method,
                                                                          Type[]? parameters = null,
                                                                          Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => GetDelegate<TDelegate>(instance, methodInfo);

        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(object? instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => instance is null || methodInfo is null ? null : Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo.Name) as TDelegate;
    }
}