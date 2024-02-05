using BUTR.CrashReport.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashUploader;

internal interface ICrashUploader
{
    Task<CrashUploaderResult> UploadAsync(CrashReportModel crashReportModel, IEnumerable<LogSource> logSources);
}