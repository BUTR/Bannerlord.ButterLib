using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.CrashUploader;
using Bannerlord.ButterLib.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.ExceptionHandler.WinForms
{
    public partial class HtmlCrashReportForm : Form
    {
        private static bool HasBetterExceptionWindow =>
            ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase));


        private static string GetCompressedMiniDump()
        {
            try
            {
                if (!MiniDump.TryDump(out var stream)) return string.Empty;

                using var _ = stream;
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetCompressedSaveFile()
        {
            try
            {
                var gameSavesDirectory = new PlatformDirectoryPath(PlatformFileType.User, "Game Saves\\");
                // TODO: What to with Xbox version? No write time available
                var gameSavesPath = PlatformFileHelperPCExtended.GetDirectoryFullPath(gameSavesDirectory);
                if (string.IsNullOrEmpty(gameSavesPath)) return string.Empty;

                var latestSaveFile = new DirectoryInfo(gameSavesPath).EnumerateFiles("*.sav", SearchOption.TopDirectoryOnly)
                    .OrderByDescending(x => x.LastWriteTimeUtc)
                    .FirstOrDefault();
                if (latestSaveFile is null) return string.Empty;

                using var compressedDataStream = new MemoryStream();
                using var fs = latestSaveFile.OpenRead();
                using var zipStream = new GZipStream(compressedDataStream, CompressionMode.Compress, true);
                fs.Position = 0;
                fs.CopyTo(zipStream);
                return Convert.ToBase64String(compressedDataStream.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetScreenshot()
        {
            try
            {
                var tempBmp = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.bmp");

                Utilities.TakeScreenshot(tempBmp);

                using var image = Image.FromFile(tempBmp);
                using var encoderParameters = new EncoderParameters(1) { Param = { [0] = new EncoderParameter(Encoder.Quality, 80L) } };

                using var stream = new MemoryStream();
                image.Save(stream, ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid), encoderParameters);
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        // https://gist.github.com/eikes/2299607
        // Copyright: Eike Send http://eike.se/nd
        // License: MIT License
        private static string ScriptText = @"
if (!document.getElementsByClassName) {
  document.getElementsByClassName = function(search) {
    var d = document, elements, pattern, i, results = [];
    if (d.querySelectorAll) { // IE8
      return d.querySelectorAll(""."" + search);
    }
    if (d.evaluate) { // IE6, IE7
      pattern = "".//*[contains(concat(' ', @class, ' '), ' "" + search + "" ')]"";
      elements = d.evaluate(pattern, d, null, 0, null);
      while ((i = elements.iterateNext())) {
        results.push(i);
      }
    } else {
      elements = d.getElementsByTagName(""*"");
      pattern = new RegExp(""(^|\\s)"" + search + ""(\\s|$)"");
      for (i = 0; i < elements.length; i++) {
        if ( pattern.test(elements[i].className) ) {
          results.push(elements[i]);
        }
      }
    }
    return results;
  }
}
function handleIncludeMiniDump(cb) {
  window.external.SetIncludeMiniDump(cb.checked);
}
function handleIncludeSaveFile(cb) {
  window.external.SetIncludeSaveFile(cb.checked);
}
function handleIncludeScreenshot(cb) {
  window.external.SetIncludeScreenshot(cb.checked);
}
";

        private static string TableText = @$"
<table style='width: 100%;'>
  <tbody>
  <tr>
    <td style='width: 50%;'>
      <h1>Intercepted an exception!</h1>
    </td>
    <td>
      <button style='float:right; margin-left:10px;' onclick='window.external.Close()'>Close Report</button>
      {(CrashUploaderSubSystem.Instance?.IsEnabled == true
            ? "<button style='float:right; margin-left:10px;' onclick='window.external.UploadReport()'>Upload Report as a Permalink</button>"
            : "")}
      <button style='float:right; margin-left:10px;' onclick='window.external.SaveReport()'>Save Report</button>
      <button style='float:right; margin-left:10px;' onclick='window.external.CopyAsHTML()'>Copy as HTML</button>
    </td>
  </tr>
  <tr>
    <td style='width: 50%;'>
    </td>
    <td>
      <input style='float:right;' type='checkbox' onclick='handleIncludeMiniDump(this);'>
      <label style='float:right; margin-left:10px;'>Include Mini Dump:</label>
      <input style='float:right;' type='checkbox' onclick='handleIncludeSaveFile(this);'>
      <label style='float:right; margin-left:10px;'>Include Latest Save File:</label>
      <input style='float:right;' type='checkbox' onclick='handleIncludeScreenshot(this);'>
      <label style='float:right; margin-left:10px;'>Include Screenshot:</label>
    </td>
  </tr>
  </tbody>
</table>
Clicking 'Close Report' will continue with the Game's error report mechanism.
<hr/>";

        private CrashReport CrashReport { get; }
        private string ReportInHtml { get; }

        public bool IncludeMiniDump { get; set; }
        public bool IncludeSaveFile { get; set; }
        public bool IncludeScreenshot { get; set; }

        internal HtmlCrashReportForm(CrashReport crashReport)
        {
            CrashReport = crashReport;
            ReportInHtml = HtmlBuilder.Build(crashReport);

            InitializeComponent();
            HtmlRender.ObjectForScripting = this;
            HtmlRender.DocumentText = ReportInHtml;
            HtmlRender.DocumentCompleted += (sender, args) =>
            {
                if (HtmlRender.Document is { Body: { } body } document)
                {
                    if (document.CreateElement("script") is { } scriptElement)
                    {
                        scriptElement.SetAttribute("text", ScriptText);
                        body.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, scriptElement);
                    }

                    if (document.CreateElement("div") is { } tableElement && body.FirstChild is { } firstChild)
                    {
                        tableElement.InnerHtml = TableText;
                        firstChild.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeBegin, tableElement);
                    }
                }
            };
        }

        public void SetIncludeMiniDump(bool value) => IncludeMiniDump = value;

        public void SetIncludeSaveFile(bool value) => IncludeSaveFile = value;

        public void SetIncludeScreenshot(bool value) => IncludeScreenshot = value;

        public async void CopyAsHTML()
        {
            await SetClipboardTextAsync(ReportInHtml);
        }

        public async void UploadReport()
        {
            var crashUploader = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ICrashUploader>();
            if (crashUploader is null)
            {
                MessageBox.Show("Failed to get the crash uploader!", "Error!");
                return;
            }

            var result = await crashUploader.UploadAsync(CrashReport).ConfigureAwait(false);
            switch (result.Status)
            {
                case CrashUploaderStatus.Success:
                {
                    await SetClipboardTextAsync(result.Url ?? string.Empty);
                    MessageBox.Show($"Report available at\n{result.Url}\nThe url was copied to the clipboard!", "Success!");
                    break;
                }
                case CrashUploaderStatus.MetadataNotFound:
                case CrashUploaderStatus.ResponseIsNotHttpWebResponse:
                case CrashUploaderStatus.ResponseStreamIsNull:
                    MessageBox.Show($"The crash uploader could not upload the report!\nPlease report this to the mod developers!\nStatus: {result.Status}", "Error!");
                    break;
                case CrashUploaderStatus.WrongStatusCode:
                    MessageBox.Show($"The crash uploader could not upload the report!\nPlease report this to the mod developers!\nStatus: {result.Status}\nStatusCode: {result.StatusCode}", "Error!");
                    break;
                case CrashUploaderStatus.FailedWithException:
                    MessageBox.Show($"The crash uploader could not upload the report!\nPlease report this to the mod developers!\nStatus: {result.Status}\nException: {result.Exception}", "Error!");
                    break;
            }
        }

        public void SaveReport()
        {
            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "HTML files|*.html;*.htm|All files (*.*)|*.*",
                RestoreDirectory = true,
                AddExtension = true,
                CheckPathExists = true,
                ValidateNames = true,
                FileName = "crashreport",
                CreatePrompt = true,
                OverwritePrompt = true
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.OpenFile() is { } fileStream)
            {
                var report = ReportInHtml;

                if (IncludeMiniDump)
                    report = report
                        .Replace(HtmlBuilder.MiniDumpTag, GetCompressedMiniDump())
                        .Replace(HtmlBuilder.MiniDumpButtonTag, @"
<![if !IE]>
              <br/>
              <br/>
              <button onclick='minidump(this)'>Get MiniDump</button>
<![endif]>");

                if (IncludeSaveFile)
                    report = report
                        .Replace(HtmlBuilder.SaveFileTag, GetCompressedSaveFile())
                        .Replace(HtmlBuilder.SaveFileButtonTag, @"
<![if !IE]>
              <br/>
              <br/>
              <button onclick='savefile(this)'>Get Save File</button>
<![endif]>");

                if (IncludeScreenshot)
                    report = report
                        .Replace(HtmlBuilder.ScreenshotTag, GetScreenshot())
                        .Replace(HtmlBuilder.ScreenshotButtonTag, @"
<![if !IE]>
              <br/>
              <br/>
              <button onclick='screenshot(this)'>Show Screenshot</button>
<![endif]>");

                if (IncludeMiniDump || IncludeSaveFile)
                    report = report
                        .Replace(HtmlBuilder.DecompressScriptTag, @"
<![if !IE]>
    <script src=""https://cdn.jsdelivr.net/pako/1.0.3/pako_inflate.min.js""></script>
<![endif]>");

                using var stream = fileStream;
                using var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(report);
            }
        }

        private void HtmlRender_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() is { } uri && UriIsValid(uri))
            {
                e.Cancel = true;
                Process.Start(uri);
            }
        }

        private static bool UriIsValid(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        private static async Task SetClipboardTextAsync(string text)
        {
            var completionSource = new TaskCompletionSource<object?>();
            var staThread = new Thread(() =>
            {
                var dataObject = new DataObject();
                dataObject.SetText(text, TextDataFormat.Text);
                Clipboard.SetDataObject(dataObject, true, 10, 100);
                completionSource.SetResult(null);
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            await completionSource.Task;
        }
    }
}