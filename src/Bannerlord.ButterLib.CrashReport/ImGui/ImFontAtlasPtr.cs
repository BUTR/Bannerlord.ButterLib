using ImGuiNET;

using System;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

internal readonly ref struct ImFontAtlasPtr
{
    private readonly CmGui _imgui;
    public unsafe ImFontAtlas* NativePtr { get; }

    public unsafe ImFontAtlasPtr(CmGui imgui, ImFontAtlas* nativePtr)
    {
        _imgui = imgui;
        NativePtr = nativePtr;
    }

    public unsafe ref IntPtr TexID => ref Unsafe.AsRef<IntPtr>(&NativePtr->TexID);

    public unsafe void GetTexDataAsRGBA32(out IntPtr out_pixels, out int out_width, out int out_height)
    {
        var out_bytes_per_pixel = (int*) null;
        fixed (IntPtr* out_pixels1 = &out_pixels)
        fixed (int* out_width1 = &out_width)
        fixed (int* out_height1 = &out_height)
        {
            _imgui.ImFontAtlas_GetTexDataAsRGBA32(NativePtr, out_pixels1, out_width1, out_height1, out_bytes_per_pixel);
        }
    }

    public unsafe void SetTexID(IntPtr id) => _imgui.ImFontAtlas_SetTexID(NativePtr, id);
}