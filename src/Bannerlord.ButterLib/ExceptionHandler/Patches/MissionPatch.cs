using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.MountAndBlade;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    /// <summary>
    /// While we cover this via DotNetManagedPatch, we want to have the same behavior as BEW
    /// </summary>
    internal sealed class MissionPatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<MissionPatch>>() ??
                      NullLogger<MissionPatch>.Instance;

            if (TickMethod == null)
                _logger.LogError("TickMethod is null");

            if (TickMethod == null)
            {
                return;
            }

            harmony.Patch(
                TickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(TickMethod, FinalizerMethod);
        }

        private static readonly MethodInfo? TickMethod = Method(typeof(Mission), "Tick");

        private static readonly MethodInfo? FinalizerMethod = Method(typeof(MissionPatch), nameof(Finalizer));

        public static void Finalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}