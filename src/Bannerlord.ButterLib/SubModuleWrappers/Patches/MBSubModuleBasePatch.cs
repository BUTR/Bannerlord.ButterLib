using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers.Patches
{
    /// <summary>
    /// Patches <see cref="MBSubModuleBase.OnMissionBehaviorInitialize(Mission)"/> so that it calls <see cref="MBSubModuleBaseListWrapper.OnMissionBehaviorInitialize(Mission)"/>
    /// </summary>
    internal sealed class MBSubModuleBasePatch
    {
        private static ILogger _log = default!;

        private static readonly Type? TargetType = typeof(MBSubModuleBase);

        private static readonly MethodInfo? TargetMethod = AccessTools.Method(TargetType, "OnMissionBehaviorInitialize") ?? AccessTools.Method(TargetType, "OnMissionBehaviourInitialize");
        private static readonly MethodInfo? PatchMethod = AccessTools.Method(typeof(MBSubModuleBasePatch), nameof(CallPostfix));

        internal static bool Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _log = provider?.GetRequiredService<ILogger<MBSubModuleBasePatch>>() ?? NullLogger<MBSubModuleBasePatch>.Instance;

            return NotNull(TargetType, nameof(TargetType))
                   & NotNull(TargetMethod, nameof(TargetMethod))
                   & NotNull(PatchMethod, nameof(PatchMethod))
                   && harmony.Patch(TargetMethod, postfix: new HarmonyMethod(PatchMethod)) is not null;
        }

        internal static bool Disable(Harmony harmony)
        {
            if (NotNull(TargetType, nameof(TargetType))
                & NotNull(TargetMethod, nameof(TargetMethod))
                & NotNull(PatchMethod, nameof(PatchMethod)))
            {
                harmony.Unpatch(TargetMethod, PatchMethod);
            }

            return true;
        }

        private static void CallPostfix(Mission mission, MBSubModuleBase __instance)
        {
            if (__instance is MBSubModuleBaseListWrapper listWrapper)
            {
                listWrapper.OnMissionBehaviorInitialize(mission);
            }
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