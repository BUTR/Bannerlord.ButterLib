using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

unsafe partial class Glfw
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    private static GlfwWindowHandle* AsPointer(ref readonly GlfwWindowHandle windowHandle)
    {
        fixed (GlfwWindowHandle* windowPtr = &windowHandle)
            return windowPtr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    private static ref GlfwWindowHandle AsRef(void* source) => ref *(GlfwWindowHandle*) source;

    [Conditional("DEBUG")]
    public void CheckError()
    {
        var error = GetError(out var errorCode);
        if (errorCode is GLFW_NO_ERROR)
            return;

        Debug.Print($"GLFW {error}: {errorCode}");
    }

    [Conditional("DEBUG")]
    public void CheckErrorIgnoreInit()
    {
        var error = GetError(out var errorCode);
        if (errorCode is GLFW_NO_ERROR or GLFW_NOT_INITIALIZED)
            return;

        throw new InvalidOperationException($"GLFW {error}: {errorCode}");
    }
}