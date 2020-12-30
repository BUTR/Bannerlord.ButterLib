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
        public static string PathPrefix => Path.Combine(BasePath.Name, "Modules");

        public static string GetPath(string id) => Path.Combine(PathPrefix, id, "SubModule.xml");

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
        //public List<SubModuleInfo> SubModules { get; } = new();
        public List<DependedModule> DependedModules { get; } = new();

        public string Url { get; private set; } = string.Empty;

        public List<DependedModuleMetadata> DependedModuleMetadatas { get; }  = new();
        public List<ExtendedSubModuleInfo> ExtendedSubModules { get; } = new();

        public string XmlPath { get; private set; } = string.Empty;
        public string Folder { get; private set; } = string.Empty;

        public void Load(string alias)
        {
            Alias = alias;
            //SubModules.Clear();
            ExtendedSubModules.Clear();
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
            var dependedModulesList = dependedModules?.SelectNodes("DependedModule");
            for (var i = 0; i < dependedModulesList?.Count; i++)
            {
                if (dependedModulesList[i]?.Attributes["Id"] is { } idAttr)
                {
                    ApplicationVersionUtils.TryParse(dependedModulesList[i]?.Attributes?["DependentVersion"]?.InnerText, out var version);
                    DependedModules.Add(new DependedModule
                    {
                        ModuleId = idAttr.InnerText,
                        Version = version
                    });
                }
            }

            var subModules = moduleNode?.SelectSingleNode("SubModules");
            var subModuleList = subModules?.SelectNodes("SubModule");
            for (var i = 0; i < subModuleList?.Count; i++)
            {
                //var subModuleInfo = new SubModuleInfo();
                //try
                //{
                //    subModuleInfo.LoadFrom(subModuleList[i], Path.Combine(PathPrefix, alias));
                //    SubModules.Add(subModuleInfo);
                //}
                //catch { }

                var extendedSubModuleInfo = new ExtendedSubModuleInfo();
                try
                {
                    extendedSubModuleInfo.LoadFrom(subModuleList[i], Path.Combine(PathPrefix, alias));
                    ExtendedSubModules.Add(extendedSubModuleInfo);
                }
                catch { }
            }

            // Custom data
            Url = moduleNode?.SelectSingleNode("Url")?.Attributes?["value"]?.InnerText ?? string.Empty;

            var dependedModuleMetadatas = moduleNode?.SelectSingleNode("DependedModuleMetadatas");
            var dependedModuleMetadatasList = dependedModuleMetadatas?.SelectNodes("DependedModuleMetadata");
            for (var i = 0; i < dependedModuleMetadatasList?.Count; i++)
            {
                if (dependedModuleMetadatasList[i]?.Attributes["id"] is { } idAttr)
                {
                    var incompatible = dependedModuleMetadatasList[i]?.Attributes["incompatible"]?.InnerText.Equals("true") ?? false;
                    if (incompatible)
                    {
                        DependedModuleMetadatas.Add(new DependedModuleMetadata
                        {
                            Id = idAttr.InnerText,
                            LoadType = LoadType.NONE,
                            IsOptional = false,
                            IsIncompatible = incompatible,
                            Version = ApplicationVersion.Empty
                        });
                    }
                    else if (dependedModuleMetadatasList[i]?.Attributes["order"] is { } orderAttr && Enum.TryParse<LoadTypeParse>(orderAttr.InnerText, out var order))
                    {
                        var optional = dependedModuleMetadatasList[i]?.Attributes["optional"]?.InnerText.Equals("true") ?? false;
                        var version = ApplicationVersionUtils.TryParse(dependedModuleMetadatasList[i]?.Attributes["version"]?.InnerText, out var v) ? v : ApplicationVersion.Empty;
                        DependedModuleMetadatas.Add(new DependedModuleMetadata
                        {
                            Id = idAttr.InnerText,
                            LoadType = (LoadType) order,
                            IsOptional = optional,
                            IsIncompatible = incompatible,
                            Version = version
                        });
                    }
                }
            }

            // Fixed Launcher supported optional tag
            var loadAfterModules = moduleNode?.SelectSingleNode("LoadAfterModules");
            var loadAfterModuleList = loadAfterModules?.SelectNodes("LoadAfterModule");
            for (var i = 0; i < loadAfterModuleList?.Count; i++)
            {
                if (loadAfterModuleList[i]?.Attributes["Id"] is { } idAttr)
                {
                    DependedModuleMetadatas.Add(new DependedModuleMetadata
                    {
                        Id = idAttr.InnerText,
                        LoadType = LoadType.NONE,
                        IsOptional = true,
                        IsIncompatible = false,
                        Version = ApplicationVersion.Empty
                    });
                }
            }

            // Bannerlord Launcher supported optional tag
            var optionalDependModules = moduleNode?.SelectSingleNode("OptionalDependModules");
            var optionalDependModuleList =
                (dependedModules?.SelectNodes("OptionalDependModule")?.Cast<XmlNode>() ?? Enumerable.Empty<XmlNode>())
                .Concat(optionalDependModules?.SelectNodes("OptionalDependModule")?.Cast<XmlNode>() ?? Enumerable.Empty<XmlNode>())
                .Concat(optionalDependModules?.SelectNodes("DependModule")?.Cast<XmlNode>() ?? Enumerable.Empty<XmlNode>()).ToList();
            for (var i = 0; i < optionalDependModuleList.Count; i++)
            {
                if (optionalDependModuleList[i]?.Attributes["Id"] is { } idAttr)
                {
                    DependedModuleMetadatas.Add(new DependedModuleMetadata
                    {
                        Id = idAttr.InnerText,
                        LoadType = LoadType.NONE,
                        IsOptional = true,
                        IsIncompatible = false,
                        Version = ApplicationVersion.Empty
                    });
                }
            }

            XmlPath = GetPath(Alias);
            Folder = Path.GetDirectoryName(XmlPath)!;
        }

        public bool IsNative() => Id.Equals("native", StringComparison.OrdinalIgnoreCase);

        public override string ToString() => $"{Id} - {Version}";
    }
}