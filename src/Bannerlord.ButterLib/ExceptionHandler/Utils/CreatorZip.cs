using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bannerlord.ButterLib.ExceptionHandler.Utils;

internal static class CreatorZip
{
    public static void Create(CrashReportModel crashReport, ICollection<LogSource> logSources, Stream stream)
    {
        using var crashReportStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(crashReport, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter() }
        })));
        using var logsStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(logSources, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter() }
        })));

        using var miniDumpSteam = CreatorShared.GetMiniDump();
        using var saveFileSteam = CreatorShared.GetSaveFile();
        using var screenshotSteam = CreatorShared.GetScreenshot();
        using var archiveSteam = CrashReportArchiveRenderer.Build(crashReport, logSources, crashReportStream, logsStream, miniDumpSteam, saveFileSteam, screenshotSteam);
        archiveSteam.Seek(0, SeekOrigin.Begin);
        archiveSteam.CopyTo(stream);
    }
}