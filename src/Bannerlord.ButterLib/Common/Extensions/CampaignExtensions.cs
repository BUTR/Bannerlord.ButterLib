using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.DistanceMatrix;

using Microsoft.Extensions.DependencyInjection;

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
        internal static ICampaignExtensions? _instance;
        internal static ICampaignExtensions Instance =>
            _instance ??= SubModule.ServiceProvider.GetRequiredService<ICampaignExtensions>();

        /// <summary>Gets ID of the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Alphanumeric string key for the campaign or null if campaign is not started or not yet identified.</returns>
        public static string? GetCampaignId(this Campaign campaign) => Instance.GetCampaignId(campaign);

        /// <summary>Gets CampaignDescriptor for the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// <see cref="CampaignDescriptor" /> object corresponding with the campaign 
        /// or null if campaign is not started or not yet identified.
        /// </returns>
        public static CampaignDescriptor? GetCampaignDescriptor(this Campaign campaign) => Instance.GetCampaignDescriptor(campaign);

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Settlement" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the towns, castles and villages of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        public static DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(this Campaign campaign) => Instance.GetDefaultSettlementDistanceMatrix(campaign);

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
        public static DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(this Campaign campaign) => Instance.GetDefaultClanDistanceMatrix(campaign);

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
        public static DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(this Campaign campaign) => Instance.GetDefaultKingdomDistanceMatrix(campaign);
    }
}