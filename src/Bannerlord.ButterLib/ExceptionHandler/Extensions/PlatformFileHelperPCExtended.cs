using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.ExceptionHandler.Extensions;

// TODO: What to with Xbox version?
internal static class PlatformFileHelperPCExtended
{
    private delegate string GetDirectoryFullPathDelegate(object instance, PlatformDirectoryPath directoryPath);
    private static readonly GetDirectoryFullPathDelegate? GetDirectoryFullPathMethod =
        AccessTools2.GetDelegate<GetDirectoryFullPathDelegate>("TaleWorlds.Library.PlatformFileHelperPC:GetDirectoryFullPath");

    private delegate string GetFileFullPathDelegate(object instance, PlatformFilePath filePath);
    private static readonly GetFileFullPathDelegate? GetFileFullPathMethod =
        AccessTools2.GetDelegate<GetFileFullPathDelegate>("TaleWorlds.Library.PlatformFileHelperPC:GetFileFullPath");

    private delegate object GetPlatformFileHelperDelegate();
    private static readonly GetPlatformFileHelperDelegate? GetPlatformFileHelper =
        AccessTools2.GetPropertyGetterDelegate<GetPlatformFileHelperDelegate>("TaleWorlds.Library.Common:PlatformFileHelper");


    public static string? GetFileFullPath(PlatformFilePath filePath) =>
        GetPlatformFileHelper is not null && GetFileFullPathMethod is not null && GetPlatformFileHelper() is { } obj
            ? GetFileFullPathMethod(obj, filePath)
            : null;

    public static string? GetDirectoryFullPath(PlatformDirectoryPath directoryPath) =>
        GetPlatformFileHelper is not null && GetDirectoryFullPathMethod is not null && GetPlatformFileHelper() is { } obj
            ? GetDirectoryFullPathMethod(obj, directoryPath)
            : null;
}