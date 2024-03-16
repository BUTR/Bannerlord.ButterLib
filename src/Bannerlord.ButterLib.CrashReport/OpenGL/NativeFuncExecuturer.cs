using System;
using System.IO;
using System.Runtime.InteropServices;
using Bannerlord.BUTR.Shared.Helpers;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

internal static class NativeFuncExecuturer
{
    private const int RtldLazy = 0x0001;

    private static class Windows
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryW(string lpszLib);
    }

    private static class Linux
    {
        [DllImport("libdl.so.2")]
        public static extern IntPtr dlopen(string path, int flags);

        [DllImport("libdl.so.2")]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);
    }

    public static IntPtr LoadLibraryExt(string libName)
    {
        var moduleFolder = ModuleInfoHelper.GetModulePath(typeof(NativeFuncExecuturer))!;
        var ret = LoadLibrary(Path.Combine(moduleFolder, "bin", TaleWorlds.Library.Common.ConfigName, libName));

        if (ret == IntPtr.Zero) ret = LoadLibrary(libName);
        if (ret == IntPtr.Zero) throw new Exception("Failed to load library: " + libName);
        return ret;
    }

    private static IntPtr LoadLibrary(string libName) => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? Windows.LoadLibraryW(libName)
        : Linux.dlopen(libName, RtldLazy);

    public static T LoadFunction<T>(IntPtr library, string function)
    {
        var ret = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Windows.GetProcAddress(library, function)
            : Linux.dlsym(library, function);

        return Marshal.GetDelegateForFunctionPointer<T>(ret);
    }
}