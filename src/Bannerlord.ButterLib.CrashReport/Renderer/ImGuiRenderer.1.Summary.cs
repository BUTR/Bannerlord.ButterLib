using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport.Bannerlord;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
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

    private bool _addScreenshots;
    private bool _addLatestSave;
    private bool _addMiniDump;

    private async void CopyAsHtml()
    {
        var reportAsHtml = CrashReportHtmlRenderer.Build(_crashReport, _logSources);

        if (!await SetClipboardTextAsync(reportAsHtml))
            MessageBox.Show("Failed to copy the HTML content to the clipboard!", "Error!");
    }

    private async void UploadReport()
    {
        var result = await _upload(_crashReport, _logSources);
        if (result.Item1)
        {
            MessageBox.Show($"""
                             Report available at
                             {result.Item2}
                             The url was copied to the clipboard!
                             """, "Success!");
        }
        else
        {
            MessageBox.Show($"""
                             The crash uploader could not upload the report!
                             Please report this to the mod developers!
                             {result.Item2}
                             """, "Error!");
        }
    }

    private void SaveCrashReportAsHtml()
    {
        var reportAsHtml = CrashReportHtmlRenderer.Build(_crashReport, _logSources);

        using var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "HTML files|*.html|All files (*.*)|*.*";
        saveFileDialog.RestoreDirectory = true;
        saveFileDialog.AddExtension = true;
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.ValidateNames = true;
        saveFileDialog.FileName = "crashreport";
        saveFileDialog.CreatePrompt = true;
        saveFileDialog.OverwritePrompt = true;
        if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.OpenFile() is { } fileStream)
        {
            using var stream = fileStream;
            CreatorHtml.Create(_crashReport, reportAsHtml, _addMiniDump, _addLatestSave, _addScreenshots, stream);
        }
    }

    private void SaveCrashReportAsZip()
    {
        using var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "ZIP files|*.zip|All files (*.*)|*.*";
        saveFileDialog.RestoreDirectory = true;
        saveFileDialog.AddExtension = true;
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.ValidateNames = true;
        saveFileDialog.FileName = "crashreport";
        saveFileDialog.CreatePrompt = true;
        saveFileDialog.OverwritePrompt = true;
        if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.OpenFile() is { } fileStream)
        {
            using var stream = fileStream;
            CreatorZip.Create(_crashReport, _logSources, stream);
        }
    }

    private void RenderSummary()
    {
        if (_imgui.BeginTable("Buttons\0"u8, 2))
        {
            _imgui.TableNextColumn();
            _imgui.SetWindowFontScale(2);
            _imgui.TextWrapped("Intercepted an exception!\0"u8);
            _imgui.SetWindowFontScale(1);
            _imgui.TableNextColumn();

            if (_imgui.Button("Save Report as HTML\0"u8)) SaveCrashReportAsHtml();
            _imgui.SameLine();
            if (_imgui.Button("Save Report as ZIP\0"u8)) SaveCrashReportAsZip();
            _imgui.SameLine();
            if (_imgui.Button("Close Report and Continue\0"u8, in Secondary, in Secondary2, in Secondary3)) _onClose();
            _imgui.TableNextColumn();
            _imgui.TableNextColumn();

            if (_imgui.Button("Copy as HTML\0"u8)) CopyAsHtml();
            _imgui.SameLine();
            if (_imgui.Button("Upload Report as Permalink\0"u8)) UploadReport();
            _imgui.TableNextColumn();
            _imgui.TableNextColumn();

            _imgui.Text("Save Report as HTML Options:\0"u8);
            _imgui.TableNextColumn();
            _imgui.TableNextColumn();

            _imgui.Checkbox("Include Screenshot\0"u8, ref _addScreenshots);
            _imgui.TableNextColumn();
            _imgui.TableNextColumn();

            _imgui.Checkbox("Include Latest Save File\0"u8, ref _addLatestSave);
            _imgui.TableNextColumn();
            _imgui.TableNextColumn();

            _imgui.Checkbox("Include Mini Dump\0"u8, ref _addMiniDump);
            _imgui.EndTable();
        }

        _imgui.Text("Clicking 'Close Report and Continue' will continue with the Game's error report mechanism.\0"u8);

        _imgui.Separator();
        _imgui.NewLine();

        _imgui.SetWindowFontScale(2);
        _imgui.TextSameLine(_crashReport.Metadata.GameName ?? string.Empty);
        _imgui.Text(" has encountered a problem and will close itself!\0"u8);
        _imgui.SetWindowFontScale(1);

        _imgui.NewLine();

        _imgui.Text("This is a community Crash Report. Please save it and use it for reporting the error. Do not provide screenshots, provide the report!\0"u8);

        _imgui.Text("Most likely this error was caused by a custom installed module.\0"u8);

        _imgui.NewLine();

        _imgui.Text("If you were in the middle of something, the progress might be lost.\0"u8);

        _imgui.NewLine();

        _imgui.TextSameLine("Launcher: \0"u8);
        _imgui.TextSameLine(_crashReport.Metadata.LauncherType ?? string.Empty);
        _imgui.TextSameLine(" (\0"u8);
        _imgui.TextSameLine(_crashReport.Metadata.LauncherVersion ?? string.Empty);
        _imgui.Text(")\0"u8);

        _imgui.TextSameLine("Runtime: \0"u8);
        _imgui.Text(_crashReport.Metadata.Runtime ?? string.Empty);
    }
}