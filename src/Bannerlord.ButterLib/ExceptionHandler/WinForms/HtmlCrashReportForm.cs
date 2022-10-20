using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.CrashUploader;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bannerlord.ButterLib.ExceptionHandler.WinForms
{
    public partial class HtmlCrashReportForm : Form
    {
        private static bool HasBetterExceptionWindow =>
            BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase));

        private static string GetCompressedMiniDump()
        {
            if (ExceptionHandlerSubSystem.Instance?.IncludeMiniDump != true ||!MiniDump.TryDump(out var stream)) return string.Empty;

            using var _ = stream;
            return Convert.ToBase64String(stream.ToArray());
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
}";

        private static string TableText = @$"
<table style='width: 100%;'>
  <tbody>
  <tr>
    <td style='width: 50%;'>
      <h1>Intercepted an exception!</h1>
    </td>
    <td>
      {(HasBetterExceptionWindow
            ? "<button style='float:right; margin-left:10px;' onclick='window.external.Close()'>BEW Report</button>"
            : "<button style='float:right; margin-left:10px;' onclick='window.external.Close()'>Close Report</button>")}
      {(CrashUploaderSubSystem.Instance?.IsEnabled == true
            ? "<button style='float:right; margin-left:10px;' onclick='window.external.UploadReport()'>Upload Report as a Permalink</button>"
            : "")}
      <button style='float:right; margin-left:10px;' onclick='window.external.SaveReport()'>Save Report</button>
      <button style='float:right; margin-left:10px;' onclick='window.external.CopyAsHTML()'>Copy as HTML</button>
    </td>
  </tr>
  </tbody>
</table>
{(HasBetterExceptionWindow
            ? "Click 'BEW Report' to close this report and open the report from Better Exception Window."
            : "Clicking 'Close Report' will continue with the Game's error report mechanism.")}
<hr/>";

        private CrashReport CrashReport { get; }
        private string ReportInHtml { get; }

        internal HtmlCrashReportForm(CrashReport crashReport)
        {
            CrashReport = crashReport;
            ReportInHtml = HtmlBuilder.Build(crashReport, GetCompressedMiniDump());

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

            var url = await crashUploader.UploadAsync(CrashReport).ConfigureAwait(false);
            if (url is null)
            {
                MessageBox.Show("The crash uploader could not upload the report!", "Error!");
                return;
            }

            await SetClipboardTextAsync(url);
            MessageBox.Show($"Report available at\n{url}\nThe url was copied to the clipboard!", "Success!");
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
                using (fileStream)
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(ReportInHtml);
                }
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
                Clipboard.SetText(text);
                completionSource.SetResult(null);
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            await completionSource.Task;
        }
    }
}