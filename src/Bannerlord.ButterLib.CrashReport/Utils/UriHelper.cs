/*
using System.Diagnostics;
using System.Runtime.InteropServices;
using Bannerlord.ButterLib.ExceptionHandler;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

internal static class UriHelper
{
    public static Process? OpenUrl(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (RegistryUtils.GetDefaultBrowser() is not { } progId)
                return null;
            if (RegistryUtils.GetBrowserPath(progId) is not { } browserPath)
                return null;

            browserPath = browserPath.TrimStart('\"');
            var idx = browserPath.IndexOf('\"');
            browserPath = idx >= 0 ? browserPath.Substring(0, idx) : browserPath;

            url = url.Replace("&", "^&");

            return Process.Start(new ProcessStartInfo(browserPath, $"-private -private-window -incognito -inprivate \"{url}\"") { UseShellExecute = true });
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Process.Start(new ProcessStartInfo("xdg-open", $"-private -private-window -incognito -inprivate \"{url}\""));
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Process.Start(new ProcessStartInfo("open", $"-private -private-window -incognito -inprivate \"{url}\""));
        }

        return null;
    }
}
*/