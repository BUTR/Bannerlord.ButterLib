using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

using static GLFW;
using static GL;

internal class ImGuiController : IDisposable
{
    private static ImGuiKey GlfwKeyToImGuiKey(int key) => key switch
    {
        GLFW_KEY_TAB => ImGuiKey.Tab,
        GLFW_KEY_LEFT => ImGuiKey.LeftArrow,
        GLFW_KEY_RIGHT => ImGuiKey.RightArrow,
        GLFW_KEY_UP => ImGuiKey.UpArrow,
        GLFW_KEY_DOWN => ImGuiKey.DownArrow,
        GLFW_KEY_PAGE_UP => ImGuiKey.PageUp,
        GLFW_KEY_PAGE_DOWN => ImGuiKey.PageDown,
        GLFW_KEY_HOME => ImGuiKey.Home,
        GLFW_KEY_END => ImGuiKey.End,
        GLFW_KEY_INSERT => ImGuiKey.Insert,
        GLFW_KEY_DELETE => ImGuiKey.Delete,
        GLFW_KEY_BACKSPACE => ImGuiKey.Backspace,
        GLFW_KEY_SPACE => ImGuiKey.Space,
        GLFW_KEY_ENTER => ImGuiKey.Enter,
        GLFW_KEY_ESCAPE => ImGuiKey.Escape,
        GLFW_KEY_APOSTROPHE => ImGuiKey.Apostrophe,
        GLFW_KEY_COMMA => ImGuiKey.Comma,
        GLFW_KEY_MINUS => ImGuiKey.Minus,
        GLFW_KEY_PERIOD => ImGuiKey.Period,
        GLFW_KEY_SLASH => ImGuiKey.Slash,
        GLFW_KEY_SEMICOLON => ImGuiKey.Semicolon,
        GLFW_KEY_EQUAL => ImGuiKey.Equal,
        GLFW_KEY_LEFT_BRACKET => ImGuiKey.LeftBracket,
        GLFW_KEY_BACKSLASH => ImGuiKey.Backslash,
        GLFW_KEY_RIGHT_BRACKET => ImGuiKey.RightBracket,
        GLFW_KEY_GRAVE_ACCENT => ImGuiKey.GraveAccent,
        GLFW_KEY_CAPS_LOCK => ImGuiKey.CapsLock,
        GLFW_KEY_SCROLL_LOCK => ImGuiKey.ScrollLock,
        GLFW_KEY_NUM_LOCK => ImGuiKey.NumLock,
        GLFW_KEY_PRINT_SCREEN => ImGuiKey.PrintScreen,
        GLFW_KEY_PAUSE => ImGuiKey.Pause,
        GLFW_KEY_KP_0 => ImGuiKey.Keypad0,
        GLFW_KEY_KP_1 => ImGuiKey.Keypad1,
        GLFW_KEY_KP_2 => ImGuiKey.Keypad2,
        GLFW_KEY_KP_3 => ImGuiKey.Keypad3,
        GLFW_KEY_KP_4 => ImGuiKey.Keypad4,
        GLFW_KEY_KP_5 => ImGuiKey.Keypad5,
        GLFW_KEY_KP_6 => ImGuiKey.Keypad6,
        GLFW_KEY_KP_7 => ImGuiKey.Keypad7,
        GLFW_KEY_KP_8 => ImGuiKey.Keypad8,
        GLFW_KEY_KP_9 => ImGuiKey.Keypad9,
        GLFW_KEY_KP_DECIMAL => ImGuiKey.KeypadDecimal,
        GLFW_KEY_KP_DIVIDE => ImGuiKey.KeypadDivide,
        GLFW_KEY_KP_MULTIPLY => ImGuiKey.KeypadMultiply,
        GLFW_KEY_KP_SUBTRACT => ImGuiKey.KeypadSubtract,
        GLFW_KEY_KP_ADD => ImGuiKey.KeypadAdd,
        GLFW_KEY_KP_ENTER => ImGuiKey.KeypadEnter,
        GLFW_KEY_KP_EQUAL => ImGuiKey.KeypadEqual,
        GLFW_KEY_LEFT_SHIFT => ImGuiKey.LeftShift,
        GLFW_KEY_LEFT_CONTROL => ImGuiKey.LeftCtrl,
        GLFW_KEY_LEFT_ALT => ImGuiKey.LeftAlt,
        GLFW_KEY_LEFT_SUPER => ImGuiKey.LeftSuper,
        GLFW_KEY_RIGHT_SHIFT => ImGuiKey.RightShift,
        GLFW_KEY_RIGHT_CONTROL => ImGuiKey.RightCtrl,
        GLFW_KEY_RIGHT_ALT => ImGuiKey.RightAlt,
        GLFW_KEY_RIGHT_SUPER => ImGuiKey.RightSuper,
        GLFW_KEY_MENU => ImGuiKey.Menu,
        GLFW_KEY_0 => ImGuiKey._0,
        GLFW_KEY_1 => ImGuiKey._1,
        GLFW_KEY_2 => ImGuiKey._2,
        GLFW_KEY_3 => ImGuiKey._3,
        GLFW_KEY_4 => ImGuiKey._4,
        GLFW_KEY_5 => ImGuiKey._5,
        GLFW_KEY_6 => ImGuiKey._6,
        GLFW_KEY_7 => ImGuiKey._7,
        GLFW_KEY_8 => ImGuiKey._8,
        GLFW_KEY_9 => ImGuiKey._9,
        GLFW_KEY_A => ImGuiKey.A,
        GLFW_KEY_B => ImGuiKey.B,
        GLFW_KEY_C => ImGuiKey.C,
        GLFW_KEY_D => ImGuiKey.D,
        GLFW_KEY_E => ImGuiKey.E,
        GLFW_KEY_F => ImGuiKey.F,
        GLFW_KEY_G => ImGuiKey.G,
        GLFW_KEY_H => ImGuiKey.H,
        GLFW_KEY_I => ImGuiKey.I,
        GLFW_KEY_J => ImGuiKey.J,
        GLFW_KEY_K => ImGuiKey.K,
        GLFW_KEY_L => ImGuiKey.L,
        GLFW_KEY_M => ImGuiKey.M,
        GLFW_KEY_N => ImGuiKey.N,
        GLFW_KEY_O => ImGuiKey.O,
        GLFW_KEY_P => ImGuiKey.P,
        GLFW_KEY_Q => ImGuiKey.Q,
        GLFW_KEY_R => ImGuiKey.R,
        GLFW_KEY_S => ImGuiKey.S,
        GLFW_KEY_T => ImGuiKey.T,
        GLFW_KEY_U => ImGuiKey.U,
        GLFW_KEY_V => ImGuiKey.V,
        GLFW_KEY_W => ImGuiKey.W,
        GLFW_KEY_X => ImGuiKey.X,
        GLFW_KEY_Y => ImGuiKey.Y,
        GLFW_KEY_Z => ImGuiKey.Z,
        GLFW_KEY_F1 => ImGuiKey.F1,
        GLFW_KEY_F2 => ImGuiKey.F2,
        GLFW_KEY_F3 => ImGuiKey.F3,
        GLFW_KEY_F4 => ImGuiKey.F4,
        GLFW_KEY_F5 => ImGuiKey.F5,
        GLFW_KEY_F6 => ImGuiKey.F6,
        GLFW_KEY_F7 => ImGuiKey.F7,
        GLFW_KEY_F8 => ImGuiKey.F8,
        GLFW_KEY_F9 => ImGuiKey.F9,
        GLFW_KEY_F10 => ImGuiKey.F10,
        GLFW_KEY_F11 => ImGuiKey.F11,
        GLFW_KEY_F12 => ImGuiKey.F12,
        _ => ImGuiKey.None
    };
    
    private const string VertexShader = """
                                        #version 330 core
                                                    layout (location = 0) in vec2 Position;
                                                    layout (location = 1) in vec2 UV;
                                                    layout (location = 2) in vec4 Color;
                                                    uniform mat4 ProjMtx;
                                                    out vec2 Frag_UV;
                                                    out vec4 Frag_Color;
                                                    void main()
                                                    {
                                                        Frag_UV = UV;
                                                        Frag_Color = Color;
                                                        gl_Position = ProjMtx * vec4(Position.xy,0,1);
                                                    }
                                        """;

    private const string FragmentShader = """
                                          #version 330 core
                                                      in vec2 Frag_UV;
                                                      in vec4 Frag_Color;
                                                      uniform sampler2D Texture;
                                                      layout (location = 0) out vec4 Out_Color;
                                                      void main()
                                                      {
                                                          Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
                                                      }
                                          """;
    
    private readonly IntPtr _window;
    private ImGuiIOPtr _io;

    private double _time;
    private readonly bool[] _mouseJustPressed = new bool[(int) ImGuiMouseButton.COUNT];
    private readonly IntPtr[] _mouseCursors = new IntPtr[(int) ImGuiMouseCursor.COUNT];

    private uint _fontTexture;
    private uint _shaderHandle, _vertHandle, _fragHandle;
    private int _attribLocationTex, _attribLocationProjMtx;
    private uint _attribLocationVtxPos, _attribLocationVtxUv, _attribLocationVtxColor;
    private uint _vboHandle, _elementsHandle;

    private GLFWmousebuttonfun _userCallbackMousebutton = default!;
    private GLFWscrollfun _userCallbackScroll = default!;
    private GLFWkeyfun _userCallbackKey = default!;
    private GLFWcharfun _userCallbackChar = default!;

    private delegate void SetClipboardTextFn(IntPtr userData, string text);
    private delegate void GetClipboardTextFn(IntPtr userData);

    private readonly SetClipboardTextFn _setClipboardTextFn;
    private readonly GetClipboardTextFn _getClipboardTextFn;

    public ImGuiController(IntPtr window)
    {
        _window = window;

        ImGui.CreateContext();
        ImGui.StyleColorsDark();

        _io = ImGui.GetIO();

        _setClipboardTextFn = (data, text) => glfwSetClipboardString(data, text);
        _getClipboardTextFn = data => glfwGetClipboardString(data);
    }

    public void Init()
    {
        _io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        _io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
        _io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

        InitGlfw();
        CreateDeviceObjects();
    }

    private void InitGlfw()
    {
        _io.SetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(_setClipboardTextFn);
        _io.GetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(_getClipboardTextFn);
        _io.ClipboardUserData = _window;

        _mouseCursors[(int) ImGuiMouseCursor.Arrow] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.TextInput] = glfwCreateStandardCursor(GLFW_IBEAM_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.ResizeNS] = glfwCreateStandardCursor(GLFW_VRESIZE_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.ResizeEW] = glfwCreateStandardCursor(GLFW_HRESIZE_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.Hand] = glfwCreateStandardCursor(GLFW_HAND_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.ResizeAll] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.ResizeNESW] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.ResizeNWSE] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
        _mouseCursors[(int) ImGuiMouseCursor.NotAllowed] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);

        _userCallbackMousebutton = MouseButtonCallback;
        _userCallbackScroll = ScrollCallback;
        _userCallbackKey = KeyCallback;
        _userCallbackChar = CharCallback;

        glfwSetMouseButtonCallback(_window, _userCallbackMousebutton);
        glfwSetScrollCallback(_window, _userCallbackScroll);
        glfwSetKeyCallback(_window, _userCallbackKey);
        glfwSetCharCallback(_window, _userCallbackChar);
    }

    private void MouseButtonCallback(IntPtr window, int button, int action, int mods)
    {
        if (action == GLFW_PRESS && button >= 0 && button < _mouseJustPressed.Length)
            _mouseJustPressed[button] = true;
    }

    private void ScrollCallback(IntPtr window, double xo, double yo)
    {
        _io.MouseWheelH += (float) xo;
        _io.MouseWheel += (float) yo;
    }

    private void KeyCallback(IntPtr window, int key, int scancode, int action, int mods)
    {
        if (action != GLFW_PRESS && action != GLFW_RELEASE)
            return;

        var imguiKey = GlfwKeyToImGuiKey(key);
        if (imguiKey != ImGuiKey.None)
            //_io.AddKeyEvent(imguiKey, action == GLFW_PRESS, key, scancode);
            _io.AddKeyEvent(imguiKey, action == GLFW_PRESS);
    }

    private void CharCallback(IntPtr window, uint c)
    {
        _io.AddInputCharacter(c);
    }

    private void CreateDeviceObjects()
    {
        glGetIntegerv(GL_TEXTURE_BINDING_2D, out var lastTexture);
        glGetIntegerv(GL_ARRAY_BUFFER_BINDING, out var lastArrayBuffer);
        glGetIntegerv(GL_VERTEX_ARRAY_BINDING, out var lastVertexArray);

        _vertHandle = glCreateShader(GL_VERTEX_SHADER);
        glShaderSource(_vertHandle, 1, new[] {VertexShader}, VertexShader.Length);
        glCompileShader(_vertHandle);
        CheckShader(_vertHandle);

        _fragHandle = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(_fragHandle, 1, new[] {FragmentShader}, FragmentShader.Length);
        glCompileShader(_fragHandle);
        CheckShader(_fragHandle);

        _shaderHandle = glCreateProgram();
        glAttachShader(_shaderHandle, _vertHandle);
        glAttachShader(_shaderHandle, _fragHandle);
        glLinkProgram(_shaderHandle);
        CheckProgram(_shaderHandle);

        _attribLocationTex = glGetUniformLocation(_shaderHandle, "Texture");
        _attribLocationProjMtx = glGetUniformLocation(_shaderHandle, "ProjMtx");
        _attribLocationVtxPos = (uint) glGetAttribLocation(_shaderHandle, "Position");
        _attribLocationVtxUv = (uint) glGetAttribLocation(_shaderHandle, "UV");
        _attribLocationVtxColor = (uint) glGetAttribLocation(_shaderHandle, "Color");

        glGenBuffers(1, out _vboHandle);
        glGenBuffers(1, out _elementsHandle);

        CreateFontsTexture();

        glBindTexture(GL_TEXTURE_2D, (uint) lastTexture);
        glBindBuffer(GL_ARRAY_BUFFER, (uint) lastArrayBuffer);
        glBindVertexArray((uint) lastVertexArray);
    }

    private void CheckShader(uint handle)
    {
        glGetShaderiv(handle, GL_COMPILE_STATUS, out var status);
        if (status != GL_FALSE) return;

        glGetShaderiv(handle, GL_INFO_LOG_LENGTH, out var logLength);
        glGetShaderInfoLog(handle, logLength, out _, out var info);
        throw new Exception(info);
    }

    private void CheckProgram(uint handle)
    {
        glGetProgramiv(handle, GL_LINK_STATUS, out var status);
        if (status != GL_FALSE) return;

        glGetProgramiv(handle, GL_INFO_LOG_LENGTH, out var logLength);
        glGetProgramInfoLog(handle, logLength, out _, out var info);
        throw new Exception(info);
    }

    private void CreateFontsTexture()
    {
        _io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out var width, out var height);

        glGetIntegerv(GL_TEXTURE_BINDING_2D, out var lastTexture);
        glGenTextures(1, out _fontTexture);
        glBindTexture(GL_TEXTURE_2D, _fontTexture);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

        glPixelStorei(GL_UNPACK_ROW_LENGTH, 0);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels);
        _io.Fonts.SetTexID((IntPtr) _fontTexture);
        glBindTexture(GL_TEXTURE_2D, (uint) lastTexture);
    }

    public void Update()
    {
        if (!_io.Fonts.IsBuilt())
            throw new Exception("Font atlas not built !");

        glfwGetWindowSize(_window, out var w, out var h);
        glfwGetFramebufferSize(_window, out var displayW, out var displayH);
        _io.DisplaySize = new Vector2(w, h);
        if (w > 0 && h > 0)
            _io.DisplayFramebufferScale = new Vector2((float) displayW / w, (float) displayH / h);

        var currentTime = glfwGetTime();
        _io.DeltaTime = _time > 0.0 ? (float) (currentTime - _time) : 1.0f / 60.0f;
        _time = currentTime;

        UpdateMousePosAndButtons();
        UpdateMouseCursor();

        ImGui.NewFrame();
    }

    private void UpdateMousePosAndButtons()
    {
        for (var i = 0; i < _io.MouseDown.Count; i++)
        {
            _io.MouseDown[i] = _mouseJustPressed[i] || glfwGetMouseButton(_window, i) != 0;
            _mouseJustPressed[i] = false;
        }

        var mousePosBackup = _io.MousePos;
        _io.MousePos = new Vector2(-float.MaxValue, -float.MaxValue);
        var focused = glfwGetWindowAttrib(_window, GLFW_FOCUSED) != 0;
        if (!focused) return;

        if (_io.WantSetMousePos)
        {
            glfwSetCursorPos(_window, mousePosBackup.X, mousePosBackup.Y);
        }
        else
        {
            glfwGetCursorPos(_window, out var mouseX, out var mouseY);
            _io.MousePos = new Vector2((float) mouseX, (float) mouseY);
        }
    }

    private void UpdateMouseCursor()
    {
        var flag = (_io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0;
        if (flag || glfwGetInputMode(_window, GLFW_CURSOR) == GLFW_CURSOR_DISABLED) return;

        var imguiCursor = ImGui.GetMouseCursor();
        if (imguiCursor == ImGuiMouseCursor.None || _io.MouseDrawCursor)
        {
            glfwSetInputMode(_window, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
        }
        else
        {
            glfwSetCursor(_window, _mouseCursors[(int) imguiCursor] != IntPtr.Zero ? _mouseCursors[(int) imguiCursor] : _mouseCursors[(int) ImGuiMouseCursor.Arrow]);
            glfwSetInputMode(_window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
        }
    }

    public void Render()
    {
        ImGui.Render();
        RenderDrawData(ImGui.GetDrawData());
    }

    private void RenderDrawData(ImDrawDataPtr drawData)
    {
        if (drawData.CmdListsCount == 0) return;

        var fbWidth = (int) (drawData.DisplaySize.X * drawData.FramebufferScale.X);
        var fbHeight = (int) (drawData.DisplaySize.Y * drawData.FramebufferScale.Y);
        if (fbWidth <= 0 || fbHeight <= 0) return;

        glGetIntegerv(GL_ACTIVE_TEXTURE, out var lastActiveTexture);
        glActiveTexture(GL_TEXTURE0);
        glGetIntegerv(GL_CURRENT_PROGRAM, out var lastProgram);
        glGetIntegerv(GL_TEXTURE_BINDING_2D, out var lastTexture);
        glGetIntegerv(GL_SAMPLER_BINDING, out var lastSampler);
        glGetIntegerv(GL_VERTEX_ARRAY_BINDING, out var lastVertexArrayObject);
        glGetIntegerv(GL_ARRAY_BUFFER_BINDING, out var lastArrayBuffer);
        glGetIntegerv(GL_POLYGON_MODE, out var lastPolygonMode);
        glGetIntegerv(GL_VIEWPORT, out var lastViewport);
        glGetIntegerv(GL_SCISSOR_BOX, out var lastScissorBox);
        glGetIntegerv(GL_BLEND_SRC_RGB, out var lastBlendSrcRgb);
        glGetIntegerv(GL_BLEND_DST_RGB, out var lastBlendDstRgb);
        glGetIntegerv(GL_BLEND_SRC_ALPHA, out var lastBlendSrcAlpha);
        glGetIntegerv(GL_BLEND_DST_ALPHA, out var lastBlendDstAlpha);
        glGetIntegerv(GL_BLEND_EQUATION_RGB, out var lastBlendEquationRgb);
        glGetIntegerv(GL_BLEND_EQUATION_ALPHA, out var lastBlendEquationAlpha);

        var lastEnableBlend = glIsEnabled(GL_BLEND);
        var lastEnableCullFace = glIsEnabled(GL_CULL_FACE);
        var lastEnableDepthTest = glIsEnabled(GL_DEPTH_TEST);
        var lastEnableStencilTest = glIsEnabled(GL_STENCIL_TEST);
        var lastEnableScissorTest = glIsEnabled(GL_SCISSOR_TEST);
        var lastEnablePrimitiveRestart = glIsEnabled(GL_PRIMITIVE_RESTART);

        glGenVertexArrays(1, out var vertexArrayObject);
        SetupRenderState(drawData, fbWidth, fbHeight, vertexArrayObject);

        var clipOff = drawData.DisplayPos;
        var clipScale = drawData.FramebufferScale;

        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var cmdList = drawData.CmdLists[n];

            glBufferData(GL_ARRAY_BUFFER, (ulong) (cmdList.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>()), cmdList.VtxBuffer.Data, GL_STREAM_DRAW);
            glBufferData(GL_ELEMENT_ARRAY_BUFFER, (ulong) cmdList.IdxBuffer.Size * sizeof(ushort), cmdList.IdxBuffer.Data, GL_STREAM_DRAW);

            for (var cmdI = 0; cmdI < cmdList.CmdBuffer.Size; cmdI++)
            {
                var pcmd = cmdList.CmdBuffer[cmdI];
                if (pcmd.UserCallback != IntPtr.Zero)
                    throw new NotImplementedException();

                var clipRect = new Vector4
                {
                    X = (pcmd.ClipRect.X - clipOff.X) * clipScale.X,
                    Y = (pcmd.ClipRect.Y - clipOff.Y) * clipScale.Y,
                    Z = (pcmd.ClipRect.Z - clipOff.X) * clipScale.X,
                    W = (pcmd.ClipRect.W - clipOff.Y) * clipScale.Y
                };

                if (!(clipRect.X < fbWidth) || !(clipRect.Y < fbHeight) || !(clipRect.Z >= 0.0f) || !(clipRect.W >= 0.0f)) continue;

                glScissor((int) clipRect.X, (int) (fbHeight - clipRect.W), (int) (clipRect.Z - clipRect.X), (int) (clipRect.W - clipRect.Y));

                glBindTexture(GL_TEXTURE_2D, (uint) pcmd.TextureId);
                glDrawElementsBaseVertex(GL_TRIANGLES, (int) pcmd.ElemCount, GL_UNSIGNED_SHORT, (IntPtr) (pcmd.IdxOffset * sizeof(ushort)), (int) pcmd.VtxOffset);
            }
        }

        glDeleteVertexArrays(1, vertexArrayObject);

        glUseProgram((uint) lastProgram);
        glBindTexture(GL_TEXTURE_2D, (uint) lastTexture);
        glBindSampler(0, (uint) lastSampler);
        glActiveTexture((uint) lastActiveTexture);
        glBindVertexArray((uint) lastVertexArrayObject);
        glBindBuffer(GL_ARRAY_BUFFER, (uint) lastArrayBuffer);
        glBlendEquationSeparate((uint) lastBlendEquationRgb, (uint) lastBlendEquationAlpha);
        glBlendFuncSeparate((uint) lastBlendSrcRgb, (uint) lastBlendDstRgb, (uint) lastBlendSrcAlpha, (uint) lastBlendDstAlpha);
        if (lastEnableBlend) glEnable(GL_BLEND);
        else glDisable(GL_BLEND);
        if (lastEnableCullFace) glEnable(GL_CULL_FACE);
        else glDisable(GL_CULL_FACE);
        if (lastEnableDepthTest) glEnable(GL_DEPTH_TEST);
        else glDisable(GL_DEPTH_TEST);
        if (lastEnableStencilTest) glEnable(GL_STENCIL_TEST);
        else glDisable(GL_STENCIL_TEST);
        if (lastEnableScissorTest) glEnable(GL_SCISSOR_TEST);
        else glDisable(GL_SCISSOR_TEST);
        if (lastEnablePrimitiveRestart) glEnable(GL_PRIMITIVE_RESTART);
        else glDisable(GL_PRIMITIVE_RESTART);
        glPolygonMode(GL_FRONT_AND_BACK, (uint) lastPolygonMode);
        glViewport(lastViewport, lastViewport, lastViewport, lastViewport);
        glScissor(lastScissorBox, lastScissorBox, lastScissorBox, lastScissorBox);
    }

    private void SetupRenderState(ImDrawDataPtr drawData, int fbWidth, int fbHeight, uint vertexArrayObject)
    {
        glEnable(GL_BLEND);
        glBlendEquation(GL_FUNC_ADD);
        glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE_MINUS_SRC_ALPHA);
        glDisable(GL_CULL_FACE);
        glDisable(GL_DEPTH_TEST);
        glDisable(GL_STENCIL_TEST);
        glEnable(GL_SCISSOR_TEST);
        glDisable(GL_PRIMITIVE_RESTART);
        glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

        glViewport(0, 0, fbWidth, fbHeight);
        var l = drawData.DisplayPos.X;
        var r = drawData.DisplayPos.X + drawData.DisplaySize.X;
        var t = drawData.DisplayPos.Y;
        var b = drawData.DisplayPos.Y + drawData.DisplaySize.Y;

        var orthoProjection = new[,]
        {
            {2.0f / (r - l), 0.0f, 0.0f, 0.0f},
            {0.0f, 2.0f / (t - b), 0.0f, 0.0f},
            {0.0f, 0.0f, -1.0f, 0.0f},
            {(r + l) / (l - r), (t + b) / (b - t), 0.0f, 1.0f},
        };
        glUseProgram(_shaderHandle);
        glUniform1i(_attribLocationTex, 0);
        glUniformMatrix4fv(_attribLocationProjMtx, 1, false, orthoProjection[0, 0]);

        glBindSampler(0, 0);

        glBindVertexArray(vertexArrayObject);

        glBindBuffer(GL_ARRAY_BUFFER, _vboHandle);
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _elementsHandle);
        glEnableVertexAttribArray(_attribLocationVtxPos);
        glEnableVertexAttribArray(_attribLocationVtxUv);
        glEnableVertexAttribArray(_attribLocationVtxColor);

        glVertexAttribPointer(_attribLocationVtxPos, 2, GL_FLOAT, false, Unsafe.SizeOf<ImDrawVert>(), Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.pos)));
        glVertexAttribPointer(_attribLocationVtxUv, 2, GL_FLOAT, false, Unsafe.SizeOf<ImDrawVert>(), Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.uv)));
        glVertexAttribPointer(_attribLocationVtxColor, 4, GL_UNSIGNED_BYTE, true, Unsafe.SizeOf<ImDrawVert>(), Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.col)));
    }

    private void DestroyDeviceObjects()
    {
        if (_vboHandle != 0)
        {
            glDeleteBuffers(1, _vboHandle);
            _vboHandle = 0;
        }

        if (_elementsHandle != 0)
        {
            glDeleteBuffers(1, _elementsHandle);
            _elementsHandle = 0;
        }

        if (_shaderHandle != 0 && _vertHandle != 0)
            glDetachShader(_shaderHandle, _vertHandle);

        if (_shaderHandle != 0 && _fragHandle != 0)
            glDetachShader(_shaderHandle, _fragHandle);

        if (_vertHandle != 0)
        {
            glDeleteShader(_vertHandle);
            _vertHandle = 0;
        }

        if (_fragHandle != 0)
        {
            glDeleteShader(_fragHandle);
            _fragHandle = 0;
        }

        if (_shaderHandle != 0)
        {
            glDeleteProgram(_shaderHandle);
            _shaderHandle = 0;
        }
    }

    private void DestroyFontsTexture()
    {
        if (_fontTexture == 0) return;

        glDeleteTextures(1, _fontTexture);
        _io.Fonts.SetTexID(IntPtr.Zero);
        _fontTexture = 0;
    }

    private void GlfwShutdown()
    {
        for (ImGuiMouseCursor cursorN = 0; cursorN < ImGuiMouseCursor.COUNT; cursorN++)
        {
            glfwDestroyCursor(_mouseCursors[(int) cursorN]);
            _mouseCursors[(int) cursorN] = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        DestroyDeviceObjects();
        DestroyFontsTexture();
        GlfwShutdown();
        ImGui.DestroyContext();
    }
}