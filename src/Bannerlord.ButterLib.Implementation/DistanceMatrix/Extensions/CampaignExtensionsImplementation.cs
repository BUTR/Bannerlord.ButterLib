using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.SettlementDistanceMatrix
                : null;
        }

        /// <inheritdoc/>
        public DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.ClanDistanceMatrix
                : null;
        }

        /// <inheritdoc/>
        public DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.KingdomDistanceMatrix
                : null;
        }
    }
}