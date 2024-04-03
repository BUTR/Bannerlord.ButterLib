using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using ImGuiNET;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

unsafe partial class CmGui
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TextSameLine(string value)
    {
        Text(value);
        SameLine(0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool BeginChild(string label, ref readonly Vector2 size, ref readonly Vector4 color, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags)
    {
        PushStyleVar(ImGuiStyleVar.ChildRounding, 5f);
        PushStyleColor(ImGuiCol.ChildBg, in color);
        var result = BeginChild(label, size, child_flags, window_flags);
        PopStyleColor();
        PopStyleVar();
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool TreeNode(string label, ImGuiTreeNodeFlags flags) => TreeNodeEx(label, flags);


    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Text(string value)
    {
        var utf8ByteCount = Encoding.UTF8.GetMaxByteCount(value.Length);
        var span = utf8ByteCount <= 2048 ? stackalloc byte[utf8ByteCount + 1] : new byte[utf8ByteCount + 1];
        var length = UnsafeHelper.Utf16ToUtf8(value, span);
        span[length] = 0;
        fixed (byte* ptr = span)
        {
            igText(ptr);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool SmallButton(string label)
    {
        PushStyleVar(ImGuiStyleVar.FrameRounding, 3f);

        var utf8ByteCount = Encoding.UTF8.GetMaxByteCount(label.Length);
        var span = utf8ByteCount <= 2048 ? stackalloc byte[utf8ByteCount + 1] : new byte[utf8ByteCount + 1];
        var length = UnsafeHelper.Utf16ToUtf8(label, span);
        span[length] = 0;
        fixed (byte* ptr = span)
        {
            var result = igSmallButton(ptr) > 0;
            PopStyleVar();
            return result;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool SmallButtonSameLine(string label)
    {
        PushStyleVar(ImGuiStyleVar.FrameRounding, 3f);
        var result = SmallButton(label);
        PopStyleVar();
        SameLine(0, 0);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool TreeNode(string label)
    {
        var utf8ByteCount = Encoding.UTF8.GetMaxByteCount(label.Length);
        var span = utf8ByteCount <= 2048 ? stackalloc byte[utf8ByteCount + 1] : new byte[utf8ByteCount + 1];
        var length = UnsafeHelper.Utf16ToUtf8(label, span);
        span[length] = 0;
        fixed (byte* ptr = span)
        {
            return igTreeNode_Str(ptr) > 0;
        }
    }

    public bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags)
    {
        var utf8ByteCount = Encoding.UTF8.GetMaxByteCount(label.Length);
        var span = utf8ByteCount <= 2048 ? stackalloc byte[utf8ByteCount + 1] : new byte[utf8ByteCount + 1];
        var length = UnsafeHelper.Utf16ToUtf8(label, span);
        span[length] = 0;
        fixed (byte* ptr = span)
        {
            return igTreeNodeEx_Str(ptr, flags) > 0;
        }
    }

    public bool BeginChild(string str_id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags)
    {
        var utf8ByteCount = Encoding.UTF8.GetMaxByteCount(str_id.Length);
        var span = utf8ByteCount <= 2048 ? stackalloc byte[utf8ByteCount + 1] : new byte[utf8ByteCount + 1];
        var length = UnsafeHelper.Utf16ToUtf8(str_id, span);
        span[length] = 0;
        fixed (byte* ptr = span)
        {
            return igBeginChild_Str(ptr, size, child_flags, window_flags) > 0;
        }
    }

    public bool InputTextMultiline(string label, ref string input, uint maxLength, Vector2 size, ImGuiInputTextFlags flags)
    {
        return ImGuiNET.ImGui.InputTextMultiline(label, ref input, maxLength, size, flags, null, IntPtr.Zero);
    }

    /*
    public unsafe bool InputTextMultiline(string label, ref string input, uint maxLength, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
    {
        int byteCount1 = Encoding.UTF8.GetByteCount(label);
        byte* numPtr1 = byteCount1 <= 2048 ? stackalloc byte[byteCount1 + 1] : Util.Allocate(byteCount1 + 1);
        Util.GetUtf8(label, numPtr1, byteCount1);
        int byteCount2 = Encoding.UTF8.GetByteCount(input);
        int num1 = Math.Max((int) maxLength + 1, byteCount2 + 1);
        byte* numPtr2;
        byte* numPtr3;
        if (num1 > 2048)
        {
            numPtr2 = Util.Allocate(num1);
            numPtr3 = Util.Allocate(num1);
        }
        else
        {
            numPtr2 = stackalloc byte[num1];
            numPtr3 = stackalloc byte[num1];
        }
        Util.GetUtf8(input, numPtr2, num1);
        uint byteCount3 = (uint) (num1 - byteCount2);
        Unsafe.InitBlockUnaligned((void*) (numPtr2 + byteCount2), (byte) 0, byteCount3);
        Unsafe.CopyBlock((void*) numPtr3, (void*) numPtr2, (uint) num1);
        byte num2 = igInputTextMultiline(numPtr1, numPtr2, (uint) num1, size, flags, callback, user_data.ToPointer());
        if (!Util.AreStringsEqual(numPtr3, num1, numPtr2))
            input = Util.StringFromPtr(numPtr2);
        if (byteCount1 > 2048)
            Util.Free(numPtr1);
        if (num1 > 2048)
        {
            Util.Free(numPtr2);
            Util.Free(numPtr3);
        }
        return num2 > (byte) 0;
    }
    */

    public bool InputText(string label, ref string input, uint maxLength, ImGuiInputTextFlags flags)
    {
        return ImGuiNET.ImGui.InputText(label, ref input, maxLength, flags, null, IntPtr.Zero);
    }

    /*
    public unsafe bool InputText(string label, ref string input, uint maxLength, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
    {
        int byteCount1 = Encoding.UTF8.GetByteCount(label);
        var numPtr1 = byteCount1 <= 2048 ? stackalloc byte[byteCount1 + 1] : new byte[byteCount1 + 1];
        Util.GetUtf8(label, numPtr1, byteCount1);
        int byteCount2 = Encoding.UTF8.GetByteCount(input);
        int num1 = Math.Max((int) maxLength + 1, byteCount2 + 1);
        byte* numPtr2;
        byte* numPtr3;
        if (num1 > 2048)
        {
            numPtr2 = Util.Allocate(num1);
            numPtr3 = Util.Allocate(num1);
        }
        else
        {
            numPtr2 = stackalloc byte[num1];
            numPtr3 = stackalloc byte[num1];
        }
        Util.GetUtf8(input, numPtr2, num1);
        uint byteCount3 = (uint) (num1 - byteCount2);
        Unsafe.InitBlockUnaligned((void*) (numPtr2 + byteCount2), (byte) 0, byteCount3);
        Unsafe.CopyBlock((void*) numPtr3, (void*) numPtr2, (uint) num1);
        byte num2 = igInputText(numPtr1, numPtr2, (uint) num1, flags, callback, user_data.ToPointer());
        if (!Util.AreStringsEqual(numPtr3, num1, numPtr2))
            input = Util.StringFromPtr(numPtr2);
        if (byteCount1 > 2048)
            Util.Free(numPtr1);
        if (num1 > 2048)
        {
            Util.Free(numPtr2);
            Util.Free(numPtr3);
        }
        return num2 > (byte) 0;
    }
    */
}