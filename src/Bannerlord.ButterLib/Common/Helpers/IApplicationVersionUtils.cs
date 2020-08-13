using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    internal interface IApplicationVersionUtils
    {
        string GameVersionStr();

        bool TryParse(string? versionAsString, out ApplicationVersion version);
    }
}