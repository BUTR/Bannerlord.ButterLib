using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Common.Extensions
{
    internal interface IApplicationVersionExtensions
    {
        bool IsSameWithRevision(ApplicationVersion @this, ApplicationVersion other);
        bool IsSameWithoutRevision(ApplicationVersion @this, ApplicationVersion other);
    }
}