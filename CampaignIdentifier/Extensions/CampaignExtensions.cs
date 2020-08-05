using System.Linq;
using TaleWorlds.CampaignSystem;
using CampaignIdentifier.CampaignBehaviors;

namespace CampaignIdentifier.Extensions
{
  public static class CampaignExtensions
  {
    public static string GetCampaignID(this Campaign campaign)
    {
      return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is CampaignIdentifierBehavior identifierBehavior ? identifierBehavior.CampaignID : null;
    }

    public static CampaignDescriptor GetCampaignDescriptor(this Campaign campaign)
    {
      return campaign.GameStarted && Campaign.Current.GetCampaignBehavior<CampaignIdentifierBehavior>() is CampaignIdentifierBehavior identifierBehavior ? identifierBehavior.CampaignDescriptor : null;
    }
  }
}
