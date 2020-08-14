using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class ApplicationVersionExtensions
    {
        internal static IApplicationVersionExtensions? _instance;
        internal static IApplicationVersionExtensions Instance =>
            _instance ??= SubModule.ServiceProvider.GetRequiredService<IApplicationVersionExtensions>();

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
        public static bool IsSameWithRevision(this ApplicationVersion @this, ApplicationVersion other) => Instance.IsSameWithRevision(@this, other);

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
        public static bool IsSameWithoutRevision(this ApplicationVersion @this, ApplicationVersion other) => Instance.IsSameWithRevision(@this, other);
    }
}
