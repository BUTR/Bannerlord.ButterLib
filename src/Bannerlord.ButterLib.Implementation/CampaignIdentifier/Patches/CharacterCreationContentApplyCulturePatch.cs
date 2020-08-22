using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers;

using HarmonyLib;

using Helpers;

using StoryMode.CharacterCreationSystem;

using System;
using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches
{
    [HarmonyPatch(typeof(CharacterCreationContent), "ApplyCulture")]
    internal static class CharacterCreationContentApplyCulturePatch
    {
        public static void Postfix()
        {
            try
            {
                //Assign player a random town from chosen culture as a born settlement
                var bornSettlement = SettlementHelper.FindRandomSettlement(s => s.Culture == Hero.MainHero.Culture && s.IsTown);
                FieldAccessHelper.ClanHomeSettlementByRef(Clan.PlayerClan) = bornSettlement;
                foreach (var hero in Clan.PlayerClan.Heroes)
                {
                    hero.UpdateHomeSettlement();
                }
                Hero.MainHero.BornSettlement = Clan.PlayerClan.HomeSettlement;
            }
            catch (Exception ex)
            {
                var methodInfo = MethodBase.GetCurrentMethod();
                DebugHelper.HandleException(ex, methodInfo, "Harmony patch for CharacterCreationContent. ApplyCulture");
            }
        }
    }
}