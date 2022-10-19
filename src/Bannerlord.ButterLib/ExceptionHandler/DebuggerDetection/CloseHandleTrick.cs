/*
using System;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler.DebuggerDetection
{
    /// <summary>
    /// Starting with Windows XP, Windows systems have had a mechanism for kernel object handle tracing.
    /// When the tracing mode is on, all operations with handlers are saved to the circular buffer, also,
    /// when trying to use a nonexistent handler, for instance to close it using the CloseHandle function,
    /// the EXCEPTION_INVALID_HANDLE exception will be generated. If a process is started not from the debugger,
    /// the CloseHandle function will return FALSE.
    /// It was originally published on https://www.apriorit.com/
    /// </summary>
    internal static class CloseHandleTrick
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
    
        public static bool Check()
        {
            try
            {
                if (IntPtr.Size == 8)
                    CloseHandle((IntPtr) 0xDEADBEEF);
                else
                    CloseHandle((IntPtr) 0xDEADBEE);
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
*/