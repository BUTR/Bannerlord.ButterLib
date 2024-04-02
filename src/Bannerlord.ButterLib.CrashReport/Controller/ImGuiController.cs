using Bannerlord.ButterLib.CrashReportWindow.ImGui;
using Bannerlord.ButterLib.CrashReportWindow.OpenGL;
using Bannerlord.ButterLib.CrashReportWindow.Windowing;

using ImGuiNET;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

using static Glfw;

internal partial class ImGuiController : IDisposable
{
    private static readonly uint _sizeOfImDrawVert = (uint) Unsafe.SizeOf<ImDrawVert>();
    private static readonly IntPtr _offsetOfImDrawVertPos = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.pos));
    private static readonly IntPtr _offsetOfImDrawVertUV = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.uv));
    private static readonly IntPtr _offsetOfImDrawVertCol = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.col));


    private readonly CmGui _imgui;
    private readonly Glfw _glfw;
    private readonly Gl _gl;
    private readonly GlfwWindowPtr _windowPtr;
    private readonly IntPtr _context;
    private readonly ImGui.ImGuiIOPtr _io;

    private readonly IntPtr[] _mouseCursors = new IntPtr[(int) ImGuiMouseCursor.COUNT];

    private Shader _shader = default!;
    private Texture _fontTexture = default!;
    private int _attribLocationTex, _attribLocationProjMtx;
    private uint _attribLocationVtxPos, _attribLocationVtxUv, _attribLocationVtxColor;
    private uint _vboHandle, _elementsHandle, _vertexArrayObject;
    private uint _windowsWidth, _windowsHeight, _frameBufferWidth, _frameBufferHeight;

    private readonly WindowSizeCallback _userCallbackWindowsSize;
    private readonly FramebufferSizeCallback _userCallbackFramebufferSize;
    private readonly MouseButtonCallback _userCallbackMouseButton;
    private readonly ScrollCallback _userCallbackScroll;
    private readonly KeyCallback _userCallbackKey;
    private readonly CharCallback _userCallbackChar;

    public ImGuiController(CmGui imgui, Glfw glfw, Gl gl, GlfwWindowPtr windowPtr)
    {
        _imgui = imgui;
        _glfw = glfw;
        _gl = gl;
        _windowPtr = windowPtr;

        _context = _imgui.CreateContext();
        _imgui.SetCurrentContext(_context);

        _io = _imgui.GetIO();

        _userCallbackWindowsSize = WindowsSizeCallback;
        _userCallbackFramebufferSize = FramebufferSizeCallback;
        _userCallbackMouseButton = MouseButtonCallback;
        _userCallbackScroll = ScrollCallback;
        _userCallbackKey = KeyCallback;
        _userCallbackChar = CharCallback;
    }
}