using Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

using StoryMode;

using System.Linq;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors
{
    public class CampaignIdentifierBehavior : CampaignBehaviorBase
    {
        private CampaignDescriptorManager _descriptorManager;

        public string CampaignId => _descriptorManager.CampaignDescriptor.KeyValue;
        public CampaignDescriptor CampaignDescriptor => _descriptorManager.CampaignDescriptor;

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

        public void UpdateCampaignDescriptorOnCharacterOrClanModified()
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