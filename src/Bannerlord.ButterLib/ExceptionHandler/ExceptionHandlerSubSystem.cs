using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class ExceptionHandlerSubSystem : ISubSystem
    {
        public static ExceptionHandlerSubSystem? Instance { get; private set; }

        internal static readonly Harmony Harmony = new Harmony("Bannerlord.ButterLib.ExceptionHandler");

        public string Id => "ExceptionHandler";
        public string Description => "Captures game crashes and creates reports out of them.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => true;


        public ExceptionHandlerSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            EngineCallbackPatches.Enable(Harmony);
            LibraryCallbackPatches.Enable(Harmony);
            MBCallbackPatches.Enable(Harmony);

            //MissionPatch.Enable(Harmony);
            //MissionViewPatch.Enable(Harmony);
            //ModulePatch.Enable(Harmony);

            // re-enable BetterExceptionWindow and keep it as it is. It has now features we have yet to match.
            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                var betterExceptionWindowModulePath = Path.Combine(Utilities.GetBasePath(), "Modules", "BetterExceptionWindow");
                var configPath = Path.Combine(betterExceptionWindowModulePath, "config.json");

                if (!File.Exists($"{configPath}.bl.bak"))
                    return;

                File.Copy($"{configPath}.bl.bak", configPath, true);

                ReloadBetterExceptionWindow();
            }
        }

        public void Disable()
        {
            IsEnabled = false;

            EngineCallbackPatches.Disable(Harmony);
            LibraryCallbackPatches.Disable(Harmony);
            MBCallbackPatches.Disable(Harmony);

            //MissionPatch.Disable(Harmony);
            //MissionViewPatch.Disable(Harmony);
            //ModulePatch.Disable(Harmony);
        }


        private static Assembly? GetBetterExceptionWindowAssembly() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .FirstOrDefault(a => string.Equals(Path.GetFileNameWithoutExtension(a.Location), "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase));

        private static void ReloadBetterExceptionWindow()
        {
            var assembly = GetBetterExceptionWindowAssembly();
            var utils = assembly?.GetTypes().FirstOrDefault(t => string.Equals(t.FullName, "BetterExceptionWindow.Util", StringComparison.InvariantCultureIgnoreCase));
            var reloadMethod = AccessTools.Method(utils, "ReadConfig");
            reloadMethod?.Invoke(null, Array.Empty<object>());
        }
    }
}