using ImGuiNET;

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private readonly ref struct ImGuiBool
    {
        private readonly byte Value;

        public static implicit operator bool(ImGuiBool value) => value.Value != 0;
    }

    private readonly ref struct ImGuiViewportPtr
    {
        private unsafe ImGuiViewport* NativePtr { get; }

        public unsafe ImGuiViewportPtr(ref readonly ImGuiViewport nativeRef) => NativePtr = (ImGuiViewport*) Unsafe.AsPointer(ref Unsafe.AsRef(nativeRef));

        public unsafe ref uint ID => ref Unsafe.AsRef(NativePtr->ID);
        public unsafe ref Vector2 WorkPos => ref Unsafe.AsRef(NativePtr->WorkPos);
        public unsafe ref Vector2 WorkSize => ref Unsafe.AsRef(NativePtr->WorkSize);
    }
}