using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using ImGuiNET;

using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private class IntEqualityComparer : IEqualityComparer<int>
    {
        public static IntEqualityComparer Instance { get; } = new();
        public bool Equals(int x, int y) => x == y;
        public int GetHashCode(int obj) => obj;
    }

    private enum CodeType { IL, CSharpILMixed, CSharp, Native }
    private FrozenDictionary<MethodSimple, byte[][]> _methodIdUtf8 = default!;
    private FrozenDictionary<MethodSimple, byte[][]> _methodCodeLinesUtf8 = default!;
    private FrozenDictionary<MethodSimple, int[]> _methodCodeLineCount = default!;
    private FrozenDictionary<int, byte[]> _offsetsUtf8 = default!;

    private static void SetCodeDictionary<TValue>(IDictionary<MethodSimple, TValue[]> methodDict, MethodSimple key, CodeType codeType, TValue value)
    {
        if (!methodDict.TryGetValue(key, out var codeArray))
            methodDict[key] = (codeArray = new TValue[(int) CodeType.Native + 1]);

        codeArray[Unsafe.As<CodeType, int>(ref codeType)] = value;
    }

    private void InitializeCodeLines()
    {
        var colapsingHeaderId = 0;
        string GetLabelId(string name)
        {
            var uniqueId = string.Concat(Enumerable.Repeat("\u00a0", colapsingHeaderId));
            colapsingHeaderId++;
            return $"{name}{uniqueId}";
        }

        var methodIdUtf8 = new Dictionary<MethodSimple, byte[][]>();
        var methodCodeLinesUtf8 = new Dictionary<MethodSimple, byte[][]>();
        var methodCodeLineCount = new Dictionary<MethodSimple, int[]>();
        var offsetsUtf8 = new Dictionary<int, byte[]>(IntEqualityComparer.Instance);

        void SetupCodeExecuting(MethodExecuting? method)
        {
            if (method is null) return;

            SetupCode(method);

            SetCodeDictionary(methodIdUtf8, method, CodeType.Native, UnsafeHelper.ToUtf8Array(GetLabelId("Native:")));
            SetCodeDictionary(methodCodeLinesUtf8, method, CodeType.Native, UnsafeHelper.ToUtf8Array(string.Join("\n", method.NativeInstructions)));
            SetCodeDictionary(methodCodeLineCount, method, CodeType.Native, method.NativeInstructions.Count);
        }
        void SetupCode(MethodSimple? method)
        {
            if (method is null) return;

            SetCodeDictionary(methodIdUtf8, method, CodeType.IL, UnsafeHelper.ToUtf8Array(GetLabelId("IL:")));
            SetCodeDictionary(methodCodeLinesUtf8, method, CodeType.IL, UnsafeHelper.ToUtf8Array(string.Join("\n", method.ILInstructions)));
            SetCodeDictionary(methodCodeLineCount, method, CodeType.IL, method.ILInstructions.Count);

            SetCodeDictionary(methodIdUtf8, method, CodeType.CSharpILMixed, UnsafeHelper.ToUtf8Array(GetLabelId("IL with C#:")));
            SetCodeDictionary(methodCodeLinesUtf8, method, CodeType.CSharpILMixed, UnsafeHelper.ToUtf8Array(string.Join("\n", method.CSharpILMixedInstructions)));
            SetCodeDictionary(methodCodeLineCount, method, CodeType.CSharpILMixed, method.CSharpILMixedInstructions.Count);

            SetCodeDictionary(methodIdUtf8, method, CodeType.CSharp, UnsafeHelper.ToUtf8Array(GetLabelId("C#:")));
            SetCodeDictionary(methodCodeLinesUtf8, method, CodeType.CSharp, UnsafeHelper.ToUtf8Array(string.Join("\n", method.CSharpInstructions)));
            SetCodeDictionary(methodCodeLineCount, method, CodeType.CSharp, method.CSharpInstructions.Count);
        }

        for (var i = 0; i < _crashReport.EnhancedStacktrace.Count; i++)
        {
            var stacktrace = _crashReport.EnhancedStacktrace[i];
            if (stacktrace.ILOffset is not null)
                offsetsUtf8[stacktrace.ILOffset.Value] = UnsafeHelper.ToUtf8Array($"{stacktrace.ILOffset:X4}");

            if (stacktrace.NativeOffset is not null)
                offsetsUtf8[stacktrace.NativeOffset.Value] = UnsafeHelper.ToUtf8Array($"{stacktrace.NativeOffset:X4}");

            SetupCodeExecuting(stacktrace.ExecutingMethod);
            SetupCode(stacktrace.OriginalMethod);
            for (var j = 0; j < stacktrace.PatchMethods.Count; j++)
            {
                SetupCode(stacktrace.PatchMethods[j]);
            }
        }

        _methodIdUtf8 = methodIdUtf8.ToFrozenDictionary();
        _methodCodeLinesUtf8 = methodCodeLinesUtf8.ToFrozenDictionary();
        _methodCodeLineCount = methodCodeLineCount.ToFrozenDictionary();
        _offsetsUtf8 = offsetsUtf8.ToFrozenDictionary();
    }

    private void RenderMethodLines(MethodSimple method, CodeType codeType)
    {
        var id = _methodIdUtf8[method][Unsafe.As<CodeType, int>(ref codeType)];
        var lines = _methodCodeLinesUtf8[method][Unsafe.As<CodeType, int>(ref codeType)];
        var lineCount = _methodCodeLineCount[method][Unsafe.As<CodeType, int>(ref codeType)];

        if (lines.Length == 0 || lineCount == 0) return;

        if (JmGui.TreeNode(id))
        {
            JmGui.PushStyleColor(ImGuiCol.ChildBg, in Background);
            if (JmGui.BeginChild(id, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();

                JmGui.InputTextMultiline(id, lines, lineCount);
            }
            ImGui.EndChild();
            ImGui.TreePop();
        }
    }
    private void RenderCodeExecuting(MethodExecuting? method)
    {
        if (method is null) return;

        RenderCode(method);
        RenderMethodLines(method, CodeType.Native);
    }
    private void RenderCode(MethodSimple? method)
    {
        if (method is null) return;

        RenderMethodLines(method, CodeType.IL);
        RenderMethodLines(method, CodeType.CSharpILMixed);
        RenderMethodLines(method, CodeType.CSharp);
    }

    private void RenderEnhancedStacktrace()
    {
        for (var i = 0; i < _crashReport.EnhancedStacktrace.Count; i++)
        {
            var stacktrace = _crashReport.EnhancedStacktrace[i];
            var moduleId1 = stacktrace.ExecutingMethod.ModuleId ?? "UNKNOWN";
            var pluginId1 = stacktrace.ExecutingMethod.LoaderPluginId ?? "UNKNOWN";

            if (ImGui.TreeNode(stacktrace.FrameDescription))
            {
                JmGui.Text("Executing Method:\0"u8);

                ImGui.Bullet();
                ImGui.Indent();
                JmGui.TextSameLine("Method: \0"u8);
                JmGui.Text(stacktrace.ExecutingMethod.MethodFullDescription);

                if (moduleId1 != "UNKNOWN") JmGui.RenderId("Module Id:\0"u8, moduleId1);
                if (pluginId1 != "UNKNOWN") JmGui.RenderId("Plugin Id:\0"u8, pluginId1);

                JmGui.TextSameLine("Method From Stackframe Issue: \0"u8);
                JmGui.Text(stacktrace.MethodFromStackframeIssue);
                JmGui.TextSameLine("Approximate IL Offset: \0"u8);
                JmGui.Text(stacktrace.ILOffset is not null ? _offsetsUtf8[stacktrace.ILOffset.Value] : "UNKNOWN\0"u8);
                JmGui.TextSameLine("Native Offset: \0"u8);
                JmGui.Text(stacktrace.NativeOffset is not null ? _offsetsUtf8[stacktrace.NativeOffset.Value] : "UNKNOWN\0"u8);

                RenderCodeExecuting(stacktrace.ExecutingMethod);

                ImGui.Unindent();

                if (stacktrace.PatchMethods.Count > 0)
                {
                    JmGui.Text("Patch Methods:\0"u8);

                    for (var j = 0; j < stacktrace.PatchMethods.Count; j++)
                    {
                        var method = stacktrace.PatchMethods[j];
                        ImGui.Bullet();
                        ImGui.Indent();
                        JmGui.TextSameLine("Method: \0"u8);
                        JmGui.Text(method.MethodFullDescription);

                        var moduleId2 = method.ModuleId ?? "UNKNOWN";
                        var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";
                        var harmonyPatch = method as MethodHarmonyPatch;

                        if (moduleId2 != "UNKNOWN") JmGui.RenderId("Module Id:\0"u8, moduleId2);
                        if (pluginId2 != "UNKNOWN") JmGui.RenderId("Plugin Id:\0"u8, pluginId2);

                        JmGui.TextSameLine("Type: \0"u8);
                        JmGui.Text(harmonyPatch is not null ? "Harmony\0"u8 : "UNKNOWN\0"u8);

                        if (harmonyPatch is not null)
                        {
                            var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                            JmGui.TextSameLine("Patch Type: \0"u8);
                            JmGui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                        }

                        RenderCode(method);

                        ImGui.Unindent();
                    }
                }

                if (stacktrace.OriginalMethod is not null)
                {
                    var moduleId3 = stacktrace.OriginalMethod.ModuleId ?? "UNKNOWN";
                    var pluginId3 = stacktrace.OriginalMethod.LoaderPluginId ?? "UNKNOWN";

                    JmGui.Text("Original Method:\0"u8);

                    ImGui.Bullet();
                    ImGui.Indent();
                    JmGui.TextSameLine("Method: \0"u8);
                    JmGui.Text(stacktrace.OriginalMethod.MethodFullDescription);

                    if (moduleId3 != "UNKNOWN") JmGui.RenderId("Module Id:\0"u8, moduleId3);
                    if (pluginId3 != "UNKNOWN") JmGui.RenderId("Plugin Id:\0"u8, pluginId3);

                    RenderCode(stacktrace.OriginalMethod);

                    ImGui.Unindent();
                }

                ImGui.TreePop();
            }
        }
    }
}