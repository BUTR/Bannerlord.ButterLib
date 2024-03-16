using System;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

partial class GLFW
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWerrorfun(int errorcode, string description);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowposfun(IntPtr window, int posx, int posy);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowsizefun(IntPtr window, int width, int height);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowclosefun(IntPtr window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowrefreshfun(IntPtr window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowfocusfun(IntPtr window, int focused);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowiconifyfun(IntPtr window, int iconified);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowmaximizefun(IntPtr window, int maximized);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWframebuffersizefun(IntPtr window, int width, int height);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowcontentscalefun(IntPtr window, float xscale, float yscale);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWmousebuttonfun(IntPtr window, int button, int action, int mods);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcursorposfun(IntPtr window, double mousex, double mousey);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcursorenterfun(IntPtr window, int entered);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWscrollfun(IntPtr window, double xoffset, double yoffset);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWkeyfun(IntPtr window, int key, int scancode, int action, int mods);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcharfun(IntPtr window, uint codepoint);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcharmodsfun(IntPtr window, int codepoint, int mods);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWdropfun(IntPtr window, int count, string[] paths);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWmonitorfun(IntPtr window, int monitorevent);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWjoystickfun(int jid, int ev);
}