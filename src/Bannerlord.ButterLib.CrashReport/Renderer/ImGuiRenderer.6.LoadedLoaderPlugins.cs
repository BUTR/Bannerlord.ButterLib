using System.Numerics;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private void RenderLoadedLoaderPlugins()
    {
        if (_crashReport.LoaderPlugins.Count == 0) return;

        foreach (var loaderPlugin in _crashReport.LoaderPlugins)
        {
            ImGui.PushStyleColor(ImGuiCol.ChildBg, Plugin);
            if (ImGui.BeginChild(loaderPlugin.Id, Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                
                if (ImGui.TreeNode(loaderPlugin.Id))
                {
                    RenderId("Id: ", loaderPlugin.Id);
                    ImGui.Text($"Name: {loaderPlugin.Name}");
                    If(!string.IsNullOrEmpty(loaderPlugin.Version), () => ImGui.Text($"Version: {loaderPlugin.Version}"));
                    If(!string.IsNullOrEmpty(loaderPlugin.UpdateInfo), () => ImGui.Text($"Update Info: {loaderPlugin.UpdateInfo}"));

                    ImGui.TreePop();
                }
            }
            ImGui.EndChild();
        }
    }
}