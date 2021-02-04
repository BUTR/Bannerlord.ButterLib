using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed partial class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.SettlementDistanceMatrix
                : null;
        }

        /// <inheritdoc/>
        public DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.ClanDistanceMatrix
                : null;
        }

        /// <inheritdoc/>
        public DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.KingdomDistanceMatrix
                : null;
        }
    }
}