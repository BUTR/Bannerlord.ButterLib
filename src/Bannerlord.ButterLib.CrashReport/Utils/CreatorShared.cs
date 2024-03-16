using System;
using System.IO;
using System.Linq;
using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using TaleWorlds.Engine;
using TaleWorlds.Library;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.ExceptionHandler;

internal static class CreatorShared
{
    public static Stream GetMiniDump()
    {
        try
        {
            if (!MiniDump.TryDump(out var stream)) return Stream.Null;
            return stream;
        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }

    public static Stream GetSaveFile()
    {
        try
        {
            var gameSavesDirectory = new PlatformDirectoryPath(PlatformFileType.User, "Game Saves\\");
            // TODO: What to with Xbox version? No write time available
            var gameSavesPath = PlatformFileHelperPCExtended.GetDirectoryFullPath(gameSavesDirectory);
            if (string.IsNullOrEmpty(gameSavesPath)) return Stream.Null;

            var latestSaveFile = new DirectoryInfo(gameSavesPath).EnumerateFiles("*.sav", SearchOption.TopDirectoryOnly)
                .OrderByDescending(x => x.LastWriteTimeUtc)
                .FirstOrDefault();
            if (latestSaveFile is null) return Stream.Null;

            return latestSaveFile.OpenRead();
        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }

    public static Stream GetScreenshot()
    {
        try
        {
            var tempBmp = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.bmp");

            Utilities.TakeScreenshot(tempBmp);

#if !NETSTANDARD2_0
            using var image = System.Drawing.Image.FromFile(tempBmp);
            using var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1) { Param = { [0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L) } };

            var stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid), encoderParameters);
            return stream;
#else
            return Stream.Null;
#endif

        }
        catch (Exception)
        {
            return Stream.Null;
        }
    }
}