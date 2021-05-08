using Bannerlord.BUTR.Shared.Helpers;

using System;

using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class for handling the game version.</summary>
    [Obsolete("Use Bannerlord.BUTR.Shared ApplicationVersionHelper", true)]
    public static class ApplicationVersionUtils
    {
        /// <summary>Gets the game version as an <see cref="ApplicationVersion" /> struct.</summary>
        /// <returns>
        /// <see cref="ApplicationVersion" /> struct representation of the game version
        /// if it was successfully  obtained and parsed; otherwise <see langword="null" />.
        /// </returns>
        public static ApplicationVersion? GameVersion() => ApplicationVersionHelper.GameVersion();

        /// <summary>Gets the version of the game as string.</summary>
        /// <returns>A string representation of the game version.</returns>
        public static string GameVersionStr() => ApplicationVersionHelper.GameVersionStr();

        /// <summary>
        /// Converts string representation of the game version to an <see cref="ApplicationVersion" /> struct.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="versionAsString">A string containing a version to convert.</param>
        /// <param name="version">
        /// When this method returns, contains the <see cref="ApplicationVersion" /> struct equivalent
        /// of the version contained in <paramref name="versionAsString" />, if the conversion succeeded
        /// or <see langword="default" /> if the conversion failed.
        /// </param>
        /// <returns><see langword="true" /> if <paramref name="versionAsString" /> was converted successfully; otherwise <see langword="false" />.</returns>
        public static bool TryParse(string? versionAsString, out ApplicationVersion version) => ApplicationVersionHelper.TryParse(versionAsString, out version);
    }
}