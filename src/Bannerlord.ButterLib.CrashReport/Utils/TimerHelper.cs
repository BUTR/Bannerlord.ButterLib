// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;
using System.Threading;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

internal static class TimerHelper
{
    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern int NtQueryTimerResolution(out uint MinimumResolution, out uint MaximumResolution, out uint CurrentResolution);

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