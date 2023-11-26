using Bannerlord.BLSE;

using BUTR.CrashReport;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bannerlord.ButterLib.ExceptionHandler;

[BLSEExceptionHandler]
public static class ExceptionReporter
{
    private static void OnException(Exception exception) => Show(exception);

    public static void Show(Exception exception)
    {
        if (BEWPatch.SuppressedExceptions.Contains(BEWPatch.ExceptionIdentifier.FromException(exception)))
        {
            BEWPatch.SuppressedExceptions.Remove(BEWPatch.ExceptionIdentifier.FromException(exception));
            return;
        }

        var metadata = new Dictionary<string, string>
        {
            {"METADATA:TW_ConfigName", TaleWorlds.Library.Common.ConfigName},
        };
        if (Process.GetCurrentProcess().ParentProcess() is { } pProcess)
        {
            metadata.Add("Parent_Process_Name", pProcess.ProcessName);
            metadata.Add("Parent_Process_File_Version", pProcess.MainModule?.FileVersionInfo.FileVersion ?? "0");
        }

        BuildAndShow(new CrashReportInfo(exception, new CrashReportHelper(), metadata));
    }

    public static void BuildAndShow(CrashReportInfo crashReport)
    {
#if !NETSTANDARD2_0_OR_GREATER
        using var form = new Bannerlord.ButterLib.ExceptionHandler.WinForms.HtmlCrashReportForm(crashReport);
        form.ShowDialog();
#endif
    }
}