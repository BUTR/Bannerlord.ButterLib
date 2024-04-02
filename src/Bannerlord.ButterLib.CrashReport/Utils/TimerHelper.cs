// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

internal static class TimerHelper
{
    [GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.11423")]
    [SkipLocalsInit]
    private static unsafe int NtQueryTimerResolution(out uint MinimumResolution, out uint MaximumResolution, out uint CurrentResolution)
    {
        //int __lastError;
        Unsafe.SkipInit(out MinimumResolution);
        Unsafe.SkipInit(out MaximumResolution);
        Unsafe.SkipInit(out CurrentResolution);
        int __retVal;
        // Pin - Pin data in preparation for calling the P/Invoke.
        fixed (uint* __CurrentResolution_native = &CurrentResolution)
        fixed (uint* __MaximumResolution_native = &MaximumResolution)
        fixed (uint* __MinimumResolution_native = &MinimumResolution)
        {
            //Marshal.SetLastSystemError(0);
            __retVal = __PInvoke(__MinimumResolution_native, __MaximumResolution_native, __CurrentResolution_native);
            //__lastError = Marshal.GetLastSystemError();
        }

        //Marshal.SetLastPInvokeError(__lastError);
        return __retVal;
        // Local P/Invoke
        [DllImport("ntdll.dll", EntryPoint = "NtQueryTimerResolution", ExactSpelling = true)]
        static extern int __PInvoke(uint* __MinimumResolution_native, uint* __MaximumResolution_native, uint* __CurrentResolution_native);
    }

    private static readonly double LowestSleepThreshold;

    static TimerHelper()
    {
        NtQueryTimerResolution(out var min, out var max, out var current);
        LowestSleepThreshold = 1.0 + (max / 10000.0);
    }

    /// <summary>
    /// Returns the current timer resolution in milliseconds
    /// </summary>
    public static double GetCurrentResolution()
    {
        NtQueryTimerResolution(out var min, out var max, out var current);
        return current / 10000.0;
    }

    /// <summary>
    /// Sleeps as long as possible without exceeding the specified period
    /// </summary>
    public static void SleepForNoMoreThan(double milliseconds)
    {
        // Assumption is that Thread.Sleep(t) will sleep for at least (t), and at most (t + timerResolution)
        if (milliseconds < LowestSleepThreshold)
            return;
        var sleepTime = (int) (milliseconds - GetCurrentResolution());
        if (sleepTime > 0)
            Thread.Sleep(sleepTime);
    }
}