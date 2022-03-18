using Bannerlord.ButterLib.DistanceMatrix;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace Bannerlord.ButterLib.Extensions
{
    internal interface ICampaignExtensions
    {
        /// <summary>
        /// Gets the <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" />
        /// calculated by default for the <see cref="Settlement" /> object type.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// Distance matrix for all the towns, castles and villages of the current campaign,
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