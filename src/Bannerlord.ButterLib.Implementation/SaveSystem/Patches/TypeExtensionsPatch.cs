using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches
{
    /*
     * <summary>
     * Replaces TaleWorlds.SaveSystem.TypeExtensions.IsContainer(this Type, out ContainerType)
     * </summary>
     * <remarks>
     * Our implementation is much more flexible and allows for the SaveSystem to support many more types of containers
     * in a safe way (i.e., no issues with the deserialization of these containers if ButterLib is removed).
     * </remarks>
     */
    internal sealed class TypeExtensionsPatch
    {
        private static ILogger _log = default!;

        internal static bool Apply(Harmony harmony)
        {
            _log = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<TypeExtensionsPatch>>()
                   ?? NullLogger<TypeExtensionsPatch>.Instance;

            return NotNull(TargetType, nameof(TargetType))
                 & NotNull(TargetMethod, nameof(TargetMethod))
                 & NotNull(PatchMethod, nameof(PatchMethod))
                && harmony.Patch(TargetMethod, prefix: new HarmonyMethod(PatchMethod)) is not null;
        }

        private static readonly Type? TargetType = typeof(MetaData).Assembly.GetType("TaleWorlds.SaveSystem.TypeExtensions");
        private static readonly Type[] TargetMethodParams = new[] { typeof(Type), typeof(ContainerType).MakeByRefType() };
        private static readonly MethodInfo? TargetMethod = AccessTools.Method(TargetType, "IsContainer", TargetMethodParams);
        private static readonly MethodInfo? PatchMethod = AccessTools.Method(typeof(TypeExtensionsPatch), "IsContainerPrefix");

        private static bool IsContainerPrefix(Type type, out ContainerType containerType, ref bool __result)
        {
            containerType = ContainerType.None;

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            if (type.IsArray)
                containerType = ContainerType.Array;
            else if (typeof(IDictionary).IsAssignableFrom(type))
                containerType = ContainerType.Dictionary;
            else if (typeof(IList).IsAssignableFrom(type))
                containerType = ContainerType.List;
            else if (type == typeof(Queue<>) || type == typeof(Queue))
                containerType = ContainerType.Queue; // Piggybacking ICollection<T> on to the "Queue" category is phase 2

            __result = containerType != ContainerType.None;
            return false;
        }

        private static bool NotNull<T>(T obj, string name) where T : class?
        {
            if (obj is null)
            {
                _log.LogError($"{name} is null!");
                return false;
            }

            return true;
        }
    }
}