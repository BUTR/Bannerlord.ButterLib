using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public static class ModuleInfoHelper
    {
        private static List<ExtendedModuleInfo>? LoadedModules { get; set; }

        // We can cache it because it is not expected for the game to change Module\Module order
        public static List<ModuleInfo> GetLoadedModules() => (LoadedModules ??= GetLoadedModulesEnumerable().ToList()).Cast<ModuleInfo>().ToList();
        public static List<ExtendedModuleInfo> GetExtendedLoadedModules() => LoadedModules ??= GetLoadedModulesEnumerable().ToList();

        private static IEnumerable<ExtendedModuleInfo> GetLoadedModulesEnumerable()
        {
            var modulesNames = Utilities.GetModulesNames();
            for (var i = 0; i < modulesNames.Length; i++)
            {
                var moduleInfo = new ExtendedModuleInfo();
                moduleInfo.Load(modulesNames[i]);
                yield return moduleInfo;
            }
        }

        private static string GetFullPathWithEndingSlashes(string input) =>
            $"{Path.GetFullPath(input).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)}{Path.DirectorySeparatorChar}";

        private static string GetPath(Assembly assembly)
        {
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return path;
        }

        public static ModuleInfo? GetModuleInfo(Type type)
        {
            if (!typeof(MBSubModuleBase).IsAssignableFrom(type))
                return null;

            var assemblyPath = GetPath(type.Assembly);

            var fullAssemblyPath= Path.GetFullPath(assemblyPath);
            foreach (var loadedModule in GetLoadedModules())
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