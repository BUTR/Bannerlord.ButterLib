using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.MountAndBlade.View.Missions;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class MissionViewPatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<MissionViewPatch>>() ??
                      NullLogger<MissionViewPatch>.Instance;

            if (OnMissionScreenTickMethod == null)
                _logger.LogError("OnMissionScreenTickMethod is null");

            if (OnMissionScreenTickMethod == null)
            {
                return;
            }

            harmony.Patch(OnMissionScreenTickMethod, finalizer: new HarmonyMethod(OnMissionScreenTickFinalizerMethod));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(OnMissionScreenTickMethod, OnMissionScreenTickFinalizerMethod);
        }

        private static readonly MethodInfo? OnMissionScreenTickMethod =
            Method(typeof(MissionView), "OnMissionScreenTick");

        private static readonly MethodInfo? OnMissionScreenTickFinalizerMethod =
            Method(typeof(MissionViewPatch), nameof(OnMissionScreenTickFinalizer));

        public static void OnMissionScreenTickFinalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}