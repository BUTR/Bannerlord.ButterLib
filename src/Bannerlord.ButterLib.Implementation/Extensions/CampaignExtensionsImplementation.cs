using Bannerlord.ButterLib.Extensions;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed partial class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public string? GetCampaignId(Campaign campaign) => Campaign.Current.UniqueGameId;
    }
}