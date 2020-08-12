using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.DistanceMatrix;
using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Common.Extensions
{
    /// <summary>
    /// Helper extension of the <see cref="Campaign" /> class 
    /// returning additional information, provided by the ButterLib.
    /// </summary>
    /// <remarks>
    /// Contains easy accessible getters for the current CampaignId and <see cref="CampaignDescriptor" />
    /// provided by CampaignIdentifier service as well as various geopolitical distance matrixes
    /// held in <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> objects.
    /// </remarks>
    public static class CampaignExtensions
    {
        /// <summary>Gets ID of the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Alphanumeric string key for the campaign or null if campaign is not started or not yet identified.</returns>
        public static string? GetCampaignId(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignId
                : null;
        }

        /// <summary>Gets CampaignDescriptor for the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// <see cref="CampaignDescriptor" /> object corresponding with the campaign 
        /// or null if campaign is not started or not yet identified.
        /// </returns>
        public static CampaignDescriptor? GetCampaignDescriptor(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignDescriptor
                : null;
        }

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Settlement" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the towns, castles and vilages of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        public static DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
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
        public static DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
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
        public static DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<GeopoliticsCachingBehavior>() is { } geopoliticsCachingBehavior
                ? geopoliticsCachingBehavior.KingdomDistanceMatrix
                : null;
        }
    }
}