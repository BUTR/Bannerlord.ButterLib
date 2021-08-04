using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper", true)]
    public static class ModuleInfoHelper
    {
        [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper.GetLoadedModules", true)]
        public static List<ExtendedModuleInfo> GetExtendedLoadedModules() => new();

        [Obsolete("Use Bannerlord.BUTR.Shared ModuleInfoHelper.GetModuleByType", true)]
        public static ExtendedModuleInfo? GetModuleInfo(Type type) => null;
    }
}