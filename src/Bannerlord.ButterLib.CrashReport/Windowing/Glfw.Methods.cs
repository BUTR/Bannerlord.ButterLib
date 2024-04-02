using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

unsafe partial class Glfw
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr GetProcAddress(ReadOnlySpan<byte> procname)
    {
        if (GetCurrentContext() == IntPtr.Zero)
            throw new InvalidOperationException("No context is current");

        fixed (byte* procnamePtr = procname)
            return _glfwGetProcAddress(procnamePtr);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool Init()
    {
        return _glfwInit() != GLFW_FALSE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Terminate()
    {
        _glfwTerminate();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public string GetError(out int statusCode)
    {
        Unsafe.SkipInit(out statusCode);
        byte* descriptionPtr = null;
        statusCode = _glfwGetError(descriptionPtr);
        return descriptionPtr is not null ? UnsafeHelper.ToString(UnsafeHelper.CreateReadOnlySpanFromNullTerminated(descriptionPtr)) : string.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void WindowHint(int hint, int value)
    {
        _glfwWindowHint(hint, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public GlfwWindowPtr CreateWindow(int width, int height, ReadOnlySpan<byte> title, IntPtr monitor, ref readonly GlfwWindowHandle share)
    {
        fixed (byte* titlePtr = title)
            return new GlfwWindowPtr(ref AsRef(_glfwCreateWindow(width, height, titlePtr, monitor, AsPointer(in share))));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DestroyWindow(ref readonly GlfwWindowHandle windowHandle)
    {
        _glfwDestroyWindow(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int WindowShouldClose(ref readonly GlfwWindowHandle windowHandle)
    {
        return _glfwWindowShouldClose(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetWindowShouldClose(ref readonly GlfwWindowHandle windowHandle, int value)
    {
        _glfwSetWindowShouldClose(AsPointer(in windowHandle), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetWindowSize(ref readonly GlfwWindowHandle windowHandle, out int width, out int height)
    {
        Unsafe.SkipInit(out width);
        Unsafe.SkipInit(out height);
        fixed (int* widthPtr = &width)
        fixed (int* heightPtr = &height)
        {
            _glfwGetWindowSize(AsPointer(in windowHandle), widthPtr, heightPtr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetFramebufferSize(ref readonly GlfwWindowHandle windowHandle, out int width, out int height)
    {
        Unsafe.SkipInit(out width);
        Unsafe.SkipInit(out height);
        fixed (int* widthPtr = &width)
        fixed (int* heightPtr = &height)
        {
            _glfwGetFramebufferSize(AsPointer(in windowHandle), widthPtr, heightPtr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void FocusWindow(ref readonly GlfwWindowHandle windowHandle)
    {
        _glfwFocusWindow(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetWindowAttrib(ref readonly GlfwWindowHandle windowHandle, int attrib)
    {
        return _glfwGetWindowAttrib(AsPointer(in windowHandle), attrib);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public WindowSizeCallback SetWindowSizeCallback(ref readonly GlfwWindowHandle windowHandle, WindowSizeCallback callback)
    {
        return _glfwSetWindowSizeCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public FramebufferSizeCallback SetFramebufferSizeCallback(ref readonly GlfwWindowHandle windowHandle, FramebufferSizeCallback callback)
    {
        return _glfwSetFramebufferSizeCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PollEvents()
    {
        _glfwPollEvents();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetInputMode(ref readonly GlfwWindowHandle windowHandle, int mode)
    {
        return _glfwGetInputMode(AsPointer(in windowHandle), mode);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetInputMode(ref readonly GlfwWindowHandle windowHandle, int mode, int value)
    {
        _glfwSetInputMode(AsPointer(in windowHandle), mode, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetKey(ref readonly GlfwWindowHandle windowHandle, int key)
    {
        return _glfwGetKey(AsPointer(in windowHandle), key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetCursorPos(ref readonly GlfwWindowHandle windowHandle, ref Vector2 pos)
    {
        double xpos = pos.X;
        double ypos = pos.Y;
        _glfwGetCursorPos(AsPointer(in windowHandle), &xpos, &ypos);
        pos.X = (float) xpos;
        pos.Y = (float) ypos;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetCursorPos(ref readonly GlfwWindowHandle windowHandle, ref readonly Vector2 pos)
    {
        _glfwSetCursorPos(AsPointer(in windowHandle), pos.X, pos.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr CreateStandardCursor(int shape)
    {
        return _glfwCreateStandardCursor(shape);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DestroyCursor(IntPtr cursor)
    {
        _glfwDestroyCursor(cursor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetCursor(ref readonly GlfwWindowHandle windowHandle, IntPtr cursor)
    {
        _glfwSetCursor(AsPointer(in windowHandle), cursor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public MouseButtonCallback SetMouseButtonCallback(ref readonly GlfwWindowHandle windowHandle, MouseButtonCallback callback)
    {
        return _glfwSetMouseButtonCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ScrollCallback SetScrollCallback(ref readonly GlfwWindowHandle windowHandle, ScrollCallback callback)
    {
        return _glfwSetScrollCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public KeyCallback SetKeyCallback(ref readonly GlfwWindowHandle windowHandle, KeyCallback callback)
    {
        return _glfwSetKeyCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public CharCallback SetCharCallback(ref readonly GlfwWindowHandle windowHandle, CharCallback callback)
    {
        return _glfwSetCharCallback(AsPointer(in windowHandle), callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetClipboardString(ref readonly GlfwWindowHandle windowHandle, string @string)
    {
        _glfwSetClipboardString(AsPointer(in windowHandle), @string);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public string GetClipboardString(ref readonly GlfwWindowHandle windowHandle)
    {
        return _glfwGetClipboardString(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public double GetTime()
    {
        return _glfwGetTime();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void MakeContextCurrent(ref readonly GlfwWindowHandle windowHandle)
    {
        _glfwMakeContextCurrent(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr GetCurrentContext()
    {
        return _glfwGetCurrentContext();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SwapBuffers(ref readonly GlfwWindowHandle windowHandle)
    {
        _glfwSwapBuffers(AsPointer(in windowHandle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SwapInterval(int interval)
    {
        _glfwSwapInterval(interval);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ref Utf8ZPtr GetRequiredInstanceExtensions(out uint count)
    {
        Unsafe.SkipInit(out count);
        fixed (uint* countPtr = &count)
        {
            return ref Unsafe.AsRef<Utf8ZPtr>(_glfwGetRequiredInstanceExtensions(countPtr));
        }
    }
}