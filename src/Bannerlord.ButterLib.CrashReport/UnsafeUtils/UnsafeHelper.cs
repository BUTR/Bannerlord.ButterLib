using HarmonyLib.BUTR.Extensions;

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

internal static unsafe class UnsafeHelper
{
    private delegate int IndexOfSpanDelegate(ref byte searchSpace, byte value, int length);
    private static readonly IndexOfSpanDelegate? _indexOfSpan =
        AccessTools2.GetDelegate<IndexOfSpanDelegate>("System.SpanHelpers:IndexOf", [typeof(byte).MakeByRefType(), typeof(byte), typeof(int)]);

    public static ref readonly T NullRef<T>() where T : unmanaged => ref *(T*) null;

    public static Span<T> CreateSpan<T>(ref T reference, int length) => new(Unsafe.AsPointer(ref reference), length);

    public static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(ref readonly byte valueRef)
    {
        return CreateReadOnlySpanFromNullTerminated((byte*) Unsafe.AsPointer(ref Unsafe.AsRef(in valueRef)));
    }

    public static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(byte* valuePtr)
    {
        if (valuePtr is null)
            return new ReadOnlySpan<byte>();

        if (_indexOfSpan is not null)
            return new ReadOnlySpan<byte>(valuePtr, _indexOfSpan(ref *valuePtr, 0, int.MaxValue));

#if NET6_0_OR_GREATER
        return System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpanFromNullTerminated(valuePtr);
#endif

        for (var i = 0; i < 512; i++)
        {
            var b = valuePtr[i];
            if (b == 0)
                return new ReadOnlySpan<byte>(valuePtr, i);
        }

        return new ReadOnlySpan<byte>();
    }

    public static string ToString(ReadOnlySpan<byte> utf8)
    {
#if NET6_0_OR_GREATER
        fixed (byte* utf8Ptr = utf8)
        {
            return string.Create(utf8.Length, (IntPtr) utf8Ptr, static (utf16, utf8Ptr) =>
            {
                var utf8 = new ReadOnlySpan<byte>((byte*) utf8Ptr, utf16.Length);
                System.Text.Unicode.Utf8.ToUtf16(utf8, utf16, out _, out _);
            });
        }
#endif

        fixed (byte* utf8Ptr = utf8)
        {
            return Encoding.UTF8.GetString(utf8Ptr, utf8.Length);
        }
    }

    public static int Utf16ToUtf8(string utf16, Span<byte> utf8)
    {
        if (string.IsNullOrEmpty(utf16) || utf8.IsEmpty)
            return 0;

#if NET6_0_OR_GREATER
        System.Text.Unicode.Utf8.FromUtf16(utf16, utf8, out _, out var bytesWritten);
        return bytesWritten;
#endif

        fixed (char* utf16Ptr = utf16)
        fixed (byte* utf8Ptr = utf8)
        {
            return Encoding.UTF8.GetBytes(utf16Ptr, utf16.Length, utf8Ptr, utf8.Length);
        }
    }

    public static int Utf16ToUtf8(ReadOnlySpan<char> utf16, Span<byte> utf8)
    {
        if (utf16.IsEmpty || utf8.IsEmpty)
            return 0;

#if NET6_0_OR_GREATER
        System.Text.Unicode.Utf8.FromUtf16(utf16, utf8, out _, out var bytesWritten);
        return bytesWritten;
#endif

        fixed (char* utf16Ptr = utf16)
        fixed (byte* utf8Ptr = utf8)
        {
            return Encoding.UTF8.GetBytes(utf16Ptr, utf16.Length, utf8Ptr, utf8.Length);
        }
    }

    public static byte[] ToUtf8Array(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Array.Empty<byte>();

        var length = Encoding.UTF8.GetMaxByteCount(value.Length) + 1;
        var array = new byte[length];

        var charSpan = value.AsSpan();
        var arraySpan = array.AsSpan();

        var lengthWritten = Utf16ToUtf8(charSpan, arraySpan);
        array[lengthWritten] = 0;
        return array;
    }
}