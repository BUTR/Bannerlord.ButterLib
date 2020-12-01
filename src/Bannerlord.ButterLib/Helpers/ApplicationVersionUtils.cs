using Bannerlord.ButterLib.Helpers;

using HarmonyLib;

using System;
using System.Reflection;
using System.Runtime.Serialization;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class for handling the game version.</summary>
    public static class ApplicationVersionUtils
    {
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
        public static bool TryParse(string? versionAsString, out ApplicationVersion version)
        {
            var changeSet = 0;
            version = default;
            if (versionAsString is null)
                return false;

            var array = versionAsString.Split('.');
            if (array.Length != 3 && array.Length != 4 && array[0].Length == 0)
                return false;

            var applicationVersionType = ApplicationVersion.ApplicationVersionTypeFromString(array[0][0].ToString());
            if (!int.TryParse(array[0].Substring(1), out var major))
                return false;
            if (!int.TryParse(array[1], out var minor))
                return false;
            if (!int.TryParse(array[2], out var revision))
                return false;
            if (array.Length == 4)
            {
                if (!int.TryParse(array[3], out changeSet))
                    return false;
            }

            version = Create(applicationVersionType, major, minor, revision, changeSet);
            return true;
        }


        private static readonly MethodInfo? GetVersionStrMethod = AccessTools.DeclaredMethod(typeof(Managed), "GetVersionStr");

        private delegate ApplicationVersion ConstructorV1Delegate(ApplicationVersionType a, int b, int c, int d, int e);
        private static ConstructorInfo? ConstructorV1 { get; } = AccessTools.Constructor(typeof(ApplicationVersion), new[]
        {
            typeof(ApplicationVersionType),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int)
        });
        private static ConstructorV1Delegate? ConstructorV1Func { get; } = ConstructorV1 is not null
            ? ConstructorHelper.Delegate<ConstructorV1Delegate>(ConstructorV1)
            : null;

        private delegate ApplicationVersion ConstructorV2Delegate(ApplicationVersionType a, int b, int c, int d);
        private static ConstructorInfo? ConstructorV2 { get; } = AccessTools.Constructor(typeof(ApplicationVersion), new[]
        {
            typeof(ApplicationVersionType),
            typeof(int),
            typeof(int),
            typeof(int)
        });
        private static ConstructorV2Delegate? ConstructorV2Func { get; } = ConstructorV2 is not null
            ? ConstructorHelper.Delegate<ConstructorV2Delegate>(ConstructorV2)
            : null;

        private delegate ApplicationVersion ConstructorV3Delegate(ApplicationVersionType p0, int p1, int p2, int p3, int p4, ApplicationVersionGameType p5);
        private static ConstructorInfo? ConstructorV3 { get; } = AccessTools.Constructor(typeof(ApplicationVersion), new[]
        {
            typeof(ApplicationVersionType),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(ApplicationVersionGameType)
        });
        private static ConstructorV3Delegate? ConstructorV3Func { get; } = ConstructorV3 is not null
            ? ConstructorHelper.Delegate<ConstructorV3Delegate>(ConstructorV3)
            : null;

        private static PropertyInfo? ApplicationVersionTypeProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), nameof(ApplicationVersion.ApplicationVersionType));
        private static PropertyInfo? MajorProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), "Major");
        private static PropertyInfo? MinorProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), "Minor");
        private static PropertyInfo? RevisionProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), "Revision");
        private static PropertyInfo? ChangeSetProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), "ChangeSet");
        private static PropertyInfo? VersionGameTypeProperty { get; } = AccessTools.Property(typeof(ApplicationVersion), "VersionGameType");

        /// <summary>
        /// e1.0.11 didn't had ChangeSet.
        /// It may be an overkill for such a minor game version, but why not.
        /// </summary>
        private static ApplicationVersion Create(ApplicationVersionType applicationVersionType, int major, int minor, int revision, int changeSet)
        {
            if (ConstructorV1Func is not null)
                return ConstructorV1Func(applicationVersionType, major, minor, revision, changeSet);

            if (ConstructorV2Func is not null)
                return ConstructorV2Func(applicationVersionType, major, minor, revision);

            // Can't get the GameType when parsing
            if (ConstructorV3Func is not null)
                return ConstructorV3Func(applicationVersionType, major, minor, revision, changeSet, ApplicationVersionGameType.Singleplayer);

            // Fallback
            var boxedVersion = FormatterServices.GetUninitializedObject(typeof(ApplicationVersion)); // https://stackoverflow.com/a/6280540
            ApplicationVersionTypeProperty?.SetValue(boxedVersion, applicationVersionType);
            MajorProperty?.SetValue(boxedVersion, major);
            MinorProperty?.SetValue(boxedVersion, minor);
            RevisionProperty?.SetValue(boxedVersion, revision);
            ChangeSetProperty?.SetValue(boxedVersion, changeSet);
            VersionGameTypeProperty?.SetValue(boxedVersion, 0);
            return (ApplicationVersion)boxedVersion;
        }
    }
}