using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches
{
    /// <summary>
    /// Fixes TW's new implementation. They plan to switch namespaces, to Type.FullName will break saves.
    /// Instead, only TW will use Type.Name, the modding community will use Type.FullName
    /// </summary>
    internal sealed class BehaviourNamePatch
    {
        internal static bool Enable(Harmony harmony)
        {
            return harmony.TryPatch(
                AccessTools2.Constructor(typeof(CampaignBehaviorBase)),
                postfix: AccessTools2.Method("Bannerlord.ButterLib.Implementation.SaveSystem.Patches.BehaviourNamePatch:CampaignBehaviorBaseCtorPostfix"));
        }

        internal static bool Disable(Harmony harmony)
        {
            harmony.Unpatch(
                AccessTools2.Constructor(typeof(CampaignBehaviorBase)),
                AccessTools2.Method("Bannerlord.ButterLib.Implementation.SaveSystem.Patches.BehaviourNamePatch:CampaignBehaviorBaseCtorPostfix"));

            return true;
        }

        private static void CampaignBehaviorBaseCtorPostfix(CampaignBehaviorBase __instance, ref string? ___StringId)
        {
            var module = ModuleInfoHelper.GetModuleByType(__instance.GetType());
            if (module is null) // A non-module dll
            {
                return;
            }

            if (module.IsOfficial) // A TW module
            {
                return;
            }

            ___StringId = __instance.GetType().FullName;
        }
    }
}