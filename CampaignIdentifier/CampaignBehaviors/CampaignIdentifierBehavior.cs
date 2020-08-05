using System.Linq;
using TaleWorlds.CampaignSystem;
using StoryMode;
using CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

namespace CampaignIdentifier.CampaignBehaviors
{
  public class CampaignIdentifierBehavior : CampaignBehaviorBase
  {
    private CampaignDescriptorManager _descriptorManager;

    public string CampaignID => _descriptorManager.CampaignDescriptor.KeyValue;
    public CampaignDescriptor CampaignDescriptor => _descriptorManager.CampaignDescriptor;

    public CampaignIdentifierBehavior()
    {
      _descriptorManager = new CampaignDescriptorManager();
    }

    public override void RegisterEvents()
    {
      CampaignIdentifierEvents.OnDescriptorRelatedDataChangedEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnCharacterOrClanModified);
      StoryModeEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnCharacterOrClanModified);
      CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, IntitializeCampaignDescriptorOnCreate);
      CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, UpdateCampaignDescriptorOnLoad);
    }

    private void IntitializeCampaignDescriptorOnCreate(CampaignGameStarter gameStarter)
    {
      _descriptorManager.GenerateNewGameDescriptor();
    }

    private void UpdateCampaignDescriptorOnLoad(CampaignGameStarter gameStarter)
    {
      _descriptorManager.CheckCampaignDescriptor();
    }

    public void UpdateCampaignDescriptorOnCharacterOrClanModified()
    {
      if (Hero.All.FirstOrDefault(h => h.Id.SubId == 1) is Hero initialHero && initialHero.Clan == Clan.PlayerClan)
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
