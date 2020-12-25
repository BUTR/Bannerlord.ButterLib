using Bannerlord.ButterLib.Helpers.ModuleInfo;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    public sealed class ExtendedModuleInfo
    {
        private static string PathPrefix => BasePath.Name + "Modules/";

        public static string GetPath(string id) => $"{PathPrefix}{id}/SubModule.xml";

        private delegate IEnumerable<string> GetModulePathsDelegate(string directoryPath, int searchDepth);
        private static readonly GetModulePathsDelegate? GetModulePaths = AccessTools2.GetDelegate<GetModulePathsDelegate>(typeof(ModuleInfo), "GetModulePaths");

        public static IEnumerable<ExtendedModuleInfo> GetExtendedModules()
        {
            if (GetModulePaths is null)
                yield break;

            foreach (var path in GetModulePaths(System.IO.Path.Combine(BasePath.Name, "Modules"), 1).ToArray())
            {
                var moduleInfo = new ExtendedModuleInfo();
                try { moduleInfo.Load(System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path))); }
                catch (Exception) { continue; }

                yield return moduleInfo;
            }
        }


        public string Id { get; private set; } = string.Empty;
		public string Name { get; private set; } = string.Empty;
		public bool IsOfficial { get; private set; }
        public ApplicationVersion Version { get; private set; }
        public string Alias { get; private set; } = string.Empty;
        public bool IsSingleplayerModule { get; private set; }
        public bool IsMultiplayerModule { get; private set; }
        public bool IsSelected { get; set; }
        public List<SubModuleInfo> SubModules { get; } = new();
        public List<DependedModule> DependedModules { get; } = new();


        public string Url { get; private set; } = string.Empty;
        public List<ExtendedSubModuleInfo> ExtendedSubModules { get; } = new();
        public List<DependedModuleMetadata> DependedModuleMetadatas { get; }  = new();
        public string XmlPath { get; private set; } = string.Empty;
        public string Folder { get; private set; } = string.Empty;

        public void Load(string alias)
        {
			Alias = alias;
			SubModules.Clear();
			DependedModules.Clear();
            DependedModuleMetadatas.Clear();

            var xmlDocument = new XmlDocument();
			xmlDocument.Load(ModuleInfo.GetPath(alias));

			var moduleNode = xmlDocument.SelectSingleNode("Module");

			Name = moduleNode?.SelectSingleNode("Name")?.Attributes?["value"]?.InnerText ?? string.Empty;
			Id = moduleNode?.SelectSingleNode("Id")?.Attributes?["value"]?.InnerText ?? string.Empty;
            ApplicationVersionUtils.TryParse(moduleNode?.SelectSingleNode("Version")?.Attributes?["value"]?.InnerText, out var parsedVersion);
            Version = parsedVersion;

			IsOfficial = moduleNode?.SelectSingleNode("Official")?.Attributes?["value"]?.InnerText?.Equals("true") == true;
            IsSelected = moduleNode?.SelectSingleNode("DefaultModule")?.Attributes?["value"]?.InnerText?.Equals("true") == true || IsNative();
            IsSingleplayerModule = moduleNode?.SelectSingleNode("SingleplayerModule")?.Attributes?["value"]?.InnerText?.Equals("true") == true;
            IsMultiplayerModule = moduleNode?.SelectSingleNode("MultiplayerModule")?.Attributes?["value"]?.InnerText?.Equals("true") == true;

            var dependedModules = moduleNode?.SelectSingleNode("DependedModules");
            foreach (var xmlElement in dependedModules?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>())
			{
				var innerText = xmlElement?.Attributes?["Id"]?.InnerText ?? string.Empty;
                ApplicationVersionUtils.TryParse(xmlElement?.Attributes?["DependentVersion"]?.InnerText, out var version);
                DependedModules.Add(new DependedModule(innerText, version));
			}

            var subModules = moduleNode?.SelectSingleNode("SubModules")?.SelectNodes("SubModule");
            for (var i = 0; i < subModules?.Count; i++)
            {
                var subModuleInfo = new SubModuleInfo();
                try
                {
                    subModuleInfo.LoadFrom(subModules[i], PathPrefix + alias);
                    SubModules.Add(subModuleInfo);
                }
                catch { }
            }

            Url = moduleNode?.SelectSingleNode("Url")?.Attributes?["value"]?.InnerText ?? string.Empty;

            var dependedModuleMetadatas = moduleNode?.SelectSingleNode("DependedModuleMetadatas");
            foreach (var xmlElement in dependedModuleMetadatas?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>())
            {
                if (xmlElement.Attributes["id"] is { } idAttr && xmlElement.Attributes["order"] is { } orderAttr && Enum.TryParse<LoadType>(orderAttr.InnerText, out var order))
                {
                    var optional = xmlElement.Attributes["optional"]?.InnerText.Equals("true") ?? false;
                    var version = ApplicationVersionUtils.TryParse(xmlElement.Attributes["version"]?.InnerText, out var v) ? v : ApplicationVersion.Empty;
                    DependedModuleMetadatas.Add(new DependedModuleMetadata(idAttr.InnerText, order, optional, version));
                }
            }

            XmlPath = GetPath(Alias);
            Folder = Path.GetDirectoryName(XmlPath)!;
        }

        public bool IsNative() => Id.Equals("native", StringComparison.OrdinalIgnoreCase);

        public override string ToString() => $"{Id} - {Version}";
    }
}