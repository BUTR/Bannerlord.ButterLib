using Bannerlord.ButterLib.Helpers.ModuleInfo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using TaleWorlds.Engine;
using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedModuleInfo : ModuleInfo
    {
        public string Url { get; private set; } = string.Empty;
        public List<ExtendedSubModuleInfo> ExtendedSubModules { get; } = new List<ExtendedSubModuleInfo>();
        public string XmlPath { get; private set; } = string.Empty;
        public string Folder { get; private set; } = string.Empty;

        public readonly List<DependedModuleMetadata> DependedModuleMetadatas = new List<DependedModuleMetadata>();

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
            foreach (var xmlElement in xmlNodeSubModules?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>())
            {
                var subModuleInfo = new ExtendedSubModuleInfo();
                subModuleInfo.LoadFrom(xmlElement, System.IO.Path.Combine(Utilities.GetBasePath(), "Modules", alias));
                ExtendedSubModules.Add(subModuleInfo);
            }

            XmlPath = GetPath(Alias);
            Folder = System.IO.Path.GetDirectoryName(XmlPath)!;

            DependedModuleMetadatas.Clear();
            var xmlNodeDependedModuleMetadatas = xmlNodeModule?.SelectSingleNode("DependedModuleMetadatas");
            foreach (var xmlElement in xmlNodeDependedModuleMetadatas?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>())
            {
                if (xmlElement.Attributes["id"] is { } idAttr && xmlElement.Attributes["order"] is { } orderAttr && Enum.TryParse<LoadType>(orderAttr.InnerText, out var order))
                {
                    var optional = xmlElement.Attributes["optional"]?.InnerText.Equals("true") ?? false;
                    DependedModuleMetadatas.Add(new DependedModuleMetadata(idAttr.InnerText, order, optional));
                }
            }
        }

        public override string ToString() => $"{Id} - {Version}";
    }
}