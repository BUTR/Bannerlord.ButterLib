using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using BUTR.CrashReport.Models;

using System;
using System.Linq;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private byte[][] _exceptionsUtf8 = Array.Empty<byte[]>();
    private EnhancedStacktraceFrameModel?[] _stacktracesUtf8 = Array.Empty<EnhancedStacktraceFrameModel>();
    private byte[][] _levelInputIdUtf8 = Array.Empty<byte[]>();
    private int[] _callstackLineCount = Array.Empty<int>();

    private void InitializeExceptionRecursively()
    {
        var level = 0;
        var curr = _crashReport.Exception;
        while (curr is not null)
        {
            level++;
            curr = curr.InnerException;
        }
        _exceptionsUtf8 = new byte[level][];
        _stacktracesUtf8 = new EnhancedStacktraceFrameModel?[level];
        _levelInputIdUtf8 = Enumerable.Range(0, level).Select(x =>
        {
            var arr = "##stacktrace_00\0"u8.ToArray();
            arr[arr.Length - 2] = x < 10 ? (byte) (x + '0') : (byte) (x / 10 + '0');
            arr[arr.Length - 1] = (byte) (x % 10 + '0');
            return arr;
        }).ToArray();
        _callstackLineCount = new int[level];

        level = 0;
        curr = _crashReport.Exception;
        while (curr is not null)
        {
            var callStackLines = curr.CallStack.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x).ToArray();

            var sb = new StringBuilder();
            for (var i = 0; i < callStackLines.Length; i++)
            {
                sb.Append($"{" ".PadLeft(i > 98 ? 1 : i > 8 ? 2 : 3)}{i + 1}.{callStackLines[i].Trim()}");
                if (i < callStackLines.Length - 1) sb.AppendLine();
            }

            _exceptionsUtf8[level] = UnsafeHelper.ToUtf8Array(sb.ToString());

            var fistCallstackLine = callStackLines[0].Trim();
            _stacktracesUtf8[level] = _crashReport.EnhancedStacktrace.FirstOrDefault(x => fistCallstackLine == $"at {x.FrameDescription}");

            _callstackLineCount[level] = callStackLines.Length;

            level++;
            curr = curr.InnerException;
        }
    }

    private void RenderExceptionRecursively(ExceptionModel? ex, int level)
    {
        if (ex is null) return;

        var moduleId = _stacktracesUtf8[level]?.ExecutingMethod.ModuleId ?? "UNKNOWN";
        var sourceModuleId = ex.SourceModuleId ?? "UNKNOWN";

        var pluginId = _stacktracesUtf8[level]?.ExecutingMethod.LoaderPluginId ?? "UNKNOWN";
        var sourcePluginId = ex.SourceLoaderPluginId ?? "UNKNOWN";

        _imgui.Text("Exception Information:\0"u8);

        if (moduleId != "UNKNOWN") _imgui.RenderId("Potential Module Id:\0"u8, moduleId);
        if (sourceModuleId != "UNKNOWN") _imgui.RenderId("Potential Source Module Id:\0"u8, sourceModuleId);
        if (pluginId != "UNKNOWN") _imgui.RenderId("Potential Plugin Id:\0"u8, pluginId);
        if (sourcePluginId != "UNKNOWN") _imgui.RenderId("Potential Source Plugin Id:\0"u8, sourcePluginId);

        _imgui.TextSameLine("Type: \0"u8);
        _imgui.Text(ex.Type);

        if (!string.IsNullOrWhiteSpace(ex.Message))
        {
            _imgui.TextSameLine("Message: \0"u8);
            _imgui.Text(ex.Message);
        }

        if (!string.IsNullOrWhiteSpace(ex.CallStack))
        {
            _imgui.Text("Stacktrace:\0"u8);
            _imgui.Indent();
            _imgui.InputTextMultiline(_levelInputIdUtf8[level], _exceptionsUtf8[level], _callstackLineCount[level]);
            _imgui.Unindent();
        }

        if (ex.InnerException is not null)
        {
            _imgui.Text("Inner Exception:\0"u8);
            _imgui.Indent();
            RenderExceptionRecursively(ex.InnerException, level + 1);
            _imgui.Unindent();
        }
    }
}