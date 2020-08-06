using Bannerlord.ButterLib.CampaignIdentifier.Helpers;

using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier.Patches
{
    [HarmonyPatch(typeof(Clan), "InitializeClan")]
    internal static class ClanInitializeClanPatch
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
                var methodInfo = MethodBase.GetCurrentMethod();
                DebugHelper.HandleException(ex, methodInfo, "Harmony patch for Clan. InitializeClan");
            }
        }
    }
}
