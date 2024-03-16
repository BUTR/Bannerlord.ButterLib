using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Extensions;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static void RenderDependencies(ICollection<DependencyMetadataModel> moduleDependencyMetadatas)
    {
        if (moduleDependencyMetadatas.Count == 0) return;
            
        ImGui.Text("Dependencies:");
        foreach (var dependentModule in moduleDependencyMetadatas)
        {
            var optional = dependentModule.IsOptional ? " (optional)" : string.Empty;
            var version = !string.IsNullOrEmpty(dependentModule.Version) ? $" >= {dependentModule.Version}" : string.Empty;
            var versionRange = !string.IsNullOrEmpty(dependentModule.VersionRange) ? $" {dependentModule.VersionRange}" : string.Empty;
                
            ImGui.Bullet();
            TextSameLine(dependentModule.Type switch
            {
                DependencyMetadataModelType.LoadBefore => "Load Before ",
                DependencyMetadataModelType.LoadAfter => "Load After ",
                DependencyMetadataModelType.Incompatible => "Incompatible ",
                _ => ""
            });
            SmallButtonSameLine(dependentModule.ModuleOrPluginId);
            ImGui.Text($"{optional}{version}{versionRange}");
        }
    }
    
    private static void RenderCapabilitiesDependencies(ICollection<ModuleCapabilities> getModuleCapabilities)
    {
        if (getModuleCapabilities.Count == 0) return;
            
        ImGui.Text("Capabilities:");
        foreach (var capability in getModuleCapabilities)
        {
            ImGui.Bullet();
            ImGui.Text(GetEnumDescription(capability));
        }
    }
    
    private static void RenderSubModules(ICollection<ModuleSubModuleModel> moduleSubModules)
    {
        if (moduleSubModules.Count == 0) return;
            
        ImGui.Text("SubModules:");
        foreach (var subModule in moduleSubModules)
        {
            ImGui.Bullet();
            ImGui.PushStyleColor(ImGuiCol.ChildBg, SubModule);
            if (ImGui.BeginChild(subModule.Name, Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();

                ImGui.Text($"Name: {subModule.Name}");
                ImGui.Text($"DLLName: {subModule.AssemblyId?.Name}");
                ImGui.Text($"SubModuleClassType: {subModule.Entrypoint}");

                var tags = subModule.AdditionalMetadata.Where(x => !x.Key.StartsWith("METADATA:")).ToArray();
                If(tags.Length > 0, () =>
                {
                    ImGui.Text("Tags:");
                    foreach (var metadata in tags)
                    {
                        ImGui.Bullet();
                        ImGui.Text($"{metadata.Key}: {metadata.Value}");
                    }
                });

                var assemblies = subModule.AdditionalMetadata.Where(x => x.Key == "METADATA:Assembly").ToArray();
                If(assemblies.Length > 0, () =>
                {
                    ImGui.Text("Assemblies:");
                    foreach (var metadata in assemblies)
                    {
                        ImGui.Bullet();
                        ImGui.Text(metadata.Value);
                    }
                });
            }
            ImGui.EndChild();
        }
    }
    
    private void RenderAdditionalAssemblies(string moduleId)
    {
        var assembliesPresent = _crashReport.Assemblies.Where(y => y.ModuleId == moduleId).ToArray();
        if (assembliesPresent.Length == 0) return;
            
        ImGui.Text("Assemblies Present:");
            
        foreach (var assembly in assembliesPresent)
        {
            ImGui.Bullet();
            ImGui.Text($"{assembly.Id.Name} ({assembly.GetFullName()})");
        }
    }
    
    private void RenderInstalledModules()
    {
        foreach (var module in _crashReport.Modules)
        {
            var isVortexManaged = module.AdditionalMetadata.FirstOrDefault(x => x.Key == "METADATA:MANAGED_BY_VORTEX")?.Value is { } str && bool.TryParse(str, out var val) && val;

            var color = module switch
            {
                { IsOfficial: true } => OfficialModule,
                { IsExternal: true } => ExternalModule,
                _ => UnofficialModule,
            };

            ImGui.PushStyleColor(ImGuiCol.ChildBg, color);
            if (ImGui.BeginChild(module.Id, Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                
                if (ImGui.TreeNode(module.Id))
                {
                    RenderId("Id:", module.Id);
                    ImGui.Text($"Name: {module.Name}");
                    ImGui.Text($"Version: {module.Version}");
                    ImGui.Text($"External: {module.IsExternal}");
                    ImGui.Text($"Vortex: {isVortexManaged}");
                    ImGui.Text($"Official: {module.IsOfficial}");
                    ImGui.Text($"Singleplayer: {module.IsSingleplayer}");
                    ImGui.Text($"Multiplayer: {module.IsMultiplayer}");
                    
                    RenderDependencies(module.DependencyMetadatas);
                        
                    RenderCapabilitiesDependencies(CrashReportShared.GetModuleCapabilities(_crashReport, module).ToArray());

                    If(module.Url is not null, () =>
                    {
                        ImGui.Text("Url:");
                        ImGui.SameLine();
                        if (ImGui.SmallButton(module.Url))
                            Process.Start(new ProcessStartInfo(module.Url) { UseShellExecute = true, Verb = "open" });
                    });
                    
                    If(module.UpdateInfo is not null, () => ImGui.Text($"Update Info: {module.UpdateInfo}"));

                    RenderSubModules(module.SubModules);
                        
                    RenderAdditionalAssemblies(module.Id);
                    
                    ImGui.TreePop();
                }
            }
            ImGui.EndChild();
        }
    }
}