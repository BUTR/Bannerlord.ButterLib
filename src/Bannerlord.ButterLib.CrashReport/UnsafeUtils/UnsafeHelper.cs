using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

internal static unsafe class UnsafeHelper
{
    public static ref readonly T NullRef<T>() where T : unmanaged => ref *(T*) null;

    public static Span<T> CreateSpan<T>(ref T reference, int length) => new(Unsafe.AsPointer(ref reference), length);

    public static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(ref readonly byte value)
    {
        ref var valueRef = ref Unsafe.AsRef(in value);

        if (Unsafe.IsNullRef(ref valueRef))
            return new ReadOnlySpan<byte>();

#if NET6_0_OR_GREATER
        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*) Unsafe.AsPointer(ref valueRef));
#endif

        var valuePtr = (byte*) Unsafe.AsPointer(ref valueRef);

        /*
        var i = 0;
        var b = valuePtr[i];
        while (b != 0)
        {

            b = valuePtr[i];
        }
        if (b == 0)
            return new ReadOnlySpan<byte>(valuePtr, i);
        */
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
        return string.Create(utf8.Length, (IntPtr) Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8)), static (utf16, utf8Ptr) =>
        {
            var utf8 = new ReadOnlySpan<byte>((byte*)utf8Ptr, utf16.Length);
            System.Text.Unicode.Utf8.ToUtf16(utf8, utf16, out _, out _);
        });
#endif

        return Encoding.UTF8.GetString((byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8)), utf8.Length);
    }

    public static void Utf16ToUtf8(ReadOnlySpan<char> utf16, Span<byte> utf8)
    {
#if NET6_0_OR_GREATER
        System.Text.Unicode.Utf8.FromUtf16(utf16, utf8, out _, out _);
        return;
#endif

        Encoding.UTF8.GetBytes((char*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf16)), utf16.Length, (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8)), utf8.Length);
    }

    public static byte[] ToUtf8Array(string value)
    {
        var length = Encoding.UTF8.GetByteCount(value) + 1;
        var array = new byte[length];

        var charSpan = value.AsSpan();
        var arraySpan = array.AsSpan();

        Utf16ToUtf8(charSpan, arraySpan);
        array[length - 1] = 0;
        return array;
    }
}