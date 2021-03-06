using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using static HarmonyLib.AccessTools;
using AccessTools3 = HarmonyLib.BUTR.Extensions.AccessTools2;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
    [Obsolete("Use package Harmony.Extensions instead!", false)]
    public static class AccessTools2
    {
        /// <summary>Creates a static field reference delegate</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldInfo">The field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`1" /> delegate</returns>
        public static FieldRef<TField>? StaticFieldRefAccess<TField>(FieldInfo? fieldInfo)
            => AccessTools3.StaticFieldRefAccess<TField>(fieldInfo);

        /// <summary>Creates an instance field reference delegate for a private type</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="type">The class/type</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`2" /> delegate</returns>
        public static FieldRef<object, TField>? FieldRefAccess<TField>(Type type, string fieldName)
            => AccessTools3.FieldRefAccess<TField>(type, fieldName);

        /// <summary>Creates an instance field reference</summary>
        /// <typeparam name="TObject">The class the field is defined in</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable field reference delegate</returns>
        public static FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(string fieldName)
            => AccessTools3.FieldRefAccess<TObject, TField>(fieldName);

        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? constructorInfo) where TDelegate : Delegate
            => AccessTools3.GetDelegate<TDelegate>(constructorInfo);

        public static TDelegate? GetConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => AccessTools3.GetConstructorDelegate<TDelegate>(type, parameters);

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => AccessTools3.GetDeclaredConstructorDelegate<TDelegate>(type, parameters);

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
            => AccessTools3.GetDelegateObjectInstance<TDelegate>(type, method);

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
            => AccessTools3.GetDelegateObjectInstance<TDelegate>(type, method, parameters, generics);

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
            => AccessTools3.TryGetDelegateObjectInstance<TDelegate>(out outDelegate, type, method, parameters, generics);

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
            => AccessTools3.GetDeclaredDelegateObjectInstance<TDelegate>(type, method);

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
            => AccessTools3.GetDeclaredDelegateObjectInstance<TDelegate>(type, method, parameters, generics);

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
            => AccessTools3.TryGetDeclaredDelegateObjectInstance<TDelegate>(out outDelegate, type, method, parameters, generics);

        /// <summary>Get a delegate bound to the instance type <see cref="object"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
            => AccessTools3.GetDelegateObjectInstance<TDelegate>(methodInfo);

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
            => AccessTools3.GetDelegate<TDelegate>(type, method);

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
            => AccessTools3.GetDelegate<TDelegate>(type, method, parameters, generics);

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
            => AccessTools3.TryGetDelegate<TDelegate>(out outDelegate, type, method, parameters, generics);

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
            => AccessTools3.GetDeclaredDelegate<TDelegate>(type, method);

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
            => AccessTools3.GetDeclaredDelegate<TDelegate>(type, method, parameters, generics);

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
            => AccessTools3.TryGetDeclaredDelegate<TDelegate>(out outDelegate, type, method, parameters, generics);

        /// <summary>Get a delegate for a method described by <paramref name="methodInfo"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
            => AccessTools3.GetDelegate<TDelegate>(methodInfo);

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
            => AccessTools3.GetDelegate<TDelegate, TInstance>(instance, method);

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
            => AccessTools3.GetDelegate<TDelegate, TInstance>(instance, method, parameters, generics);

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
            => AccessTools3.TryGetDelegate<TDelegate, TInstance>(out outDelegate, instance, method, parameters, generics);

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
            => AccessTools3.GetDeclaredDelegate<TDelegate, TInstance>(instance, method);

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
            => AccessTools3.GetDeclaredDelegate<TDelegate, TInstance>(instance, method, parameters, generics);

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
            => AccessTools3.TryGetDelegate<TDelegate, TInstance>(out outDelegate, instance, method, parameters, generics);

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
            => AccessTools3.GetDelegate<TDelegate, TInstance>(instance, methodInfo);

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
            => AccessTools3.GetDelegate<TDelegate>(instance, methodInfo);
    }
}