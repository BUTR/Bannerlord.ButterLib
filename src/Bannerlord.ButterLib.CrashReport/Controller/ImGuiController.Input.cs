using ImGuiNET;

using Silk.NET.Input;
using Silk.NET.Maths;

using System.Numerics;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

partial class ImGuiController
{
    private void WindowResized(Vector2D<int> size)
    {
        _windowsWidth = (uint) size.X;
        _windowsHeight = (uint) size.Y;
    }

    private void KeyboardOnKeyDown(IKeyboard keyboard, Key key, int arg3)
    {
        /*
        _io.KeyCtrl = action == GLFW_PRESS && (mods & GLFW_MOD_CONTROL) != 0;
        _io.KeyAlt = action == GLFW_PRESS && (mods & GLFW_MOD_ALT) != 0;
        _io.KeyShift = action == GLFW_PRESS && (mods & GLFW_MOD_SHIFT) != 0;
        _io.KeySuper = action == GLFW_PRESS && (mods & GLFW_MOD_SUPER) != 0;
        */

        var convertedKey = KeyToImGuiKey(key);
        if (convertedKey != ImGuiKey.None)
            _io.AddKeyEvent(convertedKey, true);
    }

    private void KeyboardOnKeyUp(IKeyboard keyboard, Key key, int arg3)
    {
        /*
        _io.KeyCtrl = action == GLFW_PRESS && (mods & GLFW_MOD_CONTROL) != 0;
        _io.KeyAlt = action == GLFW_PRESS && (mods & GLFW_MOD_ALT) != 0;
        _io.KeyShift = action == GLFW_PRESS && (mods & GLFW_MOD_SHIFT) != 0;
        _io.KeySuper = action == GLFW_PRESS && (mods & GLFW_MOD_SUPER) != 0;
        */

        var convertedKey = KeyToImGuiKey(key);
        if (convertedKey != ImGuiKey.None)
            _io.AddKeyEvent(convertedKey, false);
    }

    private void OnKeyChar(IKeyboard keyboard, char c)
    {
        _io.AddInputCharacter(c);
    }

    private void MouseOnScroll(IMouse mouse, ScrollWheel data)
    {
        _io.AddMouseWheelEvent(data.X, data.Y);
    }

    private void MouseOnMouseDown(IMouse mouse, MouseButton button)
    {
        if (button is < 0 or > (MouseButton) ImGuiMouseButton.COUNT)
            return;

        _io.AddMouseButtonEvent((int) button, true);
    }

    private void MouseOnMouseUp(IMouse mouse, MouseButton button)
    {
        if (button is < 0 or > (MouseButton) ImGuiMouseButton.COUNT)
            return;

        _io.AddMouseButtonEvent((int) button, false);
    }

    private void MouseOnMouseMove(IMouse mouse, Vector2 pos)
    {
        var mousePosBackup = _io.MousePos;

        /*
        var focused = _glfw.GetWindowAttrib(_windowPtr, WindowAttributeGetter.Focused);
        if (!focused)
        {
            _io.MousePos.X = -float.MaxValue;
            _io.MousePos.Y = -float.MaxValue;
            return;
        }
        */

        if (_io.WantSetMousePos)
        {
            mouse.Position = mousePosBackup;
        }
        else
        {
            _io.MousePos.X = pos.X;
            _io.MousePos.Y = pos.Y;
        }
    }


    public void Update(double delta)
    {
        var oldCtx = _imgui.GetCurrentContext();
        if (oldCtx != _context)
        {
            _imgui.SetCurrentContext(_context);
        }

        SetPerFrameImGuiData(delta);

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
    private void SetPerFrameImGuiData(double delta)
    {
        _io.DisplaySize.X = _windowsWidth;
        _io.DisplaySize.Y = _windowsHeight;

        if (_windowsWidth > 0 && _windowsHeight > 0)
        {
            _io.DisplayFramebufferScale.X = (float) _view.FramebufferSize.X / _windowsWidth;
            _io.DisplayFramebufferScale.Y = (float) _view.FramebufferSize.Y / _windowsHeight;
        }

        _io.DeltaTime = (float) delta;
    }

    private void UpdateMouseCursor()
    {
        var flag = (_io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0;
        if (flag || Mouse.Cursor.CursorMode == CursorMode.Disabled) return;

        var imguiCursor = _imgui.GetMouseCursor();
        if (imguiCursor == ImGuiMouseCursor.None || _io.MouseDrawCursor)
        {
            Mouse.Cursor.CursorMode = CursorMode.Hidden;
        }
        else
        {
            Mouse.Cursor.StandardCursor = _mouseCursors[(int) imguiCursor] != StandardCursor.Default ? _mouseCursors[(int) imguiCursor] : _mouseCursors[(int) ImGuiMouseCursor.Arrow];
            Mouse.Cursor.CursorMode = CursorMode.Normal;
        }
    }
}