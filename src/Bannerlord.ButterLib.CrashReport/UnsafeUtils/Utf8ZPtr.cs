using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

/// <summary>
/// Null-terminated UTF-8 string pointer.
/// </summary>
internal readonly struct Utf8ZPtr
{
    public static Span<Utf8ZPtr> AsSpan(ref Utf8ZPtr reference, int length) => UnsafeHelper.CreateSpan(ref reference, length);
    public static ref readonly Utf8ZPtr AsRef(ReadOnlySpan<Utf8ZPtr> stackallocArray) => ref MemoryMarshal.GetReference(stackallocArray);


    private readonly IntPtr NativePtr;
    public unsafe ref readonly byte Data => ref Unsafe.AsRef<byte>(NativePtr.ToPointer());

    public unsafe Utf8ZPtr(ref byte reference) { NativePtr = new IntPtr(Unsafe.AsPointer(ref reference)); }
    public Utf8ZPtr(ref readonly ReadOnlySpan<byte> span) : this(ref MemoryMarshal.GetReference(span)) { }
    public Utf8ZPtr(ref readonly Span<byte> span) : this(ref MemoryMarshal.GetReference(span)) { }
    public Utf8ZPtr(ref readonly byte[] array) : this(ref MemoryMarshal.GetReference(array.AsSpan())) { }

    public unsafe ReadOnlySpan<byte> AsUtf8(int length = -1) =>
        length != -1 ? new ReadOnlySpan<byte>(NativePtr.ToPointer(), length) : UnsafeHelper.CreateReadOnlySpanFromNullTerminated(in Data);

    public string AsUtf16String() => UnsafeHelper.ToString(AsUtf8());
}