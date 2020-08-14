using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.DistanceMatrix;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Common.Extensions
{
    internal interface ICampaignExtensions
    {
        /// <summary>Gets ID of the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Alphanumeric string key for the campaign or null if campaign is not started or not yet identified.</returns>
        string? GetCampaignId(Campaign campaign);

        /// <summary>Gets CampaignDescriptor for the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// <see cref="CampaignDescriptor" /> object corresponding with the campaign 
        /// or null if campaign is not started or not yet identified.
        /// </returns>
        CampaignDescriptor? GetCampaignDescriptor(Campaign campaign);

        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Settlement" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the towns, castles and vilages of the current campaign, 
        /// or null if the campaign has not started yet.
        /// </returns>
        DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(Campaign campaign);

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
        DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(Campaign campaign);

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
        DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(Campaign campaign);
    }
}