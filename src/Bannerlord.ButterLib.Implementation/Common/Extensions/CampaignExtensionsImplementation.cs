using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public string? GetCampaignId(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignId
                : null;
        }

        /// <inheritdoc/>
        public CampaignDescriptor? GetCampaignDescriptor(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignDescriptor
                : null;
        }

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