using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Extensions
{
    /// <summary>
    /// An extension class for the <see cref="ApplicationVersion" />.
    /// </summary>
    public static class ApplicationVersionExtensions
    {
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
    }
}