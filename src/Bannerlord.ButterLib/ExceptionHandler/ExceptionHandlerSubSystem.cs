using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.Patches;

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
    internal static class ExceptionHandlerSubSystem
    {
        private static readonly Harmony Harmony = new Harmony("Bannerlord.ButterLib.ExceptionHandler");

        public static void Enable()
        {
            MissionPatch.Enable(Harmony);
            MissionViewPatch.Enable(Harmony);
            ModulePatch.Enable(Harmony);
            ScreenManagerPatch.Enable(Harmony);

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

        public static void Disable()
        {
            MissionPatch.Disable(Harmony);
            MissionViewPatch.Disable(Harmony);
            ModulePatch.Disable(Harmony);
            ScreenManagerPatch.Disable(Harmony);

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