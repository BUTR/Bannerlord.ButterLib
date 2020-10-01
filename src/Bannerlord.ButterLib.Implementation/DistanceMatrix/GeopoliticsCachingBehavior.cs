using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    /// <summary>Custom behavior used to calculate default distance matrices.</summary>
    internal sealed class GeopoliticsCachingBehavior : CampaignBehaviorBase
    {
        /// <summary>Distances between settlements of the current campaign.</summary>
        /// <value>Distance matrix for all the towns, castles and villages of the current campaign.</value>
        public DistanceMatrixImplementation<Settlement>? SettlementDistanceMatrix { get; private set; }

        /// <summary>Distances between active clans of the current campaign.</summary>
        /// <value>
        /// Distance matrix for all the active clans of the current campaign,
        /// calculated based on the fiefs owned by these clans.
        /// </value>
        public DistanceMatrixImplementation<Clan>? ClanDistanceMatrix { get; private set; }

        /// <summary>Distances between active kingdoms of the current campaign.</summary>
        /// <value>
        /// Distance matrix for all the active clans of the current campaign,
        /// calculated based on the fiefs owned by these kingdoms.
        /// </value>
        public DistanceMatrixImplementation<Kingdom>? KingdomDistanceMatrix { get; private set; }

        public override void RegisterEvents()
        {
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, InitializeOnLoad);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, InitializeOnLoad);
            CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, UpdateOnSettlementOwnerChanged);
        }

        private void InitializeOnLoad(CampaignGameStarter gameStarterObject)
        {
            SettlementDistanceMatrix = new DistanceMatrixImplementation<Settlement>();
            ClanDistanceMatrix = new DistanceMatrixImplementation<Clan>();
            KingdomDistanceMatrix = new DistanceMatrixImplementation<Kingdom>();
        }

        private void UpdateOnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
        {
            var lst = ButterLib.DistanceMatrix.DistanceMatrix.GetSettlementOwnersPairedList(SettlementDistanceMatrix!);
            if (lst == null)
                return;

            if ((newOwner.Clan != null || oldOwner.Clan != null) && newOwner.Clan != oldOwner.Clan)
            {
                var clans = Clan.All.Where(c => c.IsInitialized && c.Fortifications.Count > 0).ToList();

                if (oldOwner.Clan != null)
                {
                    foreach (Clan clan in clans)
                    {
                        if (clan != oldOwner.Clan)
                        {
                            var distance = ButterLib.DistanceMatrix.DistanceMatrix.CalculateDistanceBetweenClans(oldOwner.Clan, clan, lst);
                            ClanDistanceMatrix!.SetDistance(oldOwner.Clan, clan, distance.GetValueOrDefault());
                        }
                    }
                }
                if (newOwner.Clan != null)
                {
                    foreach (Clan clan in clans)
                    {
                        if (clan != newOwner.Clan)
                        {
                            var distance = ButterLib.DistanceMatrix.DistanceMatrix.CalculateDistanceBetweenClans(newOwner.Clan, clan, lst);
                            ClanDistanceMatrix!.SetDistance(newOwner.Clan, clan, distance.GetValueOrDefault());
                        }
                    }
                }
            }

            if ((newOwner.Clan?.Kingdom != null || oldOwner.Clan?.Kingdom != null) && newOwner.Clan?.Kingdom != oldOwner.Clan?.Kingdom)
            {
                KingdomDistanceMatrix = new DistanceMatrixImplementation<Kingdom>();
            }
        }

        public override void SyncData(IDataStore dataStore) { }
    }
}
