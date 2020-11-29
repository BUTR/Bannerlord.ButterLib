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
    /// <summary>
    /// While we cover this via DotNetManagedPatch, we want to have the same behavior as BEW
    /// </summary>
    internal sealed class ModulePatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<ModulePatch>>() ??
                      NullLogger<ModulePatch>.Instance;

            if (OnApplicationTickMethod == null)
                _logger.LogError("OnApplicationTickMethod is null");

            if (OnApplicationTickMethod == null)
            {
                return;
            }

            harmony.Patch(
                OnApplicationTickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(OnApplicationTickMethod, FinalizerMethod);
        }

        private static readonly MethodInfo? OnApplicationTickMethod = Method(typeof(TaleWorlds.MountAndBlade.Module), "OnApplicationTick");

        private static readonly MethodInfo? FinalizerMethod = Method(typeof(ModulePatch), nameof(Finalizer));

        public static void Finalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}