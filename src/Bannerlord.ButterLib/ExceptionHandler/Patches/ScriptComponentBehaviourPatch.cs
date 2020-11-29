using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.Engine;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    /// <summary>
    /// Entrypoint into C# engine part
    /// </summary>
    internal sealed class ScriptComponentBehaviourPatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<ScriptComponentBehaviourPatch>>() ??
                      NullLogger<ScriptComponentBehaviourPatch>.Instance;

            if (OnTickMethod == null)
                _logger.LogError("TickMethod is null");

            if (OnTickMethod == null)
            {
                return;
            }

            harmony.Patch(
                OnTickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(OnTickMethod, FinalizerMethod);
        }

        private static readonly MethodInfo? OnTickMethod = Method(typeof(ScriptComponentBehaviour), "OnTick");

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