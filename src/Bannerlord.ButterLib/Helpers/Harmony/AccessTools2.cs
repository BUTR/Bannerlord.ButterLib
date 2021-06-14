using System;
using System.Reflection;

using static HarmonyLib.AccessTools;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use package Harmony.Extensions instead!", true)]
    public static class AccessTools2
    {
        public static FieldRef<TField>? StaticFieldRefAccess<TField>(FieldInfo? _) => null;

        public static FieldRef<object, TField>? FieldRefAccess<TField>(Type _, string __) => null;
        public static FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(string _) => null;

        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? _) where TDelegate : Delegate => null;

        public static TDelegate? GetConstructorDelegate<TDelegate>(Type _, Type[]? __ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type _, Type[]? __ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type _, string __) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type _, string __) where TDelegate : Delegate => null;
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? _) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegate<TDelegate>(Type _, string __) where TDelegate : Delegate => null;
        public static TDelegate? GetDelegate<TDelegate>(Type _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type _, string __) where TDelegate : Delegate => null;
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? _) where TDelegate : Delegate => null;
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance _, string __) where TDelegate : Delegate => null;
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance _, string __) where TDelegate : Delegate => null;
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance _, string __, Type[]? ___, Type[]? ____ = null) where TDelegate : Delegate => null;

        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance _, MethodInfo? __) where TDelegate : Delegate => null;
        public static TDelegate? GetDelegate<TDelegate>(object? _, MethodInfo? __) where TDelegate : Delegate => null;
    }
}