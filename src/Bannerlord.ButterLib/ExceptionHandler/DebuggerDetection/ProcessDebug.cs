using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler.DebuggerDetection
{
    internal static class ProcessDebug
    {
        private const int PROCESS_DEBUG_OBJECT_HANDLE = 0x1E;
        private const int PROCESS_DEBUG_PORT = 7;

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass,
            out IntPtr processInformation, int processInformationLength, IntPtr returnLength);

        /// <summary>
        /// Starting with Windows XP, a "debug object" is created for a debugged process.
        /// Here's an example of checking for a "debug object" in the current process:
        /// If a debug object exists, then the process is being debugged.
        /// It was originally published on https://www.apriorit.com/
        /// </summary>
        public static bool CheckProcessDebugObjectHandle()
        {
            var status = NtQueryInformationProcess
            (
                Process.GetCurrentProcess().Handle,
                PROCESS_DEBUG_OBJECT_HANDLE,
                out var flProcessDebugObject,
                IntPtr.Size,
                IntPtr.Zero
            );
            return status == 0 && (IntPtr) 0 != flProcessDebugObject;
        }

        /*
        /// <summary>
        /// The CheckRemoteDebuggerPresent function is assigned the DebugPort value,
        /// as the ProcessInformationClass parameter value (the second one) is 7.
        /// It was originally published on https://www.apriorit.com/
        /// </summary>
        /// <returns></returns>
        public static bool CheckProcessDebugPort()
        {
            var status = NtQueryInformationProcess
            (
                Process.GetCurrentProcess().Handle,
                PROCESS_DEBUG_PORT,
                out var flProcessDebugPort,
                IntPtr.Size,
                IntPtr.Zero
            );
            return status == 0 && (IntPtr) (-1) == flProcessDebugPort;
        }
        */
    }
}