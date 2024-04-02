using ImGuiNET;

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

unsafe partial class CmGui
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization | AggressiveOptimization)]
    public void RenderId(ReadOnlySpan<byte> title, string id)
    {
        Text(title);
        SameLine();
        SmallButton(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool InputTextMultiline(ReadOnlySpan<byte> label, Span<byte> input, int lineCount)
    {
        var buf_size = input.Length;
        var size = new Vector2(-1, GetTextLineHeight() * (lineCount + 1));
        var flags = ImGuiInputTextFlags.ReadOnly;
        var callback = (ImGuiInputTextCallback?) null;
        var user_data = (void*) null;

        PushStyleColor(ImGuiCol.FrameBg, in Zero4);
        fixed (byte* labelPtr = label)
        fixed (byte* inputPtr = input)
        {
            var result = igInputTextMultiline(labelPtr, inputPtr, Unsafe.As<int, uint>(ref buf_size), size, Unsafe.As<ImGuiInputTextFlags, int>(ref flags), callback, user_data);
            PopStyleColor();
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Text(bool fmt)
    {
        var @true = "true"u8;
        var @false = "false"u8;
        Text(fmt ? @true : @false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Text(ReadOnlySpan<byte> fmt)
    {
        fixed (byte* fmtPtr = fmt)
        {
            igText(fmtPtr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextSameLine(int value)
    {
        Span<byte> valueUtf8 = stackalloc byte[sizeof(int) * sizeof(char) + 1];
        Utf8Formatter.TryFormat(value, valueUtf8, out _);
        valueUtf8[valueUtf8.Length - 1] = 0;
        Text(valueUtf8);
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextSameLine(ref readonly DateTimeOffset value)
    {
        Span<byte> valueUtf8 = stackalloc byte[64];
        Utf8Formatter.TryFormat(value, valueUtf8, out var written, new StandardFormat('O'));
        valueUtf8[written + 1] = 0;
        Text(valueUtf8);
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextSameLine(ReadOnlySpan<byte> fmt)
    {
        fixed (byte* fmtPtr = fmt)
        {
            igText(fmtPtr);
        }
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PadRight(int toAppend)
    {
        fixed (byte* paddingPtr = _padding)
        {
            var offset = _padding.Length - toAppend;
            var paddingPtrWithOffset = (byte*) Unsafe.Add<byte>(paddingPtr, offset);
            igText(paddingPtrWithOffset);
            SameLine(0, 0);
        }
    }
    public void TextSameLine(ref readonly byte fmt)
    {
        fixed (byte* fmtPtr = &fmt)
        {
            igText(fmtPtr);
        }
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextWrapped(ReadOnlySpan<byte> fmt)
    {
        fixed (byte* fmtPtr = fmt)
        {
            igTextWrapped(fmtPtr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextColored(ref readonly Vector4 col, ReadOnlySpan<byte> fmt)
    {
        fixed (byte* fmtPtr = fmt)
        {
            igTextColored(col, fmtPtr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextColoredSameLine(ref readonly Vector4 col, ReadOnlySpan<byte> fmt)
    {
        fixed (byte* fmtPtr = fmt)
        {
            igTextColored(col, fmtPtr);
        }
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool Checkbox(ReadOnlySpan<byte> label, ref bool v)
    {
        PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1f);
        PushStyleVar(ImGuiStyleVar.FrameRounding, 3f);
        var num1 = (byte) (v ? 1 : 0);
        fixed (byte* labelPtr = label)
        {
            var result = igCheckbox(labelPtr, &num1);
            PopStyleVar(2);
            v = num1 > 0;
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool CheckboxSameLine(ReadOnlySpan<byte> label, ref bool v)
    {
        PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1f);
        PushStyleVar(ImGuiStyleVar.FrameRounding, 3f);
        var num1 = (byte) (v ? 1 : 0);
        fixed (byte* labelPtr = label)
        {
            var result = igCheckbox(labelPtr, &num1);
            PopStyleVar(2);
            SameLine(0, 0);
            v = num1 > 0;
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool Button(ReadOnlySpan<byte> label)
    {
        fixed (byte* labelPtr = label)
        {
            var result = igButton(labelPtr, Zero2);
            return result > 0;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool Button(ReadOnlySpan<byte> label, ref readonly Vector4 color, ref readonly Vector4 hovered, ref readonly Vector4 active)
    {
        PushStyleColor(ImGuiCol.Button, in color);
        PushStyleColor(ImGuiCol.ButtonHovered, in hovered);
        PushStyleColor(ImGuiCol.ButtonActive, in active);
        fixed (byte* labelPtr = label)
        {
            var result = igButton(labelPtr, Zero2);
            PopStyleColor(3);
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool SmallButton(ReadOnlySpan<byte> label)
    {
        PushStyleVar(ImGuiStyleVar.FrameRounding, 3f);
        fixed (byte* labelPtr = label)
        {
            var result = igSmallButton(labelPtr);
            PopStyleVar();
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool Begin(ReadOnlySpan<byte> name, ref readonly Vector4 color, ImGuiWindowFlags flags)
    {
        PushStyleColor(ImGuiCol.WindowBg, in color);
        var p_open = (byte*) null;
        fixed (byte* namePtr = name)
        {
            var result = igBegin(namePtr, p_open, Unsafe.As<ImGuiWindowFlags, int>(ref flags));
            PopStyleColor();
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool BeginTable(ReadOnlySpan<byte> strId, int column)
    {
        var flags = ImGuiTableFlags.None;
        const float inner_width = 0.0f;
        fixed (byte* strIdPtr = strId)
        {
            var result = igBeginTable(strIdPtr, column, Unsafe.As<ImGuiTableFlags, int>(ref flags), Zero2, inner_width);
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool BeginChild(ReadOnlySpan<byte> strId, ref readonly Vector2 size, ref readonly Vector4 color, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags)
    {
        PushStyleVar(ImGuiStyleVar.ChildRounding, 5f);
        PushStyleColor(ImGuiCol.ChildBg, in color);
        fixed (byte* strIdPtr = strId)
        {
            var result = igBeginChild_Str(strIdPtr, size, Unsafe.As<ImGuiChildFlags, int>(ref child_flags), Unsafe.As<ImGuiWindowFlags, int>(ref window_flags));
            PopStyleColor();
            PopStyleVar();
            return result > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool TreeNode(ReadOnlySpan<byte> label)
    {
        fixed (byte* labelPtr = label)
        {
            return igTreeNode_Str(labelPtr) > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool TreeNode(ReadOnlySpan<byte> label, ImGuiTreeNodeFlags flags)
    {
        fixed (byte* labelPtr = label)
        {
            return igTreeNodeEx_Str(labelPtr, Unsafe.As<ImGuiTreeNodeFlags, int>(ref flags)) > 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PushStyleColor(ImGuiCol idx, ref readonly Vector4 col) => igPushStyleColor_Vec4((int) idx, col);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetNextWindowPos(ref readonly Vector2 pos) => igSetNextWindowPos(pos, (int) ImGuiCond.None, Zero2);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetNextWindowSize(ref readonly Vector2 size) => igSetNextWindowSize(size, (int) ImGuiCond.None);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetNextWindowViewport(uint viewport_id) => igSetNextWindowViewport(viewport_id);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ImGuiViewportPtr GetMainViewport() => new(igGetMainViewport());


    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TreePop() => igTreePop();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void NewLine() => igNewLine();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void StyleColorsLight() => igStyleColorsLight(null);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void StyleColorsDark() => igStyleColorsDark(null);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ImGuiStylePtr GetStyle() => new(igGetStyle());

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetWindowFontScale(float scale) => igSetWindowFontScale(scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void EndChild() => igEndChild();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void End() => igEnd();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool TableNextColumn() => igTableNextColumn() > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SameLine() => igSameLine(0.0f, -1f);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void EndTable() => igEndTable();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Separator() => igSeparator();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PushStyleVar(ImGuiStyleVar idx, float val) => igPushStyleVar_Float(Unsafe.As<ImGuiStyleVar, int>(ref idx), val);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Bullet() => igBullet();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Indent() => igIndent(0.0f);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Unindent() => igUnindent(0.0f);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SameLine(float offset_from_start_x, float spacing) => igSameLine(offset_from_start_x, spacing);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PopStyleVar() => igPopStyleVar(1);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PopStyleVar(int count) => igPopStyleVar(count);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PopStyleColor() => igPopStyleColor(1);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PopStyleColor(int count) => igPopStyleColor(count);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public float GetTextLineHeight() => igGetTextLineHeight();




    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr CreateContext() => igCreateContext(null);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public IntPtr GetCurrentContext() => igGetCurrentContext();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void SetCurrentContext(IntPtr ctx) => igSetCurrentContext(ctx);

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ImGuiIOPtr GetIO() => new(this, igGetIO());

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Render() => igRender();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ImDrawDataPtr GetDrawData() => new(igGetDrawData());

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void NewFrame() => igNewFrame();

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public ImGuiMouseCursor GetMouseCursor()
    {
        var result = igGetMouseCursor();
        return Unsafe.As<int, ImGuiMouseCursor>(ref result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DestroyContext(IntPtr ctx) => igDestroyContext(ctx);
}