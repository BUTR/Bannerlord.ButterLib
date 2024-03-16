using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

internal static unsafe partial class GLFW
{
    private const string LibWindows = "glfw3.dll";
    private const string LibLinux = "libglfw.so.3.3";
    private static readonly IntPtr NativeLibrary = GetNativeLibrary();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntPtr GetNativeLibrary() => NativeFuncExecuturer.LoadLibraryExt(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? LibWindows : LibLinux);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T LoadFunction<T>(ReadOnlySpan<byte> function) => NativeFuncExecuturer.LoadFunction<T>(NativeLibrary, function);





    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void** glfwGetMonitorsDelegate(out int count);
    private static readonly glfwGetMonitorsDelegate _glfwGetMonitors = LoadFunction<glfwGetMonitorsDelegate>("glfwGetMonitors"u8);
    public static IntPtr[] glfwGetMonitors(out int count)
    {
        var monitors = _glfwGetMonitors(out count);
        var ret = new IntPtr[count];
        for (var i = 0; i < count; i++)
            ret[i] = (IntPtr) monitors[i];
        return ret;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetVideoModeDelegate(IntPtr monitor);
    private static readonly glfwGetVideoModeDelegate _glfwGetVideoMode = LoadFunction<glfwGetVideoModeDelegate>("glfwGetVideoMode"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWvidmode glfwGetVideoMode(IntPtr monitor) => Marshal.PtrToStructure<GLFWvidmode>(_glfwGetVideoMode(monitor));

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwInitDelegate();
    private static readonly glfwInitDelegate _glfwInit = LoadFunction<glfwInitDelegate>("glfwInit"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwInit() => _glfwInit();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwTerminateDelegate();
    private static readonly glfwTerminateDelegate _glfwTerminate = LoadFunction<glfwTerminateDelegate>("glfwTerminate"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwTerminate() => _glfwTerminate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwInitHintDelegate(int hint, int value);
    private static readonly glfwInitHintDelegate _glfwInitHint = LoadFunction<glfwInitHintDelegate>("glfwInitHint"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwInitHint(int hint, int value) => _glfwInitHint(hint, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetVersionDelegate(out int major, out int minor, out int rev);
    private static readonly glfwGetVersionDelegate _glfwGetVersion = LoadFunction<glfwGetVersionDelegate>("glfwGetVersion"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetVersion(out int major, out int minor, out int rev) => _glfwGetVersion(out major, out minor, out rev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetVersionStringDelegate();
    private static readonly glfwGetVersionStringDelegate _glfwGetVersionString = LoadFunction<glfwGetVersionStringDelegate>("glfwGetVersionString"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetVersionString() => _glfwGetVersionString();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetErrorDelegate(out string description);
    private static readonly glfwGetErrorDelegate _glfwGetError = LoadFunction<glfwGetErrorDelegate>("glfwGetError"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetError(out string description) => _glfwGetError(out description);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWerrorfun glfwSetErrorCallbackDelegate(GLFWerrorfun callback);
    private static readonly glfwSetErrorCallbackDelegate _glfwSetErrorCallback = LoadFunction<glfwSetErrorCallbackDelegate>("glfwSetErrorCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun callback) => _glfwSetErrorCallback(callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetPrimaryMonitorDelegate();
    private static readonly glfwGetPrimaryMonitorDelegate _glfwGetPrimaryMonitor = LoadFunction<glfwGetPrimaryMonitorDelegate>("glfwGetPrimaryMonitor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetPrimaryMonitor() => _glfwGetPrimaryMonitor();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetMonitorPosDelegate(IntPtr monitor, out int xpos, out int ypos);
    private static readonly glfwGetMonitorPosDelegate _glfwGetMonitorPos = LoadFunction<glfwGetMonitorPosDelegate>("glfwGetMonitorPos"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetMonitorPos(IntPtr monitor, out int xpos, out int ypos) => _glfwGetMonitorPos(monitor, out xpos, out ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetMonitorWorkareaDelegate(IntPtr monitor, out int xpos, out int ypos, out int width, out int height);
    private static readonly glfwGetMonitorWorkareaDelegate _glfwGetMonitorWorkarea = LoadFunction<glfwGetMonitorWorkareaDelegate>("glfwGetMonitorWorkarea"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetMonitorWorkarea(IntPtr monitor, out int xpos, out int ypos, out int width, out int height) => _glfwGetMonitorWorkarea(monitor, out xpos, out ypos, out width, out height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetMonitorPhysicalSizeDelegate(IntPtr monitor, out int widthMM, out int heightMM);
    private static readonly glfwGetMonitorPhysicalSizeDelegate _glfwGetMonitorPhysicalSize = LoadFunction<glfwGetMonitorPhysicalSizeDelegate>("glfwGetMonitorPhysicalSize"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetMonitorPhysicalSize(IntPtr monitor, out int widthMM, out int heightMM) => _glfwGetMonitorPhysicalSize(monitor, out widthMM, out heightMM);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetMonitorContentScaleDelegate(IntPtr monitor, out float xscale, out float yscale);
    private static readonly glfwGetMonitorContentScaleDelegate _glfwGetMonitorContentScale = LoadFunction<glfwGetMonitorContentScaleDelegate>("glfwGetMonitorContentScale"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetMonitorContentScale(IntPtr monitor, out float xscale, out float yscale) => _glfwGetMonitorContentScale(monitor, out xscale, out yscale);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetMonitorNameDelegate(IntPtr monitor);
    private static readonly glfwGetMonitorNameDelegate _glfwGetMonitorName = LoadFunction<glfwGetMonitorNameDelegate>("glfwGetMonitorName"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetMonitorName(IntPtr monitor) => _glfwGetMonitorName(monitor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetMonitorUserPointerDelegate(IntPtr monitor, IntPtr pointer);
    private static readonly glfwSetMonitorUserPointerDelegate _glfwSetMonitorUserPointer = LoadFunction<glfwSetMonitorUserPointerDelegate>("glfwSetMonitorUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetMonitorUserPointer(IntPtr monitor, IntPtr pointer) => _glfwSetMonitorUserPointer(monitor, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetMonitorUserPointerDelegate(IntPtr monitor);
    private static readonly glfwGetMonitorUserPointerDelegate _glfwGetMonitorUserPointer = LoadFunction<glfwGetMonitorUserPointerDelegate>("glfwGetMonitorUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetMonitorUserPointer(IntPtr monitor) => _glfwGetMonitorUserPointer(monitor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWmonitorfun glfwSetMonitorCallbackDelegate(GLFWmonitorfun callback);
    private static readonly glfwSetMonitorCallbackDelegate _glfwSetMonitorCallback = LoadFunction<glfwSetMonitorCallbackDelegate>("glfwSetMonitorCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWmonitorfun glfwSetMonitorCallback(GLFWmonitorfun callback) => _glfwSetMonitorCallback(callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetVideoModesDelegate(IntPtr monitor, out int count);
    private static readonly glfwGetVideoModesDelegate _glfwGetVideoModes = LoadFunction<glfwGetVideoModesDelegate>("glfwGetVideoModes"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetVideoModes(IntPtr monitor, out int count) => _glfwGetVideoModes(monitor, out count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetGammaDelegate(IntPtr monitor, float gamma);
    private static readonly glfwSetGammaDelegate _glfwSetGamma = LoadFunction<glfwSetGammaDelegate>("glfwSetGamma"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetGamma(IntPtr monitor, float gamma) => _glfwSetGamma(monitor, gamma);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetGammaRampDelegate(IntPtr monitor);
    private static readonly glfwGetGammaRampDelegate _glfwGetGammaRamp = LoadFunction<glfwGetGammaRampDelegate>("glfwGetGammaRamp"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetGammaRamp(IntPtr monitor) => _glfwGetGammaRamp(monitor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetGammaRampDelegate(IntPtr monitor, IntPtr ramp);
    private static readonly glfwSetGammaRampDelegate _glfwSetGammaRamp = LoadFunction<glfwSetGammaRampDelegate>("glfwSetGammaRamp"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetGammaRamp(IntPtr monitor, IntPtr ramp) => _glfwSetGammaRamp(monitor, ramp);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwDefaultWindowHintsDelegate();
    private static readonly glfwDefaultWindowHintsDelegate _glfwDefaultWindowHints = LoadFunction<glfwDefaultWindowHintsDelegate>("glfwDefaultWindowHints"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwDefaultWindowHints() => _glfwDefaultWindowHints();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwWindowHintDelegate(int hint, int value);
    private static readonly glfwWindowHintDelegate _glfwWindowHint = LoadFunction<glfwWindowHintDelegate>("glfwWindowHint"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwWindowHint(int hint, int value) => _glfwWindowHint(hint, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwWindowHintStringDelegate(int hint, string value);
    private static readonly glfwWindowHintStringDelegate _glfwWindowHintString = LoadFunction<glfwWindowHintStringDelegate>("glfwWindowHintString"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwWindowHintString(int hint, string value) => _glfwWindowHintString(hint, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwCreateWindowDelegate(int width, int height, string title, IntPtr monitor, IntPtr share);
    private static readonly glfwCreateWindowDelegate _glfwCreateWindow = LoadFunction<glfwCreateWindowDelegate>("glfwCreateWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share) => _glfwCreateWindow(width, height, title, monitor, share);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwDestroyWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwDestroyWindowDelegate _glfwDestroyWindow = LoadFunction<glfwDestroyWindowDelegate>("glfwDestroyWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwDestroyWindow(ref readonly GLFWWindow window) => _glfwDestroyWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwWindowShouldCloseDelegate(ref readonly GLFWWindow window);
    private static readonly glfwWindowShouldCloseDelegate _glfwWindowShouldClose = LoadFunction<glfwWindowShouldCloseDelegate>("glfwWindowShouldClose"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwWindowShouldClose(ref readonly GLFWWindow window) => _glfwWindowShouldClose(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowShouldCloseDelegate(ref readonly GLFWWindow window, int value);
    private static readonly glfwSetWindowShouldCloseDelegate _glfwSetWindowShouldClose = LoadFunction<glfwSetWindowShouldCloseDelegate>("glfwSetWindowShouldClose"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowShouldClose(ref readonly GLFWWindow window, int value) => _glfwSetWindowShouldClose(in window, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowTitleDelegate(ref readonly GLFWWindow window, string title);
    private static readonly glfwSetWindowTitleDelegate _glfwSetWindowTitle = LoadFunction<glfwSetWindowTitleDelegate>("glfwSetWindowTitle"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowTitle(ref readonly GLFWWindow window, string title) => _glfwSetWindowTitle(in window, title);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowIconDelegate(ref readonly GLFWWindow window, int count, IntPtr images);
    private static readonly glfwSetWindowIconDelegate _glfwSetWindowIcon = LoadFunction<glfwSetWindowIconDelegate>("glfwSetWindowIcon"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowIcon(ref readonly GLFWWindow window, int count, IntPtr images) => _glfwSetWindowIcon(in window, count, images);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetWindowPosDelegate(ref readonly GLFWWindow window, out int xpos, out int ypos);
    private static readonly glfwGetWindowPosDelegate _glfwGetWindowPos = LoadFunction<glfwGetWindowPosDelegate>("glfwGetWindowPos"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetWindowPos(ref readonly GLFWWindow window, out int xpos, out int ypos) => _glfwGetWindowPos(in window, out xpos, out ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowPosDelegate(ref readonly GLFWWindow window, int xpos, int ypos);
    private static readonly glfwSetWindowPosDelegate _glfwSetWindowPos = LoadFunction<glfwSetWindowPosDelegate>("glfwSetWindowPos"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowPos(ref readonly GLFWWindow window, int xpos, int ypos) => _glfwSetWindowPos(in window, xpos, ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetWindowSizeDelegate(ref readonly GLFWWindow window, out int width, out int height);
    private static readonly glfwGetWindowSizeDelegate _glfwGetWindowSize = LoadFunction<glfwGetWindowSizeDelegate>("glfwGetWindowSize"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetWindowSize(ref readonly GLFWWindow window, out int width, out int height) => _glfwGetWindowSize(in window, out width, out height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowSizeLimitsDelegate(ref readonly GLFWWindow window, int minwidth, int minheight, int maxwidth, int maxheight);
    private static readonly glfwSetWindowSizeLimitsDelegate _glfwSetWindowSizeLimits = LoadFunction<glfwSetWindowSizeLimitsDelegate>("glfwSetWindowSizeLimits"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowSizeLimits(ref readonly GLFWWindow window, int minwidth, int minheight, int maxwidth, int maxheight) => _glfwSetWindowSizeLimits(in window, minwidth, minheight, maxwidth, maxheight);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowAspectRatioDelegate(ref readonly GLFWWindow window, int numer, int denom);
    private static readonly glfwSetWindowAspectRatioDelegate _glfwSetWindowAspectRatio = LoadFunction<glfwSetWindowAspectRatioDelegate>("glfwSetWindowAspectRatio"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowAspectRatio(ref readonly GLFWWindow window, int numer, int denom) => _glfwSetWindowAspectRatio(in window, numer, denom);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowSizeDelegate(ref readonly GLFWWindow window, int width, int height);
    private static readonly glfwSetWindowSizeDelegate _glfwSetWindowSize = LoadFunction<glfwSetWindowSizeDelegate>("glfwSetWindowSize"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowSize(ref readonly GLFWWindow window, int width, int height) => _glfwSetWindowSize(in window, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetFramebufferSizeDelegate(ref readonly GLFWWindow window, out int width, out int height);
    private static readonly glfwGetFramebufferSizeDelegate _glfwGetFramebufferSize = LoadFunction<glfwGetFramebufferSizeDelegate>("glfwGetFramebufferSize"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetFramebufferSize(ref readonly GLFWWindow window, out int width, out int height) => _glfwGetFramebufferSize(in window, out width, out height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetWindowFrameSizeDelegate(ref readonly GLFWWindow window, out int left, out int top, out int right, out int bottom);
    private static readonly glfwGetWindowFrameSizeDelegate _glfwGetWindowFrameSize = LoadFunction<glfwGetWindowFrameSizeDelegate>("glfwGetWindowFrameSize"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetWindowFrameSize(ref readonly GLFWWindow window, out int left, out int top, out int right, out int bottom) => _glfwGetWindowFrameSize(in window, out left, out top, out right, out bottom);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetWindowContentScaleDelegate(ref readonly GLFWWindow window, out float xscale, out float yscale);
    private static readonly glfwGetWindowContentScaleDelegate _glfwGetWindowContentScale = LoadFunction<glfwGetWindowContentScaleDelegate>("glfwGetWindowContentScale"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetWindowContentScale(ref readonly GLFWWindow window, out float xscale, out float yscale) => _glfwGetWindowContentScale(in window, out xscale, out yscale);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate float glfwGetWindowOpacityDelegate(ref readonly GLFWWindow window);
    private static readonly glfwGetWindowOpacityDelegate _glfwGetWindowOpacity = LoadFunction<glfwGetWindowOpacityDelegate>("glfwGetWindowOpacity"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float glfwGetWindowOpacity(ref readonly GLFWWindow window) => _glfwGetWindowOpacity(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowOpacityDelegate(ref readonly GLFWWindow window, float opacity);
    private static readonly glfwSetWindowOpacityDelegate _glfwSetWindowOpacity = LoadFunction<glfwSetWindowOpacityDelegate>("glfwSetWindowOpacity"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowOpacity(ref readonly GLFWWindow window, float opacity) => _glfwSetWindowOpacity(in window, opacity);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwIconifyWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwIconifyWindowDelegate _glfwIconifyWindow = LoadFunction<glfwIconifyWindowDelegate>("glfwIconifyWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwIconifyWindow(ref readonly GLFWWindow window) => _glfwIconifyWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwRestoreWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwRestoreWindowDelegate _glfwRestoreWindow = LoadFunction<glfwRestoreWindowDelegate>("glfwRestoreWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwRestoreWindow(ref readonly GLFWWindow window) => _glfwRestoreWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwMaximizeWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwMaximizeWindowDelegate _glfwMaximizeWindow = LoadFunction<glfwMaximizeWindowDelegate>("glfwMaximizeWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwMaximizeWindow(ref readonly GLFWWindow window) => _glfwMaximizeWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwShowWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwShowWindowDelegate _glfwShowWindow = LoadFunction<glfwShowWindowDelegate>("glfwShowWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwShowWindow(ref readonly GLFWWindow window) => _glfwShowWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwHideWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwHideWindowDelegate _glfwHideWindow = LoadFunction<glfwHideWindowDelegate>("glfwHideWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwHideWindow(ref readonly GLFWWindow window) => _glfwHideWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwFocusWindowDelegate(ref readonly GLFWWindow window);
    private static readonly glfwFocusWindowDelegate _glfwFocusWindow = LoadFunction<glfwFocusWindowDelegate>("glfwFocusWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwFocusWindow(ref readonly GLFWWindow window) => _glfwFocusWindow(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwRequestWindowAttentionDelegate(ref readonly GLFWWindow window);
    private static readonly glfwRequestWindowAttentionDelegate _glfwRequestWindowAttention = LoadFunction<glfwRequestWindowAttentionDelegate>("glfwRequestWindowAttention"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwRequestWindowAttention(ref readonly GLFWWindow window) => _glfwRequestWindowAttention(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetWindowMonitorDelegate(ref readonly GLFWWindow window);
    private static readonly glfwGetWindowMonitorDelegate _glfwGetWindowMonitor = LoadFunction<glfwGetWindowMonitorDelegate>("glfwGetWindowMonitor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetWindowMonitor(ref readonly GLFWWindow window) => _glfwGetWindowMonitor(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowMonitorDelegate(ref readonly GLFWWindow window, IntPtr monitor, int xpos, int ypos, int width, int height, int refreshRate);
    private static readonly glfwSetWindowMonitorDelegate _glfwSetWindowMonitor = LoadFunction<glfwSetWindowMonitorDelegate>("glfwSetWindowMonitor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowMonitor(ref readonly GLFWWindow window, IntPtr monitor, int xpos, int ypos, int width, int height, int refreshRate) => _glfwSetWindowMonitor(in window, monitor, xpos, ypos, width, height, refreshRate);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetWindowAttribDelegate(ref readonly GLFWWindow window, int attrib);
    private static readonly glfwGetWindowAttribDelegate _glfwGetWindowAttrib = LoadFunction<glfwGetWindowAttribDelegate>("glfwGetWindowAttrib"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetWindowAttrib(ref readonly GLFWWindow window, int attrib) => _glfwGetWindowAttrib(in window, attrib);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowAttribDelegate(ref readonly GLFWWindow window, int attrib, int value);
    private static readonly glfwSetWindowAttribDelegate _glfwSetWindowAttrib = LoadFunction<glfwSetWindowAttribDelegate>("glfwSetWindowAttrib"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowAttrib(ref readonly GLFWWindow window, int attrib, int value) => _glfwSetWindowAttrib(in window, attrib, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetWindowUserPointerDelegate(ref readonly GLFWWindow window, IntPtr pointer);
    private static readonly glfwSetWindowUserPointerDelegate _glfwSetWindowUserPointer = LoadFunction<glfwSetWindowUserPointerDelegate>("glfwSetWindowUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetWindowUserPointer(ref readonly GLFWWindow window, IntPtr pointer) => _glfwSetWindowUserPointer(in window, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetWindowUserPointerDelegate(ref readonly GLFWWindow window);
    private static readonly glfwGetWindowUserPointerDelegate _glfwGetWindowUserPointer = LoadFunction<glfwGetWindowUserPointerDelegate>("glfwGetWindowUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetWindowUserPointer(ref readonly GLFWWindow window) => _glfwGetWindowUserPointer(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowposfun glfwSetWindowPosCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowposfun callback);
    private static readonly glfwSetWindowPosCallbackDelegate _glfwSetWindowPosCallback = LoadFunction<glfwSetWindowPosCallbackDelegate>("glfwSetWindowPosCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowposfun glfwSetWindowPosCallback(ref readonly GLFWWindow window, GLFWwindowposfun callback) => _glfwSetWindowPosCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowsizefun glfwSetWindowSizeCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowsizefun callback);
    private static readonly glfwSetWindowSizeCallbackDelegate _glfwSetWindowSizeCallback = LoadFunction<glfwSetWindowSizeCallbackDelegate>("glfwSetWindowSizeCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowsizefun glfwSetWindowSizeCallback(ref readonly GLFWWindow window, GLFWwindowsizefun callback) => _glfwSetWindowSizeCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowclosefun glfwSetWindowCloseCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowclosefun callback);
    private static readonly glfwSetWindowCloseCallbackDelegate _glfwSetWindowCloseCallback = LoadFunction<glfwSetWindowCloseCallbackDelegate>("glfwSetWindowCloseCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowclosefun glfwSetWindowCloseCallback(ref readonly GLFWWindow window, GLFWwindowclosefun callback) => _glfwSetWindowCloseCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowrefreshfun glfwSetWindowRefreshCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowrefreshfun callback);
    private static readonly glfwSetWindowRefreshCallbackDelegate _glfwSetWindowRefreshCallback = LoadFunction<glfwSetWindowRefreshCallbackDelegate>("glfwSetWindowRefreshCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowrefreshfun glfwSetWindowRefreshCallback(ref readonly GLFWWindow window, GLFWwindowrefreshfun callback) => _glfwSetWindowRefreshCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowfocusfun glfwSetWindowFocusCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowfocusfun callback);
    private static readonly glfwSetWindowFocusCallbackDelegate _glfwSetWindowFocusCallback = LoadFunction<glfwSetWindowFocusCallbackDelegate>("glfwSetWindowFocusCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowfocusfun glfwSetWindowFocusCallback(ref readonly GLFWWindow window, GLFWwindowfocusfun callback) => _glfwSetWindowFocusCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowiconifyfun glfwSetWindowIconifyCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowiconifyfun callback);
    private static readonly glfwSetWindowIconifyCallbackDelegate _glfwSetWindowIconifyCallback = LoadFunction<glfwSetWindowIconifyCallbackDelegate>("glfwSetWindowIconifyCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowiconifyfun glfwSetWindowIconifyCallback(ref readonly GLFWWindow window, GLFWwindowiconifyfun callback) => _glfwSetWindowIconifyCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowmaximizefun glfwSetWindowMaximizeCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowmaximizefun callback);
    private static readonly glfwSetWindowMaximizeCallbackDelegate _glfwSetWindowMaximizeCallback = LoadFunction<glfwSetWindowMaximizeCallbackDelegate>("glfwSetWindowMaximizeCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(ref readonly GLFWWindow window, GLFWwindowmaximizefun callback) => _glfwSetWindowMaximizeCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWframebuffersizefun glfwSetFramebufferSizeCallbackDelegate(ref readonly GLFWWindow window, GLFWframebuffersizefun callback);
    private static readonly glfwSetFramebufferSizeCallbackDelegate _glfwSetFramebufferSizeCallback = LoadFunction<glfwSetFramebufferSizeCallbackDelegate>("glfwSetFramebufferSizeCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWframebuffersizefun glfwSetFramebufferSizeCallback(ref readonly GLFWWindow window, GLFWframebuffersizefun callback) => _glfwSetFramebufferSizeCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWwindowcontentscalefun glfwSetWindowContentScaleCallbackDelegate(ref readonly GLFWWindow window, GLFWwindowcontentscalefun callback);
    private static readonly glfwSetWindowContentScaleCallbackDelegate _glfwSetWindowContentScaleCallback = LoadFunction<glfwSetWindowContentScaleCallbackDelegate>("glfwSetWindowContentScaleCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(ref readonly GLFWWindow window, GLFWwindowcontentscalefun callback) => _glfwSetWindowContentScaleCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwPollEventsDelegate();
    private static readonly glfwPollEventsDelegate _glfwPollEvents = LoadFunction<glfwPollEventsDelegate>("glfwPollEvents"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwPollEvents() => _glfwPollEvents();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwWaitEventsDelegate();
    private static readonly glfwWaitEventsDelegate _glfwWaitEvents = LoadFunction<glfwWaitEventsDelegate>("glfwWaitEvents"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwWaitEvents() => _glfwWaitEvents();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwWaitEventsTimeoutDelegate(double timeout);
    private static readonly glfwWaitEventsTimeoutDelegate _glfwWaitEventsTimeout = LoadFunction<glfwWaitEventsTimeoutDelegate>("glfwWaitEventsTimeout"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwWaitEventsTimeout(double timeout) => _glfwWaitEventsTimeout(timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwPostEmptyEventDelegate();
    private static readonly glfwPostEmptyEventDelegate _glfwPostEmptyEvent = LoadFunction<glfwPostEmptyEventDelegate>("glfwPostEmptyEvent"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwPostEmptyEvent() => _glfwPostEmptyEvent();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetInputModeDelegate(ref readonly GLFWWindow window, int mode);
    private static readonly glfwGetInputModeDelegate _glfwGetInputMode = LoadFunction<glfwGetInputModeDelegate>("glfwGetInputMode"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetInputMode(ref readonly GLFWWindow window, int mode) => _glfwGetInputMode(in window, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetInputModeDelegate(ref readonly GLFWWindow window, int mode, int value);
    private static readonly glfwSetInputModeDelegate _glfwSetInputMode = LoadFunction<glfwSetInputModeDelegate>("glfwSetInputMode"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetInputMode(ref readonly GLFWWindow window, int mode, int value) => _glfwSetInputMode(in window, mode, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwRawMouseMotionSupportedDelegate();
    private static readonly glfwRawMouseMotionSupportedDelegate _glfwRawMouseMotionSupported = LoadFunction<glfwRawMouseMotionSupportedDelegate>("glfwRawMouseMotionSupported"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwRawMouseMotionSupported() => _glfwRawMouseMotionSupported();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetKeyNameDelegate(int key, int scancode);
    private static readonly glfwGetKeyNameDelegate _glfwGetKeyName = LoadFunction<glfwGetKeyNameDelegate>("glfwGetKeyName"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetKeyName(int key, int scancode) => _glfwGetKeyName(key, scancode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetKeyScancodeDelegate(int key);
    private static readonly glfwGetKeyScancodeDelegate _glfwGetKeyScancode = LoadFunction<glfwGetKeyScancodeDelegate>("glfwGetKeyScancode"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetKeyScancode(int key) => _glfwGetKeyScancode(key);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetKeyDelegate(ref readonly GLFWWindow window, int key);
    private static readonly glfwGetKeyDelegate _glfwGetKey = LoadFunction<glfwGetKeyDelegate>("glfwGetKey"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetKey(ref readonly GLFWWindow window, int key) => _glfwGetKey(in window, key);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetMouseButtonDelegate(ref readonly GLFWWindow window, int button);
    private static readonly glfwGetMouseButtonDelegate _glfwGetMouseButton = LoadFunction<glfwGetMouseButtonDelegate>("glfwGetMouseButton"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetMouseButton(ref readonly GLFWWindow window, int button) => _glfwGetMouseButton(in window, button);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwGetCursorPosDelegate(ref readonly GLFWWindow window, out double xpos, out double ypos);
    private static readonly glfwGetCursorPosDelegate _glfwGetCursorPos = LoadFunction<glfwGetCursorPosDelegate>("glfwGetCursorPos"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwGetCursorPos(ref readonly GLFWWindow window, out double xpos, out double ypos) => _glfwGetCursorPos(in window, out xpos, out ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetCursorPosDelegate(ref readonly GLFWWindow window, double xpos, double ypos);
    private static readonly glfwSetCursorPosDelegate _glfwSetCursorPos = LoadFunction<glfwSetCursorPosDelegate>("glfwSetCursorPos"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetCursorPos(ref readonly GLFWWindow window, double xpos, double ypos) => _glfwSetCursorPos(in window, xpos, ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwCreateCursorDelegate(IntPtr image, int xhot, int yhot);
    private static readonly glfwCreateCursorDelegate _glfwCreateCursor = LoadFunction<glfwCreateCursorDelegate>("glfwCreateCursor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwCreateCursor(IntPtr image, int xhot, int yhot) => _glfwCreateCursor(image, xhot, yhot);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwCreateStandardCursorDelegate(int shape);
    private static readonly glfwCreateStandardCursorDelegate _glfwCreateStandardCursor = LoadFunction<glfwCreateStandardCursorDelegate>("glfwCreateStandardCursor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwCreateStandardCursor(int shape) => _glfwCreateStandardCursor(shape);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwDestroyCursorDelegate(IntPtr cursor);
    private static readonly glfwDestroyCursorDelegate _glfwDestroyCursor = LoadFunction<glfwDestroyCursorDelegate>("glfwDestroyCursor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwDestroyCursor(IntPtr cursor) => _glfwDestroyCursor(cursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetCursorDelegate(ref readonly GLFWWindow window, IntPtr cursor);
    private static readonly glfwSetCursorDelegate _glfwSetCursor = LoadFunction<glfwSetCursorDelegate>("glfwSetCursor"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetCursor(ref readonly GLFWWindow window, IntPtr cursor) => _glfwSetCursor(in window, cursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWkeyfun glfwSetKeyCallbackDelegate(ref readonly GLFWWindow window, GLFWkeyfun callback);
    private static readonly glfwSetKeyCallbackDelegate _glfwSetKeyCallback = LoadFunction<glfwSetKeyCallbackDelegate>("glfwSetKeyCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWkeyfun glfwSetKeyCallback(ref readonly GLFWWindow window, GLFWkeyfun callback) => _glfwSetKeyCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWcharfun glfwSetCharCallbackDelegate(ref readonly GLFWWindow window, GLFWcharfun callback);
    private static readonly glfwSetCharCallbackDelegate _glfwSetCharCallback = LoadFunction<glfwSetCharCallbackDelegate>("glfwSetCharCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWcharfun glfwSetCharCallback(ref readonly GLFWWindow window, GLFWcharfun callback) => _glfwSetCharCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWcharmodsfun glfwSetCharModsCallbackDelegate(ref readonly GLFWWindow window, GLFWcharmodsfun callback);
    private static readonly glfwSetCharModsCallbackDelegate _glfwSetCharModsCallback = LoadFunction<glfwSetCharModsCallbackDelegate>("glfwSetCharModsCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWcharmodsfun glfwSetCharModsCallback(ref readonly GLFWWindow window, GLFWcharmodsfun callback) => _glfwSetCharModsCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWmousebuttonfun glfwSetMouseButtonCallbackDelegate(ref readonly GLFWWindow window, GLFWmousebuttonfun callback);
    private static readonly glfwSetMouseButtonCallbackDelegate _glfwSetMouseButtonCallback = LoadFunction<glfwSetMouseButtonCallbackDelegate>("glfwSetMouseButtonCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWmousebuttonfun glfwSetMouseButtonCallback(ref readonly GLFWWindow window, GLFWmousebuttonfun callback) => _glfwSetMouseButtonCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWcursorposfun glfwSetCursorPosCallbackDelegate(ref readonly GLFWWindow window, GLFWcursorposfun callback);
    private static readonly glfwSetCursorPosCallbackDelegate _glfwSetCursorPosCallback = LoadFunction<glfwSetCursorPosCallbackDelegate>("glfwSetCursorPosCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWcursorposfun glfwSetCursorPosCallback(ref readonly GLFWWindow window, GLFWcursorposfun callback) => _glfwSetCursorPosCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWcursorenterfun glfwSetCursorEnterCallbackDelegate(ref readonly GLFWWindow window, GLFWcursorenterfun callback);
    private static readonly glfwSetCursorEnterCallbackDelegate _glfwSetCursorEnterCallback = LoadFunction<glfwSetCursorEnterCallbackDelegate>("glfwSetCursorEnterCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWcursorenterfun glfwSetCursorEnterCallback(ref readonly GLFWWindow window, GLFWcursorenterfun callback) => _glfwSetCursorEnterCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWscrollfun glfwSetScrollCallbackDelegate(ref readonly GLFWWindow window, GLFWscrollfun callback);
    private static readonly glfwSetScrollCallbackDelegate _glfwSetScrollCallback = LoadFunction<glfwSetScrollCallbackDelegate>("glfwSetScrollCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWscrollfun glfwSetScrollCallback(ref readonly GLFWWindow window, GLFWscrollfun callback) => _glfwSetScrollCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWdropfun glfwSetDropCallbackDelegate(ref readonly GLFWWindow window, GLFWdropfun callback);
    private static readonly glfwSetDropCallbackDelegate _glfwSetDropCallback = LoadFunction<glfwSetDropCallbackDelegate>("glfwSetDropCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWdropfun glfwSetDropCallback(ref readonly GLFWWindow window, GLFWdropfun callback) => _glfwSetDropCallback(in window, callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwJoystickPresentDelegate(int jid);
    private static readonly glfwJoystickPresentDelegate _glfwJoystickPresent = LoadFunction<glfwJoystickPresentDelegate>("glfwJoystickPresent"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwJoystickPresent(int jid) => _glfwJoystickPresent(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate float** glfwGetJoystickAxesDelegate(int jid, out int count);
    private static readonly glfwGetJoystickAxesDelegate _glfwGetJoystickAxes = LoadFunction<glfwGetJoystickAxesDelegate>("glfwGetJoystickAxes"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float** glfwGetJoystickAxes(int jid, out int count) => _glfwGetJoystickAxes(jid, out count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetJoystickButtonsDelegate(int jid, out int count);
    private static readonly glfwGetJoystickButtonsDelegate _glfwGetJoystickButtons = LoadFunction<glfwGetJoystickButtonsDelegate>("glfwGetJoystickButtons"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetJoystickButtons(int jid, out int count) => _glfwGetJoystickButtons(jid, out count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetJoystickHatsDelegate(int jid, out int count);
    private static readonly glfwGetJoystickHatsDelegate _glfwGetJoystickHats = LoadFunction<glfwGetJoystickHatsDelegate>("glfwGetJoystickHats"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetJoystickHats(int jid, out int count) => _glfwGetJoystickHats(jid, out count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetJoystickNameDelegate(int jid);
    private static readonly glfwGetJoystickNameDelegate _glfwGetJoystickName = LoadFunction<glfwGetJoystickNameDelegate>("glfwGetJoystickName"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetJoystickName(int jid) => _glfwGetJoystickName(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetJoystickGUIDDelegate(int jid);
    private static readonly glfwGetJoystickGUIDDelegate _glfwGetJoystickGUID = LoadFunction<glfwGetJoystickGUIDDelegate>("glfwGetJoystickGUID"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetJoystickGUID(int jid) => _glfwGetJoystickGUID(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetJoystickUserPointerDelegate(int jid, IntPtr pointer);
    private static readonly glfwSetJoystickUserPointerDelegate _glfwSetJoystickUserPointer = LoadFunction<glfwSetJoystickUserPointerDelegate>("glfwSetJoystickUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetJoystickUserPointer(int jid, IntPtr pointer) => _glfwSetJoystickUserPointer(jid, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetJoystickUserPointerDelegate(int jid);
    private static readonly glfwGetJoystickUserPointerDelegate _glfwGetJoystickUserPointer = LoadFunction<glfwGetJoystickUserPointerDelegate>("glfwGetJoystickUserPointer"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetJoystickUserPointer(int jid) => _glfwGetJoystickUserPointer(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwJoystickIsGamepadDelegate(int jid);
    private static readonly glfwJoystickIsGamepadDelegate _glfwJoystickIsGamepad = LoadFunction<glfwJoystickIsGamepadDelegate>("glfwJoystickIsGamepad"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwJoystickIsGamepad(int jid) => _glfwJoystickIsGamepad(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLFWjoystickfun glfwSetJoystickCallbackDelegate(GLFWjoystickfun callback);
    private static readonly glfwSetJoystickCallbackDelegate _glfwSetJoystickCallback = LoadFunction<glfwSetJoystickCallbackDelegate>("glfwSetJoystickCallback"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GLFWjoystickfun glfwSetJoystickCallback(GLFWjoystickfun callback) => _glfwSetJoystickCallback(callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwUpdateGamepadMappingsDelegate(string @string);
    private static readonly glfwUpdateGamepadMappingsDelegate _glfwUpdateGamepadMappings = LoadFunction<glfwUpdateGamepadMappingsDelegate>("glfwUpdateGamepadMappings"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwUpdateGamepadMappings(string @string) => _glfwUpdateGamepadMappings(@string);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetGamepadNameDelegate(int jid);
    private static readonly glfwGetGamepadNameDelegate _glfwGetGamepadName = LoadFunction<glfwGetGamepadNameDelegate>("glfwGetGamepadName"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetGamepadName(int jid) => _glfwGetGamepadName(jid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwGetGamepadStateDelegate(int jid, IntPtr state);
    private static readonly glfwGetGamepadStateDelegate _glfwGetGamepadState = LoadFunction<glfwGetGamepadStateDelegate>("glfwGetGamepadState"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwGetGamepadState(int jid, IntPtr state) => _glfwGetGamepadState(jid, state);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetClipboardStringDelegate(ref readonly GLFWWindow window, string @string);
    private static readonly glfwSetClipboardStringDelegate _glfwSetClipboardString = LoadFunction<glfwSetClipboardStringDelegate>("glfwSetClipboardString"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetClipboardString(ref readonly GLFWWindow window, string @string) => _glfwSetClipboardString(in window, @string);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate string glfwGetClipboardStringDelegate(ref readonly GLFWWindow window);
    private static readonly glfwGetClipboardStringDelegate _glfwGetClipboardString = LoadFunction<glfwGetClipboardStringDelegate>("glfwGetClipboardString"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string glfwGetClipboardString(ref readonly GLFWWindow window) => _glfwGetClipboardString(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate double glfwGetTimeDelegate();
    private static readonly glfwGetTimeDelegate _glfwGetTime = LoadFunction<glfwGetTimeDelegate>("glfwGetTime"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double glfwGetTime() => _glfwGetTime();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSetTimeDelegate(double time);
    private static readonly glfwSetTimeDelegate _glfwSetTime = LoadFunction<glfwSetTimeDelegate>("glfwSetTime"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSetTime(double time) => _glfwSetTime(time);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate ulong glfwGetTimerValueDelegate();
    private static readonly glfwGetTimerValueDelegate _glfwGetTimerValue = LoadFunction<glfwGetTimerValueDelegate>("glfwGetTimerValue"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong glfwGetTimerValue() => _glfwGetTimerValue();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate ulong glfwGetTimerFrequencyDelegate();
    private static readonly glfwGetTimerFrequencyDelegate _glfwGetTimerFrequency = LoadFunction<glfwGetTimerFrequencyDelegate>("glfwGetTimerFrequency"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong glfwGetTimerFrequency() => _glfwGetTimerFrequency();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwMakeContextCurrentDelegate(ref readonly GLFWWindow window);
    private static readonly glfwMakeContextCurrentDelegate _glfwMakeContextCurrent = LoadFunction<glfwMakeContextCurrentDelegate>("glfwMakeContextCurrent"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwMakeContextCurrent(ref readonly GLFWWindow window) => _glfwMakeContextCurrent(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetCurrentContextDelegate();
    private static readonly glfwGetCurrentContextDelegate _glfwGetCurrentContext = LoadFunction<glfwGetCurrentContextDelegate>("glfwGetCurrentContext"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetCurrentContext() => _glfwGetCurrentContext();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSwapBuffersDelegate(ref readonly GLFWWindow window);
    private static readonly glfwSwapBuffersDelegate _glfwSwapBuffers = LoadFunction<glfwSwapBuffersDelegate>("glfwSwapBuffers"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSwapBuffers(ref readonly GLFWWindow window) => _glfwSwapBuffers(in window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glfwSwapIntervalDelegate(int interval);
    private static readonly glfwSwapIntervalDelegate _glfwSwapInterval = LoadFunction<glfwSwapIntervalDelegate>("glfwSwapInterval"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void glfwSwapInterval(int interval) => _glfwSwapInterval(interval);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwExtensionSupportedDelegate(string extension);
    private static readonly glfwExtensionSupportedDelegate _glfwExtensionSupported = LoadFunction<glfwExtensionSupportedDelegate>("glfwExtensionSupported"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwExtensionSupported(string extension) => _glfwExtensionSupported(extension);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetProcAddressDelegate(string procname);
    private static readonly glfwGetProcAddressDelegate _glfwGetProcAddress = LoadFunction<glfwGetProcAddressDelegate>("glfwGetProcAddress"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetProcAddress(string procname) => _glfwGetProcAddress(procname);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int glfwVulkanSupportedDelegate();
    private static readonly glfwVulkanSupportedDelegate _glfwVulkanSupported = LoadFunction<glfwVulkanSupportedDelegate>("glfwVulkanSupported"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int glfwVulkanSupported() => _glfwVulkanSupported();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate ref Utf8ZPtr glfwGetRequiredInstanceExtensionsDelegate(out uint count);
    private static readonly glfwGetRequiredInstanceExtensionsDelegate _glfwGetRequiredInstanceExtensions = LoadFunction<glfwGetRequiredInstanceExtensionsDelegate>("glfwGetRequiredInstanceExtensions"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Utf8ZPtr glfwGetRequiredInstanceExtensions(out uint count) => ref _glfwGetRequiredInstanceExtensions(out count);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr glfwGetProcAddressPtrDelegate(ref readonly byte procname);
    private static readonly glfwGetProcAddressPtrDelegate _glfwGetProcAddressPtr = LoadFunction<glfwGetProcAddressPtrDelegate>("glfwGetProcAddress"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr glfwGetProcAddress(ReadOnlySpan<byte> procname) => _glfwGetProcAddressPtr(ref MemoryMarshal.GetReference(procname));

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate ref readonly GLFWWindow glfwCreateWindowPtrDelegate(int width, int height, ref readonly byte title, IntPtr monitor, IntPtr share);
    private static readonly glfwCreateWindowPtrDelegate _glfwCreateWindowPtr = LoadFunction<glfwCreateWindowPtrDelegate>("glfwCreateWindow"u8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly GLFWWindow glfwCreateWindow(int width, int height, ReadOnlySpan<byte> title, IntPtr monitor, IntPtr share) => ref _glfwCreateWindowPtr(width, height, ref MemoryMarshal.GetReference(title), monitor, share);
}