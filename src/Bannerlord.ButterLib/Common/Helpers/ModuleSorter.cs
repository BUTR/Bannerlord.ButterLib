using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public static class ModuleSorter
    {
        public static IList<ExtendedModuleInfo> Sort(ICollection<ExtendedModuleInfo> unorderedModules) =>
            MBMath.TopologySort(unorderedModules, module => GetDependentModulesOf(unorderedModules, module));

        private static IEnumerable<ExtendedModuleInfo> GetDependentModulesOf(ICollection<ExtendedModuleInfo> source, ExtendedModuleInfo module)
        {
            foreach (var dependedModuleId in module.DependedModuleIds)
            {
                if (source.FirstOrDefault(i => i.Id == dependedModuleId) is { } moduleInfo)
                {
                    yield return moduleInfo;
                }
            }

            foreach (var dependedModuleMetadata in module.DependedModuleMetadatas)
            {
                if (source.FirstOrDefault(i => i.Id == dependedModuleMetadata.Id) is { } moduleInfo && dependedModuleMetadata.LoadType == LoadType.LoadBeforeThis)
                {
                    yield return moduleInfo;
                }
            }

            foreach (var moduleInfo in source)
            {
                foreach (var dependedModuleMetadata in moduleInfo.DependedModuleMetadatas)
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