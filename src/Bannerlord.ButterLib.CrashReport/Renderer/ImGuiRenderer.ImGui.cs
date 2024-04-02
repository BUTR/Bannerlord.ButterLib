using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Renderer;

partial class ImGuiRenderer
{
    private static readonly Vector2 Zero2 = Vector2.Zero;
    private static readonly Vector3 Zero3 = Vector3.Zero;
    private static readonly Vector4 Zero4 = Vector4.Zero;
    private static readonly Vector4 Black = FromColor(0, 0, 0, 255);
    private static readonly Vector4 White = FromColor(255, 255, 255, 255);
    private static readonly Vector4 Red = FromColor(255, 0, 0, 255);
    private static readonly Vector4 Orange = FromColor(255, 165, 0, 255);

    private static readonly Vector4 Background = FromColor(236, 236, 236, 255);
    private static readonly Vector4 Plugin = FromColor(255, 255, 224, 255);
    private static readonly Vector4 OfficialModule = FromColor(244, 252, 220, 255);
    private static readonly Vector4 UnofficialModule = FromColor(255, 255, 224, 255);
    private static readonly Vector4 ExternalModule = FromColor(255, 255, 224, 255);
    private static readonly Vector4 SubModule = FromColor(248, 248, 231, 255);

    private static readonly Vector4 Primary = FromColor(151, 129, 082, 135);
    private static readonly Vector4 Primary2 = FromColor(151, 129, 082, 180);
    private static readonly Vector4 Primary3 = FromColor(151, 129, 082, 210);
    private static readonly Vector4 Secondary = FromColor(255, 230, 128, 135);
    private static readonly Vector4 Secondary2 = FromColor(255, 230, 128, 180);
    private static readonly Vector4 Secondary3 = FromColor(255, 230, 128, 210);
    //private static readonly Vector4 PrimaryActive = FromColor(174, 137, 59, 180);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector4 FromColor(byte r, byte g, byte b, byte a) => new((float) r / 255f, (float) g / 255f, (float) b / 255f, (float) a / 255f);
}