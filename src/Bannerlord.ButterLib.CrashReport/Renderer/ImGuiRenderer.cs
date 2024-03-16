using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

internal partial class ImGuiRenderer
{
    private static readonly Vector4 Black = FromColor(0, 0, 0, 255);
    private static readonly Vector4 White = FromColor(255, 255, 255, 255);
    private static readonly Vector4 Red = FromColor(255, 0, 0, 255);
    private static readonly Vector4 Orange = FromColor(255, 165, 0, 255);
    
    private static readonly Vector4 Background = FromColor(236, 236, 236, 255);
    private static readonly Vector4 Plugin = FromColor(255, 255, 224, 255);
    private static readonly Vector4 OfficialModule = FromColor(244, 252, 220, 255);
    private static readonly Vector4 UnofficialModule = FromColor(255, 255, 224, 255);
    private static readonly Vector4 ExternalModule = FromColor(255, 255, 224, 255);
    private static readonly Vector4 SubModule = FromColor(248, 248, 231, 255);
    
    private static Vector4 FromColor(byte r, byte g, byte b, byte a) => new((float) r / 255f, (float) g / 255f, (float) b / 255f, (float) a / 255f);

    private static void If(bool value, Action action)
    {
        if (value) action();
    }

    private static string GetEnumDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        return value.GetType().GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
    }
    
    private static bool InputText(string label, ref string input)
    {
        return ImGui.InputText(label, ref input, ushort.MaxValue, ImGuiInputTextFlags.ReadOnly);
    }
    
    private static bool InputTextMultiline(string label, ref string input, int lineCount)
    {
        ImGui.PushStyleColor(ImGuiCol.FrameBg, Vector4.Zero);
        var result = ImGui.InputTextMultiline(label, ref input, ushort.MaxValue, new Vector2(-1, ImGui.GetTextLineHeight() * (lineCount + 2)), ImGuiInputTextFlags.ReadOnly);
        ImGui.PopStyleColor();
        
        return result;
    }

    private static void TextSameLine(string value)
    {
        ImGui.Text(value);
        ImGui.SameLine(0, 0);
    }

    private static void TextColoredSameLine(Vector4 col, string fmt)
    {
        ImGui.TextColored(col, fmt);
        ImGui.SameLine(0, 0);
    }

    private static bool CheckboxSameLine(string label, ref bool v)
    {
        var result = ImGui.Checkbox(label, ref v);
        ImGui.SameLine(0, 0);
        return result;
    }

    private static bool SmallButtonSameLine(string label)
    {
        var result = ImGui.SmallButton(label);
        ImGui.SameLine(0, 0);
        return result;
    }
    
    private static void RenderId(string title,  string id)
    {
        ImGui.Text(title);
        ImGui.SameLine();
        ImGui.SmallButton(id);
    }
    
    private readonly CrashReportModel _crashReport;
    private readonly ICollection<LogSource> _logSources;
    private readonly Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> _upload;
    private readonly Action _close;

    public ImGuiRenderer(CrashReportModel crashReport, ICollection<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload, Action close)
    {
        _crashReport = crashReport;
        _logSources = logSources;
        _upload = upload;
        _close = close;
        
        InitializeExceptionRecursively(crashReport.Exception, 0);
    }


    public void Render()
    {
        var viewPort = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewPort.WorkPos);
        ImGui.SetNextWindowSize(viewPort.WorkSize);
        ImGui.SetNextWindowViewport(viewPort.ID);
        
        ImGui.StyleColorsLight();
        ImGui.PushStyleColor(ImGuiCol.WindowBg, Background);
        if (ImGui.Begin("Crash Report", ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
        {
            ImGui.PopStyleColor();
            
            RenderSummary();
            
            ImGui.NewLine();
            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Exception", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Exception"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderExceptionRecursively(_crashReport.Exception, 0);
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Enhanced Stacktrace", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Enhanced Stacktrace"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderEnhancedStacktrace();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Involved Modules and Plugins", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Involved Modules and Plugins"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderInvolvedModulesAndPlugins();
                } 
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Installed Modules", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Installed Modules"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderInstalledModules();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            var loadedPlugins = $"Loaded {_crashReport.Metadata.LoaderPluginProviderName} Plugins";
            if (ImGui.BeginChild(loadedPlugins, Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode(loadedPlugins))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderLoadedLoaderPlugins();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Assemblies", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Assemblies"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderAssemblies();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Harmony Patches", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Harmony Patches"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderHarmonyPatches();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, White);
            if (ImGui.BeginChild("Log Files", Vector2.Zero, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (ImGui.TreeNode("Log Files"))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderLogFiles();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            ImGui.End();
        }
        ImGui.StyleColorsDark();
    }
}