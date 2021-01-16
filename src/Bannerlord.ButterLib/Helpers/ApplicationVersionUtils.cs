using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class for handling the game version.</summary>
    public static class ApplicationVersionUtils
    {
        private static readonly MethodInfo? GetVersionStrMethod = AccessTools.DeclaredMethod(typeof(Managed), "GetVersionStr");


        /// <summary>Gets the game version as an <see cref="ApplicationVersion" /> struct.</summary>
        /// <returns>
        /// <see cref="ApplicationVersion" /> struct representation of the game version
        /// if it was successfully  obtained and parsed; otherwise <see langword="null" />.
        /// </returns>
        public static ApplicationVersion? GameVersion() => TryParse(GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;

        /// <summary>Gets the version of the game as string.</summary>
        /// <returns>A string representation of the game version.</returns>
        public static string GameVersionStr()
        {
            if (GetVersionStrMethod is null)
                return "e1.0.0";

            var @params = GetVersionStrMethod.GetParameters();
            if (@params.Length == 0)
                return GetVersionStrMethod.Invoke(null, Array.Empty<object>()) as string ?? "e1.0.0";
            if (@params.Length == 1 && @params[0].ParameterType == typeof(string))
                return GetVersionStrMethod.Invoke(null, new object[] { "Singleplayer" }) as string ?? "e1.0.0";

            return "e1.0.0";
        }

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