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
    /// <summary>
    /// Entrypoint into C# engine part
    /// </summary>
    internal static class ScreenManagerPatch
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
                PreTickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod));
            harmony.Patch(
                TickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
            harmony.Patch(
                LateTickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod));
            harmony.Patch(
                UpdateMethod,
                finalizer: new HarmonyMethod(FinalizerMethod));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(PreTickMethod, FinalizerMethod);
            harmony.Unpatch(TickMethod, FinalizerMethod);
            harmony.Unpatch(LateTickMethod, FinalizerMethod);
            harmony.Unpatch(UpdateMethod, FinalizerMethod);
        }

        private static readonly MethodInfo? PreTickMethod = Method(typeof(ScreenManager), "PreTick");
        private static readonly MethodInfo? TickMethod = Method(typeof(ScreenManager), "Tick");
        private static readonly MethodInfo? LateTickMethod = Method(typeof(ScreenManager), "LateTick");
        private static readonly MethodInfo? UpdateMethod = Method(typeof(ScreenManager), "Update");

        private static readonly MethodInfo? FinalizerMethod = Method(typeof(ScreenManagerPatch), nameof(Finalizer));

        public static void Finalizer(Exception? __exception)
        {
            if (__exception is not null)
            {
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
    }
}