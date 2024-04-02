using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;
using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport.Models;

using HonkPerf.NET.Core;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;

using ImGuiNET;

using System;
using System.Collections.Generic;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private class HarmonyPatchesModelEqualityComparer : IEqualityComparer<HarmonyPatchesModel>
    {
        public static HarmonyPatchesModelEqualityComparer Instance { get; } = new();
        public bool Equals(HarmonyPatchesModel? x, HarmonyPatchesModel? y) => ReferenceEquals(x, y); // We can just reference compare here
        public int GetHashCode(HarmonyPatchesModel obj) => obj.GetHashCode();
    }

    private class HarmonyPatchModelEqualityComparer : IEqualityComparer<HarmonyPatchModel>
    {
        public static HarmonyPatchModelEqualityComparer Instance { get; } = new();
        public bool Equals(HarmonyPatchModel? x, HarmonyPatchModel? y) => ReferenceEquals(x, y); // We can just reference compare here
        public int GetHashCode(HarmonyPatchModel obj) => obj.GetHashCode();
    }

    private readonly Dictionary<HarmonyPatchesModel, byte[]> _harmonyMethodNameFullUtf8 = new(HarmonyPatchesModelEqualityComparer.Instance);
    private readonly Dictionary<HarmonyPatchModel, byte[]> _harmonyBeforeUtf8 = new(HarmonyPatchModelEqualityComparer.Instance);
    private readonly Dictionary<HarmonyPatchModel, byte[]> _harmonyAfterUtf8 = new(HarmonyPatchModelEqualityComparer.Instance);

    private void InitializeHarmonyPatches()
    {
        for (var i = 0; i < _crashReport.HarmonyPatches.Count; i++)
        {
            var harmonyPatch = _crashReport.HarmonyPatches[i];
            var methodNameFull = !string.IsNullOrEmpty(harmonyPatch.OriginalMethodDeclaredTypeName)
                ? $"{harmonyPatch.OriginalMethodDeclaredTypeName}.{harmonyPatch.OriginalMethodName}"
                : harmonyPatch.OriginalMethodName ?? string.Empty;
            _harmonyMethodNameFullUtf8[harmonyPatch] = UnsafeHelper.ToUtf8Array(methodNameFull);

            for (var j = 0; j < harmonyPatch.Patches.Count; j++)
            {
                var patch = harmonyPatch.Patches[j];
                for (var k = 0; k < patch.Before.Count; k++)
                {
                    var before = patch.Before[k];
                    _harmonyBeforeUtf8[patch] = UnsafeHelper.ToUtf8Array(before);
                }
                for (var m = 0; m < patch.After.Count; m++)
                {
                    var after = patch.After[m];
                    _harmonyAfterUtf8[patch] = UnsafeHelper.ToUtf8Array(after);
                }
            }
        }
    }

    private void RenderHarmonyPatches(ReadOnlySpan<byte> name, RefLinqEnumerable<HarmonyPatchModel, Where<HarmonyPatchModel, PureValueDelegate<HarmonyPatchModel, bool>, IListEnumerator<HarmonyPatchModel>>> patches)
    {
        foreach (var patch in patches)
        {
            var moduleId = patch.ModuleId ?? "UNKNOWN";
            var pluginId = patch.LoaderPluginId ?? "UNKNOWN";

            _imgui.Bullet();
            _imgui.Text(name);
            _imgui.Indent();

            if (moduleId != "UNKNOWN") { _imgui.RenderId("Module Id:\0"u8, moduleId); _imgui.SameLine(0, 0); }
            if (pluginId != "UNKNOWN") { _imgui.RenderId("Plugin Id:\0"u8, pluginId); _imgui.SameLine(0, 0); }
            _imgui.TextSameLine(" Owner: \0"u8);
            _imgui.TextSameLine(patch.Owner);
            _imgui.TextSameLine(" Namespace: \0"u8);
            _imgui.TextSameLine(patch.Namespace);
            if (patch.Index != 0) { _imgui.TextSameLine(" Index: \0"u8); _imgui.TextSameLine(patch.Index); }
            if (patch.Priority != 400) { _imgui.TextSameLine(" Priority: \0"u8); _imgui.TextSameLine(patch.Index); }
            if (patch.Before.Count > 0) { _imgui.TextSameLine(" Before: \0"u8); _imgui.TextSameLine(_harmonyBeforeUtf8[patch]); }
            if (patch.After.Count > 0) { _imgui.TextSameLine(" After: \0"u8); _imgui.TextSameLine(_harmonyAfterUtf8[patch]); }
            _imgui.NewLine();

            _imgui.Unindent();
        }
    }

    private void RenderHarmonyPatches()
    {
        for (var i = 0; i < _crashReport.HarmonyPatches.Count; i++)
        {
            var harmonyPatch = _crashReport.HarmonyPatches[i];
            var methodNameFull = _harmonyMethodNameFullUtf8[harmonyPatch];

            if (_imgui.TreeNode(methodNameFull, ImGuiTreeNodeFlags.DefaultOpen))
            {
                RenderHarmonyPatches("Prefixes\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Prefix));
                RenderHarmonyPatches("Postfixes\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Postfix));
                RenderHarmonyPatches("Finalizers\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Finalizer));
                RenderHarmonyPatches("Transpilers\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Transpiler));
                _imgui.NewLine();

                _imgui.TreePop();
            }
        }
    }
}