using BUTR.CrashReport.Models;

namespace Bannerlord.ButterLib.CrashReportWindow.Extensions;

internal static class AssemblyModelTypeExtensions
{
    public static bool IsSet(this AssemblyModelType self, AssemblyModelType flag) => (self & flag) == flag;
}