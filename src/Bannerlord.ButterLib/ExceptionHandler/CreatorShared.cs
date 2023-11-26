using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Logger;

using BUTR.CrashReport.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.ExceptionHandler;

internal static class CreatorShared
{
    public static IEnumerable<LogSource> GetLogSources()
    {
        var now = DateTimeOffset.Now;

        foreach (var logSource in ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IEnumerable<ILogSource>>() ?? Enumerable.Empty<ILogSource>())
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
                                Level = line.Substring(idxLevelStart, idxLevelEnd - idxLevelStart),
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
                        Logs = logs.ToImmutable(),
                        AdditionalMetadata = ImmutableArray<MetadataModel>.Empty,
                    };

                    break;
                }
            }
        }
    }

    public static Stream GetMiniDump()
    {
        try
        {
            if (!MiniDump.TryDump(out var stream)) return Stream.Null;
            return stream;
        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }

    public static Stream GetSaveFile()
    {
        try
        {
            var gameSavesDirectory = new PlatformDirectoryPath(PlatformFileType.User, "Game Saves\\");
            // TODO: What to with Xbox version? No write time available
            var gameSavesPath = PlatformFileHelperPCExtended.GetDirectoryFullPath(gameSavesDirectory);
            if (string.IsNullOrEmpty(gameSavesPath)) return Stream.Null;

            var latestSaveFile = new DirectoryInfo(gameSavesPath).EnumerateFiles("*.sav", SearchOption.TopDirectoryOnly)
                .OrderByDescending(x => x.LastWriteTimeUtc)
                .FirstOrDefault();
            if (latestSaveFile is null) return Stream.Null;

            return latestSaveFile.OpenRead();
        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }

    public static Stream GetScreenshot()
    {
        try
        {
            var tempBmp = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.bmp");

            Utilities.TakeScreenshot(tempBmp);

#if !NETSTANDARD2_0
            using var image = System.Drawing.Image.FromFile(tempBmp);
            using var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1) { Param = { [0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L) } };

            var stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid), encoderParameters);
            return stream;
#else
            return Stream.Null;
#endif

        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }
}