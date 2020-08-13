using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class for handling the game version.</summary>
    public static class ApplicationVersionUtils
    {
        internal static IApplicationVersionUtils? _instance;
        internal static IApplicationVersionUtils Instance
        {
            get
            {
                if (_instance == null)
                    DI.TryGetImplementation(out _instance);
                return _instance!;
            }
        }


        /// <summary>Gets the version of the game as string.</summary>
        /// <returns>A string representation of the game version.</returns>
        public static string GameVersionStr() => Instance.GameVersionStr();

        /// <summary>
        /// Converts string representation of the game version to an <see cref="ApplicationVersion" /> struct.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="versionAsString">A string containing a version to convert.</param>
        /// <param name="version">
        /// When this method returns, contains the <see cref="ApplicationVersion" /> struct equivalent
        /// of the verion contained in <paramref name="versionAsString" />, if the conversion succeeded
        /// or <see langword="default" /> if the conversion failed.
        /// </param>
        /// <returns><see langword="true" /> if <paramref name="versionAsString" /> was converted successfully; otherwise <see langword="false" />.</returns>
        public static bool TryParse(string? versionAsString, out ApplicationVersion version) => Instance.TryParse(versionAsString, out version);

        /// <summary>Gets the game version as an <see cref="ApplicationVersion" /> struct.</summary>
        /// <returns>
        /// <see cref="ApplicationVersion" /> struct representation of the game version
        /// if it was successfully  obtained and parsed; otherwise <see langword="null" />.
        /// </returns>
        public static ApplicationVersion? GameVersion() => TryParse(GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;
    }
}