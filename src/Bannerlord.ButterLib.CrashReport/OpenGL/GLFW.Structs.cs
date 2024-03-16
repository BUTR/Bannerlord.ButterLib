using System;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

partial class GLFW
{
    public readonly struct GLFWWindowPtr
    {
        private readonly unsafe GLFWWindow* _windowNativePtr;

        public unsafe GLFWWindowPtr(IntPtr window)
        {
            _windowNativePtr = (GLFWWindow*) window;
        }
        public unsafe GLFWWindowPtr(ref readonly GLFWWindow window)
        {
            fixed (GLFWWindow* windowPtr = &window)
                _windowNativePtr = windowPtr;
        }

        public unsafe IntPtr Ptr => new(_windowNativePtr);
        public unsafe ref readonly GLFWWindow Ref => ref *_windowNativePtr;
    }

    public readonly ref struct GLFWWindow
    {
        public readonly nuint Handle;
    }

    public struct GLFWvidmode
    {
        public int Width;
        public int Height;
        public int RedBits;
        public int GreenBits;
        public int BlueBits;
        public int RefreshRate;
    }
}