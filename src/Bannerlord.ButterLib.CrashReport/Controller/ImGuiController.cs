using Bannerlord.ButterLib.CrashReportWindow.ImGui;

using ImGuiNET;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

internal partial class ImGuiController : IDisposable
{
    private static readonly uint _sizeOfImDrawVert = (uint) Unsafe.SizeOf<ImDrawVert>();
    private static readonly IntPtr _offsetOfImDrawVertPos = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.pos));
    private static readonly IntPtr _offsetOfImDrawVertUV = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.uv));
    private static readonly IntPtr _offsetOfImDrawVertCol = Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.col));

    private static readonly StandardCursor[] _mouseCursors =
    [
        StandardCursor.Arrow,   // ImGuiMouseCursor.Arrow
        StandardCursor.IBeam,   // ImGuiMouseCursor.TextInput
        StandardCursor.Arrow,   // ImGuiMouseCursor.ResizeAll
        StandardCursor.VResize, // ImGuiMouseCursor.ResizeNS
        StandardCursor.HResize, // ImGuiMouseCursor.ResizeEW
        StandardCursor.Arrow,   // ImGuiMouseCursor.ResizeNESW
        StandardCursor.Arrow,   // ImGuiMouseCursor.ResizeNWSE
        StandardCursor.Hand,    // ImGuiMouseCursor.Hand
        StandardCursor.Arrow    // ImGuiMouseCursor.NotAllowed
    ];

    private readonly CmGui _imgui;
    private readonly GL _gl;
    private readonly IntPtr _context;
    private readonly ImGui.ImGuiIOPtr _io;

    private readonly IView _view;
    private readonly IInputContext _input;

    private IKeyboard Keyboard => _input.Keyboards[0];
    private IMouse Mouse => _input.Mice[0];

    private Shader _shader = default!;
    private Texture _fontTexture = default!;
    private int _attribLocationTex, _attribLocationProjMtx;
    private uint _attribLocationVtxPos, _attribLocationVtxUv, _attribLocationVtxColor;
    private uint _vboHandle, _elementsHandle, _vertexArrayObject;
    private uint _windowsWidth, _windowsHeight;

    public ImGuiController(CmGui imgui, GL gl, IView view, IInputContext input)
    {
        _imgui = imgui;
        _gl = gl;
        _context = _imgui.CreateContext();
        _io = _imgui.GetIO();

        _view = view;
        _input = input;

        _imgui.SetCurrentContext(_context);
    }
}