using System;
using System.Linq;
using System.Xml;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedSubModuleInfo : SubModuleInfo
    {
        public MBSubModuleBase? SubModuleInstance { get; private set; }
        public bool IsLoadable { get; set; }

        public new void LoadFrom(XmlNode subModuleNode, string path)
        {
            base.LoadFrom(subModuleNode, path);

            SubModuleInstance = Module.CurrentModule.SubModules.FirstOrDefault(s => string.Equals(s.GetType().FullName, SubModuleClassType, StringComparison.Ordinal));
            IsLoadable = Module.CurrentModule.CheckIfSubmoduleCanBeLoadable(this);
        }
    }
}