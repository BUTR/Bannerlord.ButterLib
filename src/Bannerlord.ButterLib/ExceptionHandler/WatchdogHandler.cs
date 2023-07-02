using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal static unsafe class WatchdogHandler
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern byte* LoadLibrary(string libname);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualProtect(byte* address, nint dwSize, int newProtect, out int oldProtect);

        private static readonly int PAGE_EXECUTE_READWRITE = 0x40;

        private static readonly string WatchdogLibraryName = "TaleWorlds.Native.dll";
        private static readonly byte[] WatchdogOriginal = "Watchdog.exe"u8.ToArray();
        private static readonly byte[] WatchdogReplacement = "Wetchdog.exe"u8.ToArray();

        // Disable Watchdog by renaming it, thus performing a soft delete in it's eyes
        public static void DisableTWWatchdog()
        {
            var libraryPtr = LoadLibrary(WatchdogLibraryName);
            // Don't like the fact that I can't get the concrete memory size
            var size = (int) new FileInfo(WatchdogLibraryName).Length;

            var librarySpan = new ReadOnlySpan<byte>(libraryPtr, size);

            var searchSpan = librarySpan;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogOriginal) is var idx and not -1)
            {
                var watchdogLocationPtr = libraryPtr + searchSpanOffset + idx;
                var watchdogLocationSpan = new Span<byte>(watchdogLocationPtr, WatchdogOriginal.Length);

                VirtualProtect(watchdogLocationPtr, watchdogLocationSpan.Length, PAGE_EXECUTE_READWRITE, out var old);
                WatchdogReplacement.CopyTo(watchdogLocationSpan);
                VirtualProtect(watchdogLocationPtr, watchdogLocationSpan.Length, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }

        public static void EnableTWWatchdog()
        {
            var libraryPtr = LoadLibrary(WatchdogLibraryName);
            // Don't like the fact that I can't get the concrete memory size
            var size = (int) new FileInfo(WatchdogLibraryName).Length;

            var librarySpan = new ReadOnlySpan<byte>(libraryPtr, size);

            var searchSpan = librarySpan;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogReplacement) is var idx and not -1)
            {
                var watchdogLocationPtr = libraryPtr + searchSpanOffset + idx;
                var watchdogLocationSpan = new Span<byte>(watchdogLocationPtr, WatchdogOriginal.Length);

                VirtualProtect(watchdogLocationPtr, watchdogLocationSpan.Length, PAGE_EXECUTE_READWRITE, out var old);
                WatchdogOriginal.CopyTo(watchdogLocationSpan);
                VirtualProtect(watchdogLocationPtr, watchdogLocationSpan.Length, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }
    }
}