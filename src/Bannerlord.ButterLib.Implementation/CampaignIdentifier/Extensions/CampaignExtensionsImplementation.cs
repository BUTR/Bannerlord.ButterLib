using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.Common.Extensions
{
    /// <inheritdoc/>
    internal sealed partial class CampaignExtensionsImplementation : ICampaignExtensions
    {
        /// <inheritdoc/>
        public string? GetCampaignId(Campaign campaign)
        {
#if e143 || e150 || e151 || e152 || e153
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignId
                : null;
#elif e154 || e155 || e156 || e157 || e158 || e159 || e1510
            return Campaign.Current.UniqueGameId;
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
        }

        /// <inheritdoc/>
        public CampaignDescriptor? GetCampaignDescriptor(Campaign campaign)
        {
#if e143 || e150 || e151 || e152 || e153
            return campaign.GameStarted && campaign.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignDescriptor
                : null;
#elif e154 || e155 || e156 || e157 || e158 || e159 || e1510
            return new BlankCampaignDescriptor(Campaign.Current);
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
        }
    }
}