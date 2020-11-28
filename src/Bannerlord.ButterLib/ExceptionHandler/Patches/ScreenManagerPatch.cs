using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class ScreenManagerPatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<ScreenManagerPatch>>() ??
                      NullLogger<ScreenManagerPatch>.Instance;

            if (TickMethod == null)
                _logger.LogError("TickMethod is null");

            if (TickMethod == null)
            {
                return;
            }

            harmony.Patch(
                TickMethod,
                finalizer: new HarmonyMethod(TickFinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(TickMethod, TickFinalizerMethod);
        }

        private static readonly MethodInfo? TickMethod =
            Method(typeof(ScreenManager), "Tick");

        private static readonly MethodInfo? TickFinalizerMethod =
            Method(typeof(ScreenManagerPatch), nameof(TickFinalizer));

        public static void TickFinalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}