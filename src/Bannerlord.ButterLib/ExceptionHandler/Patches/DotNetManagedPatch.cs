using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.DotNet;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    /// <summary>
    /// DotNet.Managed if one of the highest entrypoint into the C# engine part.
    /// We should check if we should handle more exception catches
    /// </summary>
    internal sealed class DotNetManagedPatch
    {
        private static ILogger _logger = default!;

        internal static void Enable(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<DotNetManagedPatch>>() ??
                      NullLogger<DotNetManagedPatch>.Instance;

            if (ApplicationTickMethod == null)
                _logger.LogError("TickMethod is null");

            if (ApplicationTickMethod == null)
            {
                return;
            }

            harmony.Patch(
                ApplicationTickMethod,
                finalizer: new HarmonyMethod(FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(ApplicationTickMethod, FinalizerMethod);
        }

        private static readonly MethodInfo? ApplicationTickMethod = Method(typeof(Managed), "ApplicationTick");

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