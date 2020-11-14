using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    public class CrashReport
    {
        public Exception Exception { get; set; }
        public List<ExtendedModuleInfo> LoadedModules { get; set; } = ModuleInfoHelper.GetExtendedLoadedModules();
        public List<Assembly> ModuleLoadedAssemblies { get; set; } = new List<Assembly>();
        public List<Assembly> ExternalLoadedAssemblies { get; set; } = new List<Assembly>();
        public Dictionary<MethodBase, HarmonyLib.Patches> LoadedHarmonyPatches { get; set; } = new Dictionary<MethodBase, HarmonyLib.Patches>();

        public CrashReport(Exception exception)
        {
            Exception = exception;

            var moduleAssemblies = new List<string>();
            foreach (var subModule in LoadedModules.SelectMany(module => module.SubModules))
            {
                moduleAssemblies.Add(Path.GetFileNameWithoutExtension(subModule.DLLName));
                moduleAssemblies.AddRange(subModule.Assemblies.Select(Path.GetFileNameWithoutExtension));
            }

            ModuleLoadedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a => moduleAssemblies.Contains(a.GetName().Name)));
            ExternalLoadedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a => !moduleAssemblies.Contains(a.GetName().Name)));

            foreach (var originalMethod in Harmony.GetAllPatchedMethods())
            {
                var patches = Harmony.GetPatchInfo(originalMethod);
                if (originalMethod is null || patches is null) continue;
                LoadedHarmonyPatches.Add(originalMethod, patches);
            }
        }
    }
}