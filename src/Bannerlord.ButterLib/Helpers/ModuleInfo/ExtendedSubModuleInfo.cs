using Bannerlord.ButterLib.Common.Extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedSubModuleInfo
    {
        private delegate bool GetSubModuleValiditiyDelegate(Module instance, SubModuleInfo.SubModuleTags tag, string value);
        private static readonly GetSubModuleValiditiyDelegate? GetSubModuleValiditiy =
            AccessTools2.GetDelegate<GetSubModuleValiditiyDelegate>(typeof(Module), "GetSubModuleValiditiy");

        private static bool CheckIfSubmoduleCanBeLoadable(ExtendedSubModuleInfo subModuleInfo)
        {
            if (GetSubModuleValiditiy == null)
                return true;

            if (subModuleInfo.Tags.Count > 0)
            {
                foreach (var (key, value) in subModuleInfo.Tags)
                {
                    if (!GetSubModuleValiditiy(Module.CurrentModule, key, value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string Name { get; private set; } = string.Empty;
        public string DLLName { get; private set; } = string.Empty;
        //public bool IsVerifiedDLL { get; private set; }
        public bool DLLExists { get; private set; }
        public List<string> Assemblies { get; } = new();
        public string SubModuleClassType { get; private set; } = string.Empty;
        public Dictionary<SubModuleInfo.SubModuleTags, string> Tags { get; } = new();

        public MBSubModuleBase? SubModuleInstance { get; private set; }
        public bool IsLoadable { get; private set; }

        public void LoadFrom(XmlNode subModuleNode, string path)
        {
            Assemblies.Clear();
            Tags.Clear();

            Name = subModuleNode?.SelectSingleNode("Name")?.Attributes?["value"]?.InnerText ?? string.Empty;
            DLLName = subModuleNode?.SelectSingleNode("DLLName")?.Attributes?["value"]?.InnerText ?? string.Empty;

            if (!string.IsNullOrEmpty(DLLName))
            {
                DLLExists = File.Exists(Path.Combine(path, "bin\\Win64_Shipping_Client", DLLName));
                //IsVerifiedDLL = (DLLExists && this.GetIsTWCertified(text));
            }

            SubModuleClassType = subModuleNode?.SelectSingleNode("SubModuleClassType")?.Attributes?["value"]?.InnerText ?? string.Empty;

            var assemblies = subModuleNode?.SelectSingleNode("Assemblies")?.SelectNodes("Assembly");
            for (var i = 0; i < assemblies?.Count; i++)
            {
                Assemblies.Add(assemblies[i].Attributes["value"].InnerText);
            }

            var tags = subModuleNode?.SelectSingleNode("Tags")?.SelectNodes("Tag");
            for (var j = 0; j < tags?.Count; j++)
            {
                if (Enum.TryParse<SubModuleInfo.SubModuleTags>(tags[j].Attributes["key"].InnerText, out var subModuleTags))
                {
                    var innerText = tags[j].Attributes["value"].InnerText;
                    Tags.Add(subModuleTags, innerText);
                    //if (subModuleTags == SubModuleInfo.SubModuleTags.DedicatedServerType && innerText != "none")
                    //    IsVerifiedDLL = true;
                }
            }

            SubModuleInstance = Module.CurrentModule.SubModules.FirstOrDefault(s => string.Equals(s.GetType().FullName, SubModuleClassType, StringComparison.Ordinal));
            IsLoadable = CheckIfSubmoduleCanBeLoadable(this);
        }

        public override string ToString() => $"{Name} - {DLLName}";
    }
}