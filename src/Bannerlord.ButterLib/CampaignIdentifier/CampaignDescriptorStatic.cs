using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    internal abstract class CampaignDescriptorStatic
    {
        public abstract CampaignDescriptor? Create(Hero hero);
    }
}