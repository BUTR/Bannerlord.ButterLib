using Bannerlord.BUTR.Shared.Helpers;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

internal static unsafe class NativeFuncExecuturer
{
    private static class Windows
    {
        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, byte* procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryW(char* lpszLib);
    }

    private static class Linux
    {
        public const int RtldLazy = 0x0001;

        [DllImport("libdl.so.2")]
        public static extern IntPtr dlopen(string path, int flags);

        [DllImport("libdl.so.2")]
        public static extern IntPtr dlsym(IntPtr handle, byte* symbol);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr LoadLibraryExt(string libName)
    {


        var moduleFolder = ModuleInfoHelper.GetModulePath(typeof(NativeFuncExecuturer))!;
        var ptr = LoadLibrary(Path.Combine(moduleFolder, "bin", TaleWorlds.Library.Common.ConfigName, libName));

        if (ptr == IntPtr.Zero) ptr = LoadLibrary(libName);
        if (ptr == IntPtr.Zero) throw new Exception($"Failed to load library: {libName}");
        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntPtr LoadLibrary(string libName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            fixed (char* libNamePtr = libName)
            {
                return Windows.LoadLibraryW(libNamePtr);
            }
        }

        // TODO:?
        return Linux.dlopen(libName, Linux.RtldLazy);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T LoadFunction<T>(IntPtr library, ReadOnlySpan<byte> function)
    {
        fixed (byte* functionPtr = function)
        {
            var ptr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Windows.GetProcAddress(library, functionPtr)
                : Linux.dlsym(library, functionPtr);
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr LoadFunction(IntPtr library, ReadOnlySpan<byte> function)
    {
        fixed (byte* functionPtr = function)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Windows.GetProcAddress(library, functionPtr)
                : Linux.dlsym(library, functionPtr);
        }
    }
}