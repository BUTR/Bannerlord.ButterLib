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
        [],
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
        foreach (var kv in _enhancedStacktraceGroupedByModuleId)
        {
            if (kv.Key == "UNKNOWN") continue;

            if (_imgui.TreeNode(kv.Key, ImGuiTreeNodeFlags.DefaultOpen))
            {
                _imgui.RenderId("Module Id:\0"u8, kv.Key);

                for (var j = 0; j < kv.Value.Length; j++)
                {
                    var stacktrace = kv.Value[j];
                    _imgui.Bullet();
                    _imgui.Indent();

                    _imgui.TextSameLine("Method: \0"u8);
                    _imgui.Text(stacktrace.ExecutingMethod.MethodFullDescription);
                    _imgui.TextSameLine("Frame: \0"u8);
                    _imgui.Text(stacktrace.FrameDescription);

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        _imgui.Text("Patches:\0"u8);

                        _imgui.Bullet();
                        _imgui.Indent();
                        for (var k = 0; k < stacktrace.PatchMethods.Count; k++)
                        {
                            var method = stacktrace.PatchMethods[k];
                            var harmonyPatch = method as MethodHarmonyPatch;

                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var moduleId2 = method.ModuleId ?? "UNKNOWN";

                            if (moduleId2 == "UNKNOWN") _imgui.RenderId("Module Id:\0"u8, kv.Key);
                            _imgui.TextSameLine("Method: \0"u8);
                            _imgui.Text(method.MethodFullDescription);
                            if (harmonyPatch is not null)
                            {
                                var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                                _imgui.TextSameLine("Harmony Patch Type: }\0"u8);
                                _imgui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                            }
                        }

                        _imgui.Unindent();
                    }

                    _imgui.Unindent();
                }

                _imgui.TreePop();
            }
        }
    }

    private void RenderInvolvedPlugins()
    {
        foreach (var kv in _enhancedStacktraceGroupedByLoaderPluginIdId)
        {
            if (kv.Key == "UNKNOWN") continue;

            if (_imgui.TreeNode(kv.Key, ImGuiTreeNodeFlags.DefaultOpen))
            {
                _imgui.RenderId("Plugin Id:\0"u8, kv.Key);

                for (var j = 0; j < kv.Value.Length; j++)
                {
                    var stacktrace = kv.Value[j];
                    _imgui.Bullet();
                    _imgui.Indent();

                    _imgui.TextSameLine("Method: \0"u8);
                    _imgui.Text(stacktrace.ExecutingMethod.MethodFullDescription);
                    _imgui.TextSameLine("Frame: \0"u8);
                    _imgui.Text(stacktrace.FrameDescription);

                    if (stacktrace.PatchMethods.Count > 0)
                    {
                        _imgui.Text("Patches:\0"u8);

                        _imgui.Bullet();
                        _imgui.Indent();

                        for (var k = 0; k < stacktrace.PatchMethods.Count; k++)
                        {
                            var method = stacktrace.PatchMethods[k];
                            var harmonyPatch = method as MethodHarmonyPatch;

                            // Ignore blank transpilers used to force the jitter to skip inlining
                            if (method.MethodName == "BlankTranspiler") continue;
                            var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";

                            if (pluginId2 == "UNKNOWN") _imgui.RenderId("Plugin Id:\0"u8, kv.Key);
                            _imgui.TextSameLine("Method: \0"u8);
                            _imgui.Text(method.MethodFullDescription);
                            if (harmonyPatch is not null)
                            {
                                var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                                _imgui.TextSameLine("Harmony Patch Type: \0"u8);
                                _imgui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                            }
                        }

                        _imgui.Unindent();
                    }

                    _imgui.Unindent();
                }

                _imgui.TreePop();
            }
        }
    }

    private void RenderInvolvedModulesAndPlugins()
    {
        _imgui.Text("Based on Stacktrace:\0"u8);
        _imgui.Indent();
        RenderInvolvedModules();
        RenderInvolvedPlugins();
        _imgui.Unindent();
    }
}