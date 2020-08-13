using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    /// <summary>Custom behavior used to calculate default distance matrices.</summary>
    public class GeopoliticsCachingBehavior : CampaignBehaviorBase
    {
        /// <summary>Distances between settlements of the current campaign.</summary>
        /// <value>Distance matrix for all the towns, castles and villages of the current campaign.</value>
        public DistanceMatrix<Settlement>? SettlementDistanceMatrix { get; private set; }

        /// <summary>Distances between active clans of the current campaign.</summary>
        /// <value>
        /// Distance matrix for all the active clans of the current campaign,
        /// calculated based on the fiefs owned by these clans.
        /// </value>
        public DistanceMatrix<Clan>? ClanDistanceMatrix { get; private set; }

        /// <summary>Distances between active kingdoms of the current campaign.</summary>
        /// <value>
        /// Distance matrix for all the active clans of the current campaign,
        /// calculated based on the fiefs owned by these kingdoms.
        /// </value>
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
