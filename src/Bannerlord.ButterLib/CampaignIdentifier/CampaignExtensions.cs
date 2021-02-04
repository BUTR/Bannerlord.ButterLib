using Bannerlord.ButterLib.CampaignIdentifier;

using TaleWorlds.CampaignSystem;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Extensions
{
    public static partial class CampaignExtensions
    {
        /// <summary>Gets ID of the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Alphanumeric string key for the campaign or null if campaign is not started or not yet identified.</returns>
        public static string? GetCampaignId(this Campaign campaign) => Instance?.GetCampaignId(campaign);

        /// <summary>Gets CampaignDescriptor for the <see cref="Campaign" />.</summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>
        /// <see cref="CampaignDescriptor" /> object corresponding with the campaign
        /// or null if campaign is not started or not yet identified.
        /// </returns>
        public static CampaignDescriptor? GetCampaignDescriptor(this Campaign campaign) => Instance?.GetCampaignDescriptor(campaign);
    }
}