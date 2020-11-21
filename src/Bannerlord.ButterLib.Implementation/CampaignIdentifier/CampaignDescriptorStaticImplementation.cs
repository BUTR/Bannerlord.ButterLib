using Bannerlord.ButterLib.CampaignIdentifier;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    internal sealed class CampaignDescriptorStaticImplementation : ICampaignDescriptorStatic
    {
        public CampaignDescriptor Create(Hero hero) => new CampaignDescriptorImplementation(hero);
    }
}