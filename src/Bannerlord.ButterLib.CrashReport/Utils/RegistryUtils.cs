/*
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

internal static class RegistryUtils
{
    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out int phkResult);

    [DllImport("Advapi32.dll")]
    private static extern uint RegCloseKey(int hKey);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern int RegQueryValueEx(int hKey, string lpValueName, int lpReserved, ref uint lpType, StringBuilder lpData, ref uint lpcbData);

    private const int KEY_QUERY_VALUE = 0x0001;
    private const int KEY_WOW64_64KEY = 0x0100;

    private const long HKEY_CLASSES_ROOT = 0x80000000;
    private const long HKEY_CURRENT_USER = 0x80000001;

    public static string? GetDefaultBrowser()
    {
        var EPG_REGKEY = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
        var hkey = 0;

        try
        {
            var lResult = RegOpenKeyEx((UIntPtr) HKEY_CURRENT_USER, EPG_REGKEY, 0, KEY_QUERY_VALUE | KEY_WOW64_64KEY, out hkey);
            if (0 != lResult) return null;
            uint lpType = 0;
            uint lpcbData = 1024;
            var progIdBuffer = new StringBuilder(1024);
            RegQueryValueEx(hkey, "ProgID", 0, ref lpType, progIdBuffer, ref lpcbData);
            var progId = progIdBuffer.ToString();
            return progId;
        }
        finally
        {
            if (0 != hkey) RegCloseKey(hkey);
        }
    }
    public static string? GetBrowserPath(string progId)
    {
        var EPG_REGKEY = @$"{progId}\shell\open\command";
        var hkey = 0;

        try
        {
            var lResult = RegOpenKeyEx((UIntPtr) HKEY_CLASSES_ROOT, EPG_REGKEY, 0, KEY_QUERY_VALUE | KEY_WOW64_64KEY, out hkey);
            if (0 != lResult) return null;
            uint lpType = 0;
            uint lpcbData = 1024;
            var pathBuffer = new StringBuilder(1024);
            RegQueryValueEx(hkey, "", 0, ref lpType, pathBuffer, ref lpcbData);
            var path = pathBuffer.ToString();
            return path;
        }
        finally
        {
            if (0 != hkey) RegCloseKey(hkey);
        }
    }
}
*/