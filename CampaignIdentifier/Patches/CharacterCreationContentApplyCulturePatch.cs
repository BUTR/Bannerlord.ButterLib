using HarmonyLib;
using System;
using System.Reflection;
using Helpers;
using StoryMode.CharacterCreationSystem;
using TaleWorlds.CampaignSystem;
using CampaignIdentifier.Helpers;

namespace CampaignIdentifier.Patches
{
  [HarmonyPatch(typeof(CharacterCreationContent), "ApplyCulture")]
  class CharacterCreationContentApplyCulturePatch
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
        MethodInfo methodInfo = MethodBase.GetCurrentMethod() as MethodInfo;
        DebugHelper.HandleException(ex, methodInfo, "Harmony patch for CharacterCreationContent. ApplyCulture");
      }
    }
  }
}
