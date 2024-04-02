using Microsoft.Win32.SafeHandles;

using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Bannerlord.ButterLib.CrashReportWindow.Extensions;

internal static class ProcessExtensions
{
    public static Process? ParentProcess(this Process process) => ParentProcessId(process.Id) is var pId && pId is not -1 ? Process.GetProcessById(pId) : null;

    private static int ParentProcessId(int Id)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var pe32 = new PROCESSENTRY32
            {
                dwSize = (uint) Marshal.SizeOf(typeof(PROCESSENTRY32))
            };
            using var hSnapshot = CreateToolhelp32Snapshot(SnapshotFlags.Process, (uint) Id);
            if (hSnapshot.IsInvalid) return -1;

            if (!Process32First(hSnapshot, ref pe32))
                return -1;

            do
            {
                if (pe32.th32ProcessID == (uint) Id)
                    return (int) pe32.th32ParentProcessID;
            } while (Process32Next(hSnapshot, ref pe32));
        }

        return -1;
    }

    [DllImport("kernel32.dll")]
    private static extern SafeSnapshotHandle CreateToolhelp32Snapshot(SnapshotFlags flags, uint id);

    [DllImport("kernel32.dll")]
    private static extern bool Process32First(SafeSnapshotHandle hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll")]
    private static extern bool Process32Next(SafeSnapshotHandle hSnapshot, ref PROCESSENTRY32 lppe);

    [Flags]
    private enum SnapshotFlags : uint
    {
        HeapList = 0x00000001,
        Process = 0x00000002,
        Thread = 0x00000004,
        Module = 0x00000008,
        Module32 = 0x00000010,
        All = (HeapList | Process | Thread | Module),
        Inherit = 0x80000000,
        NoHeaps = 0x40000000
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    };

    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    [SuppressUnmanagedCodeSecurity]
    private sealed class SafeSnapshotHandle : SafeHandleMinusOneIsInvalid
    {
        internal SafeSnapshotHandle() : base(true) { }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeSnapshotHandle(IntPtr handle) : base(true) => SetHandle(handle);

        protected override bool ReleaseHandle() => CloseHandle(handle);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
    }
}