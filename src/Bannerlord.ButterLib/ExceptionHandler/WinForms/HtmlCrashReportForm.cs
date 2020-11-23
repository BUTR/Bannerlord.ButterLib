using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Bannerlord.ButterLib.ExceptionHandler.WinForms
{
    public partial class HtmlCrashReportForm : Form
    {
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

        private static string TableText = @"
<table style='width: 100%;'>
  <tbody>
  <tr>
    <td style='width: 50%;'>
      <h1>Intercepted an exception!</h1>
    </td>
    <td>
      <button style='float:right; margin-left:10px;' onclick='window.external.Close()'>Close Report</button>
      <button style='float:right; margin-left:10px;' onclick='window.external.SaveReport()'>Save Report</button>
      <button style='float:right; margin-left:10px;' onclick='window.external.CopyAsHTML()'>Copy as HTML</button>
    </td>
  </tr>
  </tbody>
</table>
<hr/>";

        private string ReportInHtml { get; }

        public HtmlCrashReportForm(string reportInHtml)
        {
            ReportInHtml = reportInHtml;

            InitializeComponent();
            HtmlRender.ObjectForScripting = this;
            HtmlRender.DocumentText = ReportInHtml;
            HtmlRender.DocumentCompleted += (sender, args) =>
            {
                if (HtmlRender.Document is { } document && document.Body is { } body)
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

        public void CopyAsHTML()
        {
            Clipboard.SetText(ReportInHtml);
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
            if (e.Url.ToString() is {} uri && UriIsValid(uri))
            {
                e.Cancel = true;
                Process.Start(uri);
            }
        }

        private static bool UriIsValid(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}