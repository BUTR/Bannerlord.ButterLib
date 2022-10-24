using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

using static Bannerlord.ButterLib.ExceptionHandler.MiniDump.DbgHelpNativeMethods;
using static Bannerlord.ButterLib.ExceptionHandler.MiniDump.Kernel32NativeMethods;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal static class MiniDump
    {
        internal static class DbgHelpNativeMethods
        {
#if X64
            public const int CONTEXT_SIZE = 1232;
#else
            public const int CONTEXT_SIZE = 716;
#endif

            [DllImport("dbghelp.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool MiniDumpWriteDump(IntPtr hProcess, uint ProcessId, SafeHandle hFile, MINIDUMP_TYPE DumpType,
                [In] ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParams,
                [In] ref MINIDUMP_USER_STREAM_INFORMATION UserStreamParam,
                [In] ref MINIDUMP_CALLBACK_INFORMATION CallbackParam);

            [Flags]
            public enum MINIDUMP_TYPE : int
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
            public struct EXCEPTION_POINTERS
            {
                public IntPtr ExceptionRecord;
                public byte[] ContextRecord;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_EXCEPTION_INFORMATION
            {
                public uint ThreadId;
                public IntPtr ExceptionPointers;
                [MarshalAs(UnmanagedType.Bool)]
                public bool ClientPointers;
            }

            public enum MINIDUMP_STREAM_TYPE : uint
            {
                UnusedStream = 0,
                ReservedStream0 = 1,
                ReservedStream1 = 2,
                ThreadListStream = 3,
                ModuleListStream = 4,
                MemoryListStream = 5,
                ExceptionStream = 6,
                SystemInfoStream = 7,
                ThreadExListStream = 8,
                Memory64ListStream = 9,
                CommentStreamA = 10,
                CommentStreamW = 11,
                HandleDataStream = 12,
                FunctionTableStream = 13,
                UnloadedModuleListStream = 14,
                MiscInfoStream = 15,
                MemoryInfoListStream = 16,
                ThreadInfoListStream = 17,
                HandleOperationListStream = 18,
                LastReservedStream = 0xffff
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_USER_STREAM
            {
                public MINIDUMP_STREAM_TYPE Type;
                public uint BufferSize;
                public IntPtr Buffer;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_USER_STREAM_INFORMATION
            {
                public MINIDUMP_USER_STREAM_INFORMATION(params MINIDUMP_USER_STREAM[] streams)
                {
                    UserStreamCount = (uint) streams.Length;
                    int sizeOfStream = Marshal.SizeOf(typeof(MINIDUMP_USER_STREAM));
                    UserStreamArray = Marshal.AllocHGlobal((int) (UserStreamCount * sizeOfStream));
                    for (int i = 0; i < streams.Length; ++i)
                    {
                        Marshal.StructureToPtr(streams[i], UserStreamArray + (i * sizeOfStream), false);
                    }
                }

                public void Delete()
                {
                    Marshal.FreeHGlobal(UserStreamArray);
                    UserStreamCount = 0;
                    UserStreamArray = IntPtr.Zero;
                }

                public uint UserStreamCount;
                public IntPtr UserStreamArray;
            }

            public enum MINIDUMP_CALLBACK_TYPE : uint
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

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct VS_FIXEDFILEINFO
            {
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
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_THREAD_CALLBACK
            {
                public uint ThreadId;
                public IntPtr ThreadHandle;
                public unsafe fixed byte Context[CONTEXT_SIZE];
                public uint SizeOfContext;
                public ulong StackBase;
                public ulong StackEnd;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_THREAD_EX_CALLBACK
            {
                public MINIDUMP_THREAD_CALLBACK BasePart;
                public ulong BackingStoreBase;
                public ulong BackingStoreEnd;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_MODULE_CALLBACK
            {
                public IntPtr FullPath; // This is a PCWSTR
                public ulong BaseOfImage;
                public uint SizeOfImage;
                public uint CheckSum;
                public uint TimeDateStamp;
                public VS_FIXEDFILEINFO VersionInfo;
                public IntPtr CvRecord;
                public uint SizeOfCvRecord;
                public IntPtr MiscRecord;
                public uint SizeOfMiscRecord;
            }

            public struct MINIDUMP_INCLUDE_THREAD_CALLBACK
            {
                public uint ThreadId;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_INCLUDE_MODULE_CALLBACK
            {
                public ulong BaseOfImage;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct MINIDUMP_CALLBACK_INPUT
            {
#if X64
                const int CallbackTypeOffset = 4 + 8;
#else
                const int CallbackTypeOffset = 4 + 4;
#endif
                const int UnionOffset = CallbackTypeOffset + 4;

                [FieldOffset(0)]
                public uint ProcessId;
                [FieldOffset(4)]
                public IntPtr ProcessHandle;
                [FieldOffset(CallbackTypeOffset)]
                public MINIDUMP_CALLBACK_TYPE CallbackType;

                [FieldOffset(UnionOffset)]
                public int Status; // HRESULT
                [FieldOffset(UnionOffset)]
                public MINIDUMP_THREAD_CALLBACK Thread;
                [FieldOffset(UnionOffset)]
                public MINIDUMP_THREAD_EX_CALLBACK ThreadEx;
                [FieldOffset(UnionOffset)]
                public MINIDUMP_MODULE_CALLBACK Module;
                [FieldOffset(UnionOffset)]
                public MINIDUMP_INCLUDE_THREAD_CALLBACK IncludeThread;
                [FieldOffset(UnionOffset)]
                public MINIDUMP_INCLUDE_MODULE_CALLBACK IncludeModule;
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

        public static bool TryDump([NotNullWhen(true)] out MemoryStream? compressedDataStream)
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
                            compressedDataStream = null;
                            return false;
                        }

                        if (!WriteMini(fs.SafeFileHandle))
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

        private static bool Filter(IntPtr CallbackParam, ref MINIDUMP_CALLBACK_INPUT CallbackInput, ref MINIDUMP_CALLBACK_OUTPUT CallbackOutput)
        {
            switch (CallbackInput.CallbackType)
            {
                case MINIDUMP_CALLBACK_TYPE.IncludeModuleCallback: return true;
                case MINIDUMP_CALLBACK_TYPE.IncludeThreadCallback:
                {
                    var info = Marshal.PtrToStructure<Info>(CallbackParam);
                    return CallbackInput.IncludeThread.ThreadId == info.ThreadId;
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
                ExceptionPointers = Marshal.GetExceptionPointers(),
                ClientPointers = true,
            };
            var userStream = new MINIDUMP_USER_STREAM_INFORMATION
            {

            };
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
}