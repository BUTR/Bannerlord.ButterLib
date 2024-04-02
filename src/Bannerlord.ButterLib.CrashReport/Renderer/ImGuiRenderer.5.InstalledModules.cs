using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using HonkPerf.NET.RefLinq;

using ImGuiNET;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static readonly byte[][] _dependencyTypeNames =
    [
        [],
        "Load Before \0"u8.ToArray(),  // LoadBefore
        "Load After \0"u8.ToArray(),   // LoadAfter
        "Incompatible \0"u8.ToArray(), // Incompatible
    ];

    private readonly Dictionary<string, byte[]> _moduleIdUpdateInfoUtf8 = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Dictionary<string, byte[]>> _moduleDependencyTextUtf8 = new(StringComparer.Ordinal);

    private void InitializeInstalledModules()
    {
        for (var i = 0; i < _crashReport.Modules.Count; i++)
        {
            var module = _crashReport.Modules[i];

            if (module.UpdateInfo is not null)
            {
                _moduleIdUpdateInfoUtf8[module.Id] = UnsafeHelper.ToUtf8Array(module.UpdateInfo.ToString());
            }

            for (var j = 0; j < module.DependencyMetadatas.Count; j++)
            {
                var dependentModule = module.DependencyMetadatas[j];
                var optional = dependentModule.IsOptional ? " (optional)" : string.Empty;
                var version = !string.IsNullOrEmpty(dependentModule.Version) ? $" >= {dependentModule.Version}" : string.Empty;
                var versionRange = !string.IsNullOrEmpty(dependentModule.VersionRange) ? $" {dependentModule.VersionRange}" : string.Empty;

                var final = $"{optional}{version}{versionRange}";
                SetNestedDictionary(_moduleDependencyTextUtf8, module.Id, dependentModule.ModuleOrPluginId, UnsafeHelper.ToUtf8Array(final));
            }
        }
    }

    private void RenderDependencies(ModuleModel module)
    {
        if (module.DependencyMetadatas.Count == 0) return;

        _imgui.Text("Dependencies:\0"u8);
        for (var i = 0; i < module.DependencyMetadatas.Count; i++)
        {
            var dependentModule = module.DependencyMetadatas[i];
            var type = Clamp(dependentModule.Type, DependencyMetadataModelType.LoadBefore, DependencyMetadataModelType.Incompatible);
            _imgui.Bullet();
            _imgui.TextSameLine(_dependencyTypeNames[type]);
            _imgui.SmallButtonSameLine(dependentModule.ModuleOrPluginId);
            _imgui.Text(_moduleDependencyTextUtf8[module.Id][dependentModule.ModuleOrPluginId]);
        }
    }

    private void RenderCapabilities(IList<CapabilityModuleOrPluginModel> moduleCapabilities)
    {
        if (moduleCapabilities.Count == 0) return;

        _imgui.Text("Capabilities:\0"u8);
        for (var i = 0; i < moduleCapabilities.Count; i++)
        {
            var capability = moduleCapabilities[i];
            _imgui.Bullet();
            _imgui.Text(capability.Name);
        }
    }

    private void RenderSubModules(IList<ModuleSubModuleModel> moduleSubModules)
    {
        if (moduleSubModules.Count == 0) return;

        _imgui.Text("SubModules:\0"u8);
        for (var i = 0; i < moduleSubModules.Count; i++)
        {
            var subModule = moduleSubModules[i];
            _imgui.Bullet();
            if (_imgui.BeginChild(subModule.Name, in Zero2, in SubModule, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.TextSameLine("Name: \0"u8);
                _imgui.Text(subModule.Name);
                _imgui.TextSameLine("DLLName: \0"u8);
                _imgui.Text(subModule.AssemblyId?.Name ?? string.Empty);
                _imgui.TextSameLine("SubModuleClassType: \0"u8);
                _imgui.Text(subModule.Entrypoint);

                var firstTag = true;
                foreach (var tag in subModule.AdditionalMetadata.ToRefLinq().Where(x => !x.Key.StartsWith("METADATA:")))
                {
                    if (firstTag)
                    {
                        _imgui.Text("Tags:\0"u8);
                        firstTag = false;
                    }

                    _imgui.Bullet();
                    _imgui.TextSameLine(tag.Key);
                    _imgui.TextSameLine(": \0"u8);
                    _imgui.Text(tag.Value);
                }

                var firstAssembly = true;
                foreach (var assembly in subModule.AdditionalMetadata.ToRefLinq().Where(x => x.Key.StartsWith("METADATA:Assembly")))
                {
                    if (firstAssembly)
                    {
                        _imgui.Text("Assemblies:\0"u8);
                        firstAssembly = false;
                    }

                    _imgui.Bullet();
                    _imgui.Text(assembly.Value);
                }
            }

            _imgui.EndChild();
        }
    }

    private void RenderAdditionalAssemblies(string moduleId)
    {
        // TODO: Potentially a huge iteration count
        var first = true;
        for (var i = 0; i < _crashReport.Assemblies.Count; i++)
        {
            var assembly = _crashReport.Assemblies[i];
            if (assembly.ModuleId != moduleId) continue;

            if (first)
            {
                first = false;
                _imgui.Text("Assemblies Present:\0"u8);
            }

            _imgui.Bullet();
            _imgui.TextSameLine(assembly.Id.Name);
            _imgui.TextSameLine(" (\0"u8);
            _imgui.TextSameLine(_assemblyFullNameUtf8[assembly]);
            _imgui.Text(")\0"u8);
        }
    }

    private void RenderInstalledModules()
    {
        for (var i = 0; i < _crashReport.Modules.Count; i++)
        {
            var module = _crashReport.Modules[i];
            var isVortexManaged = module.AdditionalMetadata.ToRefLinq().Where(x => x.Key == "METADATA:MANAGED_BY_VORTEX").FirstOrDefault()?.Value is { } str && bool.TryParse(str, out var val) && val;

            var color = module switch
            {
                { IsOfficial: true } => OfficialModule,
                { IsExternal: true } => ExternalModule,
                _ => UnofficialModule,
            };

            if (_imgui.BeginChild(module.Id, in Zero2, in color, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                if (_imgui.TreeNode(module.Id))
                {
                    _imgui.RenderId("Id:\0"u8, module.Id);
                    _imgui.TextSameLine("Name: \0"u8);
                    _imgui.Text(module.Name);
                    _imgui.TextSameLine("Version: \0"u8);
                    _imgui.Text(module.Version);
                    _imgui.TextSameLine("External: \0"u8);
                    _imgui.Text(module.IsExternal);
                    _imgui.TextSameLine("Vortex: \0"u8);
                    _imgui.Text(isVortexManaged);
                    _imgui.TextSameLine("Official: \0"u8);
                    _imgui.Text(module.IsOfficial);
                    _imgui.TextSameLine("Singleplayer: \0"u8);
                    _imgui.Text(module.IsSingleplayer);
                    _imgui.TextSameLine("Multiplayer: \0"u8);
                    _imgui.Text(module.IsMultiplayer);

                    RenderDependencies(module);

                    RenderCapabilities(module.Capabilities);

                    if (module.Url is not null)
                    {
                        _imgui.TextSameLine("Url: \0"u8);
                        if (_imgui.SmallButton(module.Url))
                            Process.Start(new ProcessStartInfo(module.Url) { UseShellExecute = true, Verb = "open" });
                    }

                    if (module.UpdateInfo is not null)
                    {
                        _imgui.TextSameLine("Update Info: \0"u8);
                        _imgui.Text(_moduleIdUpdateInfoUtf8[module.Id]);
                    }

                    RenderSubModules(module.SubModules);

                    RenderAdditionalAssemblies(module.Id);

                    _imgui.TreePop();
                }
            }

            _imgui.EndChild();
        }
    }
}