using Bannerlord.ButterLib.CrashReportWindow.ImGui;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using ImGuiNET;

using System;
using System.Collections.Generic;
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

/// <summary>
/// This is an _almost_ zero allocation Crash Report Renderer.
/// We allocate a few bytes per second because of implecit Span{} casts and FrozenDictionary finds.
/// </summary>
internal partial class ImGuiRenderer
{
    private static void SetNestedDictionary<TDictionary, TKey, TNestedKey, TValue>(TDictionary methodDict, TKey key, TNestedKey nestedKey, TValue value)
        where TDictionary : IDictionary<TKey, Dictionary<TNestedKey, TValue>>, new() where TNestedKey : notnull
    {
        if (!methodDict.TryGetValue(key, out var nestedDict))
            methodDict[key] = (nestedDict = new Dictionary<TNestedKey, TValue>());
        nestedDict[nestedKey] = value;
    }

    private static int Clamp(int n, int min, int max)
    {
        if (n < min) return min;
        if (n > max) return max;
        return n;
    }
    private static int Clamp<TEnum>(TEnum n, TEnum min, TEnum max) where TEnum : Enum
    {
        var nInt = Unsafe.As<TEnum, int>(ref n);
        var minInt = Unsafe.As<TEnum, int>(ref min);
        var maxInt = Unsafe.As<TEnum, int>(ref max);

        if (nInt < minInt) return minInt;
        if (nInt > maxInt) return maxInt;
        return nInt;
    }

    private readonly CmGui _imgui;
    private readonly CrashReportModel _crashReport;
    private readonly IList<LogSource> _logSources;
    private readonly Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> _upload;
    private readonly Action _onClose;

    private byte[] _loadedPluginsTitle = Array.Empty<byte>();

    public ImGuiRenderer(CmGui imgui, CrashReportModel crashReport, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload, Action onClose)
    {
        _imgui = imgui;
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
        InitializeLogFiles();
    }

    private void InitializeMain()
    {
        _loadedPluginsTitle = UnsafeHelper.ToUtf8Array($"Loaded {_crashReport.Metadata.LoaderPluginProviderName} Plugins");
    }

    public void Render()
    {
        var viewPort = _imgui.GetMainViewport();
        _imgui.SetNextWindowPos(in viewPort.WorkPos);
        _imgui.SetNextWindowSize(in viewPort.WorkSize);
        _imgui.SetNextWindowViewport(viewPort.ID);

        _imgui.StyleColorsLight();
        var style = _imgui.GetStyle();
        var colors = style.Colors;
        colors[(int) ImGuiCol.Button] = Primary;
        colors[(int) ImGuiCol.ButtonHovered] = Primary2;
        colors[(int) ImGuiCol.ButtonActive] = Primary3;
        colors[(int) ImGuiCol.HeaderHovered] = Primary2;
        colors[(int) ImGuiCol.HeaderActive] = Primary3;
        colors[(int) ImGuiCol.FrameBgHovered] = Primary;
        colors[(int) ImGuiCol.FrameBgActive] = Primary2;
        colors[(int) ImGuiCol.CheckMark] = Primary3;

        if (_imgui.Begin("Crash Report\0"u8, in Background, ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
        {
            RenderSummary();

            _imgui.NewLine();
            if (_imgui.BeginChild("Exception", in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Exception\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderExceptionRecursively(_crashReport.Exception, 0);
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Enhanced Stacktrace\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Enhanced Stacktrace"))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderEnhancedStacktrace();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Involved Modules and Plugins\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Involved Modules and Plugins\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderInvolvedModulesAndPlugins();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Installed Modules\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Installed Modules\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderInstalledModules();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild(_loadedPluginsTitle, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode(_loadedPluginsTitle))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderLoadedLoaderPlugins();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Assemblies\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Assemblies\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderAssemblies();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Harmony Patches\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Harmony Patches\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderHarmonyPatches();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            if (_imgui.BeginChild("Log Files\0"u8, in Zero2, in White, ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.None))
            {
                _imgui.SetWindowFontScale(2);
                if (_imgui.TreeNode("Log Files\0"u8))
                {
                    _imgui.SetWindowFontScale(1);
                    RenderLogFiles();
                }
                _imgui.TreePop();
                _imgui.SetWindowFontScale(1);
            }
            _imgui.EndChild();

            _imgui.End();
        }
    }
}