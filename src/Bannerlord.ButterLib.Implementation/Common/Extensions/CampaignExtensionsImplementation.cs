using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <summary>
    /// Helper extension of the <see cref="Campaign" /> class 
    /// returning additional information, provided by the ButterLib.
    /// </summary>
    /// <remarks>
    /// Contains easy accessible getters for the current CampaignId and <see cref="CampaignDescriptorImplementation" />
    /// provided by CampaignIdentifier service as well as various geopolitical distance matrices
    /// held in <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> objects.
    /// </remarks>
    internal class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <summary>Gets ID of the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Alphanumeric string key for the campaign or null if campaign is not started or not yet identified.</returns>
        public string? GetCampaignId(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignId
                : null;
        }

        /// <summary>Gets CampaignDescriptor for the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// <see cref="CampaignDescriptor" /> object corresponding with the campaign 
        /// or null if campaign is not started or not yet identified.
        /// </returns>
        public CampaignDescriptor? GetCampaignDescriptor(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignDescriptor
                : null;
        }

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Settlement" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the towns, castles and villages of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        public DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.SettlementDistanceMatrix
                : null;
        }

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Clan" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the active clans of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between clans fiefs weighted by the fief type.</remarks>
        public DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.ClanDistanceMatrix
                : null;
        }

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Kingdom" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the active kingdoms of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        /// <remarks>Calculation is based on the average distance between kingdoms fiefs weighted by the fief type.</remarks>
        public DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(Campaign campaign)
        {
            return campaign.GameStarted && campaign.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.KingdomDistanceMatrix
                : null;
        }
    }
}