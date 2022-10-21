using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class MiniDump
    {
        [Flags]
        private enum MINIDUMP_TYPE
        {
            MiniDumpNormal = 0x00000000,
            MiniDumpWithDataSegs = 0x00000001,
            MiniDumpWithFullMemory = 0x00000002,
            MiniDumpWithHandleData = 0x00000004,
            MiniDumpFilterMemory = 0x00000008,
            MiniDumpScanMemory = 0x00000010,
            MiniDumpWithUnloadedModules = 0x00000020,
            MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
            MiniDumpFilterModulePaths = 0x00000080,
            MiniDumpWithProcessThreadData = 0x00000100,
            MiniDumpWithPrivateReadWriteMemory = 0x00000200,
            MiniDumpWithoutOptionalData = 0x00000400,
            MiniDumpWithFullMemoryInfo = 0x00000800,
            MiniDumpWithThreadInfo = 0x00001000,
            MiniDumpWithCodeSegs = 0x00002000,
            MiniDumpWithoutAuxiliaryState = 0x00004000,
            MiniDumpWithFullAuxiliaryState = 0x00008000,
            MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
            MiniDumpIgnoreInaccessibleMemory = 0x00020000,
            MiniDumpWithTokenInformation = 0x00040000,
            MiniDumpWithModuleHeaders = 0x00080000,
            MiniDumpFilterTriage = 0x00100000,
            MiniDumpValidTypeFlags = 0x001fffff
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            [MarshalAs(UnmanagedType.Bool)] public bool ClientPointers;
        }

        [DllImport("Dbghelp.dll")]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, MINIDUMP_TYPE dumpType,
            ref MINIDUMP_EXCEPTION_INFORMATION exceptionParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        public static bool TryDump([NotNullWhen(true)] out MemoryStream? compressedDataStream)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // A small dump containing module lists, thread lists, exception information and all stacks.
                    const MINIDUMP_TYPE mini = MINIDUMP_TYPE.MiniDumpNormal |
                                               MINIDUMP_TYPE.MiniDumpWithDataSegs |
                                               MINIDUMP_TYPE.MiniDumpWithHandleData |
                                               MINIDUMP_TYPE.MiniDumpWithThreadInfo;

                    var temp = Path.GetTempFileName();
                    using (var fs = new FileStream(temp, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        if (fs.SafeFileHandle is null || fs.SafeFileHandle.IsClosed || fs.SafeFileHandle.IsInvalid)
                        {
                            compressedDataStream = null;
                            return false;
                        }

                        if (!Write(fs.SafeFileHandle, mini))
                        {
                            compressedDataStream = null;
                            return false;
                        }

                        compressedDataStream = new MemoryStream();
                        using var zipStream = new GZipStream(compressedDataStream, CompressionMode.Compress, true);
                        fs.Position = 0;
                        fs.CopyTo(zipStream);
                    }

                    File.Delete(temp);
                    return true;
                }
            }
            catch (Exception) { }

            compressedDataStream = null;
            return false;
        }

        private static bool Write(SafeHandle file, MINIDUMP_TYPE dumpType)
        {
            using var process = Process.GetCurrentProcess();
            var exp = new MINIDUMP_EXCEPTION_INFORMATION
            {
                ThreadId = GetCurrentThreadId(),
                ExceptionPointers = Marshal.GetExceptionPointers(),
                ClientPointers = true,
            };
            var bRet = MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), file, dumpType, ref exp, IntPtr.Zero, IntPtr.Zero);
            return bRet;
        }
    }
}