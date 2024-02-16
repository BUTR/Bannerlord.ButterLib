using Bannerlord.ButterLib.DistanceMatrix;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Extensions;

public static partial class CampaignExtensions
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
    public static DistanceMatrix<Settlement>? GetDefaultSettlementDistanceMatrix(this Campaign campaign) => Instance?.GetDefaultSettlementDistanceMatrix(campaign);

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
    public static DistanceMatrix<Clan>? GetDefaultClanDistanceMatrix(this Campaign campaign) => Instance?.GetDefaultClanDistanceMatrix(campaign);

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
    public static DistanceMatrix<Kingdom>? GetDefaultKingdomDistanceMatrix(this Campaign campaign) => Instance?.GetDefaultKingdomDistanceMatrix(campaign);
}