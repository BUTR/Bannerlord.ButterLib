using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    /// <summary>Custom behavior used to calculate default distance matrixes.</summary>
    public class GeopoliticsCachingBehavior : CampaignBehaviorBase
    {
        /// <summary>Distance matrix for all the towns, castles and vilages of the current campaign.</summary>
        public DistanceMatrix<Settlement>? SettlementDistanceMatrix { get; private set; }
        /// <summary>Distance matrix for all the active clans of the current campaign.</summary>
        public DistanceMatrix<Clan>? ClanDistanceMatrix { get; private set; }
        /// <summary>Distance matrix for all the active kingdoms of the current campaign.</summary>
        public DistanceMatrix<Kingdom>? KingdomDistanceMatrix { get; private set; }

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, InitializeOnLoad);
            CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, UpdateOnSettlementOwnerChanged);
        }

        private void InitializeOnLoad(CampaignGameStarter gameStarterObject)
        {
            SettlementDistanceMatrix = new DistanceMatrix<Settlement>();
            ClanDistanceMatrix = new DistanceMatrix<Clan>();
            KingdomDistanceMatrix = new DistanceMatrix<Kingdom>();
        }

        private void UpdateOnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
        {
            if ((newOwner.Clan != null || oldOwner.Clan != null) && newOwner.Clan != oldOwner.Clan)
            {
                ClanDistanceMatrix = new DistanceMatrix<Clan>();
            }
            if ((newOwner.Clan?.Kingdom != null || oldOwner.Clan?.Kingdom != null) && newOwner.Clan?.Kingdom != oldOwner.Clan?.Kingdom)
            {
                KingdomDistanceMatrix = new DistanceMatrix<Kingdom>();
            }
        }

        public override void SyncData(IDataStore dataStore) { }
    }
}
