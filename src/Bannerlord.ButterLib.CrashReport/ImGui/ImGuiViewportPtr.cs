using ImGuiNET;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

// Made it ref so it won't be ever on heap
internal readonly ref struct ImGuiViewportPtr
{
    private unsafe ImGuiViewport* NativePtr { get; }

    public unsafe ImGuiViewportPtr(IntPtr nativePtr) => NativePtr = (ImGuiViewport*) nativePtr;

    public unsafe ref uint ID => ref Unsafe.AsRef<uint>(&NativePtr->ID);
    public unsafe ref Vector2 WorkPos => ref Unsafe.AsRef<Vector2>(&NativePtr->WorkPos);
    public unsafe ref Vector2 WorkSize => ref Unsafe.AsRef<Vector2>(&NativePtr->WorkSize);
}