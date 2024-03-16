using System.Collections.Generic;
using System.Linq;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static void RenderHarmonyPatches(string name, IEnumerable<HarmonyPatchModel> patches)
    {
        foreach (var patch in patches)
        {
            var moduleId = patch.ModuleId ?? "UNKNOWN";
            var pluginId = patch.LoaderPluginId ?? "UNKNOWN";
            
            ImGui.Bullet();
            ImGui.Text(name);
            ImGui.Indent();

            If(moduleId != "UNKNOWN", () => { RenderId("Module Id:", moduleId); ImGui.SameLine(0, 0); });
            If(pluginId != "UNKNOWN", () => { RenderId("Plugin Id:", pluginId); ImGui.SameLine(0, 0); });
            TextSameLine($" Owner: {patch.Owner}");
            TextSameLine($" Namespace: {patch.Namespace}");
            If(patch.Index != 0, () => TextSameLine($" Index: {patch.Index}"));
            If(patch.Priority != 400, () => TextSameLine($" Priority: {patch.Index}"));
            If(patch.Before.Count > 0, () => TextSameLine($" Before: {string.Join(", ", patch.Before)}"));
            If(patch.After.Count > 0, () => TextSameLine($" After: {string.Join(", ", patch.After)}"));
            ImGui.NewLine();
            
            ImGui.Unindent();
        }
    }
    
    private void RenderHarmonyPatches()
    {
        foreach (var harmonyPatch in _crashReport.HarmonyPatches)
        {
            var methodNameFull = !string.IsNullOrEmpty(harmonyPatch.OriginalMethodDeclaredTypeName)
                ? $"{harmonyPatch.OriginalMethodDeclaredTypeName}.{harmonyPatch.OriginalMethodName}"
                : harmonyPatch.OriginalMethodName;
            
            if (ImGui.TreeNodeEx(methodNameFull, ImGuiTreeNodeFlags.DefaultOpen))
            {
                RenderHarmonyPatches("Prefixes", harmonyPatch.Patches.Where(x => x.Type == HarmonyPatchType.Prefix));
                RenderHarmonyPatches("Postfixes", harmonyPatch.Patches.Where(x => x.Type == HarmonyPatchType.Postfix));
                RenderHarmonyPatches("Finalizers", harmonyPatch.Patches.Where(x => x.Type == HarmonyPatchType.Finalizer));
                RenderHarmonyPatches("Transpilers", harmonyPatch.Patches.Where(x => x.Type == HarmonyPatchType.Transpiler));
                ImGui.NewLine();
                
                ImGui.TreePop();
            }
        }
    }
}