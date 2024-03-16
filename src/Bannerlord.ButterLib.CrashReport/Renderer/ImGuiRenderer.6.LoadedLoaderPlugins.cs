using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using ImGuiNET;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private FrozenDictionary<string, byte[]> _loaderPluginIdUpdateInfoUtf8 = default!;

    private void InitializeInstalledLoaderPlugins()
    {
        var loaderPluginIdUpdateInfoUtf8 = new Dictionary<string, byte[]>();
        for (var i = 0; i < _crashReport.LoaderPlugins.Count; i++)
        {
            var loaderPlugin = _crashReport.LoaderPlugins[i];
            if (loaderPlugin.UpdateInfo is not null)
            {
                loaderPluginIdUpdateInfoUtf8[loaderPlugin.Id] = UnsafeHelper.ToUtf8Array(loaderPlugin.UpdateInfo.ToString());
            }
        }
        _loaderPluginIdUpdateInfoUtf8 = loaderPluginIdUpdateInfoUtf8.ToFrozenDictionary(StringComparer.Ordinal);
    }


    private void RenderLoadedLoaderPlugins()
    {
        if (_crashReport.LoaderPlugins.Count == 0) return;

        for (var i = 0; i < _crashReport.LoaderPlugins.Count; i++)
        {
            var loaderPlugin = _crashReport.LoaderPlugins[i];
            JmGui.PushStyleColor(ImGuiCol.ChildBg, in Plugin);
            if (JmGui.BeginChild(loaderPlugin.Id, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();

                if (ImGui.TreeNode(loaderPlugin.Id))
                {
                    JmGui.RenderId("Id:\0"u8, loaderPlugin.Id);

                    JmGui.TextSameLine("Name: \0"u8);
                    JmGui.Text(loaderPlugin.Name);

                    if (!string.IsNullOrEmpty(loaderPlugin.Version))
                    {
                        JmGui.TextSameLine("Version: \0"u8);
                        JmGui.Text(loaderPlugin.Version!);
                    }

                    if (loaderPlugin.UpdateInfo is not null)
                    {
                        JmGui.TextSameLine("Update Info: \0"u8);
                        JmGui.Text(_loaderPluginIdUpdateInfoUtf8[loaderPlugin.Id]);
                    }

                    ImGui.TreePop();
                }
            }

            ImGui.EndChild();
        }
    }
}