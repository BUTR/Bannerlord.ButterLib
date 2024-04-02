using Bannerlord.ButterLib.CrashReportWindow.Windowing;

using ImGuiNET;

using System;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

using static Glfw;

partial class ImGuiController
{
    /// <summary>
    ///     Pointer to the latest clipboard contents buffer returned to ImGUI.
    /// </summary>
    private static IntPtr ClipboardTextMemory = IntPtr.Zero;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void d_set_clipboard_text_fn(IntPtr userData, [MarshalAs(UnmanagedType.LPUTF8Str)] string text);
    private static readonly d_set_clipboard_text_fn SetClipboardTextFnDelegate = SetClipboardTextFn;
    private static void SetClipboardTextFn(IntPtr userData, string text)
    {
        //_glfw.SetClipboardString(in new GlfwWindowPtr(userData).Handle, text);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr d_get_clipboard_text_fn(IntPtr userData);
    private static readonly d_get_clipboard_text_fn GetClipboardTextFnDelegate = GetClipboardTextFn;
    private static IntPtr GetClipboardTextFn(IntPtr userData)
    {
        return IntPtr.Zero;
        /*
        Marshal.FreeCoTaskMem(ClipboardTextMemory);
        ClipboardTextMemory = StringToCoTaskMemUTF8(_glfw.GetClipboardString(in new GlfwWindowPtr(userData).Handle));
        return ClipboardTextMemory;
        */
    }

    public void Update(float delta)
    {
        var oldCtx = _imgui.GetCurrentContext();
        if (oldCtx != _context)
        {
            _imgui.SetCurrentContext(_context);
        }

        SetPerFrameImGuiData(delta);

        UpdateMousePos();
        UpdateMouseCursor();

        _imgui.NewFrame();

        if (oldCtx != _context)
        {
            _imgui.SetCurrentContext(oldCtx);
        }
    }

    /// <summary>
    /// Sets per-frame data based on the associated window.
    /// This is called by Update(float).
    /// </summary>
    private void SetPerFrameImGuiData(float delta)
    {
        _io.DisplaySize.X = _windowsWidth;
        _io.DisplaySize.Y = _windowsHeight;

        if (_windowsWidth > 0 && _windowsHeight > 0)
        {
            _io.DisplayFramebufferScale.X = (float) _frameBufferWidth / _windowsWidth;
            _io.DisplayFramebufferScale.Y = (float) _frameBufferHeight / _windowsHeight;
        }

        _io.DeltaTime = delta;
    }

    private void WindowsSizeCallback(IntPtr window, int width, int height)
    {
        _windowsWidth = (uint) width;
        _windowsHeight = (uint) height;
    }

    private void FramebufferSizeCallback(IntPtr window, int width, int height)
    {
        _frameBufferWidth = (uint) width;
        _frameBufferHeight = (uint) height;
    }

    private void MouseButtonCallback(IntPtr window, int button, int action, int mods)
    {
        if (action != GLFW_PRESS && action != GLFW_RELEASE)
            return;

        if (button is < 0 or > (int) ImGuiMouseButton.COUNT)
            return;

        _io.AddMouseButtonEvent(button, action == GLFW_PRESS);
    }

    private void ScrollCallback(IntPtr window, double xo, double yo)
    {
        _io.AddMouseWheelEvent((float) xo, (float) yo);
    }

    private void KeyCallback(IntPtr window, int key, int scancode, int action, int mods)
    {
        if (action != GLFW_PRESS && action != GLFW_RELEASE)
            return;

        /*
        _io.KeyCtrl = action == GLFW_PRESS && (mods & GLFW_MOD_CONTROL) != 0;
        _io.KeyAlt = action == GLFW_PRESS && (mods & GLFW_MOD_ALT) != 0;
        _io.KeyShift = action == GLFW_PRESS && (mods & GLFW_MOD_SHIFT) != 0;
        _io.KeySuper = action == GLFW_PRESS && (mods & GLFW_MOD_SUPER) != 0;
        */

        var imguiKey = GlfwKeyToImGuiKey(key);
        if (imguiKey != ImGuiKey.None)
            _io.AddKeyEvent(imguiKey, action == GLFW_PRESS);
    }

    private void CharCallback(IntPtr window, uint c)
    {
        _io.AddInputCharacter(c);
    }

    private void UpdateMousePos()
    {
        var mousePosBackup = _io.MousePos;
        var focused = _glfw.GetWindowAttrib(in _windowPtr.Handle, GLFW_FOCUSED) != 0;
        if (!focused)
        {
            _io.MousePos.X = -float.MaxValue;
            _io.MousePos.Y = -float.MaxValue;
            return;
        }

        if (_io.WantSetMousePos)
            _glfw.SetCursorPos(in _windowPtr.Handle, ref mousePosBackup);
        else
            _glfw.GetCursorPos(in _windowPtr.Handle, ref _io.MousePos);
    }

    private void UpdateMouseCursor()
    {
        var flag = (_io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0;
        if (flag || _glfw.GetInputMode(in _windowPtr.Handle, GLFW_CURSOR) == GLFW_CURSOR_DISABLED) return;

        var imguiCursor = _imgui.GetMouseCursor();
        if (imguiCursor == ImGuiMouseCursor.None || _io.MouseDrawCursor)
        {
            _glfw.SetInputMode(in _windowPtr.Handle, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
        }
        else
        {
            _glfw.SetCursor(in _windowPtr.Handle, _mouseCursors[(int) imguiCursor] != IntPtr.Zero ? _mouseCursors[(int) imguiCursor] : _mouseCursors[(int) ImGuiMouseCursor.Arrow]);
            _glfw.SetInputMode(in _windowPtr.Handle, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
        }
    }
}