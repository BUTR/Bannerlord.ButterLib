using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.ImGui;

internal partial class CmGui
{
    private const MethodImplOptions AggressiveOptimization = (MethodImplOptions) 512;

    private static readonly byte[] _padding = UnsafeHelper.ToUtf8Array(string.Empty.PadRight(64));
    private static readonly Vector2 Zero2 = Vector2.Zero;
    private static readonly Vector3 Zero3 = Vector3.Zero;
    private static readonly Vector4 Zero4 = Vector4.Zero;
}