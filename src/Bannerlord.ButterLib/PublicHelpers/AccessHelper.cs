using System;
using System.Reflection;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.PublicHelpers
{
    public static class AccessHelper
    {
        public static TDelegate? GetDelegate<TDelegate>(Type type, string originalMethod) where TDelegate : Delegate
        {
            MethodInfo miOriginal = Method(type, originalMethod);
            return miOriginal != null ? (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), miOriginal) : null;
        }
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string originalMethod) where TDelegate : Delegate where TInstance : class
        {
            return GetDelegate<TDelegate>(instance.GetType(), originalMethod);
        }

        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string originalMethod) where TDelegate : Delegate
        {
            MethodInfo miOriginal = DeclaredMethod(type, originalMethod);
            return miOriginal != null ? (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), miOriginal) : null;
        }
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance, string originalMethod) where TDelegate : Delegate where TInstance : class
        {
            return GetDeclaredDelegate<TDelegate>(instance.GetType(), originalMethod);
        }
    }
}
