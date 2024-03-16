using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using ImGuiNET;

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static class JmGui
    {
        private static readonly byte[] _padding = UnsafeHelper.ToUtf8Array(string.Empty.PadRight(64));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RenderId(ReadOnlySpan<byte> title, string id)
        {
            Text(title);
            ImGui.SameLine();
            ImGui.SmallButton(id);
        }

        public static unsafe void PadRight(int toAppend)
        {
            fixed (byte* paddingPtr = _padding)
            {
                ref var padding = ref Unsafe.AsRef<byte>(paddingPtr);
                var offset = _padding.Length - toAppend;
                padding = ref Unsafe.Add(ref padding, offset < 0 ? 0 : offset);
                TextSameLine(ref padding);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InputText(string label, ref string input)
        {
            return ImGui.InputText(label, ref input, ushort.MaxValue, ImGuiInputTextFlags.ReadOnly);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InputTextMultiline(string label, ref string input, int lineCount)
        {
            PushStyleColor(ImGuiCol.FrameBg, in Zero4);
            var result = ImGui.InputTextMultiline(label, ref input, ushort.MaxValue, new Vector2(-1, ImGui.GetTextLineHeight() * (lineCount + 2)), ImGuiInputTextFlags.ReadOnly);
            ImGui.PopStyleColor();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool InputTextMultiline(ReadOnlySpan<byte> label, Span<byte> input, int lineCount)
        {
            var buf_size = input.Length;
            var size = new Vector2(-1, ImGui.GetTextLineHeight() * (lineCount + 2));
            const ImGuiInputTextFlags flags = ImGuiInputTextFlags.ReadOnly;
            var callback = default(ImGuiInputTextCallback);
            var user_data = (void*) null;

            PushStyleColor(ImGuiCol.FrameBg, in Zero4);
            var result = JmGuiNative.igInputTextMultiline(ref MemoryMarshal.GetReference(label), ref MemoryMarshal.GetReference(input), (uint) buf_size, size, flags, callback, user_data);
            ImGui.PopStyleColor();
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Text(bool fmt)
        {
            const string @true = "true";
            const string @false = "false";
            ImGui.Text(fmt ? @true : @false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Text(string value)
        {
            ImGui.Text(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Text(ReadOnlySpan<byte> fmt)
        {
            JmGuiNative.igText(ref MemoryMarshal.GetReference(fmt));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextSameLine(int value)
        {
            Span<byte> valueUtf8 = stackalloc byte[sizeof(int) * sizeof(char) + 1];
            Utf8Formatter.TryFormat(value, valueUtf8, out _);
            valueUtf8[valueUtf8.Length - 1] = 0;
            Text(valueUtf8);
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextSameLine(ref readonly DateTimeOffset value)
        {
            Span<byte> valueUtf8 = stackalloc byte[64];
            Utf8Formatter.TryFormat(value, valueUtf8, out var written, new StandardFormat('O'));
            valueUtf8[written + 1] = 0;
            Text(valueUtf8);
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextSameLine(string value)
        {
            ImGui.Text(value);
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextSameLine(ReadOnlySpan<byte> fmt)
        {
            JmGuiNative.igText(ref MemoryMarshal.GetReference(fmt));
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextSameLine(ref readonly byte fmt)
        {
            JmGuiNative.igText(in fmt);
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextWrapped(ref readonly ReadOnlySpan<byte> fmt)
        {
            JmGuiNative.igTextWrapped(ref MemoryMarshal.GetReference(fmt));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextColored(ref readonly Vector4 col, ReadOnlySpan<byte> fmt)
        {
            JmGuiNative.igTextColored(col, ref MemoryMarshal.GetReference(fmt));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextColoredSameLine(ref readonly Vector4 col, ReadOnlySpan<byte> fmt)
        {
            JmGuiNative.igTextColored(col, ref MemoryMarshal.GetReference(fmt));
            ImGui.SameLine(0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextColoredSameLine(ref readonly Vector4 col, string fmt)
        {
            Span<byte> fmtUtf8 = stackalloc byte[fmt.Length + 1];
            UnsafeHelper.Utf16ToUtf8(fmt.AsSpan(), fmtUtf8);
            fmtUtf8[fmtUtf8.Length - 1] = 0;
            TextColoredSameLine(in col, fmtUtf8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Checkbox(ReadOnlySpan<byte> label, ref bool v)
        {
            var num1 = (byte) (v ? 1 : 0);
            var result = JmGuiNative.igCheckbox(ref MemoryMarshal.GetReference(label), ref num1);
            v = num1 > 0;
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckboxSameLine(string label, ref bool v)
        {
            var result = ImGui.Checkbox(label, ref v);
            ImGui.SameLine(0, 0);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckboxSameLine(ReadOnlySpan<byte> label, ref bool v)
        {
            var num1 = (byte) (v ? 1 : 0);
            var result = JmGuiNative.igCheckbox(ref MemoryMarshal.GetReference(label), ref num1);
            ImGui.SameLine(0, 0);
            v = num1 > 0;
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Button(ReadOnlySpan<byte> label)
        {
            var result = JmGuiNative.igButton(ref MemoryMarshal.GetReference(label), Zero2);
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SmallButton(ReadOnlySpan<byte> label)
        {
            var result = JmGuiNative.igSmallButton(ref MemoryMarshal.GetReference(label));
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SmallButton(string label)
        {
            Span<byte> labelUtf8 = stackalloc byte[label.Length + 1];
            UnsafeHelper.Utf16ToUtf8(label.AsSpan(), labelUtf8);
            labelUtf8[labelUtf8.Length - 1] = 0;
            return SmallButton(labelUtf8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SmallButtonSameLine(string label)
        {
            var result = ImGui.SmallButton(label);
            ImGui.SameLine(0, 0);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Begin(ReadOnlySpan<byte> name, ImGuiWindowFlags flags)
        {
            ref readonly var p_open = ref UnsafeHelper.NullRef<byte>();
            var result = JmGuiNative.igBegin(ref MemoryMarshal.GetReference(name), in p_open, flags);
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BeginTable(ReadOnlySpan<byte> str_id, int column)
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.None;
            const float inner_width = 0.0f;
            var result = JmGuiNative.igBeginTable(ref MemoryMarshal.GetReference(str_id), column, flags, Zero2, inner_width);
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BeginChild(ReadOnlySpan<byte> str_id, ref readonly Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags)
        {
            var result = JmGuiNative.igBeginChild_Str(ref MemoryMarshal.GetReference(str_id), size, child_flags, window_flags);
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BeginChild(string label, ref readonly Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags)
        {
            Span<byte> labelUtf8 = stackalloc byte[label.Length + 1];
            UnsafeHelper.Utf16ToUtf8(label.AsSpan(), labelUtf8);
            labelUtf8[labelUtf8.Length - 1] = 0;
            return BeginChild(labelUtf8, in size, child_flags, window_flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TreeNode(ReadOnlySpan<byte> label)
        {
            var result = JmGuiNative.igTreeNode_Str(ref MemoryMarshal.GetReference(label));
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TreeNode(string label)
        {
            Span<byte> labelUtf8 = stackalloc byte[label.Length + 1];
            UnsafeHelper.Utf16ToUtf8(label.AsSpan(), labelUtf8);
            labelUtf8[labelUtf8.Length - 1] = 0;
            return TreeNode(labelUtf8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TreeNode(ReadOnlySpan<byte> label, ImGuiTreeNodeFlags flags)
        {
            var result = JmGuiNative.igTreeNodeEx_Str(ref MemoryMarshal.GetReference(label), flags);
            return (bool) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TreeNode(string label, ImGuiTreeNodeFlags flags)
        {
            Span<byte> labelUtf8 = stackalloc byte[label.Length + 1];
            UnsafeHelper.Utf16ToUtf8(label.AsSpan(), labelUtf8);
            labelUtf8[labelUtf8.Length - 1] = 0;
            return TreeNode(labelUtf8, flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushStyleColor(ImGuiCol idx, ref readonly Vector4 col)
        {
            JmGuiNative.igPushStyleColor_Vec4(idx, col);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetNextWindowPos(ref readonly Vector2 pos)
        {
            const ImGuiCond cond = ImGuiCond.None;
            JmGuiNative.igSetNextWindowPos(pos, cond, in Zero2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetNextWindowSize(ref readonly Vector2 size)
        {
            const ImGuiCond cond = ImGuiCond.None;
            JmGuiNative.igSetNextWindowSize(size, cond);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetNextWindowViewport(uint viewport_id)
        {
            ImGuiNative.igSetNextWindowViewport(viewport_id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiViewportPtr GetMainViewport() => new(in JmGuiNative.igGetMainViewport());


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TreePop() => ImGuiNative.igTreePop();

        public static void NewLine() => ImGuiNative.igNewLine();
    }
}