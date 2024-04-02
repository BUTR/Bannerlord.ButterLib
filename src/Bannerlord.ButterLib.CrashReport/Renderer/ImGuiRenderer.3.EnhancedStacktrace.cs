using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using ImGuiNET;

using System.Collections.Generic;
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
    private class MethodSimpleEqualityComparer : IEqualityComparer<MethodSimple>
    {
        public static MethodSimpleEqualityComparer Instance { get; } = new();
        public bool Equals(MethodSimple? x, MethodSimple? y) => ReferenceEquals(x, y); // We can just reference compare here
        public int GetHashCode(MethodSimple obj) => obj.GetHashCode();
    }

    private enum CodeType { IL = 0, CSharpILMixed = 1, CSharp = 2, Native = 3 }
    private readonly Dictionary<MethodSimple, byte[][]> _methodIdUtf8 = new(MethodSimpleEqualityComparer.Instance);
    private readonly Dictionary<MethodSimple, byte[][]> _methodCodeLinesUtf8 = new(MethodSimpleEqualityComparer.Instance);
    private readonly Dictionary<MethodSimple, int[]> _methodCodeLineCount = new(MethodSimpleEqualityComparer.Instance);
    private readonly Dictionary<int, byte[]> _offsetsUtf8 = new(IntEqualityComparer.Instance);

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
            colapsingHeaderId++;
            return $"{name}##{colapsingHeaderId}";
        }

        void SetupCodeExecuting(MethodExecuting? method)
        {
            if (method is null) return;

            SetupCode(method);

            SetCodeDictionary(_methodIdUtf8, method, CodeType.Native, UnsafeHelper.ToUtf8Array(GetLabelId("Native:")));
            SetCodeDictionary(_methodCodeLinesUtf8, method, CodeType.Native, UnsafeHelper.ToUtf8Array(string.Join("\n", method.NativeInstructions).Trim('\n')));
            SetCodeDictionary(_methodCodeLineCount, method, CodeType.Native, method.NativeInstructions.Count);
        }
        void SetupCode(MethodSimple? method)
        {
            if (method is null) return;

            SetCodeDictionary(_methodIdUtf8, method, CodeType.IL, UnsafeHelper.ToUtf8Array(GetLabelId("IL:")));
            SetCodeDictionary(_methodCodeLinesUtf8, method, CodeType.IL, UnsafeHelper.ToUtf8Array(string.Join("\n", method.ILInstructions).Trim('\n')));
            SetCodeDictionary(_methodCodeLineCount, method, CodeType.IL, method.ILInstructions.Count);

            SetCodeDictionary(_methodIdUtf8, method, CodeType.CSharpILMixed, UnsafeHelper.ToUtf8Array(GetLabelId("IL with C#:")));
            SetCodeDictionary(_methodCodeLinesUtf8, method, CodeType.CSharpILMixed, UnsafeHelper.ToUtf8Array(string.Join("\n", method.CSharpILMixedInstructions).Trim('\n')));
            SetCodeDictionary(_methodCodeLineCount, method, CodeType.CSharpILMixed, method.CSharpILMixedInstructions.Count);

            SetCodeDictionary(_methodIdUtf8, method, CodeType.CSharp, UnsafeHelper.ToUtf8Array(GetLabelId("C#:")));
            SetCodeDictionary(_methodCodeLinesUtf8, method, CodeType.CSharp, UnsafeHelper.ToUtf8Array(string.Join("\n", method.CSharpInstructions).Trim('\n')));
            SetCodeDictionary(_methodCodeLineCount, method, CodeType.CSharp, method.CSharpInstructions.Count);
        }

        for (var i = 0; i < _crashReport.EnhancedStacktrace.Count; i++)
        {
            var stacktrace = _crashReport.EnhancedStacktrace[i];
            if (stacktrace.ILOffset is not null)
                _offsetsUtf8[stacktrace.ILOffset.Value] = UnsafeHelper.ToUtf8Array($"{stacktrace.ILOffset:X4}");

            if (stacktrace.NativeOffset is not null)
                _offsetsUtf8[stacktrace.NativeOffset.Value] = UnsafeHelper.ToUtf8Array($"{stacktrace.NativeOffset:X4}");

            SetupCodeExecuting(stacktrace.ExecutingMethod);
            SetupCode(stacktrace.OriginalMethod);
            for (var j = 0; j < stacktrace.PatchMethods.Count; j++)
            {
                SetupCode(stacktrace.PatchMethods[j]);
            }
        }
    }

    private void RenderMethodLines(MethodSimple method, CodeType codeType)
    {
        var id = _methodIdUtf8[method][Unsafe.As<CodeType, int>(ref codeType)];
        var lines = _methodCodeLinesUtf8[method][Unsafe.As<CodeType, int>(ref codeType)];
        var lineCount = _methodCodeLineCount[method][Unsafe.As<CodeType, int>(ref codeType)];

        if (lines.Length == 0 || lineCount == 0) return;

        if (_imgui.TreeNode(id))
        {
            _imgui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1);
            if (_imgui.BeginChild(id, in Zero2, in Background, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.PopStyleVar();
                _imgui.InputTextMultiline(id, lines, lineCount);
            }
            else
            {
                _imgui.PopStyleVar();
            }
            _imgui.EndChild();
            _imgui.TreePop();
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

            if (_imgui.TreeNode(stacktrace.FrameDescription))
            {
                _imgui.Text("Executing Method:\0"u8);

                _imgui.Bullet();
                _imgui.Indent();
                _imgui.TextSameLine("Method: \0"u8);
                _imgui.Text(stacktrace.ExecutingMethod.MethodFullDescription);

                if (moduleId1 != "UNKNOWN") _imgui.RenderId("Module Id:\0"u8, moduleId1);
                if (pluginId1 != "UNKNOWN") _imgui.RenderId("Plugin Id:\0"u8, pluginId1);

                _imgui.TextSameLine("Method From Stackframe Issue: \0"u8);
                _imgui.Text(stacktrace.MethodFromStackframeIssue);
                _imgui.TextSameLine("Approximate IL Offset: \0"u8);
                _imgui.Text(stacktrace.ILOffset is not null ? _offsetsUtf8[stacktrace.ILOffset.Value] : "UNKNOWN\0"u8);
                _imgui.TextSameLine("Native Offset: \0"u8);
                _imgui.Text(stacktrace.NativeOffset is not null ? _offsetsUtf8[stacktrace.NativeOffset.Value] : "UNKNOWN\0"u8);

                RenderCodeExecuting(stacktrace.ExecutingMethod);

                _imgui.Unindent();

                if (stacktrace.PatchMethods.Count > 0)
                {
                    _imgui.Text("Patch Methods:\0"u8);

                    for (var j = 0; j < stacktrace.PatchMethods.Count; j++)
                    {
                        var method = stacktrace.PatchMethods[j];
                        _imgui.Bullet();
                        _imgui.Indent();
                        _imgui.TextSameLine("Method: \0"u8);
                        _imgui.Text(method.MethodFullDescription);

                        var moduleId2 = method.ModuleId ?? "UNKNOWN";
                        var pluginId2 = method.LoaderPluginId ?? "UNKNOWN";
                        var harmonyPatch = method as MethodHarmonyPatch;

                        if (moduleId2 != "UNKNOWN") _imgui.RenderId("Module Id:\0"u8, moduleId2);
                        if (pluginId2 != "UNKNOWN") _imgui.RenderId("Plugin Id:\0"u8, pluginId2);

                        _imgui.TextSameLine("Type: \0"u8);
                        _imgui.Text(harmonyPatch is not null ? "Harmony\0"u8 : "UNKNOWN\0"u8);

                        if (harmonyPatch is not null)
                        {
                            var harmonyPatchType = Clamp(harmonyPatch.PatchType, HarmonyPatchType.Prefix, HarmonyPatchType.Transpiler);
                            _imgui.TextSameLine("Patch Type: \0"u8);
                            _imgui.Text(_harmonyPatchTypeNames[harmonyPatchType]);
                        }

                        RenderCode(method);

                        _imgui.Unindent();
                    }
                }

                if (stacktrace.OriginalMethod is not null)
                {
                    var moduleId3 = stacktrace.OriginalMethod.ModuleId ?? "UNKNOWN";
                    var pluginId3 = stacktrace.OriginalMethod.LoaderPluginId ?? "UNKNOWN";

                    _imgui.Text("Original Method:\0"u8);

                    _imgui.Bullet();
                    _imgui.Indent();
                    _imgui.TextSameLine("Method: \0"u8);
                    _imgui.Text(stacktrace.OriginalMethod.MethodFullDescription);

                    if (moduleId3 != "UNKNOWN") _imgui.RenderId("Module Id:\0"u8, moduleId3);
                    if (pluginId3 != "UNKNOWN") _imgui.RenderId("Plugin Id:\0"u8, pluginId3);

                    RenderCode(stacktrace.OriginalMethod);

                    _imgui.Unindent();
                }

                _imgui.TreePop();
            }
        }
    }
}