using System.Collections.Generic;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public class ApplicationVersionComparer : IComparer<ApplicationVersion>
    {
        public int Compare(ApplicationVersion x, ApplicationVersion y)
        {
            var applicationVersionTypeComparison = x.ApplicationVersionType.CompareTo(y.ApplicationVersionType);
            if (applicationVersionTypeComparison != 0) return applicationVersionTypeComparison;

            var majorComparison = x.Major.CompareTo(y.Major);
            if (majorComparison != 0) return majorComparison;

            var minorComparison = x.Minor.CompareTo(y.Minor);
            if (minorComparison != 0) return minorComparison;

            var revisionComparison = x.Revision.CompareTo(y.Revision);
            if (revisionComparison != 0) return revisionComparison;

            var changeSetComparison = x.ChangeSet.CompareTo(y.ChangeSet);
            if (changeSetComparison != 0) return changeSetComparison;

            var versionGameTypeComparison = x.VersionGameType.CompareTo(y.VersionGameType);
            if (versionGameTypeComparison != 0) return versionGameTypeComparison;

            return 0;
        }
    }
}