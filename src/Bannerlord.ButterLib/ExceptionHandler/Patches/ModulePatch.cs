using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class ModulePatch
    {
        private static ILogger _logger = default!;

        internal static void Apply(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<ModulePatch>>() ??
                      NullLogger<ModulePatch>.Instance;

            if (OnApplicationTickMethod == null)
                _logger.LogError("OnApplicationTickMethod is null");

            if (OnApplicationTickMethod == null)
            {
                return;
            }

            harmony.Patch(OnApplicationTickMethod, finalizer: new HarmonyMethod(OnApplicationTickFinalizerMethod));
        }

        private static readonly MethodInfo? OnApplicationTickMethod =
            Method(typeof(TaleWorlds.MountAndBlade.Module), "OnApplicationTick");

        private static readonly MethodInfo? OnApplicationTickFinalizerMethod =
            Method(typeof(ModulePatch), nameof(OnApplicationTickFinalizer));

        public static void OnApplicationTickFinalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}