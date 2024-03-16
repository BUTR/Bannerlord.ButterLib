using Bannerlord.ButterLib.CrashReportWindow.Extensions;

using BUTR.CrashReport.Models;

using HonkPerf.NET.RefLinq;

using ImGuiNET;

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

    private void RenderLogFiles()
    {
        for (var i = 0; i < _logSources.Count; i++)
        {
            var logSource = _logSources[i];
            if (JmGui.TreeNode(logSource.Name, ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (logSource.Logs.Count == 0) continue;

                var longestTypeLength = logSource.Logs.ToRefLinq().Select(x => x.Type.Length).Max();
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

                    JmGui.TextSameLine(ref date);
                    JmGui.TextSameLine(" [\0"u8);
                    JmGui.TextSameLine(logEntry.Type);
                    JmGui.TextSameLine("]\0"u8);
                    JmGui.PadRight(toAppend);
                    JmGui.TextSameLine("[\0"u8);
                    JmGui.TextColoredSameLine(in color, _logLevelNamesUtf8[level]);
                    JmGui.TextSameLine("]: \0"u8);
                    JmGui.Text(logEntry.Message);
                }

                JmGui.TreePop();
            }
        }
    }
}