using Bannerlord.ButterLib.ExceptionHandler;

using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashUploader
{
    internal class BUTRCrashUploader : ICrashUploader
    {
        public async Task<string?> UploadAsync(CrashReport crashReport)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var uploadUrlAttr = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "BUTRUploadUrl");
            if (uploadUrlAttr is null)
                return null;

            var htmlReport = HtmlBuilder.Build(crashReport);
            var data = Encoding.ASCII.GetBytes(htmlReport);

            var httpWebRequest = WebRequest.CreateHttp($"{uploadUrlAttr.Value}?id={crashReport.Id}");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "text/html";
            httpWebRequest.ContentLength = data.Length;
            httpWebRequest.UserAgent = $"ButterLib CrashUploader v{assembly.GetName().Version}";

            using var requestStream = await httpWebRequest.GetRequestStreamAsync().ConfigureAwait(false);
            await requestStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);

            if (await httpWebRequest.GetResponseAsync().ConfigureAwait(false) is HttpWebResponse response && response.GetResponseStream() is { } stream)
            {
                using var responseReader = new StreamReader(stream);
                return await responseReader.ReadLineAsync().ConfigureAwait(false);
            }

            return null;
        }
    }
}