using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    internal static class CampaignIdentifierSubSystem
    {
        private static readonly Harmony _harmony = new Harmony("Bannerlord.ButterLib.CampaignIdentifier");

        public static void Enable()
        {
            CharacterCreationContentApplyCulturePatch.Enable(_harmony);
            ClanInitializeClanPatch.Enable(_harmony);
        }

        public static void Disable()
        {
            //CharacterCreationContentApplyCulturePatch.Disable(_harmony);
            //ClanInitializeClanPatch.Disable(_harmony);
        }
    }
}