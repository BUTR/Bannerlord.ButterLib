using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using ImGuiNET;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

// Generic rules to avoid allocations:
// 1. Split any interpolated string into a hardcoded ("()"u8) and dynamic (entry.Text) parts and render them separately
// 2. Use custom equality comparer if a key doesn't implement IEquatable<T> in dictionaries
// 3. Use a FrozenDictionary instead of a standard. Set the EqualityComparer in the Dictionary
// 4. Cache all dynamic strings as utf8 byte array
// 5. Don't forget to add a null termination to your utf8 byte array
// The only allocations left are the FrozenDictionary finds. Can't do much now.

internal partial class ImGuiRenderer
{
    private static readonly Vector2 Zero2 = Vector2.Zero;
    private static readonly Vector3 Zero3 = Vector3.Zero;
    private static readonly Vector4 Zero4 = Vector4.Zero;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector4 FromColor(byte r, byte g, byte b, byte a) => new((float) r / 255f, (float) g / 255f, (float) b / 255f, (float) a / 255f);


    private static string GetEnumDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        return value.GetType().GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
    }

    private static void SetNestedDictionary<TDictionary, TKey, TNestedKey, TValue>(TDictionary methodDict, TKey key, TNestedKey nestedKey, TValue value)
        where TDictionary : IDictionary<TKey, Dictionary<TNestedKey, TValue>>, new() where TNestedKey : notnull
    {
        if (!methodDict.TryGetValue(key, out var codeDict))
            methodDict[key] = (codeDict = new Dictionary<TNestedKey, TValue>());
        codeDict[nestedKey] = value;
    }

    public static int Clamp(int n, int min, int max)
    {
        if (n < min) return min;
        if (n > max) return max;
        return n;
    }
    public static int Clamp<TEnum>(TEnum n, TEnum min, TEnum max) where TEnum : Enum
    {
        var nInt = Unsafe.As<TEnum, int>(ref n);
        var minInt = Unsafe.As<TEnum, int>(ref min);
        var maxInt = Unsafe.As<TEnum, int>(ref max);

        if (nInt < minInt) return minInt;
        if (nInt > maxInt) return maxInt;
        return nInt;
    }

    private readonly CrashReportModel _crashReport;
    private readonly IList<LogSource> _logSources;
    private readonly Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> _upload;
    private readonly Action _onClose;

    private byte[] _loadedPluginsTitle = Array.Empty<byte>();

    public ImGuiRenderer(CrashReportModel crashReport, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload, Action onClose)
    {
        _crashReport = crashReport;
        _logSources = logSources;
        _upload = upload;
        _onClose = onClose;

        InitializeMain();
        InitializeExceptionRecursively();
        InitializeCodeLines();
        InitializeInvolved();
        InitializeInstalledModules();
        InitializeInstalledLoaderPlugins();
        InitializeAssemblies();
        InitializeHarmonyPatches();
    }

    private void InitializeMain()
    {
        _loadedPluginsTitle = UnsafeHelper.ToUtf8Array($"Loaded {_crashReport.Metadata.LoaderPluginProviderName} Plugins");
    }

    public void Render()
    {
        var viewPort = JmGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewPort.WorkPos);
        ImGui.SetNextWindowSize(viewPort.WorkSize);
        ImGui.SetNextWindowViewport(viewPort.ID);

        ImGui.StyleColorsLight();
        JmGui.PushStyleColor(ImGuiCol.WindowBg, in Background);
        if (JmGui.Begin("Crash Report\0"u8, ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
        {
            ImGui.PopStyleColor();

            RenderSummary();

            ImGui.NewLine();
            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (ImGui.BeginChild("Exception", Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Exception\0"u8))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderExceptionRecursively(_crashReport.Exception, 0);
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Enhanced Stacktrace\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
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

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Involved Modules and Plugins\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Involved Modules and Plugins\0"u8))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderInvolvedModulesAndPlugins();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Installed Modules\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Installed Modules\0"u8))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderInstalledModules();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild(_loadedPluginsTitle, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode(_loadedPluginsTitle))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderLoadedLoaderPlugins();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Assemblies\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Assemblies\0"u8))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderAssemblies();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Harmony Patches\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Harmony Patches\0"u8))
                {
                    ImGui.SetWindowFontScale(1);
                    RenderHarmonyPatches();
                }
                ImGui.TreePop();
                ImGui.SetWindowFontScale(1);
            }
            ImGui.EndChild();

            JmGui.PushStyleColor(ImGuiCol.ChildBg, in White);
            if (JmGui.BeginChild("Log Files\0"u8, in Zero2, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                ImGui.PopStyleColor();
                ImGui.SetWindowFontScale(2);
                if (JmGui.TreeNode("Log Files\0"u8))
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