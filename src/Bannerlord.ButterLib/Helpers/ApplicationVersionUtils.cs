using System;

using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use Bannerlord.BUTR.Shared ApplicationVersionHelper", true)]
    public static class ApplicationVersionUtils
    {
        public static ApplicationVersion? GameVersion() => null;

        public static string GameVersionStr() => string.Empty;

        public static bool TryParse(string? versionAsString, out ApplicationVersion version)
        {
            version = default;
            return false;
        }
    }
}