using System.Collections.Generic;
using System.Linq;
using System.Xml;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedModuleInfo : ModuleInfo
    {
        public string Url { get; private set; } = string.Empty;
        public List<ExtendedSubModuleInfo> ExtendedSubModules { get; } = new List<ExtendedSubModuleInfo>();

        public new void Load(string alias)
        {
            base.Load(alias);

            Url = string.Empty;
            ExtendedSubModules.Clear();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(GetPath(alias));

            var xmlNodeModule = xmlDocument.SelectSingleNode("Module");

            Url = xmlNodeModule?.SelectSingleNode("Url")?.Attributes?["value"]?.InnerText ?? string.Empty;

            var xmlNodeSubModules = xmlNodeModule?.SelectSingleNode("SubModules");
            foreach (var xmlNodeSubModule in xmlNodeSubModules?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>())
            {
                var subModuleInfo = new ExtendedSubModuleInfo();
                subModuleInfo.LoadFrom(xmlNodeSubModule, System.IO.Path.Combine(Utilities.GetBasePath(), "Modules", alias));
                ExtendedSubModules.Add(subModuleInfo);
            }
        }
    }
}