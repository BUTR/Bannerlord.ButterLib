using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using ModuleInfoHelper = Bannerlord.BUTR.Shared.Helpers.ModuleInfoHelper;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal class CrashReport
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Exception Exception { get; }
        public List<(MethodBase Method, ExtendedModuleInfo ModuleInfo, string StackFrameDescription)> InvolvedModules { get; }
        public List<ExtendedModuleInfo> LoadedModules { get; } = ModuleInfoHelper.GetLoadedModules().ToList();
        public List<Assembly> ModuleLoadedAssemblies { get; } = new();
        public List<Assembly> ExternalLoadedAssemblies { get; } = new();
        public Dictionary<MethodBase, Patches> LoadedHarmonyPatches { get; } = new();

        public CrashReport(Exception exception)
        {
            Exception = exception;

            InvolvedModules = GetAllInvolvedModules(exception, 0).ToList();

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

        private static IEnumerable<(MethodBase, ExtendedModuleInfo, string)> GetAllInvolvedModules(Exception ex, int level)
        {
            static Patches? FindPatches(MethodBase method) => method is MethodInfo replacement
                ? Harmony.GetOriginalMethod(replacement) is { } original ? Harmony.GetPatchInfo(original) : null
                : null;

            static IEnumerable<(MethodBase, ExtendedModuleInfo)> GetPrefixes(Patches? info) => info is null
                ? Enumerable.Empty<(MethodBase, ExtendedModuleInfo)>()
                : AddMetadata(info.Prefixes.OrderBy(t => t.priority).Select(t => t.PatchMethod));

            static IEnumerable<(MethodBase, ExtendedModuleInfo)> GetPostfixes(Patches? info) => info is null
                ? Enumerable.Empty<(MethodBase, ExtendedModuleInfo)>()
                : AddMetadata(info.Postfixes.OrderBy(t => t.priority).Select(t => t.PatchMethod));

            static IEnumerable<(MethodBase, ExtendedModuleInfo)> GetTranspilers(Patches? info) => info is null
                ? Enumerable.Empty<(MethodBase, ExtendedModuleInfo)>()
                : AddMetadata(info.Transpilers.OrderBy(t => t.priority).Select(t => t.PatchMethod));

            static IEnumerable<(MethodBase, ExtendedModuleInfo)> GetFinalizers(Patches? info) => info is null
                ? Enumerable.Empty<(MethodBase, ExtendedModuleInfo)>()
                : AddMetadata(info.Finalizers.OrderBy(t => t.priority).Select(t => t.PatchMethod));

            static IEnumerable<(MethodBase, ExtendedModuleInfo)> AddMetadata(IEnumerable<MethodInfo> methods)
            {
                foreach (var method in methods)
                {
                    if (method.DeclaringType is { } declaringType && ModuleInfoHelper.GetModuleByType(declaringType) is { } moduleInfo)
                    {
                        yield return (method, moduleInfo);
                    }
                }
            }

            static ExtendedModuleInfo? GetModuleInfoIfMod(MethodBase? method) => method?.DeclaringType is not null
                ? ModuleInfoHelper.GetModuleByType(method.DeclaringType)
                : null;


            var inner = ex.InnerException;
            if (inner is not null)
            {
                foreach (var modInfo in GetAllInvolvedModules(inner, level + 1))
                {
                    yield return modInfo;
                }
            }

            var trace = new StackTrace(ex, 0, true);
            foreach (var frame in trace.GetFrames() ?? Array.Empty<StackFrame>())
            {
                var method = Harmony.GetMethodFromStackframe(frame);
                var patches = FindPatches(method);

                foreach (var (methodBase, extendedModuleInfo) in GetFinalizers(patches))
                {
                    yield return (methodBase, extendedModuleInfo, frame.ToString() ?? string.Empty);
                }
                foreach (var (methodBase, extendedModuleInfo) in GetPostfixes(patches))
                {
                    yield return (methodBase, extendedModuleInfo, frame.ToString() ?? string.Empty);
                }
                foreach (var (methodBase, extendedModuleInfo) in GetPrefixes(patches))
                {
                    yield return (methodBase, extendedModuleInfo, frame.ToString() ?? string.Empty);
                }
                foreach (var (methodBase, extendedModuleInfo) in GetTranspilers(patches))
                {
                    yield return (methodBase, extendedModuleInfo, frame.ToString() ?? string.Empty);
                }

                var moduleInfo = GetModuleInfoIfMod(method);
                if (moduleInfo is not null)
                    yield return (method, moduleInfo, frame.ToString() ?? string.Empty);
            }
        }
    }
}