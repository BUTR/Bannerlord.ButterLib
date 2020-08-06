using Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier.Extensions
{
    public static class CampaignExtensions
    {
        public static string? GetCampaignId(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignId
                : null;
        }

        public static CampaignDescriptor? GetCampaignDescriptor(this Campaign campaign)
        {
            return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is { } identifierBehavior
                ? identifierBehavior.CampaignDescriptor
                : null;
        }
    }
}