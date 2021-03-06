﻿using Bannerlord.ButterLib.Common.Helpers;
using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal class CrashReport
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Exception Exception { get; }
        public List<ExtendedModuleInfo> LoadedModules { get; } = BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().ToList();
        public List<Assembly> ModuleLoadedAssemblies { get; } = new();
        public List<Assembly> ExternalLoadedAssemblies { get; } = new();
        public Dictionary<MethodBase, Patches> LoadedHarmonyPatches { get; } = new();

        public CrashReport(Exception exception)
        {
            Exception = exception;

            var moduleAssemblies = new List<string>();
            foreach (var subModule in LoadedModules.SelectMany(module => module.ExtendedSubModules))
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