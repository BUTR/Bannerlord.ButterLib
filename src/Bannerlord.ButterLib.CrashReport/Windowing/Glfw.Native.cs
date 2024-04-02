using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;
using Bannerlord.ButterLib.CrashReportWindow.Utils;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

unsafe partial class Glfw
{
    private const string LibWindows = "glfw3.dll";
    private const string LibLinux = "libglfw.so.3";
    private const string LibOSX = "libglfw.3.dylib";
    private static readonly IntPtr NativeLibrary = GetNativeLibrary();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    private static IntPtr GetNativeLibrary()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return NativeFuncExecuturer.LoadLibraryExt(LibWindows);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return NativeFuncExecuturer.LoadLibraryExt(LibLinux);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return NativeFuncExecuturer.LoadLibraryExt(LibOSX);
        return IntPtr.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public static IntPtr LoadFunction(ReadOnlySpan<byte> function) => NativeFuncExecuturer.LoadFunction(NativeLibrary, function);

    /// <summary>
    /// Returns a function pointer for the GLFW function with the specified name. 
    /// </summary>
    /// <param name="funcNameUtf8">The name of the function to lookup.</param>
    public delegate IntPtr GetProcAddressHandler(ReadOnlySpan<byte> funcNameUtf8);

    private readonly delegate* unmanaged[Cdecl]<byte*, IntPtr> _glfwGetProcAddress;
    private readonly delegate* unmanaged[Cdecl]<int> _glfwInit;
    private readonly delegate* unmanaged[Cdecl]<byte*, int> _glfwGetError;
    private readonly delegate* unmanaged[Cdecl]<void> _glfwTerminate;
    private readonly delegate* unmanaged[Cdecl]<int, int, void> _glfwWindowHint;
    private readonly delegate* unmanaged[Cdecl]<int, int, byte*, IntPtr, GlfwWindowHandle*, GlfwWindowHandle*> _glfwCreateWindow;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void> _glfwDestroyWindow;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int> _glfwWindowShouldClose;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, void> _glfwSetWindowShouldClose;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int*, int*, void> _glfwGetWindowSize;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int*, int*, void> _glfwGetFramebufferSize;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void> _glfwFocusWindow;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int> _glfwGetWindowAttrib;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, WindowSizeCallback, WindowSizeCallback> _glfwSetWindowSizeCallback;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, FramebufferSizeCallback, FramebufferSizeCallback> _glfwSetFramebufferSizeCallback;
    private readonly delegate* unmanaged[Cdecl]<void> _glfwPollEvents;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int> _glfwGetInputMode;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int, void> _glfwSetInputMode;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int> _glfwGetKey;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, double*, double*, void> _glfwGetCursorPos;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, double, double, void> _glfwSetCursorPos;
    private readonly delegate* unmanaged[Cdecl]<int, IntPtr> _glfwCreateStandardCursor;
    private readonly delegate* unmanaged[Cdecl]<IntPtr, void> _glfwDestroyCursor;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, IntPtr, void> _glfwSetCursor;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, MouseButtonCallback, MouseButtonCallback> _glfwSetMouseButtonCallback;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, ScrollCallback, ScrollCallback> _glfwSetScrollCallback;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, KeyCallback, KeyCallback> _glfwSetKeyCallback;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, CharCallback, CharCallback> _glfwSetCharCallback;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, string, void> _glfwSetClipboardString;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, string> _glfwGetClipboardString;
    private readonly delegate* unmanaged[Cdecl]<double> _glfwGetTime;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void> _glfwMakeContextCurrent;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> _glfwGetCurrentContext;
    private readonly delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void> _glfwSwapBuffers;
    private readonly delegate* unmanaged[Cdecl]<int, void> _glfwSwapInterval;
    private readonly delegate* unmanaged[Cdecl]<uint*, Utf8ZPtr*> _glfwGetRequiredInstanceExtensions;

    public Glfw(GetProcAddressHandler getProcAddress)
    {
        _glfwGetProcAddress = (delegate* unmanaged[Cdecl]<byte*, IntPtr>) getProcAddress("glfwGetProcAddress\0"u8);
        _glfwInit = (delegate* unmanaged[Cdecl]<int>) getProcAddress("glfwInit\0"u8);
        _glfwGetError = (delegate* unmanaged[Cdecl]<byte*, int>) getProcAddress("glfwGetError\0"u8);
        _glfwTerminate = (delegate* unmanaged[Cdecl]<void>) getProcAddress("glfwTerminate\0"u8);
        _glfwWindowHint = (delegate* unmanaged[Cdecl]<int, int, void>) getProcAddress("glfwWindowHint\0"u8);
        _glfwCreateWindow = (delegate* unmanaged[Cdecl]<int, int, byte*, IntPtr, GlfwWindowHandle*, GlfwWindowHandle*>) getProcAddress("glfwCreateWindow\0"u8);
        _glfwDestroyWindow = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void>) getProcAddress("glfwDestroyWindow\0"u8);
        _glfwWindowShouldClose = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int>) getProcAddress("glfwWindowShouldClose\0"u8);
        _glfwSetWindowShouldClose = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, void>) getProcAddress("glfwSetWindowShouldClose\0"u8);
        _glfwGetWindowSize = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int*, int*, void>) getProcAddress("glfwGetWindowSize\0"u8);
        _glfwGetFramebufferSize = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int*, int*, void>) getProcAddress("glfwGetFramebufferSize\0"u8);
        _glfwFocusWindow = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void>) getProcAddress("glfwFocusWindow\0"u8);
        _glfwGetWindowAttrib = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int>) getProcAddress("glfwGetWindowAttrib\0"u8);
        _glfwSetWindowSizeCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, WindowSizeCallback, WindowSizeCallback>) getProcAddress("glfwSetWindowSizeCallback\0"u8);
        _glfwSetFramebufferSizeCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, FramebufferSizeCallback, FramebufferSizeCallback>) getProcAddress("glfwSetFramebufferSizeCallback\0"u8);
        _glfwPollEvents = (delegate* unmanaged[Cdecl]<void>) getProcAddress("glfwPollEvents\0"u8);
        _glfwGetInputMode = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int>) getProcAddress("glfwGetInputMode\0"u8);
        _glfwSetInputMode = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int, void>) getProcAddress("glfwSetInputMode\0"u8);
        _glfwGetKey = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, int, int>) getProcAddress("glfwGetKey\0"u8);
        _glfwGetCursorPos = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, double*, double*, void>) getProcAddress("glfwGetCursorPos\0"u8);
        _glfwSetCursorPos = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, double, double, void>) getProcAddress("glfwSetCursorPos\0"u8);
        _glfwCreateStandardCursor = (delegate* unmanaged[Cdecl]<int, IntPtr>) getProcAddress("glfwCreateStandardCursor\0"u8);
        _glfwDestroyCursor = (delegate* unmanaged[Cdecl]<IntPtr, void>) getProcAddress("glfwDestroyCursor\0"u8);
        _glfwSetCursor = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, IntPtr, void>) getProcAddress("glfwSetCursor\0"u8);
        _glfwSetMouseButtonCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, MouseButtonCallback, MouseButtonCallback>) getProcAddress("glfwSetMouseButtonCallback\0"u8);
        _glfwSetScrollCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, ScrollCallback, ScrollCallback>) getProcAddress("glfwSetScrollCallback\0"u8);
        _glfwSetKeyCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, KeyCallback, KeyCallback>) getProcAddress("glfwSetKeyCallback\0"u8);
        _glfwSetCharCallback = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, CharCallback, CharCallback>) getProcAddress("glfwSetCharCallback\0"u8);
        _glfwSetClipboardString = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, string, void>) getProcAddress("glfwSetClipboardString\0"u8);
        _glfwGetClipboardString = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, string>) getProcAddress("glfwGetClipboardString\0"u8);
        _glfwGetTime = (delegate* unmanaged[Cdecl]<double>) getProcAddress("glfwGetTime\0"u8);
        _glfwMakeContextCurrent = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void>) getProcAddress("glfwMakeContextCurrent\0"u8);
        _glfwGetCurrentContext = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("glfwGetCurrentContext\0"u8);
        _glfwSwapBuffers = (delegate* unmanaged[Cdecl]<GlfwWindowHandle*, void>) getProcAddress("glfwSwapBuffers\0"u8);
        _glfwSwapInterval = (delegate* unmanaged[Cdecl]<int, void>) getProcAddress("glfwSwapInterval\0"u8);
        _glfwGetRequiredInstanceExtensions = (delegate* unmanaged[Cdecl]<uint*, Utf8ZPtr*>) getProcAddress("glfwGetRequiredInstanceExtensions\0"u8);
    }
}