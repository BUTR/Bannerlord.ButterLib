using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;
using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport.Models;

using HonkPerf.NET.Core;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;

using ImGuiNET;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private FrozenDictionary<HarmonyPatchesModel, byte[]> _harmonyMethodNameFullUtf8 = default!;
    private FrozenDictionary<HarmonyPatchModel, byte[]> _harmonyBeforeUtf8 = default!;
    private FrozenDictionary<HarmonyPatchModel, byte[]> _harmonyAfterUtf8 = default!;

    private void InitializeHarmonyPatches()
    {
        var harmonyMethodNameFullUtf8 = new Dictionary<HarmonyPatchesModel, byte[]>();
        var harmonyBeforeUtf8 = new Dictionary<HarmonyPatchModel, byte[]>();
        var harmonyAfterUtf8 = new Dictionary<HarmonyPatchModel, byte[]>();

        for (var i = 0; i < _crashReport.HarmonyPatches.Count; i++)
        {
            var harmonyPatch = _crashReport.HarmonyPatches[i];
            var methodNameFull = !string.IsNullOrEmpty(harmonyPatch.OriginalMethodDeclaredTypeName)
                ? $"{harmonyPatch.OriginalMethodDeclaredTypeName}.{harmonyPatch.OriginalMethodName}"
                : harmonyPatch.OriginalMethodName ?? string.Empty;
            harmonyMethodNameFullUtf8[harmonyPatch] = UnsafeHelper.ToUtf8Array(methodNameFull);

            for (var j = 0; j < harmonyPatch.Patches.Count; j++)
            {
                var patch = harmonyPatch.Patches[j];
                for (var k = 0; k < patch.Before.Count; k++)
                {
                    var before = patch.Before[k];
                    harmonyBeforeUtf8[patch] = UnsafeHelper.ToUtf8Array(before);
                }

                for (var m = 0; m < patch.After.Count; m++)
                {
                    var after = patch.After[m];
                    harmonyAfterUtf8[patch] = UnsafeHelper.ToUtf8Array(after);
                }
            }
        }

        _harmonyMethodNameFullUtf8 = harmonyMethodNameFullUtf8.ToFrozenDictionary();
        _harmonyBeforeUtf8 = harmonyBeforeUtf8.ToFrozenDictionary();
        _harmonyAfterUtf8 = harmonyAfterUtf8.ToFrozenDictionary();
    }

    private void RenderHarmonyPatches(ReadOnlySpan<byte> name, RefLinqEnumerable<HarmonyPatchModel, Where<HarmonyPatchModel, PureValueDelegate<HarmonyPatchModel, bool>, IListEnumerator<HarmonyPatchModel>>> patches)
    {
        foreach (var patch in patches)
        {
            var moduleId = patch.ModuleId ?? "UNKNOWN";
            var pluginId = patch.LoaderPluginId ?? "UNKNOWN";

            ImGui.Bullet();
            JmGui.Text(name);
            ImGui.Indent();

            if (moduleId != "UNKNOWN") { JmGui.RenderId("Module Id:\0"u8, moduleId); ImGui.SameLine(0, 0); }
            if (pluginId != "UNKNOWN") { JmGui.RenderId("Plugin Id:\0"u8, pluginId); ImGui.SameLine(0, 0); }
            JmGui.TextSameLine(" Owner: \0"u8);
            JmGui.TextSameLine(patch.Owner);
            JmGui.TextSameLine(" Namespace: \0"u8);
            JmGui.TextSameLine(patch.Namespace);
            if (patch.Index != 0) { JmGui.TextSameLine(" Index: \0"u8); JmGui.TextSameLine(patch.Index); }
            if (patch.Priority != 400) { JmGui.TextSameLine(" Priority: \0"u8); JmGui.TextSameLine(patch.Index); }
            if (patch.Before.Count > 0) { JmGui.TextSameLine(" Before: \0"u8); JmGui.TextSameLine(_harmonyBeforeUtf8[patch]); }
            if (patch.After.Count > 0) { JmGui.TextSameLine(" After: \0"u8); JmGui.TextSameLine(_harmonyAfterUtf8[patch]); }
            JmGui.NewLine();

            ImGui.Unindent();
        }
    }

    private void RenderHarmonyPatches()
    {
        for (var i = 0; i < _crashReport.HarmonyPatches.Count; i++)
        {
            var harmonyPatch = _crashReport.HarmonyPatches[i];
            var methodNameFull = _harmonyMethodNameFullUtf8[harmonyPatch];

            if (JmGui.TreeNode(methodNameFull, ImGuiTreeNodeFlags.DefaultOpen))
            {
                RenderHarmonyPatches("Prefixes\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Prefix));
                RenderHarmonyPatches("Postfixes\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Postfix));
                RenderHarmonyPatches("Finalizers\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Finalizer));
                RenderHarmonyPatches("Transpilers\0"u8, harmonyPatch.Patches.ToRefLinq().Where(x => x.Type == HarmonyPatchType.Transpiler));
                JmGui.NewLine();

                JmGui.TreePop();
            }
        }
    }
}