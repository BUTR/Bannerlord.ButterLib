using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.Patches;

using HarmonyLib;

using System;
using System.Linq;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal static class ExceptionHandlerSubSystem
    {
        private static readonly Harmony Harmony = new Harmony("Bannerlord.ButterLib.ExceptionHandler");

        public static void Enable()
        {
            MissionPatch.Apply(Harmony);
            MissionViewPatch.Apply(Harmony);
            ModulePatch.Apply(Harmony);
            ScreenManagerPatch.Apply(Harmony);

            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                // Disable it's crash reporter?
            }
        }

        public static void Disable()
        {
            MissionPatch.Deapply(Harmony);
            MissionViewPatch.Deapply(Harmony);
            ModulePatch.Deapply(Harmony);
            ScreenManagerPatch.Deapply(Harmony);

            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                // Enable it's crash reporter?
            }
        }
    }
}