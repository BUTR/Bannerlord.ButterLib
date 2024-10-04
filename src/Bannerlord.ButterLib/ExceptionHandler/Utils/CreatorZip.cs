using BUTR.CrashReport.Models;
using BUTR.CrashReport.Renderer.Zip;

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
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter() }
        };

        using var crashReportStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(crashReport, settings)));
        using var logsStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(logSources, settings)));

        using var miniDumpSteam = CreatorShared.GetMiniDump();
        using var saveFileSteam = CreatorShared.GetSaveFile();
        using var screenshotSteam = CreatorShared.GetScreenshot();
        using var archiveSteam = CrashReportZip.Build(crashReportStream, logsStream, miniDumpSteam, saveFileSteam, screenshotSteam);
        archiveSteam.Seek(0, SeekOrigin.Begin);
        archiveSteam.CopyTo(stream);
    }
}