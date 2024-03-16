using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Bannerlord.BLSE;
using Bannerlord.ButterLib.CrashReportWindow;
using Bannerlord.ButterLib.CrashUploader;
using Bannerlord.ButterLib.Logger;
using BUTR.CrashReport.Models;
using Microsoft.Extensions.DependencyInjection;

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

        var logSources = GetLogSources().ToArray();
        CrashReportWindow.CrashReportWindow.ShowAndWait(exception, logSources, async (crashReport, logSources) =>
        {
            var crashUploader = ButterLibSubModule.ServiceProvider?.GetService<ICrashUploader>();
            if (crashUploader is null)
                return (false, "Critical Error: Failed to get the crash uploader!");

            var result = await crashUploader.UploadAsync(crashReport, logSources).ConfigureAwait(false);
            return result.Status switch
            {
                CrashUploaderStatus.Success => (true, result.Url ?? string.Empty),
                CrashUploaderStatus.MetadataNotFound => (false, $"Status: {result.Status}"),
                CrashUploaderStatus.ResponseIsNotHttpWebResponse => (false, $"Status: {result.Status}"),
                CrashUploaderStatus.ResponseStreamIsNull => (false, $"Status: {result.Status}"),
                CrashUploaderStatus.WrongStatusCode => (false, $"Status: {result.Status}\nStatusCode: {result.StatusCode}"),
                CrashUploaderStatus.FailedWithException => (false, $"Status: {result.Status}\nException: {result.Exception}"),
            };
        }, BEWPatch.FinalizerMethod);
    }
    
    private static IEnumerable<LogSource> GetLogSources()
    {
        var now = DateTimeOffset.Now;

        foreach (var logSource in ButterLibSubModule.ServiceProvider?.GetService<IEnumerable<ILogSource>>() ?? Enumerable.Empty<ILogSource>())
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