using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static void RenderCodeLines(string id, ICollection<string> lines)
    {
        if (ImGui.TreeNode(id))
        {
            ImGui.PushStyleColor(ImGuiCol.ChildBg, Background);
            if (ImGui.BeginChild(id, Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                
                var lines2 = string.Join("\n", lines);
                InputTextMultiline(id, ref lines2, lines.Count);
            }
            ImGui.EndChild();
            ImGui.TreePop();
        }
    }
    
    private void RenderEnhancedStacktrace()
    {
        var colapsingHeaderId = 0;
        string GetLabelId(string name)
        {
            var uniqueId = string.Concat(Enumerable.Repeat("\u00a0", colapsingHeaderId));
            colapsingHeaderId++;
            return $"{name}{uniqueId}";
        }
        
        foreach (var stacktrace in _crashReport.EnhancedStacktrace)
        {
            var moduleId1 = stacktrace.ExecutingMethod.ModuleId ?? "UNKNOWN";
            var pluginId1 = stacktrace.ExecutingMethod.LoaderPluginId ?? "UNKNOWN";

            if (ImGui.TreeNode(stacktrace.FrameDescription))
            {
                ImGui.Text("Executing Method:");
                
                ImGui.Bullet();
                ImGui.Indent();
                ImGui.Text($"Method: {stacktrace.ExecutingMethod.MethodFullDescription}");

                If(moduleId1 != "UNKNOWN", () => RenderId("Module Id:", moduleId1));
                If(pluginId1 != "UNKNOWN", () => RenderId("Plugin Id:", pluginId1));
                
                ImGui.Text($"Method From Stackframe Issue: {stacktrace.MethodFromStackframeIssue}");
                ImGui.Text($"Approximate IL Offset: {(stacktrace.ILOffset is not null ? $"{stacktrace.ILOffset:X4}" : "UNKNOWN")}");
                ImGui.Text($"Native Offset: {(stacktrace.NativeOffset is not null ? $"{stacktrace.NativeOffset:X4}" : "UNKNOWN")}");

                If(stacktrace.ExecutingMethod.ILInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL:"), stacktrace.ExecutingMethod.ILInstructions));
                If(stacktrace.ExecutingMethod.CSharpILMixedInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL with C#:"), stacktrace.ExecutingMethod.CSharpILMixedInstructions));
                If(stacktrace.ExecutingMethod.CSharpInstructions.Count > 0, () => RenderCodeLines(GetLabelId("C#:"), stacktrace.ExecutingMethod.CSharpInstructions));
                If(stacktrace.ExecutingMethod.NativeInstructions.Count > 0, () => RenderCodeLines(GetLabelId("Native:"), stacktrace.ExecutingMethod.NativeInstructions));
                
                ImGui.Unindent();

                if (stacktrace.PatchMethods.Count > 0)
                {
                    ImGui.Text("Patch Methods:");

                    foreach (var method in stacktrace.PatchMethods)
                    {
                        ImGui.Bullet();
                        ImGui.Indent();
                        ImGui.Text($"Method: {method.MethodFullDescription}");

                        var moduleId2 = method.ModuleId ?? "UNKNOWN";
                        var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";
                        var harmonyPatch = method as MethodHarmonyPatch;
                        
                        If(moduleId2 != "UNKNOWN", () => RenderId("Module Id:", moduleId2));
                        If(pluginId2 != "UNKNOWN", () => RenderId("Plugin Id:", pluginId2));
                
                        ImGui.Text($"Type: {(harmonyPatch is not null ? "Harmony" : "UNKNOWN")}");
                        
                        If(harmonyPatch is not null, () => ImGui.Text($"Patch Type: {harmonyPatch!.PatchType}"));

                        If(method.ILInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL:"), method.ILInstructions));
                        If(method.CSharpILMixedInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL with C#:"), method.CSharpILMixedInstructions));
                        If(method.CSharpInstructions.Count > 0, () => RenderCodeLines(GetLabelId("C#:"), method.CSharpInstructions));
                        
                        ImGui.Unindent();
                    }
                }
                
                if (stacktrace.OriginalMethod is not null)
                {
                    var moduleId3 = stacktrace.OriginalMethod.ModuleId ?? "UNKNOWN";
                    var pluginId3 = stacktrace.OriginalMethod.LoaderPluginId ?? "UNKNOWN";
                    
                    ImGui.Text("Original Method:");
                    
                    ImGui.Bullet();
                    ImGui.Indent();
                    ImGui.Text($"Method: {stacktrace.OriginalMethod.MethodFullDescription}");

                    If(moduleId3 != "UNKNOWN", () => RenderId("Module Id:", moduleId3));
                    If(pluginId3 != "UNKNOWN", () => RenderId("Plugin Id:", pluginId3));

                    If(stacktrace.OriginalMethod.ILInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL:"), stacktrace.OriginalMethod.ILInstructions));
                    If(stacktrace.OriginalMethod.CSharpILMixedInstructions.Count > 0, () => RenderCodeLines(GetLabelId("IL with C#:"), stacktrace.OriginalMethod.CSharpILMixedInstructions));
                    If(stacktrace.OriginalMethod.CSharpInstructions.Count > 0, () => RenderCodeLines(GetLabelId("C#:"), stacktrace.OriginalMethod.CSharpInstructions));

                    ImGui.Unindent();
                }
                
                ImGui.TreePop();
            }
        }
    }
}