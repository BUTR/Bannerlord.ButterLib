using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bannerlord.ButterLib.DynamicAPI
{
    public static class DynamicAPIProvider
    {
        private record TypeWithAttribute(Type Type, CustomAttributeData CustomAttributeData);
        private record MethodInfoWithAttribute(MethodInfo MethodInfo, CustomAttributeData CustomAttributeData);

        private delegate object DynamicAPIObjectActivator();

        private static readonly HashSet<string> PossibleClassNames = new()
        {
            "Bannerlord.ButterLib.DynamicAPI.DynamicAPIClassAttribute",
            "Bannerlord.DynamicAPI.DynamicAPIClassAttribute",
        };
        private static readonly HashSet<string> PossibleMethodNames = new()
        {
            "Bannerlord.ButterLib.DynamicAPI.DynamicAPIMethodAttribute",
            "Bannerlord.DynamicAPI.DynamicAPIMethodAttribute",
        };

        private static readonly ConcurrentDictionary<string, DynamicAPIObjectActivator?> CachedActivators = new();
        internal static readonly Dictionary<string, Type> APIClasses;
        internal static readonly Dictionary<Type, Dictionary<string, MethodInfo>> APIClassMethods;

        static DynamicAPIProvider()
        {
            APIClasses = GetAssembliesToScan()
                .SelectMany(x =>
                {
                    try
                    {
                        return x.GetTypes().Select(GetDynamicAPIClass);
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        return e.Types.Select(GetDynamicAPIClass);
                    }
                })
                .OfType<TypeWithAttribute>()
                .ToDictionary(DynamicAPIClassAttributeName, x => x.Type);

            APIClassMethods = APIClasses.Values
                .ToDictionary(
                    x => x,
                    x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                        .Select(GetDynamicAPIMethod)
                        .OfType<MethodInfoWithAttribute>()
                        .ToDictionary(DynamicAPIMethodAttributeName, y => y.MethodInfo));
        }

        private static TypeWithAttribute? GetDynamicAPIClass(Type? type)
        {
            bool Predicate(CustomAttributeData x) =>
                PossibleClassNames.Contains(x.AttributeType.FullName) && x.ConstructorArguments.Count == 1 && x.ConstructorArguments[0].ArgumentType == typeof(string);

            if (type is null)
                return null;

            if (type.IsAbstract)
                return null;

            if (type.CustomAttributes.FirstOrDefault(Predicate) is not { } attribute)
                return null;

            return new TypeWithAttribute(type, attribute);
        }

        private static MethodInfoWithAttribute? GetDynamicAPIMethod(MethodInfo? methodInfo)
        {
            bool Predicate(CustomAttributeData x) =>
                PossibleMethodNames.Contains(x.AttributeType.FullName) && x.ConstructorArguments.Count == 1 && x.ConstructorArguments[0].ArgumentType == typeof(string);

            if (methodInfo is null)
                return null;

            if (methodInfo.CustomAttributes.FirstOrDefault(Predicate) is not { } attribute)
                return null;

            return new MethodInfoWithAttribute(methodInfo, attribute);
        }

        private static string DynamicAPIClassAttributeName(TypeWithAttribute typeWithAttribute) =>
            (string) typeWithAttribute.CustomAttributeData.ConstructorArguments[0].Value;

        private static string DynamicAPIMethodAttributeName(MethodInfoWithAttribute methodInfoWithAttribute)
        {
            var name = (string) methodInfoWithAttribute.CustomAttributeData.ConstructorArguments[0].Value;
            return $"{(methodInfoWithAttribute.MethodInfo.IsStatic ? "0static" : "0instance")}_{name}";
        }

        private static IEnumerable<Assembly> GetAssembliesToScan()
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules().OfType<ModuleInfoExtendedWithMetadata>().ToList();
            foreach (var assembly in AccessTools2.AllAssemblies().Where(x => !x.IsDynamic && !string.IsNullOrEmpty(x.Location)))
            {
                if (loadedModules.Any(loadedModule => ModuleInfoHelper.IsModuleAssembly(loadedModule, assembly)))
                    yield return assembly;
            }
        }


        /// <summary>
        /// Return an API instance, see <see cref="DynamicAPIClassAttribute"/>.
        /// </summary>
        /// <param name="class"></param>
        public static DynamicAPIInstance? RequestAPIClass(string? @class)
        {
            if (@class is null)
                return null;

            var ctor = CachedActivators.GetOrAdd(@class, static key =>
            {
                if (!APIClasses.TryGetValue(key, out var type))
                    return null;

                if (type.GetConstructor(Type.EmptyTypes) is not { } constructorInfo)
                    return null;

                return (DynamicAPIObjectActivator) Expression.Lambda(typeof(DynamicAPIObjectActivator), Expression.New(constructorInfo)).Compile();
            });

            if (ctor is null)
                return null;

            return new(ctor());
        }

        /// <summary>
        /// Return a static API method, see <see cref="DynamicAPIMethodAttribute"/>.
        /// We recommend to save the delegate instead of calling this function multiple times.
        /// </summary>
        public static TDelegate? RequestAPIMethod<TDelegate>(string? @class, string? method) where TDelegate : Delegate
        {
            if (@class is null || method is null)
                return null;

            if (!APIClasses.TryGetValue(@class, out var type))
                return null;

            if (!APIClassMethods.TryGetValue(type, out var apiMethods))
                return null;

            if (!apiMethods.TryGetValue($"0static_{method}", out var methodInfo))
                return null;

            return AccessTools2.GetDelegate<TDelegate>(methodInfo);
        }
    }
}