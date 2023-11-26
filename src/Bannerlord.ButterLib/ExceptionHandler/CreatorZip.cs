using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.IO;
using System.Text;

namespace Bannerlord.ButterLib.ExceptionHandler;

internal static class CreatorZip
{
    public static void Create(CrashReportModel crashReport, Stream stream)
    {
        using var crashReportStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(crashReport, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter() }
        })));
        var logs = CreatorShared.GetLogSources();
        using var logsStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(logs, new JsonSerializerSettings
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
        using var archiveSteam = CrashReportArchiveRenderer.Build(crashReport, logs, crashReportStream, logsStream, miniDumpSteam, saveFileSteam, screenshotSteam);
        archiveSteam.Seek(0, SeekOrigin.Begin);
        archiveSteam.CopyTo(stream);
    }
}