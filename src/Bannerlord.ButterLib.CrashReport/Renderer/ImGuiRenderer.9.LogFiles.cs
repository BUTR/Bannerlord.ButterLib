using Bannerlord.ButterLib.CrashReportWindow.Extensions;

using BUTR.CrashReport.Models;

using HonkPerf.NET.RefLinq;

using ImGuiNET;

using System;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static readonly byte[][] _logLevelNamesUtf8 =
    [
        "   \0"u8.ToArray(), // None
        "VRB\0"u8.ToArray(), // Verbose
        "DBG\0"u8.ToArray(), // Debug
        "INF\0"u8.ToArray(), // Information
        "WRN\0"u8.ToArray(), // Warning
        "ERR\0"u8.ToArray(), // Error
        "FTL\0"u8.ToArray(), // Fatal
    ];

    private int[] _logSourceMaxTypeLengths = Array.Empty<int>();

    private void InitializeLogFiles()
    {
        _logSourceMaxTypeLengths = new int[_logSources.Count];
        for (var i = 0; i < _logSources.Count; i++)
        {
            if (_logSources[i].Logs.Count == 0) _logSourceMaxTypeLengths[i] = 0;
            else _logSourceMaxTypeLengths[i] = _logSources[i].Logs.ToRefLinq().Select(x => x.Type.Length).Max();
        }
    }

    private void RenderLogFiles()
    {
        for (var i = 0; i < _logSources.Count; i++)
        {
            var logSource = _logSources[i];
            if (_imgui.TreeNode(logSource.Name, ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (logSource.Logs.Count == 0) continue;

                var longestTypeLength = _logSourceMaxTypeLengths[i];
                for (var j = 0; j < logSource.Logs.Count; j++)
                {
                    var logEntry = logSource.Logs[j];
                    var toAppend = (longestTypeLength - logEntry.Type.Length) + 2;

                    var date = logEntry.Date;
                    var color = logEntry.Level switch
                    {
                        LogLevel.Fatal => Red,
                        LogLevel.Error => Red,
                        LogLevel.Warning => Orange,
                        _ => Black
                    };
                    var level = Clamp(logEntry.Level, LogLevel.None, LogLevel.Fatal);

                    _imgui.TextSameLine(ref date);
                    _imgui.TextSameLine(" [\0"u8);
                    _imgui.TextSameLine(logEntry.Type);
                    _imgui.TextSameLine("]\0"u8);
                    _imgui.PadRight(toAppend);
                    _imgui.TextSameLine("[\0"u8);
                    _imgui.TextColoredSameLine(in color, _logLevelNamesUtf8[level]);
                    _imgui.TextSameLine("]: \0"u8);
                    _imgui.Text(logEntry.Message);
                }

                _imgui.TreePop();
            }
        }
    }
}