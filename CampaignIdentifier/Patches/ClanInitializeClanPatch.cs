using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using CampaignIdentifier.Helpers;

namespace CampaignIdentifier.Patches
{
  [HarmonyPatch(typeof(Clan), "InitializeClan")]
  class ClanInitializeClanPatch
  {
    public static void Postfix(Clan __instance)
    {
      try
      {
        if (__instance == Clan.PlayerClan)
        {
          CampaignIdentifierEvents.Instance.OnDescriptorRelatedDataChanged();
        }
      }
      catch (Exception ex)
      {
        MethodInfo methodInfo = MethodBase.GetCurrentMethod() as MethodInfo;
        DebugHelper.HandleException(ex, methodInfo, "Harmony patch for Clan. InitializeClan");
      }
    }
  }
}
