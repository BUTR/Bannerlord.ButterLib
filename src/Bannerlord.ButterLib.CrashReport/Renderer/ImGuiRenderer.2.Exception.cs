using System;
using System.Linq;
using System.Numerics;
using System.Text;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private string[] _exceptions;
    private string[] _fistCallstackLines;
    
    private void InitializeExceptionRecursively(ExceptionModel? ex, int level)
    {
        if (level == 0)
        {
            var len = 0;
            var curr = ex;
            while (curr is not null)
            {
                len++;
                curr = curr.InnerException;
            }
            _exceptions = new string[len];
            _fistCallstackLines = new string[len];
        }
        
        if (ex is null) return;

        var callStackLines = ex.CallStack.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Select(x => x).ToArray();
        
        var sb = new StringBuilder();
        for (var i = 0; i < callStackLines.Length; i++)
        {
            sb.Append($"{i + 1}.{" ".PadLeft(i > 98 ? 1 : i > 8 ? 2 : 3)}{callStackLines[i].Trim()}");
            if (i < callStackLines.Length - 1) sb.AppendLine();
        }
        _exceptions[level] = sb.ToString();
        
        _fistCallstackLines[level] = callStackLines[0].Trim();
        
        InitializeExceptionRecursively(ex.InnerException, level + 1);
    }

    private void RenderExceptionRecursively(ExceptionModel? ex, int level)
    {
        if (ex is null) return;

        var firstCallStackLine = _fistCallstackLines[level];
        var stacktrace = _crashReport.EnhancedStacktrace.FirstOrDefault(x => firstCallStackLine == $"at {x.FrameDescription}");

        var moduleId = stacktrace?.ExecutingMethod.ModuleId ?? "UNKNOWN";
        var sourceModuleId = ex.SourceModuleId ?? "UNKNOWN";
        
        var pluginId = stacktrace?.ExecutingMethod.LoaderPluginId ?? "UNKNOWN";
        var sourcePluginId = ex.SourceLoaderPluginId ?? "UNKNOWN";

        ImGui.Text("Exception Information:");

        If(moduleId != "UNKNOWN", () => RenderId("Potential Module Id:", moduleId));
        If(sourceModuleId != "UNKNOWN", () => RenderId("Potential Source Module Id:", sourceModuleId));
        If(pluginId != "UNKNOWN", () => RenderId("Potential Plugin Id:", pluginId));
        If(sourcePluginId != "UNKNOWN", () => RenderId("Potential Source Plugin Id:", sourcePluginId));
        
        ImGui.Text($"Type: {ex.Type}");
        
        If(!string.IsNullOrWhiteSpace(ex.Message), () => ImGui.Text($"Message: {ex.Message}"));
        
        If(!string.IsNullOrWhiteSpace(ex.CallStack), () =>
        {
            ImGui.Text("Stacktrace:");
            ImGui.Indent();
            InputTextMultiline($"##stacktrace_{level}", ref _exceptions[level], ex.CallStack.Count(x => x == '\n'));
            ImGui.Unindent();
        });

        If(ex.InnerException is not null, () =>
        {
            ImGui.Text("Inner Exception:");
            ImGui.Indent();
            RenderExceptionRecursively(ex.InnerException, level + 1);
            ImGui.Unindent();
        });
    }
}