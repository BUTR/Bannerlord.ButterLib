using BUTR.CrashReport.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashUploader;

internal class BUTRCrashUploader : ICrashUploader
{
    public sealed record CrashReportUploadBody(CrashReportModel CrashReport, IEnumerable<LogSource> LogSources);

    public async Task<CrashUploaderResult> UploadAsync(CrashReportModel crashReportModel, IEnumerable<LogSource> logSources)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var uploadUrlAttr = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "BUTRUploadUrl");
            if (uploadUrlAttr is null)
                return CrashUploaderResult.MetadataNotFound();

            // Do not send a minidump
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new CrashReportUploadBody(crashReportModel, logSources)));

            var httpWebRequest = WebRequest.CreateHttp(uploadUrlAttr.Value);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = data.Length;
            httpWebRequest.UserAgent = $"ButterLib CrashUploader v{assembly.GetName().Version}";

            using var requestStream = await httpWebRequest.GetRequestStreamAsync().ConfigureAwait(false);
            await requestStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);

            using var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
            if (response is not HttpWebResponse httpWebResponse)
                return CrashUploaderResult.ResponseIsNotHttpWebResponse();

            if (httpWebResponse.StatusCode is not HttpStatusCode.OK or HttpStatusCode.Created)
                return CrashUploaderResult.WrongStatusCode(httpWebResponse.StatusCode.ToString());

            using var stream = response.GetResponseStream();
            if (stream is null)
                return CrashUploaderResult.ResponseStreamIsNull();

            using var responseReader = new StreamReader(stream);
            var result = await responseReader.ReadLineAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(result))
                return CrashUploaderResult.ResponseUrlIsNullOrEmpty();
            return CrashUploaderResult.Success(result);
        }
        catch (Exception e)
        {
            return CrashUploaderResult.FailedWithException(e.ToString());
        }
    }
}