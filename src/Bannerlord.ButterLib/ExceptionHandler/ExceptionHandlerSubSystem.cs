using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public string Id => "ExceptionHandler";
        public string Description => "Captures game crashes and creates reports out of them.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => true;

        private readonly Harmony _harmony = new Harmony("Bannerlord.ButterLib.ExceptionHandler");

        public ExceptionHandlerSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            DotNetManagedPatch.Enable(_harmony);
            MissionPatch.Enable(_harmony);
            MissionViewPatch.Enable(_harmony);
            ModulePatch.Enable(_harmony);
            ScreenManagerPatch.Enable(_harmony);
            ScriptComponentBehaviourPatch.Enable(_harmony);

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

            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {

            }

            /*
            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                var betterExceptionWindowModulePath = Path.Combine(Utilities.GetBasePath(), "Modules", "BetterExceptionWindow");
                var configPath = Path.Combine(betterExceptionWindowModulePath, "config.json");
                if (!File.Exists(configPath))
                    return;

                File.Copy(configPath, $"{configPath}.bl.bak", true);
                var configFile = File.ReadAllText(configPath);
                if (JsonConvert.DeserializeObject(configFile) is JObject config)
                {
                    config["CatchOnApplicationTick"] = false;
                    config["CatchOnMissionScreenTick"] = false;
                    config["CatchOnFrameTick"] = false;
                    config["CatchTick"] = false;

                    configFile = JsonConvert.SerializeObject(config);
                    File.WriteAllText(configPath, configFile);

                    ReloadBetterExceptionWindow();
                }
            }
            */
        }

        public void Disable()
        {
            IsEnabled = false;

            DotNetManagedPatch.Disable(_harmony);
            MissionPatch.Disable(_harmony);
            MissionViewPatch.Disable(_harmony);
            ModulePatch.Disable(_harmony);
            ScreenManagerPatch.Disable(_harmony);
            ScriptComponentBehaviourPatch.Disable(_harmony);

            /*
            if (ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                var betterExceptionWindowModulePath = Path.Combine(Utilities.GetBasePath(), "Modules", "BetterExceptionWindow");
                var configPath = Path.Combine(betterExceptionWindowModulePath, "config.json");

                if (!File.Exists($"{configPath}.bl.bak"))
                    return;

                File.Copy($"{configPath}.bl.bak", configPath, true);

                ReloadBetterExceptionWindow();
            }
            */
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