using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper", true)]
    public static class ModuleInfoHelper
    {
        internal static bool PastInitialization = false;
        internal static List<ExtendedModuleInfo>? LoadedExtendedModules { get; set; }

        // We can cache it because it is not expected for the game to change Module\Module order
        //public static List<ModuleInfo> GetLoadedModules() => PastInitialization ? LoadedModules ??= GetLoadedModulesEnumerable().ToList() : GetLoadedModulesEnumerable().ToList();
        [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper.GetLoadedModules", true)]
        public static List<ExtendedModuleInfo> GetExtendedLoadedModules() => PastInitialization ? LoadedExtendedModules ??= GetExtendedLoadedModulesEnumerable().ToList() : GetExtendedLoadedModulesEnumerable().ToList();

        internal static IEnumerable<ExtendedModuleInfo> GetExtendedLoadedModulesEnumerable()
        {
            var modulesNames = Utilities.GetModulesNames();
            for (var i = 0; i < modulesNames?.Length; i++)
            {
                var moduleInfo = new ExtendedModuleInfo();
                moduleInfo.Load(modulesNames[i]);
                yield return moduleInfo;
            }
        }

        private static string GetFullPathWithEndingSlashes(string input) =>
            $"{Path.GetFullPath(input).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)}{Path.DirectorySeparatorChar}";

        [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper.GetModuleByType", true)]
        public static ExtendedModuleInfo? GetModuleInfo(Type type)
        {
            if (!typeof(MBSubModuleBase).IsAssignableFrom(type) || string.IsNullOrWhiteSpace(type.Assembly.Location))
                return null;

            var fullAssemblyPath= Path.GetFullPath(type.Assembly.Location);
            foreach (var loadedModule in GetExtendedLoadedModules())
            {
                var loadedModuleDirectory = Path.GetFullPath(Path.Combine(Utilities.GetBasePath(), "Modules", loadedModule.Id));
                var relativePath = new Uri(GetFullPathWithEndingSlashes(loadedModuleDirectory)).MakeRelativeUri(new Uri(fullAssemblyPath));
                if (!relativePath.OriginalString.StartsWith("../"))
                    return loadedModule;
            }

            return null;
        }
    }
}