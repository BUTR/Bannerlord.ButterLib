using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ModuleManager;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Helpers.ModuleInfo
{
    public static class ModuleSorter
    {
        public static IList<ModuleInfoExtended> Sort(ICollection<ModuleInfoExtended> unorderedModules) =>
            MBMath.TopologySort(unorderedModules, module => GetDependentModulesOf(unorderedModules, module));

        private static IEnumerable<ModuleInfoExtended> GetDependentModulesOf(ICollection<ModuleInfoExtended> source, ModuleInfoExtended module)
        {
            foreach (var dependedModule in module.DependentModules)
            {
                if (source.FirstOrDefault(i => i.Id == dependedModule.Id) is { } moduleInfo)
                {
                    yield return moduleInfo;
                }
            }

            foreach (var dependedModuleMetadata in module.DependentModuleMetadatas)
            {
                if (source.FirstOrDefault(i => i.Id == dependedModuleMetadata.Id) is { } moduleInfo && dependedModuleMetadata.LoadType == LoadType.LoadBeforeThis)
                {
                    yield return moduleInfo;
                }
            }

            foreach (var moduleInfo in source)
            {
                foreach (var dependedModuleMetadata in moduleInfo.DependentModuleMetadatas)
                {
                    if (dependedModuleMetadata.Id == module.Id && dependedModuleMetadata.LoadType == LoadType.LoadAfterThis)
                    {
                        yield return moduleInfo;
                    }
                }
            }
        }
    }
}