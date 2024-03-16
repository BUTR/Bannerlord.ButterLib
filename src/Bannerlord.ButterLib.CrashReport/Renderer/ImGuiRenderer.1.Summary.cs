using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bannerlord.ButterLib.ExceptionHandler;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static bool _addScreenshots;
    private static bool _addLatestSave;
    private static bool _addMiniDump;

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
        if (ImGui.BeginTable("Buttons", 2))
        {
            ImGui.TableNextColumn();
            ImGui.SetWindowFontScale(2);
            ImGui.TextWrapped("Intercepted an exception!");
            ImGui.SetWindowFontScale(1);
            ImGui.TableNextColumn();

            if (ImGui.Button("Save Report as HTML")) SaveCrashReportAsHtml();
            ImGui.SameLine();
            if (ImGui.Button("Save Report as ZIP")) SaveCrashReportAsZip();
            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Button, Orange);
            if (ImGui.Button("Close Report and Continue")) _close();
            ImGui.PopStyleColor();
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            
            if (ImGui.Button("Copy as HTML")) CopyAsHtml();
            ImGui.SameLine();
            if (ImGui.Button("Upload Report as Permalink")) UploadReport();
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            
            ImGui.Text("Save Report as HTML Options:");
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            
            ImGui.Checkbox("Include Screenshot", ref _addScreenshots);
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            
            ImGui.Checkbox("Include Latest Save File", ref _addLatestSave);
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            
            ImGui.Checkbox("Include Mini Dump", ref _addMiniDump);
            ImGui.EndTable();
        }
        
        ImGui.Text("Clicking 'Close Report and Continue' will continue with the Game's error report mechanism.");
        
        ImGui.Separator();
        ImGui.NewLine();
        
        ImGui.SetWindowFontScale(2);
        ImGui.Text($"{_crashReport.Metadata.GameName} has encountered a problem and will close itself!");
        ImGui.SetWindowFontScale(1);
        ImGui.NewLine();
        ImGui.Text("This is a community Crash Report. Please save it and use it for reporting the error. Do not provide screenshots, provide the report!");
        ImGui.Text("Most likely this error was caused by a custom installed module.");
        ImGui.NewLine();
        ImGui.Text("If you were in the middle of something, the progress might be lost.");
        ImGui.NewLine();
        ImGui.Text($"Launcher: {_crashReport.Metadata.LauncherType} ({_crashReport.Metadata.LauncherVersion})");
        ImGui.Text($"Runtime: {_crashReport.Metadata.Runtime}");
    }
}