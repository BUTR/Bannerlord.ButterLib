using Bannerlord.BLSE;
using Bannerlord.ButterLib.ExceptionHandler.Extensions;
using Bannerlord.ButterLib.ExceptionHandler.Utils;
using Bannerlord.ButterLib.Logger;

using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;
using BUTR.CrashReport.Renderer.ImGui;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

#if NET472 || (NET6_0 && WINDOWS)
using BUTR.CrashReport.Renderer.WinForms;
#endif

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

        var filter = new StacktraceFilter(BEWPatch.FinalizerMethod);
        var helper = new CrashReportInfoHelper();
        var harmonyProvider = new HarmonyProvider();
        var crashReportRendererUtilities = new CrashReportRendererUtilities();

        var crashReport = CrashReportInfo.Create(exception, metadata, filter, helper, helper, helper, harmonyProvider);
        var crashReportModel = CrashReportInfo.ToModel(crashReport, helper, helper, helper, helper, helper, helper);
        var logSources = GetLogSources().ToArray();
        try
        {
            CrashReportImGui.ShowAndWait(crashReportModel, logSources, crashReportRendererUtilities);
        }
        catch (Exception ex)
        {
            try
            {
#if NET472 || (NET6_0 && WINDOWS)

                var forms = new CrashReportWinForms(crashReportModel, logSources, crashReportRendererUtilities);
                forms.ShowDialog();
#endif
            }
            catch (Exception ex2)
            {
                throw new AggregateException(ex, ex2);
            }
        }
    }

    private static IEnumerable<LogSource> GetLogSources()
    {
        var now = DateTimeOffset.Now;

        foreach (var logSource in ButterLibSubModule.ServiceProvider?.GetService<IEnumerable<ILogSource>>() ?? [])
        {
            switch (logSource)
            {
                case IFileLogSource(var name, var path, var sinks) when File.Exists(path):
                {
                    const string MutexNameSuffix = ".serilog";
                    var mutexName = $"{Path.GetFullPath(path).Replace(Path.DirectorySeparatorChar, ':')}{MutexNameSuffix}";
                    var mutex = new Mutex(false, mutexName);

                    foreach (var flushableFileSink in sinks)
                        flushableFileSink.FlushToDisk();

                    var logs = ImmutableArray.CreateBuilder<LogEntry>();
                    try
                    {
                        if (!mutex.WaitOne(TimeSpan.FromSeconds(5)))
                            break;

                        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        var reader = new ReverseTextReader(stream);
                        var counter = 0;
                        while (counter < 2000 && reader.ReadLine() is { } line)
                        {
                            var idxDateStart = line.IndexOf('[') + 1;
                            var idxDateEnd = line.IndexOf(']');
                            if (idxDateStart == -1 || idxDateEnd == -1) break;

                            if (!DateTimeOffset.TryParse(line.Substring(idxDateStart, idxDateEnd - idxDateStart), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.RoundtripKind, out var date))
                                break;
                            if (date - now > TimeSpan.FromMinutes(60))
                                break;

                            var idxTypeStart = line.IndexOf('[', idxDateEnd + 1) + 1;
                            var idxTypeEnd = line.IndexOf(']', idxDateEnd + 1);
                            if (idxTypeStart == -1 || idxTypeEnd == -1) break;

                            var idxLevelStart = line.IndexOf('[', idxTypeEnd + 1) + 1;
                            var idxLevelEnd = line.IndexOf(']', idxTypeEnd + 1);
                            if (idxLevelStart == -1 || idxLevelEnd == -1) break;

                            logs.Add(new()
                            {
                                Date = date,
                                Type = line.Substring(idxTypeStart, idxTypeEnd - idxTypeStart),
                                Level = line.Substring(idxLevelStart, idxLevelEnd - idxLevelStart) switch
                                {
                                    "FTL" => LogLevel.Fatal,
                                    "ERR" => LogLevel.Error,
                                    "WRN" => LogLevel.Warning,
                                    "INF" => LogLevel.Information,
                                    "DBG" => LogLevel.Debug,
                                    "VRB" => LogLevel.Verbose,
                                    _ => LogLevel.None,
                                },
                                Message = line.Substring(idxLevelEnd + 3),
                            });
                            counter++;
                        }
                    }
                    catch (Exception) { /* ignore */ }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }

                    yield return new LogSource
                    {
                        Name = name,
                        Logs = logs.ToArray(),
                        AdditionalMetadata = Array.Empty<MetadataModel>(),
                    };

                    break;
                }
            }
        }
    }
}