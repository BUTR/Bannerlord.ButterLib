using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.CrashUploader;

using BUTR.CrashReport.Models;
using BUTR.CrashReport.Renderer;

using Microsoft.Extensions.DependencyInjection;

#if !NETSTANDARD2_0
using BUTR.CrashReport.Bannerlord;

using System.Windows.Forms;
#endif

namespace Bannerlord.ButterLib.ExceptionHandler.Utils;

internal sealed class CrashReportRendererUtilities : ICrashReportRendererUtilities
{
    private static async Task<bool> SetClipboardTextAsync(string text)
    {
        var completionSource = new TaskCompletionSource<bool>();
        var staThread = new Thread(() =>
        {
            try
            {
#if !NETSTANDARD2_0
                var dataObject = new DataObject();
                dataObject.SetText(text, TextDataFormat.Text);
                Clipboard.SetDataObject(dataObject, true, 10, 100);
#endif
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

    private static async Task<(bool, string)> UploadInternal(CrashReportModel crashReport, ICollection<LogSource> logSources)
    {
        var crashUploader = ButterLibSubModule.ServiceProvider?.GetService<ICrashUploader>();
        if (crashUploader is null)
            return (false, "Critical Error: Failed to get the crash uploader!");

        var result = await crashUploader.UploadAsync(crashReport, logSources).ConfigureAwait(false);
        return result.Status switch
        {
            CrashUploaderStatus.Success => (true, result.Url ?? string.Empty),
            CrashUploaderStatus.MetadataNotFound => (false, $"Status: {result.Status}"),
            CrashUploaderStatus.ResponseIsNotHttpWebResponse => (false, $"Status: {result.Status}"),
            CrashUploaderStatus.ResponseStreamIsNull => (false, $"Status: {result.Status}"),
            CrashUploaderStatus.WrongStatusCode => (false, $"Status: {result.Status}\nStatusCode: {result.StatusCode}"),
            CrashUploaderStatus.FailedWithException => (false, $"Status: {result.Status}\nException: {result.Exception}"),
        };
    }

    public IEnumerable<string> GetNativeLibrariesFolderPath()
    {
        var modulePath = ModuleInfoHelper.GetModulePath(typeof(CrashReportRendererUtilities))!;
        yield return Path.Combine(modulePath, "bin", TaleWorlds.Library.Common.ConfigName);
    }

    public async void Upload(CrashReportModel crashReport, ICollection<LogSource> logSources)
    {
#if !NETSTANDARD2_0
        var result = await UploadInternal(crashReport, logSources);
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
#endif
    }

    public async void CopyAsHtml(CrashReportModel crashReport, ICollection<LogSource> logSources)
    {
#if !NETSTANDARD2_0
        var reportAsHtml = CrashReportHtmlRenderer.Build(crashReport, logSources);

        if (!await SetClipboardTextAsync(reportAsHtml))
            MessageBox.Show("Failed to copy the HTML content to the clipboard!", "Error!");
#endif
    }

    public void SaveCrashReportAsHtml(CrashReportModel crashReport, ICollection<LogSource> logSources, bool addMiniDump, bool addLatestSave, bool addScreenshots)
    {
#if !NETSTANDARD2_0
        var reportAsHtml = CrashReportHtmlRenderer.Build(crashReport, logSources);

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
            CreatorHtml.Create(crashReport, reportAsHtml, addMiniDump, addLatestSave, addScreenshots, stream);
        }
#endif
    }

    public void SaveCrashReportAsZip(CrashReportModel crashReport, ICollection<LogSource> logSources)
    {
#if !NETSTANDARD2_0
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
            CreatorZip.Create(crashReport, logSources, stream);
        }
#endif
    }
}