using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    internal interface ICampaignDescriptorStatic
    {
        CampaignDescriptor? Create(Hero hero);
    }
}