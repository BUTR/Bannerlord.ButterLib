using System.Collections.Generic;
using System.Xml;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedModuleInfo : ModuleInfo
    {
        public string? Url { get; private set; }

        public List<ExtendedSubModuleInfo> ExtendedSubModules { get; } = new List<ExtendedSubModuleInfo>();

        public new void Load(string alias)
        {
            base.Load(alias);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(GetPath(alias));
            var xmlNodeModule = xmlDocument.SelectSingleNode("Module");
            Url = xmlNodeModule?.SelectSingleNode("Url")?.Attributes?["value"]?.InnerText;
            var xmlNodeSubModule = xmlNodeModule?.SelectNodes("SubModule");
            for (var i = 0; i < xmlNodeSubModule?.Count; i++)
            {
                var subModuleInfo = new ExtendedSubModuleInfo();
                subModuleInfo.LoadFrom(xmlNodeSubModule[i], System.IO.Path.Combine(Utilities.GetBasePath(), "Modules", alias));
                ExtendedSubModules.Add(subModuleInfo);
            }
        }
    }
}