using Bannerlord.ButterLib.CrashReportWindow.Utils;

using ImGuiNET;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int ImGuiInputTextCallback(ImGuiInputTextCallbackData* data);

unsafe partial class CmGui
{
    private const string LibWindows = "cimgui.dll";
    private const string LibLinux = "libcimgui.so";
    private const string LibOSX = "libcimgui.dylib";
    private static readonly IntPtr NativeLibrary = GetNativeLibrary();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    private static IntPtr GetNativeLibrary()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return NativeFuncExecuturer.LoadLibraryExt(LibWindows);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return NativeFuncExecuturer.LoadLibraryExt(LibLinux);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return NativeFuncExecuturer.LoadLibraryExt(LibOSX);
        return IntPtr.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public static IntPtr LoadFunction(ReadOnlySpan<byte> function) => NativeFuncExecuturer.LoadFunction(NativeLibrary, function);

    /// <summary>
    /// Returns a function pointer for the OpenGL function with the specified name. 
    /// </summary>
    /// <param name="funcNameUtf8">The name of the function to lookup.</param>
    public delegate IntPtr GetProcAddressHandler(ReadOnlySpan<byte> funcNameUtf8);

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, int, byte> igBegin;
    private readonly delegate* unmanaged[Cdecl]<byte*, Vector2, int, int, byte> igBeginChild_Str;
    private readonly delegate* unmanaged[Cdecl]<byte*, int, int, Vector2, float, byte> igBeginTable;
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
    private readonly delegate* unmanaged[Cdecl]<int> igGetMouseCursor;
    private readonly delegate* unmanaged[Cdecl]<IntPtr> igGetStyle;
    private readonly delegate* unmanaged[Cdecl]<float> igGetTextLineHeight;
    private readonly delegate* unmanaged[Cdecl]<float, void> igIndent;
    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, uint, Vector2, int, ImGuiInputTextCallback?, void*, byte> igInputTextMultiline;
    private readonly delegate* unmanaged[Cdecl]<void> igNewFrame;
    private readonly delegate* unmanaged[Cdecl]<void> igNewLine;
    private readonly delegate* unmanaged[Cdecl]<void> igNextColumn;
    private readonly delegate* unmanaged[Cdecl]<int, void> igPopStyleColor;
    private readonly delegate* unmanaged[Cdecl]<int, void> igPopStyleVar;
    private readonly delegate* unmanaged[Cdecl]<int, Vector4, void> igPushStyleColor_Vec4;
    private readonly delegate* unmanaged[Cdecl]<int, float, void> igPushStyleVar_Float;
    private readonly delegate* unmanaged[Cdecl]<void> igRender;
    private readonly delegate* unmanaged[Cdecl]<float, float, void> igSameLine;
    private readonly delegate* unmanaged[Cdecl]<void> igSeparator;
    private readonly delegate* unmanaged[Cdecl]<IntPtr, void> igSetCurrentContext;
    private readonly delegate* unmanaged[Cdecl]<Vector2, int, Vector2, void> igSetNextWindowPos;
    private readonly delegate* unmanaged[Cdecl]<Vector2, int, void> igSetNextWindowSize;
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
    private readonly delegate* unmanaged[Cdecl]<byte*, int, byte> igTreeNodeEx_Str;
    private readonly delegate* unmanaged[Cdecl]<void> igTreePop;
    private readonly delegate* unmanaged[Cdecl]<float, void> igUnindent;
    public readonly delegate* unmanaged[Cdecl]<void*, uint, void> ImGuiIO_AddInputCharacter;
    //public readonly delegate* unmanaged[Cdecl]<void*, ImGuiKey, byte, void> ImGuiIO_AddKeyEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, int, byte, void> ImGuiIO_AddKeyEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, int, byte, void> ImGuiIO_AddMouseButtonEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, float, float, void> ImGuiIO_AddMouseWheelEvent;
    public readonly delegate* unmanaged[Cdecl]<void*, IntPtr*, int*, int*, int*, void> ImFontAtlas_GetTexDataAsRGBA32;
    public readonly delegate* unmanaged[Cdecl]<void*, IntPtr, void> ImFontAtlas_SetTexID;

    public CmGui(GetProcAddressHandler getProcAddress)
    {
        igBegin = (delegate* unmanaged[Cdecl]<byte*, byte*, int, byte>) getProcAddress("igBegin\0"u8);
        igBeginChild_Str = (delegate* unmanaged[Cdecl]<byte*, Vector2, int, int, byte>) getProcAddress("igBeginChild_Str\0"u8);
        igBeginTable = (delegate* unmanaged[Cdecl]<byte*, int, int, Vector2, float, byte>) getProcAddress("igBeginTable\0"u8);
        igBullet = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igBullet\0"u8);
        igButton = (delegate* unmanaged[Cdecl]<byte*, Vector2, byte>) getProcAddress("igButton\0"u8);
        igCheckbox = (delegate* unmanaged[Cdecl]<byte*, byte*, byte>) getProcAddress("igCheckbox\0"u8);
        igCreateContext = (delegate* unmanaged[Cdecl]<void*, IntPtr>) getProcAddress("igCreateContext\0"u8);
        igDestroyContext = (delegate* unmanaged[Cdecl]<IntPtr, void>) getProcAddress("igDestroyContext\0"u8);
        igEnd = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igEnd\0"u8);
        igEndChild = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igEndChild\0"u8);
        igEndTable = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igEndTable\0"u8);
        igGetCurrentContext = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("igGetCurrentContext\0"u8);
        igGetDrawData = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("igGetDrawData\0"u8);
        igGetIO = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("igGetIO\0"u8);
        igGetMainViewport = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("igGetMainViewport\0"u8);
        igGetMouseCursor = (delegate* unmanaged[Cdecl]<int>) getProcAddress("igGetMouseCursor\0"u8);
        igGetStyle = (delegate* unmanaged[Cdecl]<IntPtr>) getProcAddress("igGetStyle\0"u8);
        igGetTextLineHeight = (delegate* unmanaged[Cdecl]<float>) getProcAddress("igGetTextLineHeight\0"u8);
        igIndent = (delegate* unmanaged[Cdecl]<float, void>) getProcAddress("igIndent\0"u8);
        igInputTextMultiline = (delegate* unmanaged[Cdecl]<byte*, byte*, uint, Vector2, int, ImGuiInputTextCallback?, void*, byte>) getProcAddress("igInputTextMultiline\0"u8);
        igNewFrame = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igNewFrame\0"u8);
        igNewLine = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igNewLine\0"u8);
        igNextColumn = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igNextColumn\0"u8);
        igPopStyleColor = (delegate* unmanaged[Cdecl]<int, void>) getProcAddress("igPopStyleColor\0"u8);
        igPopStyleVar = (delegate* unmanaged[Cdecl]<int, void>) getProcAddress("igPopStyleVar\0"u8);
        igPushStyleColor_Vec4 = (delegate* unmanaged[Cdecl]<int, Vector4, void>) getProcAddress("igPushStyleColor_Vec4\0"u8);
        igPushStyleVar_Float = (delegate* unmanaged[Cdecl]<int, float, void>) getProcAddress("igPushStyleVar_Float\0"u8);
        igRender = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igRender\0"u8);
        igSameLine = (delegate* unmanaged[Cdecl]<float, float, void>) getProcAddress("igSameLine\0"u8);
        igSeparator = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igSeparator\0"u8);
        igSetCurrentContext = (delegate* unmanaged[Cdecl]<IntPtr, void>) getProcAddress("igSetCurrentContext\0"u8);
        igSetNextWindowPos = (delegate* unmanaged[Cdecl]<Vector2, int, Vector2, void>) getProcAddress("igSetNextWindowPos\0"u8);
        igSetNextWindowSize = (delegate* unmanaged[Cdecl]<Vector2, int, void>) getProcAddress("igSetNextWindowSize\0"u8);
        igSetNextWindowViewport = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("igSetNextWindowViewport\0"u8);
        igSetWindowFontScale = (delegate* unmanaged[Cdecl]<float, void>) getProcAddress("igSetWindowFontScale\0"u8);
        igSmallButton = (delegate* unmanaged[Cdecl]<byte*, byte>) getProcAddress("igSmallButton\0"u8);
        igStyleColorsDark = (delegate* unmanaged[Cdecl]<void*, void>) getProcAddress("igStyleColorsDark\0"u8);
        igStyleColorsLight = (delegate* unmanaged[Cdecl]<void*, void>) getProcAddress("igStyleColorsLight\0"u8);
        igTableNextColumn = (delegate* unmanaged[Cdecl]<byte>) getProcAddress("igTableNextColumn\0"u8);
        igText = (delegate* unmanaged[Cdecl]<byte*, void>) getProcAddress("igText\0"u8);
        igTextColored = (delegate* unmanaged[Cdecl]<Vector4, byte*, void>) getProcAddress("igTextColored\0"u8);
        igTextWrapped = (delegate* unmanaged[Cdecl]<byte*, void>) getProcAddress("igTextWrapped\0"u8);
        igTreeNode_Str = (delegate* unmanaged[Cdecl]<byte*, byte>) getProcAddress("igTreeNode_Str\0"u8);
        igTreeNodeEx_Str = (delegate* unmanaged[Cdecl]<byte*, int, byte>) getProcAddress("igTreeNodeEx_Str\0"u8);
        igTreePop = (delegate* unmanaged[Cdecl]<void>) getProcAddress("igTreePop\0"u8);
        igUnindent = (delegate* unmanaged[Cdecl]<float, void>) getProcAddress("igUnindent\0"u8);
        ImGuiIO_AddInputCharacter = (delegate* unmanaged[Cdecl]<void*, uint, void>) getProcAddress("ImGuiIO_AddInputCharacter\0"u8);
        ImGuiIO_AddKeyEvent = (delegate* unmanaged[Cdecl]<void*, int, byte, void>) getProcAddress("ImGuiIO_AddKeyEvent\0"u8);
        ImGuiIO_AddMouseButtonEvent = (delegate* unmanaged[Cdecl]<void*, int, byte, void>) getProcAddress("ImGuiIO_AddMouseButtonEvent\0"u8);
        ImGuiIO_AddMouseWheelEvent = (delegate* unmanaged[Cdecl]<void*, float, float, void>) getProcAddress("ImGuiIO_AddMouseWheelEvent\0"u8);
        ImFontAtlas_GetTexDataAsRGBA32 = (delegate* unmanaged[Cdecl]<void*, IntPtr*, int*, int*, int*, void>) getProcAddress("ImFontAtlas_GetTexDataAsRGBA32\0"u8);
        ImFontAtlas_SetTexID = (delegate* unmanaged[Cdecl]<void*, IntPtr, void>) getProcAddress("ImFontAtlas_SetTexID\0"u8);
    }
}