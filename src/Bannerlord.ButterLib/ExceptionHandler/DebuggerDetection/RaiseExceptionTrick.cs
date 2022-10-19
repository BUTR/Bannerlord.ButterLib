/*
using System;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler.DebuggerDetection
{
    /// <summary>
    /// Since Windows 10, the implementation of the OutputDebugString function has changed to a
    /// simple RaiseException call with the specific parameters. So, debug output exception must be now handled by the debugger.
    /// There are two exception types: DBG_PRINTEXCEPTION_C (0x40010006) and DBG_PRINTEXCEPTION_W(0x4001000A),
    /// which can be used to detect the debugger presence.
    /// It was originally published on https://www.apriorit.com/
    /// </summary>
    internal static class RaiseExceptionTrick
    {
        public const uint DBG_PRINTEXCEPTION_C = 0x40010006;
        public const uint DBG_PRINTEXCEPTION_W = 0x4001000A;

        [DllImport("kernel32.dll")]
        private static extern bool RaiseException(uint dwExceptionCode, uint dwExceptionFlags, uint nNumberOfArguments, IntPtr lpArguments);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        public static bool Check()
        {
            try
            {
                RaiseException(DBG_PRINTEXCEPTION_C, 0, 0, IntPtr.Zero);
            }
            catch
            {
                return GetLastError() == DBG_PRINTEXCEPTION_C;
            }

            return true;
        }
    }
}
*/