using HarmonyLib;

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class for handling the game version.</summary>
    public static class ApplicationVersionUtils
    {
        private static readonly MethodInfo? GetVersionStrMethod = AccessTools.Method(typeof(Managed), "GetVersionStr");

        /// <summary>Gets the version of the game as string.</summary>
        /// <returns>A string representation of the game version.</returns>
        public static string GameVersionStr()
        {
            if (GetVersionStrMethod == null)
                return "e1.0.0";

            var @params = GetVersionStrMethod.GetParameters();
            if (!@params.Any())
                return GetVersionStrMethod.Invoke(null, Array.Empty<object>()) as string ?? "e1.0.0";
            if (@params.Length == 1 && @params[0].ParameterType == typeof(string))
                return GetVersionStrMethod.Invoke(null, new object[] { "Singleplayer" }) as string ?? "e1.0.0";

            return "e1.0.0";
        }

        private static ConstructorInfo? ApplicationVersionConstructorV1 { get; } = AccessTools.Constructor(typeof(ApplicationVersion), new[]
        {
            typeof(ApplicationVersionType),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int)
        });

        private static ConstructorInfo? ApplicationVersionConstructorV2 { get; } = AccessTools.Constructor(typeof(ApplicationVersion), new []
        {
            typeof(ApplicationVersionType),
            typeof(int),
            typeof(int),
            typeof(int)
        });
        private static PropertyInfo? ApplicationVersionTypeProperty { get; } =
            AccessTools.Property(typeof(ApplicationVersion), "ApplicationVersionType");
        private static PropertyInfo? MajorProperty { get; } =
            AccessTools.Property(typeof(ApplicationVersion), "Major");
        private static PropertyInfo? MinorProperty { get; } =
            AccessTools.Property(typeof(ApplicationVersion), "Minor");
        private static PropertyInfo? RevisionProperty { get; } =
            AccessTools.Property(typeof(ApplicationVersion), "Revision");

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
        public static bool TryParse(string? versionAsString, out ApplicationVersion version)
        {
            version = default;
            if (versionAsString is null)
                return false;

            var array = versionAsString.Split('.');
            if (array.Length != 3 && array.Length != 4)
                return false;

            var applicationVersionType = ApplicationVersion.ApplicationVersionTypeFromString(array[0][0].ToString());
            if (!int.TryParse(array[0].Substring(1), out var major))
                return false;
            if (!int.TryParse(array[1], out var minor))
                return false;
            if (!int.TryParse(array[2], out var revision))
                return false;

            version = Create(applicationVersionType, major, minor, revision);
            return true;
        }

        /// <summary>
        /// e1.0.11 didn't had ChangeSet.
        /// It may be an overkill for such a minor game version, but why not.
        /// </summary>
        private static ApplicationVersion Create(ApplicationVersionType applicationVersionType, int major, int minor, int revision)
        {
            if (ApplicationVersionConstructorV1 != null)
                return (ApplicationVersion) ApplicationVersionConstructorV1.Invoke(new object[] { applicationVersionType, major, minor, revision, 0 });

            if (ApplicationVersionConstructorV2 != null)
                return (ApplicationVersion) ApplicationVersionConstructorV2.Invoke(new object[] { applicationVersionType, major, minor, revision });

            // Fallback
            var boxedVersion = FormatterServices.GetUninitializedObject(typeof(ApplicationVersion)); // https://stackoverflow.com/a/6280540
            ApplicationVersionTypeProperty?.SetValue(boxedVersion, applicationVersionType);
            MajorProperty?.SetValue(boxedVersion, major);
            MinorProperty?.SetValue(boxedVersion, minor);
            RevisionProperty?.SetValue(boxedVersion, revision);
            return (ApplicationVersion) boxedVersion;
        }

        /// <summary>
        /// Determines whether two source <see cref="ApplicationVersion" /> structs
        /// are equal up to the revisions.
        /// </summary>
        /// <param name="this">An <see cref="ApplicationVersion" /> struct to compare to second.</param>
        /// <param name="other">An <see cref="ApplicationVersion" /> struct to compare to the first.</param>
        /// <returns>
        /// <see langword="true" /> if corresponding elements of two source <see cref="ApplicationVersion" /> structs
        /// are equal according to the default equality comparer for their type; otherwise, <see langword="false" />.
        /// </returns>
        public static bool IsSameWithRevision(this ApplicationVersion @this, ApplicationVersion other)
        {
            return @this.ApplicationVersionType == other.ApplicationVersionType &&
                   @this.Major == other.Major &&
                   @this.Minor == other.Minor &&
                   @this.Revision == other.Revision;
        }

        /// <summary>
        /// Determines whether two source <see cref="ApplicationVersion" /> structs
        /// are equal, disregarding revisions.
        /// </summary>
        /// <param name="this">An <see cref="ApplicationVersion" /> struct to compare to second.</param>
        /// <param name="other">An <see cref="ApplicationVersion" /> struct to compare to the first.</param>
        /// <returns>
        /// <see langword="true" /> if corresponding elements of two source <see cref="ApplicationVersion" /> structs
        /// are equal according to the default equality comparer for their type; otherwise, <see langword="false" />.
        /// Differences in the revisions between two source <see cref="ApplicationVersion" /> structs do not affect the result.
        /// </returns>
        public static bool IsSameWithoutRevision(this ApplicationVersion @this, ApplicationVersion other)
        {
            return @this.ApplicationVersionType == other.ApplicationVersionType &&
                   @this.Major == other.Major &&
                   @this.Minor == other.Minor;
        }

        /// <summary>Gets the game version as an <see cref="ApplicationVersion" /> struct.</summary>
        /// <returns>
        /// <see cref="ApplicationVersion" /> struct representation of the game version
        /// if it was successfully  obtained and parsed; otherwise <see langword="null" />.
        /// </returns>
        public static ApplicationVersion? GameVersion() => TryParse(GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;
    }
}