using ImGuiNET;

using System.Numerics;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static class JmGuiNative
    {
        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, ref readonly Vector2 pivot);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowSize(Vector2 size, ImGuiCond cond);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ref readonly ImGuiViewport igGetMainViewport();

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe ImGuiBool igInputTextMultiline(ref readonly byte label, ref byte buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback? callback, void* user_data);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igTreeNode_Str(ref readonly byte label);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igTreeNodeEx_Str(ref readonly byte label, ImGuiTreeNodeFlags flags);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igBeginTable(ref readonly byte str_id, int column, ImGuiTableFlags flags, Vector2 outer_size, float inner_width);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igBegin(ref readonly byte name, ref readonly byte p_open, ImGuiWindowFlags flags);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igSmallButton(ref readonly byte label);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igButton(ref readonly byte label, Vector2 size);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igCheckbox(ref readonly byte label, ref readonly byte v);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextWrapped(ref readonly byte fmt);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igText(ref readonly byte fmt);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextColored(Vector4 col, ref readonly byte fmt);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiBool igBeginChild_Str(ref readonly byte str_id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags);

        [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushStyleColor_Vec4(ImGuiCol idx, Vector4 col);
    }
}