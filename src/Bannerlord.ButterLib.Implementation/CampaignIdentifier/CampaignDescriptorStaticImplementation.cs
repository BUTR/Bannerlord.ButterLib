using Bannerlord.ButterLib.CampaignIdentifier;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    internal sealed class CampaignDescriptorStaticImplementation : CampaignDescriptorStatic
    {
        public override CampaignDescriptor? Create(Hero hero) => new CampaignDescriptorImplementation(hero);
    }
}