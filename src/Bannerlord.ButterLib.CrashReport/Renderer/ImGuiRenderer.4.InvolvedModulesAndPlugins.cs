using Bannerlord.BUTR.Shared.Extensions;

using BUTR.CrashReport.Models;

using ImGuiNET;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static readonly byte[][] _harmonyPatchTypeNames =
    [
        "Prefix\0"u8.ToArray(),     // Prefix
        "Postfix\0"u8.ToArray(),    // Postfix
        "Transpiler\0"u8.ToArray(), // Transpiler
        "Finalizer\0"u8.ToArray(),  // Finalizer
    ];

    private KeyValuePair<string, EnhancedStacktraceFrameModel[]>[] _enhancedStacktraceGroupedByModuleId = Array.Empty<KeyValuePair<string, EnhancedStacktraceFrameModel[]>>();
    private KeyValuePair<string, EnhancedStacktraceFrameModel[]>[] _enhancedStacktraceGroupedByLoaderPluginIdId = Array.Empty<KeyValuePair<string, EnhancedStacktraceFrameModel[]>>();

    private void InitializeInvolved()
    {
        _enhancedStacktraceGroupedByModuleId = _crashReport.EnhancedStacktrace
            .GroupBy(x => x.ExecutingMethod.ModuleId ?? "UNKNOWN")
            .Select(x => new KeyValuePair<string, EnhancedStacktraceFrameModel[]>(x.Key, x.ToArray()))
            .ToArray();

        _enhancedStacktraceGroupedByLoaderPluginIdId = _crashReport.EnhancedStacktrace
            .GroupBy(x => x.ExecutingMethod.LoaderPluginId ?? "UNKNOWN")
            .Select(x => new KeyValuePair<string, EnhancedStacktraceFrameModel[]>(x.Key, x.ToArray()))
            .ToArray();
    }

    private void RenderInvolvedModules()
    {
        foreach (var (moduleId, stacktraces) in _enhancedStacktraceGroupedByModuleId)
        {
            if (moduleId == "UNKNOWN") continue;

            if (JmGui.TreeNode(moduleId, ImGuiTreeNodeFlags.DefaultOpen))
            {
                JmGui.RenderId("Module Id:\0"u8, moduleId);

                for (var j = 0; j < stacktraces.Length; j++)
                {
                    var stacktrace = stacktraces[j];
                    ImGui.Bullet();
                    ImGui.Indent();

                    JmGui.TextSameLine("Method: \0"u8);
                    JmGui.Text(stacktrace.ExecutingMethod.MethodFullDescription);
                    JmGui.TextSameLine("Frame: \0"u8);
                    JmGui.Text(stacktrace.FrameDescription);

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        JmGui.Text("Patches:\0"u8);

                        ImGui.Bullet();
                        ImGui.Indent();
                        for (var k = 0; k < stacktrace.PatchMethods.Count; k++)
                        {
                            var method = stacktrace.PatchMethods[k];
                            var harmonyPatch = method as MethodHarmonyPatch;

                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var moduleId2 = method.ModuleId ?? "UNKNOWN";

                            if (moduleId2 == "UNKNOWN") JmGui.RenderId("Module Id:\0"u8, moduleId);
                            JmGui.TextSameLine("Method: \0"u8);
                            JmGui.Text(method.MethodFullDescription);
                            if (harmonyPatch is not null)
                            {
                                var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                                JmGui.TextSameLine("Harmony Patch Type: }\0"u8);
                                JmGui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                            }
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
        foreach (var (pluginId, stacktraces) in _enhancedStacktraceGroupedByLoaderPluginIdId)
        {
            if (pluginId == "UNKNOWN") continue;

            if (JmGui.TreeNode(pluginId, ImGuiTreeNodeFlags.DefaultOpen))
            {
                JmGui.RenderId("Plugin Id:\0"u8, pluginId);

                for (var j = 0; j < stacktraces.Length; j++)
                {
                    var stacktrace = stacktraces[j];
                    ImGui.Bullet();
                    ImGui.Indent();

                    JmGui.TextSameLine("Method: \0"u8);
                    JmGui.Text(stacktrace.ExecutingMethod.MethodFullDescription);
                    JmGui.TextSameLine("Frame: \0"u8);
                    JmGui.Text(stacktrace.FrameDescription);

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        JmGui.Text("Patches:\0"u8);

                        ImGui.Bullet();
                        ImGui.Indent();

                        for (var k = 0; k < stacktrace.PatchMethods.Count; k++)
                        {
                            var method = stacktrace.PatchMethods[k];
                            var harmonyPatch = method as MethodHarmonyPatch;

                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";

                            if (pluginId2 == "UNKNOWN") JmGui.RenderId("Plugin Id:\0"u8, pluginId);
                            JmGui.TextSameLine("Method: \0"u8);
                            JmGui.Text(method.MethodFullDescription);
                            if (harmonyPatch is not null)
                            {
                                var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                                JmGui.TextSameLine("Harmony Patch Type: \0"u8);
                                JmGui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                            }
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
        JmGui.Text("Based on Stacktrace:\0"u8);
        ImGui.Indent();
        RenderInvolvedModules();
        RenderInvolvedPlugins();
        ImGui.Unindent();
    }
}