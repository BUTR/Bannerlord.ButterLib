using System.Linq;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private void RenderLogFiles()
    {
        foreach (var logSource in _logSources)
        {
            if (ImGui.TreeNodeEx(logSource.Name, ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (logSource.Logs.Count == 0) continue;

                var longestType = logSource.Logs.Max(x => x.Type.Length);
                foreach (var logEntry in logSource.Logs)
                {
                    var toAppend = (longestType - logEntry.Type.Length) + 1;

                    TextSameLine($"{logEntry.Date:u} [{logEntry.Type}]{string.Empty.PadRight(toAppend)}: [");
                    TextColoredSameLine(logEntry.Level switch
                    {
                        LogLevel.Fatal => Red,
                        LogLevel.Error => Red,
                        LogLevel.Warning => Orange,
                        _ => Black
                    }, logEntry.Level switch
                    {
                        LogLevel.Verbose => "VRB",
                        LogLevel.Debug => "DBG",
                        LogLevel.Information => "INF",
                        LogLevel.Warning => "WRN",
                        LogLevel.Error => "ERR",
                        LogLevel.Fatal => "FTL",
                        _ => "   ",
                    });
                    ImGui.Text($"]: {logEntry.Message}");
                }
                ImGui.TreePop();
            }
        }
    }
}