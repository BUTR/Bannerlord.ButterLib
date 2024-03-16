using System.Linq;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private void RenderInvolvedModules()
    {
        foreach (var grouping in _crashReport.EnhancedStacktrace.GroupBy(x => x.ExecutingMethod.ModuleId ?? "UNKNOWN"))
        {
            var moduleId = grouping.Key;
            if (moduleId == "UNKNOWN") continue;

            if (ImGui.TreeNodeEx(moduleId, ImGuiTreeNodeFlags.DefaultOpen))
            {
                RenderId("Module Id:", moduleId);

                foreach (var stacktrace in grouping)
                {
                    ImGui.Bullet();
                    ImGui.Indent();
                
                    ImGui.Text($"Method: {stacktrace.ExecutingMethod.MethodFullDescription}");
                    ImGui.Text($"Frame: {stacktrace.FrameDescription}");

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        ImGui.Text("Patches:");

                        ImGui.Bullet();
                        ImGui.Indent();
                        foreach (var method in stacktrace.PatchMethods)
                        {
                            var harmonyPatch = method as MethodHarmonyPatch;
                        
                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var moduleId2 = method.ModuleId ?? "UNKNOWN";

                            If(moduleId2 == "UNKNOWN", () => RenderId("Module Id:", moduleId));
                            ImGui.Text($"Method: {method.MethodFullDescription}");
                            If(harmonyPatch is not null, () => ImGui.Text($"Harmony Patch Type: {harmonyPatch!.PatchType}"));
                        }
                    
                        ImGui.Unindent();
                    }
                
                    ImGui.Unindent();
                }
                
                ImGui.TreePop();
            }
        }
    }
    
    private void RenderInvolvedPlugins()
    {
        foreach (var grouping in _crashReport.EnhancedStacktrace.GroupBy(x => x.ExecutingMethod.LoaderPluginId ?? "UNKNOWN"))
        {
            var pluginId = grouping.Key;
            if (pluginId == "UNKNOWN") continue;

            if (ImGui.TreeNodeEx(pluginId, ImGuiTreeNodeFlags.DefaultOpen))
            {
                RenderId("Plugin Id:", pluginId);

                foreach (var stacktrace in grouping)
                {
                    ImGui.Bullet();
                    ImGui.Indent();
                
                    ImGui.Text($"Method: {stacktrace.ExecutingMethod.MethodFullDescription}");
                    ImGui.Text($"Frame: {stacktrace.FrameDescription}");

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        ImGui.Text("Patches:");

                        ImGui.Bullet();
                        ImGui.Indent();
                    
                        foreach (var method in stacktrace.PatchMethods)
                        {
                            var harmonyPatch = method as MethodHarmonyPatch;
                        
                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";

                            If(pluginId2 == "UNKNOWN", () => RenderId("Plugin Id:", pluginId));
                            ImGui.Text($"Method: {method.MethodFullDescription}");
                            If(harmonyPatch is not null, () => ImGui.Text($"Harmony Patch Type: {harmonyPatch!.PatchType}"));
                        }
                    
                        ImGui.Unindent();
                    }
                
                    ImGui.Unindent();
                }
                
                ImGui.TreePop();
            }
        }
    }
    
    private void RenderInvolvedModulesAndPlugins()
    {
        ImGui.Text("Based on Stacktrace:");
        ImGui.Indent();
        RenderInvolvedModules();
        RenderInvolvedPlugins();
        ImGui.Unindent();
    }
}