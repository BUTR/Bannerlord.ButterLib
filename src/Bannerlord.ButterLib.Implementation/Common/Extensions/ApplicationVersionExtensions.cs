using Bannerlord.ButterLib.Common.Extensions;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    internal sealed class ApplicationVersionExtensions : IApplicationVersionExtensions
    {
        public bool IsSameWithRevision(ApplicationVersion @this, ApplicationVersion other)
        {
            return @this.ApplicationVersionType == other.ApplicationVersionType &&
                   @this.Major == other.Major &&
                   @this.Minor == other.Minor &&
                   @this.Revision == other.Revision;
        }

        public bool IsSameWithoutRevision(ApplicationVersion @this, ApplicationVersion other)
        {
            return @this.ApplicationVersionType == other.ApplicationVersionType &&
                   @this.Major == other.Major &&
                   @this.Minor == other.Minor;
        }
    }
}