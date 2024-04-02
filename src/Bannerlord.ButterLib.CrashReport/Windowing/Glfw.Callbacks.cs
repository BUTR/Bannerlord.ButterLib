using System;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

partial class Glfw
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowPosCallback(IntPtr window, int posx, int posy);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowSizeCallback(IntPtr window, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowCloseCallback(IntPtr window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowRefreshCallback(IntPtr window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowFocusCallback(IntPtr window, int focused);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowIconifyCallback(IntPtr window, int iconified);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowMaximizeCallback(IntPtr window, int maximized);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FramebufferSizeCallback(IntPtr window, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowContentScaleCallback(IntPtr window, float xscale, float yscale);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MouseButtonCallback(IntPtr window, int button, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CursorPosCallback(IntPtr window, double mousex, double mousey);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CursorEnterCallback(IntPtr window, int entered);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ScrollCallback(IntPtr window, double xoffset, double yoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void KeyCallback(IntPtr window, int key, int scancode, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CharCallback(IntPtr window, uint codepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CharModsCallback(IntPtr window, int codepoint, int mods);
}