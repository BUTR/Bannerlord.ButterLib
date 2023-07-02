using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal static class WatchdogHandler
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern unsafe byte* LoadLibrary(string libname);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern unsafe bool  VirtualProtect(byte* lpAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        private static string WatchdogLibraryName = "TaleWorlds.Native.dll";
        private static byte[] WatchdogOriginal = "Watchdog.exe"u8.ToArray();
        private static byte[] WatchdogReplacement = "Wetchdog.exe"u8.ToArray();

        // Disable Watchdog by renaming it, thus performing a soft delete in it's eyes
        public static unsafe void DisableTWWatchdog()
        {
            var library = LoadLibrary(WatchdogLibraryName);
            var size = (int) new FileInfo(WatchdogLibraryName).Length;
            var watchdogSize = WatchdogOriginal.Length;

            var libraryMemoryBlock = new Span<byte>(library, size);

            var searchSpan = libraryMemoryBlock;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogOriginal) is var idx and not -1)
            {
                var address = library + searchSpanOffset + idx;
                var textMemoryBlock = new Span<byte>(address, watchdogSize);

                VirtualProtect(address, watchdogSize, 0x40, out var old);
                WatchdogReplacement.CopyTo(textMemoryBlock);
                VirtualProtect(address, watchdogSize, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }

        public static unsafe void EnableTWWatchdog()
        {
            var library = LoadLibrary(WatchdogLibraryName);
            var size = (int) new FileInfo(WatchdogLibraryName).Length;
            var watchdogSize = WatchdogOriginal.Length;

            var libraryMemoryBlock = new Span<byte>(library, size);

            var searchSpan = libraryMemoryBlock;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogReplacement) is var idx and not -1)
            {
                var address = library + searchSpanOffset + idx;
                var textMemoryBlock = new Span<byte>(address, watchdogSize);

                VirtualProtect(address, watchdogSize, 0x40, out var old);
                WatchdogOriginal.CopyTo(textMemoryBlock);
                VirtualProtect(address, watchdogSize, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }
    }
}