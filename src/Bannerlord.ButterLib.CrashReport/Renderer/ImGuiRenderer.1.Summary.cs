using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport.Bannerlord;

using ImGuiNET;

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
        if (JmGui.BeginTable("Buttons\0"u8, 2))
        {
            ImGui.TableNextColumn();
            ImGui.SetWindowFontScale(2);
            JmGui.TextWrapped("Intercepted an exception!\0"u8);
            ImGui.SetWindowFontScale(1);
            ImGui.TableNextColumn();

            if (JmGui.Button("Save Report as HTML\0"u8)) SaveCrashReportAsHtml();
            ImGui.SameLine();
            if (JmGui.Button("Save Report as ZIP\0"u8)) SaveCrashReportAsZip();
            ImGui.SameLine();
            JmGui.PushStyleColor(ImGuiCol.Button, in Orange);
            if (JmGui.Button("Close Report and Continue\0"u8)) _onClose();
            ImGui.PopStyleColor();
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();

            if (JmGui.Button("Copy as HTML\0"u8)) CopyAsHtml();
            ImGui.SameLine();
            if (JmGui.Button("Upload Report as Permalink\0"u8)) UploadReport();
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();

            JmGui.Text("Save Report as HTML Options:\0"u8);
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();

            JmGui.Checkbox("Include Screenshot\0"u8, ref _addScreenshots);
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();

            JmGui.Checkbox("Include Latest Save File\0"u8, ref _addLatestSave);
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();

            JmGui.Checkbox("Include Mini Dump\0"u8, ref _addMiniDump);
            ImGui.EndTable();
        }

        JmGui.Text("Clicking 'Close Report and Continue' will continue with the Game's error report mechanism.\0"u8);

        ImGui.Separator();
        ImGui.NewLine();

        ImGui.SetWindowFontScale(2);
        JmGui.TextSameLine(_crashReport.Metadata.GameName ?? string.Empty);
        JmGui.Text(" has encountered a problem and will close itself!\0"u8);
        ImGui.SetWindowFontScale(1);

        ImGui.NewLine();

        JmGui.Text("This is a community Crash Report. Please save it and use it for reporting the error. Do not provide screenshots, provide the report!\0"u8);

        JmGui.Text("Most likely this error was caused by a custom installed module.\0"u8);

        ImGui.NewLine();

        JmGui.Text("If you were in the middle of something, the progress might be lost.\0"u8);

        ImGui.NewLine();

        JmGui.TextSameLine("Launcher: \0"u8);
        JmGui.TextSameLine(_crashReport.Metadata.LauncherType ?? string.Empty);
        JmGui.TextSameLine(" (\0"u8);
        JmGui.TextSameLine(_crashReport.Metadata.LauncherVersion ?? string.Empty);
        JmGui.Text(")\0"u8);

        JmGui.TextSameLine("Runtime: \0"u8);
        JmGui.Text(_crashReport.Metadata.Runtime ?? string.Empty);
    }
}