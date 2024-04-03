using ImGuiNET;

using Silk.NET.Core.Loader;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int ImGuiInputTextCallback(ImGuiInputTextCallbackData* data);

unsafe partial class CmGui : IDisposable
{
    private const string LibWindows = "cimgui.dll";
    private const string LibLinux = "libcimgui.so";
    private const string LibOSX = "libcimgui.dylib";

    /// <summary>
    /// Returns a function pointer for the OpenGL function with the specified name. 
    /// </summary>
    /// <param name="funcName">The name of the function to lookup.</param>
    public delegate IntPtr LoadFunctionHandler(string funcName);

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, ImGuiWindowFlags, byte> igBegin;
    private readonly delegate* unmanaged[Cdecl]<byte*, Vector2, ImGuiChildFlags, ImGuiWindowFlags, byte> igBeginChild_Str;
    private readonly delegate* unmanaged[Cdecl]<byte*, int, ImGuiTableFlags, Vector2, float, byte> igBeginTable;
    private readonly delegate* unmanaged[Cdecl]<void> igBullet;
    private readonly delegate* unmanaged[Cdecl]<byte*, Vector2, byte> igButton;
    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte> igCheckbox;
    private readonly delegate* unmanaged[Cdecl]<void*, IntPtr> igCreateContext;
    private readonly delegate* unmanaged[Cdecl]<IntPtr, void> igDestroyContext;
    private readonly delegate* unmanaged[Cdecl]<void> igEnd;
    private readonly delegate* unmanaged[Cdecl]<void> igEndChild;
    private readonly delegate* unmanaged[Cdecl]<void> igEndTable;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetCurrentContext;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetDrawData;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetIO;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetMainViewport;
    private readonly delegate* unmanaged[Cdecl]<ImGuiMouseCursor> igGetMouseCursor;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetStyle;
    private readonly delegate* unmanaged[Cdecl]<float> igGetTextLineHeight;
    private readonly delegate* unmanaged[Cdecl]<float, void> igIndent;
    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, uint, Vector2, ImGuiInputTextFlags, ImGuiInputTextCallback?, void*, byte> igInputTextMultiline;
    private readonly delegate* unmanaged[Cdecl]<void> igNewFrame;
    private readonly delegate* unmanaged[Cdecl]<void> igNewLine;
    private readonly delegate* unmanaged[Cdecl]<void> igNextColumn;
    private readonly delegate* unmanaged[Cdecl]<int, void> igPopStyleColor;
    private readonly delegate* unmanaged[Cdecl]<int, void> igPopStyleVar;
    private readonly delegate* unmanaged[Cdecl]<ImGuiCol, Vector4, void> igPushStyleColor_Vec4;
    private readonly delegate* unmanaged[Cdecl]<ImGuiStyleVar, float, void> igPushStyleVar_Float;
    private readonly delegate* unmanaged[Cdecl]<void> igRender;
    private readonly delegate* unmanaged[Cdecl]<float, float, void> igSameLine;
    private readonly delegate* unmanaged[Cdecl]<void> igSeparator;
    private readonly delegate* unmanaged[Cdecl]<IntPtr, void> igSetCurrentContext;
    private readonly delegate* unmanaged[Cdecl]<Vector2, ImGuiCond, Vector2, void> igSetNextWindowPos;
    private readonly delegate* unmanaged[Cdecl]<Vector2, ImGuiCond, void> igSetNextWindowSize;
    private readonly delegate* unmanaged[Cdecl]<uint, void> igSetNextWindowViewport;
    private readonly delegate* unmanaged[Cdecl]<float, void> igSetWindowFontScale;
    private readonly delegate* unmanaged[Cdecl]<byte*, byte> igSmallButton;
    private readonly delegate* unmanaged[Cdecl]<void*, void> igStyleColorsDark;
    private readonly delegate* unmanaged[Cdecl]<void*, void> igStyleColorsLight;
    private readonly delegate* unmanaged[Cdecl]<byte> igTableNextColumn;
    private readonly delegate* unmanaged[Cdecl]<byte*, void> igText;
    private readonly delegate* unmanaged[Cdecl]<Vector4, byte*, void> igTextColored;
    private readonly delegate* unmanaged[Cdecl]<byte*, void> igTextWrapped;
    private readonly delegate* unmanaged[Cdecl]<byte*, byte> igTreeNode_Str;
    private readonly delegate* unmanaged[Cdecl]<byte*, ImGuiTreeNodeFlags, byte> igTreeNodeEx_Str;
    private readonly delegate* unmanaged[Cdecl]<void> igTreePop;
    private readonly delegate* unmanaged[Cdecl]<float, void> igUnindent;
    public readonly delegate* unmanaged[Cdecl]<void*, uint, void> ImGuiIO_AddInputCharacter;
    public readonly delegate* unmanaged[Cdecl]<void*, ImGuiKey, byte, void> ImGuiIO_AddKeyEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, int, byte, void> ImGuiIO_AddMouseButtonEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, float, float, void> ImGuiIO_AddMouseWheelEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, IntPtr*, int*, int*, int*, void> ImFontAtlas_GetTexDataAsRGBA32;
    public readonly delegate* unmanaged[Cdecl]<void*, IntPtr, void> ImFontAtlas_SetTexID;

    private readonly IntPtr NativeLibrary;
    private readonly LibraryLoader LibraryLoader;

    public CmGui()
    {
        LibraryLoader = LibraryLoader.GetPlatformDefaultLoader();
        NativeLibrary = LibraryLoader.LoadNativeLibrary(
        [
            LibWindows,
            LibLinux,
            LibOSX
        ]);

        igBegin = (delegate* unmanaged[Cdecl]<byte*, byte*, ImGuiWindowFlags, byte>) LoadFunction("igBegin");
        igBeginChild_Str = (delegate* unmanaged[Cdecl]<byte*, Vector2, ImGuiChildFlags, ImGuiWindowFlags, byte>) LoadFunction("igBeginChild_Str");
        igBeginTable = (delegate* unmanaged[Cdecl]<byte*, int, ImGuiTableFlags, Vector2, float, byte>) LoadFunction("igBeginTable");
        igBullet = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igBullet");
        igButton = (delegate* unmanaged[Cdecl]<byte*, Vector2, byte>) LoadFunction("igButton");
        igCheckbox = (delegate* unmanaged[Cdecl]<byte*, byte*, byte>) LoadFunction("igCheckbox");
        igCreateContext = (delegate* unmanaged[Cdecl]<void*, IntPtr>) LoadFunction("igCreateContext");
        igDestroyContext = (delegate* unmanaged[Cdecl]<IntPtr, void>) LoadFunction("igDestroyContext");
        igEnd = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igEnd");
        igEndChild = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igEndChild");
        igEndTable = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igEndTable");
        igGetCurrentContext = (delegate* unmanaged[Cdecl]<IntPtr>) LoadFunction("igGetCurrentContext");
        igGetDrawData = (delegate* unmanaged[Cdecl]<IntPtr>) LoadFunction("igGetDrawData");
        igGetIO = (delegate* unmanaged[Cdecl]<IntPtr>) LoadFunction("igGetIO");
        igGetMainViewport = (delegate* unmanaged[Cdecl]<IntPtr>) LoadFunction("igGetMainViewport");
        igGetMouseCursor = (delegate* unmanaged[Cdecl]<ImGuiMouseCursor>) LoadFunction("igGetMouseCursor");
        igGetStyle = (delegate* unmanaged[Cdecl]<IntPtr>) LoadFunction("igGetStyle");
        igGetTextLineHeight = (delegate* unmanaged[Cdecl]<float>) LoadFunction("igGetTextLineHeight");
        igIndent = (delegate* unmanaged[Cdecl]<float, void>) LoadFunction("igIndent");
        igInputTextMultiline = (delegate* unmanaged[Cdecl]<byte*, byte*, uint, Vector2, ImGuiInputTextFlags, ImGuiInputTextCallback?, void*, byte>) LoadFunction("igInputTextMultiline");
        igNewFrame = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igNewFrame");
        igNewLine = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igNewLine");
        igNextColumn = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igNextColumn");
        igPopStyleColor = (delegate* unmanaged[Cdecl]<int, void>) LoadFunction("igPopStyleColor");
        igPopStyleVar = (delegate* unmanaged[Cdecl]<int, void>) LoadFunction("igPopStyleVar");
        igPushStyleColor_Vec4 = (delegate* unmanaged[Cdecl]<ImGuiCol, Vector4, void>) LoadFunction("igPushStyleColor_Vec4");
        igPushStyleVar_Float = (delegate* unmanaged[Cdecl]<ImGuiStyleVar, float, void>) LoadFunction("igPushStyleVar_Float");
        igRender = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igRender");
        igSameLine = (delegate* unmanaged[Cdecl]<float, float, void>) LoadFunction("igSameLine");
        igSeparator = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igSeparator");
        igSetCurrentContext = (delegate* unmanaged[Cdecl]<IntPtr, void>) LoadFunction("igSetCurrentContext");
        igSetNextWindowPos = (delegate* unmanaged[Cdecl]<Vector2, ImGuiCond, Vector2, void>) LoadFunction("igSetNextWindowPos");
        igSetNextWindowSize = (delegate* unmanaged[Cdecl]<Vector2, ImGuiCond, void>) LoadFunction("igSetNextWindowSize");
        igSetNextWindowViewport = (delegate* unmanaged[Cdecl]<uint, void>) LoadFunction("igSetNextWindowViewport");
        igSetWindowFontScale = (delegate* unmanaged[Cdecl]<float, void>) LoadFunction("igSetWindowFontScale");
        igSmallButton = (delegate* unmanaged[Cdecl]<byte*, byte>) LoadFunction("igSmallButton");
        igStyleColorsDark = (delegate* unmanaged[Cdecl]<void*, void>) LoadFunction("igStyleColorsDark");
        igStyleColorsLight = (delegate* unmanaged[Cdecl]<void*, void>) LoadFunction("igStyleColorsLight");
        igTableNextColumn = (delegate* unmanaged[Cdecl]<byte>) LoadFunction("igTableNextColumn");
        igText = (delegate* unmanaged[Cdecl]<byte*, void>) LoadFunction("igText");
        igTextColored = (delegate* unmanaged[Cdecl]<Vector4, byte*, void>) LoadFunction("igTextColored");
        igTextWrapped = (delegate* unmanaged[Cdecl]<byte*, void>) LoadFunction("igTextWrapped");
        igTreeNode_Str = (delegate* unmanaged[Cdecl]<byte*, byte>) LoadFunction("igTreeNode_Str");
        igTreeNodeEx_Str = (delegate* unmanaged[Cdecl]<byte*, ImGuiTreeNodeFlags, byte>) LoadFunction("igTreeNodeEx_Str");
        igTreePop = (delegate* unmanaged[Cdecl]<void>) LoadFunction("igTreePop");
        igUnindent = (delegate* unmanaged[Cdecl]<float, void>) LoadFunction("igUnindent");
        ImGuiIO_AddInputCharacter = (delegate* unmanaged[Cdecl]<void*, uint, void>) LoadFunction("ImGuiIO_AddInputCharacter");
        ImGuiIO_AddKeyEvent = (delegate* unmanaged[Cdecl]<void*, ImGuiKey, byte, void>) LoadFunction("ImGuiIO_AddKeyEvent");
        ImGuiIO_AddMouseButtonEvent = (delegate* unmanaged[Cdecl]<void*, int, byte, void>) LoadFunction("ImGuiIO_AddMouseButtonEvent");
        ImGuiIO_AddMouseWheelEvent = (delegate* unmanaged[Cdecl]<void*, float, float, void>) LoadFunction("ImGuiIO_AddMouseWheelEvent");
        ImFontAtlas_GetTexDataAsRGBA32 = (delegate* unmanaged[Cdecl]<void*, IntPtr*, int*, int*, int*, void>) LoadFunction("ImFontAtlas_GetTexDataAsRGBA32");
        ImFontAtlas_SetTexID = (delegate* unmanaged[Cdecl]<void*, IntPtr, void>) LoadFunction("ImFontAtlas_SetTexID");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr LoadFunction(string function) => LibraryLoader.LoadFunctionPointer(NativeLibrary, function);

    public void Dispose()
    {
        LibraryLoader.FreeNativeLibrary(NativeLibrary);
    }
}