using BUTR.CrashReport.Models;

using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Extensions;

internal static class AssemblyModelTypeExtensions
{
    private const MethodImplOptions AggressiveOptimization = (MethodImplOptions) 512;

    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public static bool IsSet(this AssemblyModelType self, AssemblyModelType flag) => (self & flag) == flag;
}