using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

using StoryMode;

using System.Linq;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors
{
    /// <summary>Custom behavior used by CampaignIdentifier.</summary>
    internal sealed class CampaignIdentifierBehavior : CampaignBehaviorBase
    {
        private CampaignDescriptorManager _descriptorManager;

        /// <summary>Alphanumeric campaign ID.</summary>
        public string CampaignId => _descriptorManager.CampaignDescriptor.KeyValue;

        /// <summary><see cref = "T:Bannerlord.ButterLib.CampaignIdentifier.CampaignDescriptor" /> object corresponding with the campaign.</summary>
        public CampaignDescriptorImplementation CampaignDescriptor => _descriptorManager.CampaignDescriptor;

        /// <summary>Initializes a new instance of the <see cref="CampaignIdentifierBehavior" />.</summary>
        public CampaignIdentifierBehavior()
        {
            _descriptorManager = new CampaignDescriptorManager();
        }

        public override void RegisterEvents()
        {
            CampaignIdentifierEvents.OnDescriptorRelatedDataChangedEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnCharacterOrClanModified);
            StoryModeEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnCharacterOrClanModified);
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, InitializeCampaignDescriptorOnCreate);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnLoad);
        }

        private void InitializeCampaignDescriptorOnCreate(CampaignGameStarter gameStarter)
        {
            _descriptorManager.GenerateNewGameDescriptor();
        }

        private void UpdateCampaignDescriptorOnLoad(CampaignGameStarter gameStarter)
        {
            _descriptorManager.CheckCampaignDescriptor();
        }

        internal void UpdateCampaignDescriptorOnCharacterOrClanModified()
        {
            if (Hero.All.FirstOrDefault(h => h.Id.SubId == 1) is { } initialHero && initialHero.Clan == Clan.PlayerClan)
            {
                _descriptorManager.UpdateCampaignDescriptor(initialHero);
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_descriptorManager", ref _descriptorManager);
            if (dataStore.IsLoading && _descriptorManager == null)
            {
                _descriptorManager = new CampaignDescriptorManager();
            }

            if (dataStore.IsSaving)
            {
                _descriptorManager.Sync();
            }
        }
    }
}