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
            var library = LoadLibrary(WatchdogLibraryName);
            // Don't like the fact that I can't get the concrete memory size
            var size = (int) new FileInfo(WatchdogLibraryName).Length;
            var watchdogSize = WatchdogOriginal.Length;

            var libraryMemoryBlock = new Span<byte>(library, size);

            var searchSpan = libraryMemoryBlock;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogOriginal) is var idx and not -1)
            {
                var address = library + searchSpanOffset + idx;
                var textMemoryBlock = new Span<byte>(address, watchdogSize);

                VirtualProtect(address, watchdogSize, PAGE_EXECUTE_READWRITE, out var old);
                WatchdogReplacement.CopyTo(textMemoryBlock);
                VirtualProtect(address, watchdogSize, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }

        public static void EnableTWWatchdog()
        {
            var library = LoadLibrary(WatchdogLibraryName);
            // Don't like the fact that I can't get the concrete memory size
            var size = (int) new FileInfo(WatchdogLibraryName).Length;
            var watchdogSize = WatchdogOriginal.Length;

            var libraryMemoryBlock = new Span<byte>(library, size);

            var searchSpan = libraryMemoryBlock;
            var searchSpanOffset = 0;
            while (searchSpan.IndexOf(WatchdogReplacement) is var idx and not -1)
            {
                var address = library + searchSpanOffset + idx;
                var textMemoryBlock = new Span<byte>(address, watchdogSize);

                VirtualProtect(address, watchdogSize, PAGE_EXECUTE_READWRITE, out var old);
                WatchdogOriginal.CopyTo(textMemoryBlock);
                VirtualProtect(address, watchdogSize, old, out _);

                searchSpanOffset = idx;
                searchSpan = searchSpan.Slice(searchSpanOffset);
            }
        }
    }
}