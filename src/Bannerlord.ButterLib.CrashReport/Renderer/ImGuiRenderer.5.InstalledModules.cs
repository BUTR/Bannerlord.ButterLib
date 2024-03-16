using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using HonkPerf.NET.RefLinq;

using ImGuiNET;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static readonly byte[][] _dependencyTypeNames =
    [
        "Load Before \0"u8.ToArray(),  // LoadBefore
        "Load After \0"u8.ToArray(),   // LoadAfter
        "Incompatible \0"u8.ToArray(), // Incompatible
    ];

    private static readonly byte[][] _capabilityNames = Enum.GetValues(typeof(ModuleCapabilities))
        .Cast<ModuleCapabilities>()
        .Select(x => UnsafeHelper.ToUtf8Array(GetEnumDescription(x)))
        .ToArray();

    private FrozenDictionary<string, byte[]> _moduleIdUpdateInfoUtf8 = default!;
    private FrozenDictionary<string, ModuleCapabilities[]> _moduleIdCapabilities = default!;
    private FrozenDictionary<string, Dictionary<string, byte[]>> _moduleDependencyTextUtf8 = default!;

    private void InitializeInstalledModules()
    {
        var moduleIdUpdateInfoUtf8 = new Dictionary<string, byte[]>();
        var moduleIdCapabilities = new Dictionary<string, ModuleCapabilities[]>();
        var moduleDependencyTextUtf8 = new Dictionary<string, Dictionary<string, byte[]>>();
        for (var i = 0; i < _crashReport.Modules.Count; i++)
        {
            var module = _crashReport.Modules[i];

            if (module.UpdateInfo is not null)
            {
                moduleIdUpdateInfoUtf8[module.Id] = UnsafeHelper.ToUtf8Array(module.UpdateInfo.ToString());
            }

            moduleIdCapabilities[module.Id] = CrashReportShared.GetModuleCapabilities(_crashReport, module).ToArray();

            for (var j = 0; j < module.DependencyMetadatas.Count; j++)
            {
                var dependentModule = module.DependencyMetadatas[j];
                var optional = dependentModule.IsOptional ? " (optional)" : string.Empty;
                var version = !string.IsNullOrEmpty(dependentModule.Version) ? $" >= {dependentModule.Version}" : string.Empty;
                var versionRange = !string.IsNullOrEmpty(dependentModule.VersionRange) ? $" {dependentModule.VersionRange}" : string.Empty;

                var final = $"{optional}{version}{versionRange}";
                SetNestedDictionary(moduleDependencyTextUtf8, module.Id, dependentModule.ModuleOrPluginId, UnsafeHelper.ToUtf8Array(final));
            }
        }
        _moduleIdUpdateInfoUtf8 = moduleIdUpdateInfoUtf8.ToFrozenDictionary(StringComparer.Ordinal);
        _moduleIdCapabilities = moduleIdCapabilities.ToFrozenDictionary(StringComparer.Ordinal);
        _moduleDependencyTextUtf8 = moduleDependencyTextUtf8.ToFrozenDictionary(StringComparer.Ordinal);
    }

    private void RenderDependencies(ModuleModel module)
    {
        if (module.DependencyMetadatas.Count == 0) return;

        JmGui.Text("Dependencies:\0"u8);
        for (var i = 0; i < module.DependencyMetadatas.Count; i++)
        {
            var dependentModule = module.DependencyMetadatas[i];
            var type = Clamp(dependentModule.Type, DependencyMetadataModelType.LoadBefore, DependencyMetadataModelType.Incompatible);
            ImGui.Bullet();
            JmGui.TextSameLine(_dependencyTypeNames[type]);
            JmGui.SmallButtonSameLine(dependentModule.ModuleOrPluginId);
            JmGui.Text(_moduleDependencyTextUtf8[module.Id][dependentModule.ModuleOrPluginId]);
        }
    }

    private void RenderCapabilitiesDependencies(IList<ModuleCapabilities> getModuleCapabilities)
    {
        if (getModuleCapabilities.Count == 0) return;

        JmGui.Text("Capabilities:\0"u8);
        for (var i = 0; i < getModuleCapabilities.Count; i++)
        {
            var capability = getModuleCapabilities[i];
            var type = Clamp(capability, ModuleCapabilities.None, ModuleCapabilities.Cultures);
            ImGui.Bullet();
            JmGui.Text(_capabilityNames[type]);
        }
    }

    private void RenderSubModules(IList<ModuleSubModuleModel> moduleSubModules)
    {
        if (moduleSubModules.Count == 0) return;

        JmGui.Text("SubModules:\0"u8);
        for (var i = 0; i < moduleSubModules.Count; i++)
        {
            var subModule = moduleSubModules[i];
            ImGui.Bullet();
            JmGui.PushStyleColor(ImGuiCol.ChildBg, in SubModule);
            if (JmGui.BeginChild(subModule.Name, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();

                JmGui.TextSameLine("Name: \0"u8);
                JmGui.Text(subModule.Name);
                JmGui.TextSameLine("DLLName: \0"u8);
                JmGui.Text(subModule.AssemblyId?.Name ?? string.Empty);
                JmGui.TextSameLine("SubModuleClassType: \0"u8);
                JmGui.Text(subModule.Entrypoint);

                var firstTag = true;
                foreach (var tag in subModule.AdditionalMetadata.ToRefLinq().Where(x => !x.Key.StartsWith("METADATA:")))
                {
                    if (firstTag)
                    {
                        JmGui.Text("Tags:\0"u8);
                        firstTag = false;
                    }

                    ImGui.Bullet();
                    JmGui.TextSameLine(tag.Key);
                    JmGui.TextSameLine(": \0"u8);
                    JmGui.Text(tag.Value);
                }

                var firstAssembly = true;
                foreach (var assembly in subModule.AdditionalMetadata.ToRefLinq().Where(x => !x.Key.StartsWith("METADATA:Assembly")))
                {
                    if (firstAssembly)
                    {
                        JmGui.Text("Tags:\0"u8);
                        firstAssembly = false;
                    }

                    ImGui.Bullet();
                    JmGui.Text(assembly.Value);
                }
            }

            ImGui.EndChild();
        }
    }

    private void RenderAdditionalAssemblies(string moduleId)
    {
        var first = true;
        for (var i = 0; i < _crashReport.Assemblies.Count; i++)
        {
            if (first)
            {
                first = false;
                JmGui.Text("Assemblies Present:\0"u8);
            }

            var assembly = _crashReport.Assemblies[i];
            if (assembly.ModuleId != moduleId) continue;

            ImGui.Bullet();
            JmGui.TextSameLine(assembly.Id.Name);
            JmGui.TextSameLine(" (\0"u8);
            JmGui.TextSameLine(_assemblyFullNameUtf8[assembly]);
            JmGui.Text(")\0"u8);
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

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in color);
            if (JmGui.BeginChild(module.Id, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();

                if (JmGui.TreeNode(module.Id))
                {
                    JmGui.RenderId("Id:\0"u8, module.Id);
                    JmGui.TextSameLine("Name: \0"u8);
                    JmGui.Text(module.Name);
                    JmGui.TextSameLine("Version: \0"u8);
                    JmGui.Text(module.Version);
                    JmGui.TextSameLine("External: \0"u8);
                    JmGui.Text(module.IsExternal);
                    JmGui.TextSameLine("Vortex: \0"u8);
                    JmGui.Text(isVortexManaged);
                    JmGui.TextSameLine("Official: \0"u8);
                    JmGui.Text(module.IsOfficial);
                    JmGui.TextSameLine("Singleplayer: \0"u8);
                    JmGui.Text(module.IsSingleplayer);
                    JmGui.TextSameLine("Multiplayer: \0"u8);
                    JmGui.Text(module.IsMultiplayer);

                    RenderDependencies(module);

                    RenderCapabilitiesDependencies(_moduleIdCapabilities[module.Id]);

                    if (module.Url is not null)
                    {
                        JmGui.TextSameLine("Url: \0"u8);
                        if (JmGui.SmallButton(module.Url))
                            Process.Start(new ProcessStartInfo(module.Url!) { UseShellExecute = true, Verb = "open" });
                    }

                    if (module.UpdateInfo is not null)
                    {
                        JmGui.TextSameLine("Update Info: \0"u8);
                        JmGui.Text(_moduleIdUpdateInfoUtf8[module.Id]);
                    }

                    RenderSubModules(module.SubModules);

                    RenderAdditionalAssemblies(module.Id);

                    ImGui.TreePop();
                }
            }

            ImGui.EndChild();
        }
    }
}