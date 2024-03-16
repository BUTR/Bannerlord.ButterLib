using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Extensions;
using BUTR.CrashReport.Models;

using ImGuiNET;

using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;

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

    private FrozenDictionary<AssemblyModel, byte[]> _assemblyPathUtf8 = default!;
    private FrozenDictionary<AssemblyModel, byte[]> _assemblyFullNameUtf8 = default!;

    private void InitializeAssemblies()
    {
        var assemblyPathUtf8 = new Dictionary<AssemblyModel, byte[]>();
        var assemblyFullNameUtf8 = new Dictionary<AssemblyModel, byte[]>();
        for (var i = 0; i < _crashReport.Assemblies.Count; i++)
        {
            var assembly = _crashReport.Assemblies[i];
            assemblyPathUtf8[assembly] = UnsafeHelper.ToUtf8Array($"..{Path.DirectorySeparatorChar}{assembly.AnonymizedPath}");
            assemblyFullNameUtf8[assembly] = UnsafeHelper.ToUtf8Array(assembly.GetFullName());
        }
        _assemblyPathUtf8 = assemblyPathUtf8.ToFrozenDictionary();
        _assemblyFullNameUtf8 = assemblyFullNameUtf8.ToFrozenDictionary();
    }

    private void RenderAssemblies()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1);
        JmGui.TextSameLine("Hide: \0"u8);
        JmGui.CheckboxSameLine(" System | \0"u8, ref _hideSystemAssemblies);
        JmGui.CheckboxSameLine(" GAC | \0"u8, ref _hideGACAssemblies);
        JmGui.CheckboxSameLine(" Game | \0"u8, ref _hideGameAssemblies);
        JmGui.CheckboxSameLine(" Game Modules | \0"u8, ref _hideGameModulesAssemblies);
        JmGui.CheckboxSameLine(" Modules | \0"u8, ref _hideModulesAssemblies);
        JmGui.CheckboxSameLine(" Loader | \0"u8, ref _hideLoaderAssemblies);
        JmGui.CheckboxSameLine(" Loader Plugins | \0"u8, ref _hideLoaderPluginsAssemblies);
        JmGui.Checkbox(" Dynamic \0"u8, ref _hideDynamicAssemblies);
        ImGui.PopStyleVar();

        for (var i = 0; i < _crashReport.Assemblies.Count; i++)
        {
            var assembly = _crashReport.Assemblies[i];
            if (_hideSystemAssemblies && assembly.Type.IsSet(AssemblyModelType.System)) continue;
            if (_hideGACAssemblies && assembly.Type.IsSet(AssemblyModelType.GAC)) continue;
            if (_hideGameAssemblies && assembly.Type.IsSet(AssemblyModelType.GameCore)) continue;
            if (_hideGameModulesAssemblies && assembly.Type.IsSet(AssemblyModelType.GameModule)) continue;
            if (_hideModulesAssemblies && assembly.Type.IsSet(AssemblyModelType.Module)) continue;
            if (_hideLoaderAssemblies && assembly.Type.IsSet(AssemblyModelType.Loader)) continue;
            if (_hideLoaderPluginsAssemblies && assembly.Type.IsSet(AssemblyModelType.LoaderPlugin)) continue;
            if (_hideDynamicAssemblies && assembly.Type.IsSet(AssemblyModelType.Dynamic)) continue;

            var isDynamic = assembly.Type.IsSet(AssemblyModelType.Dynamic);
            var hasPath = assembly.AnonymizedPath != "EMPTY" && assembly.AnonymizedPath != "DYNAMIC" && !string.IsNullOrWhiteSpace(assembly.AnonymizedPath);

            ImGui.Bullet();
            JmGui.TextSameLine(assembly.Id.Name);
            JmGui.TextSameLine(", \0"u8);
            JmGui.TextSameLine(assembly.Id.Version ?? string.Empty);
            JmGui.TextSameLine(", \0"u8);
            JmGui.TextSameLine(assembly.Architecture);
            if (!isDynamic)
            {
                JmGui.TextSameLine(", \0"u8);
                JmGui.TextSameLine(assembly.Hash);
            }
            if (hasPath)
            {
                JmGui.TextSameLine(", \0"u8);
                JmGui.SmallButton(_assemblyPathUtf8[assembly]);
            }
            else
            {
                JmGui.Text(isDynamic ? ", DYNAMIC\0"u8 : ", EMPTY\0"u8);
            }
        }
    }
}