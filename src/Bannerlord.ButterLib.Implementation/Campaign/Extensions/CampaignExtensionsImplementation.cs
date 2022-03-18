using Bannerlord.ButterLib.Extensions;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed partial class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public string? GetCampaignId(TaleWorlds.CampaignSystem.Campaign campaign) => TaleWorlds.CampaignSystem.Campaign.Current.UniqueGameId;
    }
}