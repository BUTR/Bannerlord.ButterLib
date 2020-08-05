using Bannerlord.ButterLib.CampaignIdentifier.Helpers;

using HarmonyLib;

using Helpers;

using StoryMode.CharacterCreationSystem;

using System;
using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier.Patches
{
    [HarmonyPatch(typeof(CharacterCreationContent), "ApplyCulture")]
    internal class CharacterCreationContentApplyCulturePatch
    {
        public static void Postfix()
        {
            try
            {
                if (Hero.MainHero.Culture != Hero.MainHero.BornSettlement.Culture)
                {
                    Clan.PlayerClan.InitializeHomeSettlement(SettlementHelper.FindRandomSettlement(s => s.Culture == Hero.MainHero.Culture && s.IsTown));
                    Hero.MainHero.BornSettlement = Clan.PlayerClan.HomeSettlement;
                }
            }
            catch (Exception ex)
            {
                var methodInfo = MethodBase.GetCurrentMethod();
                DebugHelper.HandleException(ex, methodInfo, "Harmony patch for CharacterCreationContent. ApplyCulture");
            }
        }
    }
}