using Bannerlord.ButterLib.Extensions;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed partial class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public string? GetCampaignId(Campaign campaign)
        {
#if e160 || e161 || e162 || e163 || e164 || e165 || e170 || e171
            return Campaign.Current.UniqueGameId;
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
        }
    }
}