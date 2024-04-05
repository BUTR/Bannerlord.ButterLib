using BUTR.CrashReport.Models;
using BUTR.CrashReport.Renderer.Html;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Bannerlord.ButterLib.ExceptionHandler.Utils;

internal static class CreatorHtml
{
    private static string GetCompressedMiniDump()
    {
        try
        {
            using var stream = CreatorShared.GetMiniDump();
            if (stream == Stream.Null) return string.Empty;

            using var ms = new MemoryStream();
            using var zipStream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(zipStream);
            return Convert.ToBase64String(ms.ToArray());
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
            using var stream = CreatorShared.GetSaveFile();
            if (stream == Stream.Null) return string.Empty;

            using var ms = new MemoryStream();
            using var zipStream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(zipStream);
            return Convert.ToBase64String(ms.ToArray());
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
            using var stream = CreatorShared.GetScreenshot();
            if (stream == Stream.Null) return string.Empty;

            if (stream is MemoryStream memoryStream)
                return Convert.ToBase64String(memoryStream.ToArray());

            using var ms = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private static string CompressJson(string jsonModel)
    {
        using var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel));
        using var compressedStream = new MemoryStream();
        using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
        {
            uncompressedStream.CopyTo(compressorStream);
        }
        var compressedBytes = compressedStream.ToArray();
        return Convert.ToBase64String(compressedBytes);
    }

    public static void Create(CrashReportModel crashReport, string html, bool includeMiniDump, bool includeSaveFile, bool includeScreenshot, Stream stream)
    {
        var json = JsonConvert.SerializeObject(crashReport, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter() }
        });

        var report = CrashReportHtml.AddData(html, CompressJson(json),
            includeMiniDump ? GetCompressedMiniDump() : null,
            includeSaveFile ? GetCompressedSaveFile() : null,
            includeScreenshot ? GetScreenshot() : null);

        using var streamWriter = new StreamWriter(stream);
        streamWriter.Write(report);
    }
}