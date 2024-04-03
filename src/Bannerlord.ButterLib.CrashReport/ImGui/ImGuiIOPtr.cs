using ImGuiNET;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

internal readonly struct ImGuiIOPtr
{
    private readonly CmGui _imgui;
    public unsafe ImGuiIO* NativePtr { get; }

    public unsafe ImGuiIOPtr(CmGui imgui, IntPtr nativePtr)
    {
        _imgui = imgui;
        NativePtr = (ImGuiIO*) nativePtr;
    }

    public unsafe ref ImGuiConfigFlags ConfigFlags => ref Unsafe.AsRef<ImGuiConfigFlags>(&NativePtr->ConfigFlags);
    public unsafe ref ImGuiBackendFlags BackendFlags => ref Unsafe.AsRef<ImGuiBackendFlags>(&NativePtr->BackendFlags);
    public unsafe ref Vector2 DisplaySize => ref Unsafe.AsRef<Vector2>(&NativePtr->DisplaySize);
    public unsafe ref float DeltaTime => ref Unsafe.AsRef<float>(&NativePtr->DeltaTime);
    public unsafe ImFontAtlasPtr Fonts => new(_imgui, NativePtr->Fonts);
    public unsafe ref Vector2 DisplayFramebufferScale => ref Unsafe.AsRef<Vector2>(&NativePtr->DisplayFramebufferScale);
    public unsafe ref bool MouseDrawCursor => ref Unsafe.AsRef<bool>(&NativePtr->MouseDrawCursor);
    public unsafe ref IntPtr GetClipboardTextFn => ref Unsafe.AsRef<IntPtr>(&NativePtr->GetClipboardTextFn);
    public unsafe ref IntPtr SetClipboardTextFn => ref Unsafe.AsRef<IntPtr>(&NativePtr->SetClipboardTextFn);
    public unsafe IntPtr ClipboardUserData { get => (IntPtr) NativePtr->ClipboardUserData; set => NativePtr->ClipboardUserData = (void*) value; }
    public unsafe ref bool WantSetMousePos => ref Unsafe.AsRef<bool>(&NativePtr->WantSetMousePos);
    public unsafe ref Vector2 MousePos => ref Unsafe.AsRef<Vector2>(&NativePtr->MousePos);

    public unsafe void AddInputCharacter(uint c) => _imgui.ImGuiIO_AddInputCharacter(NativePtr, c);
    public unsafe void AddKeyEvent(ImGuiKey key, bool down) => _imgui.ImGuiIO_AddKeyEvent(NativePtr, key, Unsafe.As<bool, byte>(ref down));
    public unsafe void AddMouseButtonEvent(int button, bool down) => _imgui.ImGuiIO_AddMouseButtonEvent(NativePtr, button, Unsafe.As<bool, byte>(ref down));
    public unsafe void AddMouseWheelEvent(float wheel_x, float wheel_y) => _imgui.ImGuiIO_AddMouseWheelEvent(NativePtr, wheel_x, wheel_y);
}