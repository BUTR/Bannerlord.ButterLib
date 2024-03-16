using System.IO;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static bool _hideSystemAssemblies;
    private static bool _hideGACAssemblies;
    private static bool _hideGameAssemblies;
    private static bool _hideGameModulesAssemblies;
    private static bool _hideModulesAssemblies;
    private static bool _hideLoaderAssemblies;
    private static bool _hideLoaderPluginsAssemblies;
    private static bool _hideDynamicAssemblies;
    
    private void RenderAssemblies()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1);
        TextSameLine("Hide: ");
        CheckboxSameLine(" System | ", ref _hideSystemAssemblies);
        CheckboxSameLine(" GAC | ", ref _hideGACAssemblies);
        CheckboxSameLine(" Game | ", ref _hideGameAssemblies);
        CheckboxSameLine(" Game Modules | ", ref _hideGameModulesAssemblies);
        CheckboxSameLine(" Modules | ", ref _hideModulesAssemblies);
        CheckboxSameLine(" Loader | ", ref _hideLoaderAssemblies);
        CheckboxSameLine(" Loader Plugins | ", ref _hideLoaderPluginsAssemblies);
        ImGui.Checkbox(" Dynamic ", ref _hideDynamicAssemblies);
        ImGui.PopStyleVar();
        
        foreach (var assembly in _crashReport.Assemblies)
        {
            if (_hideSystemAssemblies && assembly.Type.HasFlag(AssemblyModelType.System)) continue;
            if (_hideGACAssemblies && assembly.Type.HasFlag(AssemblyModelType.GAC)) continue;
            if (_hideGameAssemblies && assembly.Type.HasFlag(AssemblyModelType.GameCore)) continue;
            if (_hideGameModulesAssemblies && assembly.Type.HasFlag(AssemblyModelType.GameModule)) continue;
            if (_hideModulesAssemblies && assembly.Type.HasFlag(AssemblyModelType.Module)) continue;
            if (_hideLoaderAssemblies && assembly.Type.HasFlag(AssemblyModelType.Loader)) continue;
            if (_hideLoaderPluginsAssemblies && assembly.Type.HasFlag(AssemblyModelType.LoaderPlugin)) continue;
            if (_hideDynamicAssemblies && assembly.Type.HasFlag(AssemblyModelType.Dynamic)) continue;
            
            var isDynamic = assembly.Type.HasFlag(AssemblyModelType.Dynamic);
            var hasPath = assembly.AnonymizedPath != "EMPTY" && assembly.AnonymizedPath != "DYNAMIC" && !string.IsNullOrWhiteSpace(assembly.AnonymizedPath);
            var hash = !isDynamic ? $", {assembly.Hash}" : string.Empty;
            
            ImGui.Bullet();
            TextSameLine($"{assembly.Id.Name}, {assembly.Id.Version}, {assembly.Architecture}{hash}");
            if (hasPath)
            {
                TextSameLine(",");
                ImGui.SmallButton($"..{Path.DirectorySeparatorChar}{assembly.AnonymizedPath}");
            }
            else
            {
                ImGui.Text(isDynamic ? ", DYNAMIC" : ", EMPTY");
            }
        }
    }
}