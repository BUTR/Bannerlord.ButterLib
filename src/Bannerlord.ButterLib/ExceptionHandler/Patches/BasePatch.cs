using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal static class BasePatch
    {
        public static readonly MethodInfo? FinalizerMethod = AccessTools.Method(typeof(BasePatch), nameof(Finalizer));

        public static IEnumerable<MethodInfo> GetDerivedMethods<TType, TAttribute>() where TAttribute : Attribute
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(TType).IsAssignableFrom(t)).ToArray();
            return types.SelectMany(t => t.GetMethods(AccessTools.allDeclared)).Where(m => !m.IsAbstract && m.GetCustomAttribute<TAttribute>() != null);
        }

        public static IEnumerable<MethodInfo> GetMethods<TType, TAttribute>() where TAttribute : Attribute
        {
            return GetMethods<TAttribute>(typeof(TType));
        }
        public static IEnumerable<MethodInfo> GetMethods<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetMethods(AccessTools.allDeclared).Where(m => m.GetCustomAttribute<TAttribute>() != null);
        }

        private static void Finalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}