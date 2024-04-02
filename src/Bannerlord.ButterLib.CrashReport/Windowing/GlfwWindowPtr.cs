using System;

namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

public readonly struct GlfwWindowPtr
{
    public static unsafe implicit operator IntPtr(GlfwWindowPtr ptr) => new(ptr._windowNativePtr);

    private readonly unsafe GlfwWindowHandle* _windowNativePtr;

    public unsafe GlfwWindowPtr(IntPtr window)
    {
        _windowNativePtr = (GlfwWindowHandle*) window;
    }
    public unsafe GlfwWindowPtr(ref readonly GlfwWindowHandle windowHandle)
    {
        fixed (GlfwWindowHandle* windowPtr = &windowHandle)
            _windowNativePtr = windowPtr;
    }

    public unsafe ref readonly GlfwWindowHandle Handle => ref *_windowNativePtr;
}