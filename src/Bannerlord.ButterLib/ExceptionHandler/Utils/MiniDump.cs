﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

using static Bannerlord.ButterLib.ExceptionHandler.Utils.MiniDump.DbgHelpNativeMethods;
using static Bannerlord.ButterLib.ExceptionHandler.Utils.MiniDump.Kernel32NativeMethods;

namespace Bannerlord.ButterLib.ExceptionHandler.Utils;

internal static class MiniDump
{
    internal static class DbgHelpNativeMethods
    {
        [DllImport("dbghelp.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess, uint ProcessId, SafeHandle hFile, MINIDUMP_TYPE DumpType,
            ref readonly MINIDUMP_EXCEPTION_INFORMATION ExceptionParams,
            ref readonly MINIDUMP_USER_STREAM_INFORMATION UserStreamParam,
            ref readonly MINIDUMP_CALLBACK_INFORMATION CallbackParam);

        [Flags]
        public enum MINIDUMP_TYPE
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
            MiniDumpValidTypeFlags = 0x001fffff,

            ConfigGameMini = MiniDumpWithDataSegs |

                             MiniDumpWithHandleData |

                             MiniDumpScanMemory |
                             MiniDumpWithUnloadedModules |
                             MiniDumpWithIndirectlyReferencedMemory |

                             MiniDumpWithProcessThreadData |

                             MiniDumpWithFullMemoryInfo |
                             MiniDumpWithThreadInfo |

                             MiniDumpWithFullAuxiliaryState |

                             MiniDumpIgnoreInaccessibleMemory |
                             MiniDumpWithTokenInformation |
                             MiniDumpWithModuleHeaders,
            ConfigGameFull = ConfigGameMini | MiniDumpWithFullMemory,

            ConfigButterLibMini = MiniDumpFilterModulePaths |
                                  MiniDumpWithIndirectlyReferencedMemory |
                                  MiniDumpScanMemory,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }

        public struct MINIDUMP_USER_STREAM
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint Type;
            public uint BufferSize;
            public IntPtr Buffer;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_USER_STREAM_INFORMATION
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint UserStreamCount;
            public MINIDUMP_USER_STREAM[] UserStreamArray;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public enum MINIDUMP_CALLBACK_TYPE
        {
            ModuleCallback,
            ThreadCallback,
            ThreadExCallback,
            IncludeThreadCallback,
            IncludeModuleCallback,
            MemoryCallback,
            CancelCallback,
            WriteKernelMinidumpCallback,
            KernelMinidumpStatusCallback,
            RemoveMemoryCallback,
            IncludeVmRegionCallback,
            IoStartCallback,
            IoWriteAllCallback,
            IoFinishCallback,
            ReadMemoryFailureCallback,
            SecondaryFlagsCallback
        }

        public struct VS_FIXEDFILEINFO
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint dwSignature;
            public uint dwStrucVersion;
            public uint dwFileVersionMS;
            public uint dwFileVersionLS;
            public uint dwProductVersionMS;
            public uint dwProductVersionLS;
            public uint dwFileFlagsMask;
            public uint dwFileFlags;
            public uint dwFileOS;
            public uint dwFileType;
            public uint dwFileSubtype;
            public uint dwFileDateMS;
            public uint dwFileDateLS;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_THREAD_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint ThreadId;
            public IntPtr ThreadHandle;
            public uint Pad;
            //public CONTEXT Context;
            //public uint SizeOfContext;
            //public ulong StackBase;
            //public ulong StackEnd;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_THREAD_EX_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint ThreadId;
            public IntPtr ThreadHandle;
            public uint Pad;
            //public CONTEXT Context;
            //public uint SizeOfContext;
            //public ulong StackBase;
            //public ulong StackEnd;
            //public ulong BackingStoreBase;
            //public ulong BackingStoreEnd;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_MODULE_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public IntPtr FullPath;
            public ulong BaseOfImage;
            public uint SizeOfImage;
            public uint CheckSum;
            public uint TimeDateStamp;
            public VS_FIXEDFILEINFO VersionInfo;
            public IntPtr CvRecord;
            public uint SizeOfCvRecord;
            public IntPtr MiscRecord;
            public uint SizeOfMiscRecord;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_INCLUDE_THREAD_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint ThreadId;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_INCLUDE_MODULE_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public ulong BaseOfImage;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_IO_CALLBACK
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public IntPtr Handle;
            public ulong Offset;
            public IntPtr Buffer;
            public uint BufferBytes;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        public struct MINIDUMP_CALLBACK_INPUT
        {
            [StructLayout(LayoutKind.Explicit)]
            public struct UNION
            {
                [FieldOffset(0)]
                public int Status; // HRESULT
                [FieldOffset(0)]
                public MINIDUMP_THREAD_CALLBACK Thread;
                [FieldOffset(0)]
                public MINIDUMP_THREAD_EX_CALLBACK ThreadEx;
                [FieldOffset(0)]
                public MINIDUMP_MODULE_CALLBACK Module;
                [FieldOffset(0)]
                public MINIDUMP_INCLUDE_THREAD_CALLBACK IncludeThread;
                [FieldOffset(0)]
                public MINIDUMP_INCLUDE_MODULE_CALLBACK IncludeModule;
                [FieldOffset(0)]
                public MINIDUMP_IO_CALLBACK Io;
            }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public uint ProcessId;
            public IntPtr ProcessHandle;
            public MINIDUMP_CALLBACK_TYPE CallbackType;
            public UNION Union;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
        }

        [Flags]
        public enum MODULE_WRITE_FLAGS : uint
        {
            ModuleWriteModule = 0x0001,
            ModuleWriteDataSeg = 0x0002,
            ModuleWriteMiscRecord = 0x0004,
            ModuleWriteCvRecord = 0x0008,
            ModuleReferencedByMemory = 0x0010,
            ModuleWriteTlsData = 0x0020,
            ModuleWriteCodeSegs = 0x0040,
        }

        [Flags]
        public enum THREAD_WRITE_FLAGS : uint
        {
            ThreadWriteThread = 0x0001,
            ThreadWriteStack = 0x0002,
            ThreadWriteContext = 0x0004,
            ThreadWriteBackingStore = 0x0008,
            ThreadWriteInstructionWindow = 0x0010,
            ThreadWriteThreadData = 0x0020,
            ThreadWriteThreadInfo = 0x0040
        }

        [StructLayout(LayoutKind.Explicit, Pack = 4)]
        public struct MINIDUMP_CALLBACK_OUTPUT
        {
            [FieldOffset(0)]
            public int Status;
            [FieldOffset(0)]
            public MODULE_WRITE_FLAGS ModuleWriteFlags;
            [FieldOffset(0)]
            public THREAD_WRITE_FLAGS ThreadWriteFlags;
            [FieldOffset(0)]
            public IntPtr Handle;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool MINIDUMP_CALLBACK_ROUTINE([In] IntPtr CallbackParam,
            [In] ref MINIDUMP_CALLBACK_INPUT CallbackInput,
            [In, Out] ref MINIDUMP_CALLBACK_OUTPUT CallbackOutput);

        public struct MINIDUMP_CALLBACK_INFORMATION
        {
            public MINIDUMP_CALLBACK_ROUTINE CallbackRoutine;
            public IntPtr CallbackParam;
        }
    }

    internal static class Kernel32NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }

    public static bool TryDump([NotNullWhen(true)] out MemoryStream? dataStream)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var temp = Path.GetTempFileName();
                using (var fs = new FileStream(temp, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    if (fs.SafeFileHandle is null || fs.SafeFileHandle.IsClosed || fs.SafeFileHandle.IsInvalid)
                    {
                        dataStream = null;
                        return false;
                    }

                    if (!WriteMini(fs.SafeFileHandle))
                    {
                        dataStream = null;
                        return false;
                    }

                    dataStream = new MemoryStream();
                    fs.Position = 0;
                    fs.CopyTo(dataStream);
                }

                File.Delete(temp);
                return true;
            }
        }
        catch (Exception) { /* ignore */ }

        dataStream = null;
        return false;
    }

    private static bool Filter(IntPtr CallbackParam, ref MINIDUMP_CALLBACK_INPUT CallbackInput, ref MINIDUMP_CALLBACK_OUTPUT CallbackOutput)
    {
        switch (CallbackInput.CallbackType)
        {
            case MINIDUMP_CALLBACK_TYPE.IncludeModuleCallback: return true;
            case MINIDUMP_CALLBACK_TYPE.IncludeThreadCallback:
            {
                var info = Marshal.PtrToStructure<Info>(CallbackParam);
                return CallbackInput.Union.IncludeThread.ThreadId == info.ThreadId;
            }
            case MINIDUMP_CALLBACK_TYPE.ModuleCallback:
            {
                if (!CallbackOutput.ModuleWriteFlags.HasFlag(MODULE_WRITE_FLAGS.ModuleReferencedByMemory))
                {
                    CallbackOutput.ModuleWriteFlags &= ~MODULE_WRITE_FLAGS.ModuleWriteModule;
                    return true;
                }
                /*
                if (CallbackOutput.ModuleWriteFlags.HasFlag(MODULE_WRITE_FLAGS.ModuleWriteDataSeg))
                {
                    CallbackOutput.ModuleWriteFlags &= ~MODULE_WRITE_FLAGS.ModuleWriteDataSeg;
                }
                */
                return true;
            }
            case MINIDUMP_CALLBACK_TYPE.ThreadCallback: return true;
            case MINIDUMP_CALLBACK_TYPE.ThreadExCallback: return true;
            case MINIDUMP_CALLBACK_TYPE.MemoryCallback: return false;
        }
        return true;
    }

    private static bool WriteMini(SafeHandle file)
    {
        return Write(file, MINIDUMP_TYPE.ConfigButterLibMini, Filter);
    }

    public struct Info
    {
        public uint ThreadId;
    }

    private static bool Write(SafeHandle file, MINIDUMP_TYPE dumpType, MINIDUMP_CALLBACK_ROUTINE callbackRoutine)
    {
        using var process = Process.GetCurrentProcess();

        var exception = new MINIDUMP_EXCEPTION_INFORMATION
        {
            ThreadId = GetCurrentThreadId(),
#if !NETSTANDARD2_0
            ExceptionPointers = Marshal.GetExceptionPointers(),
#endif
            ClientPointers = true,
        };
        var userStream = new MINIDUMP_USER_STREAM_INFORMATION();
        var infoPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Info>());
        Marshal.StructureToPtr(new Info
        {
            ThreadId = exception.ThreadId
        }, infoPtr, false);
        var callback = new MINIDUMP_CALLBACK_INFORMATION
        {
            CallbackParam = infoPtr,
            CallbackRoutine = callbackRoutine,
        };

        return MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), file, dumpType, ref exception, ref userStream, ref callback);
    }
}