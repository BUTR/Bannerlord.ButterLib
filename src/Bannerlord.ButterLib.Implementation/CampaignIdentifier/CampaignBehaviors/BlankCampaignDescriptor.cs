using Bannerlord.ButterLib.CampaignIdentifier;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors
{
    internal sealed class BlankCampaignDescriptor : CampaignDescriptor
    {
        private readonly Campaign _campaign;

        public override string KeyValue => string.Empty;
        public override string ImmutableKey => string.Empty;
        public override string FullCharacterName => string.Empty;
        public override string Descriptor => string.Empty;
        public override CharacterCode CharacterCode => CharacterCode.CreateEmpty();

        public BlankCampaignDescriptor(Campaign campaign)
        {
            _campaign = campaign;
        }
    }
}