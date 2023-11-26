#if !NETSTANDARD2_0_OR_GREATER
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.CrashUploader;

using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using TaleWorlds.Library;

using DialogResult = System.Windows.Forms.DialogResult;

namespace Bannerlord.ButterLib.ExceptionHandler.WinForms
{
    public partial class HtmlCrashReportForm : Form
    {
        private static bool UriIsValid(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        private static async Task<bool> SetClipboardTextAsync(string text)
        {
            var completionSource = new TaskCompletionSource<bool>();
            var staThread = new Thread(() =>
            {
                try
                {
                    var dataObject = new DataObject();
                    dataObject.SetText(text, TextDataFormat.Text);
                    Clipboard.SetDataObject(dataObject, true, 10, 100);
                    completionSource.SetResult(true);
                }
                catch (Exception)
                {
                    completionSource.SetResult(false);
                }
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            return await completionSource.Task;
        }


        // https://gist.github.com/eikes/2299607
        // Copyright: Eike Send http://eike.se/nd
        // License: MIT License
        private static readonly string ScriptText = """
if (!document.getElementsByClassName) {
  document.getElementsByClassName = function(search) {
    var d = document, elements, pattern, i, results = [];
    if (d.querySelectorAll) { // IE8
      return d.querySelectorAll("." + search);
    }
    if (d.evaluate) { // IE6, IE7
      pattern = ".//*[contains(concat(' ', @class, ' '), ' " + search + " ')]";
      elements = d.evaluate(pattern, d, null, 0, null);
      while ((i = elements.iterateNext())) {
        results.push(i);
      }
    } else {
      elements = d.getElementsByTagName("*");
      pattern = new RegExp("(^|\\s)" + search + "(\\s|$)");
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
""";

        private static readonly string TableText = $"""
<table style='width: 100%;'>
  <tbody>
    <tr>
      <td style='width: 50%;'>
        <h1>Intercepted an exception!</h1>
      </td>
      <td>
        <button style='float:right; margin-left:10px;' onclick='window.external.Close()'>Close Report</button>
        {(CrashUploaderSubSystem.Instance?.IsEnabled != true ? "" : """
        <button style='float:right; margin-left:10px;' onclick='window.external.UploadReport()'>Upload Report as a Permalink</button>
        """)}
        <button style='float:right; margin-left:10px;' onclick='window.external.SaveReportZIP()'>Save Report as ZIP</button>
        <button style='float:right; margin-left:10px;' onclick='window.external.SaveReportHTML()'>Save Report as HTML</button>
        <button style='float:right; margin-left:10px;' onclick='window.external.CopyAsHTML()'>Copy as HTML</button>
      </td>
    </tr>
    <tr>
      <td style='width: 50%;'>
      </td>
      <td>
        <label style='float:left;'>Save Report as HTML Options:</label>
        <input style='float:right;' type='checkbox' onclick='handleIncludeMiniDump(this);'/>
        <label style='float:right; margin-left:10px;'>Include Mini Dump:</label>
        <input style='float:right;' type='checkbox' onclick='handleIncludeSaveFile(this);'/>
        {(ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop ? "" : """
        <label style='float:right; margin-left:10px;'>Include Latest Save File:</label>
        <input style='float:right;' type='checkbox' onclick='handleIncludeScreenshot(this);'/>
        """)}
        <label style='float:right; margin-left:10px;'>Include Screenshot:</label>
      </td>
    </tr>
  </tbody>
</table>
Clicking 'Close Report' will continue with the Game's error report mechanism.
<hr />
""";

        private CrashReportModel CrashReport { get; }
        private LogSource[] LogSources { get; }
        private string ReportInHtml { get; }

        public bool IncludeMiniDump { get; set; }
        public bool IncludeSaveFile { get; set; }
        public bool IncludeScreenshot { get; set; }

        internal HtmlCrashReportForm(CrashReportInfo crashReport)
        {
            CrashReport = CrashReportCreator.Create(crashReport);
            LogSources = CreatorShared.GetLogSources().ToArray();
            ReportInHtml = CrashReportHtmlRenderer.Build(CrashReport, LogSources);

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
            if (!await SetClipboardTextAsync(ReportInHtml))
                MessageBox.Show("Failed to copy the HTML content to the clipboard!", "Error!");
        }

        public async void UploadReport()
        {
            var crashUploader = ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<ICrashUploader>();
            if (crashUploader is null)
            {
                MessageBox.Show("Failed to get the crash uploader!", "Error!");
                return;
            }

            var result = await crashUploader.UploadAsync(CrashReport, LogSources).ConfigureAwait(false);
            switch (result.Status)
            {
                case CrashUploaderStatus.Success:
                {
                    if (await SetClipboardTextAsync(result.Url ?? string.Empty))
                        MessageBox.Show($"Report available at\n{result.Url}\nThe url was copied to the clipboard!", "Success!");
                    else
                        MessageBox.Show($"Report available at\n{result.Url}\nFailed to copy the url to the clipboard!", "Success!");
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

        [UsedImplicitly]
        public void SaveReportHTML()
        {
            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "HTML files|*.html|All files (*.*)|*.*",
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
                using var stream = fileStream;
                CreatorHtml.Create(CrashReport, ReportInHtml, IncludeMiniDump, IncludeSaveFile, IncludeScreenshot, stream);
            }
        }

        [UsedImplicitly]
        public void SaveReportZIP()
        {
            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "ZIP files|*.zip|All files (*.*)|*.*",
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
                using var stream = fileStream;
                CreatorZip.Create(CrashReport, stream);
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
    }
}
#endif