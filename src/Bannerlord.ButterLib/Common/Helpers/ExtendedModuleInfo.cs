using System.Xml;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedModuleInfo : ModuleInfo
    {
        public string? Url { get; private set; }

        public new void Load(string alias)
        {
            base.Load(alias);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(GetPath(alias));
            var xmlNode = xmlDocument.SelectSingleNode("Module");
            Url = xmlNode?.SelectSingleNode("Url")?.Attributes?["value"]?.InnerText;
        }
    }
}