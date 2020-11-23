using Bannerlord.ButterLib.Implementation.ObjectSystem.Patches;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal static class ObjectSystemSubSystem
    {
        private static readonly Harmony _harmony = new Harmony("Bannerlord.ButterLib.ObjectSystem");

        public static void Enable()
        {
            CampaignBehaviorManagerPatch.Enable(_harmony);
        }

        public static void Disable()
        {
            //CampaignBehaviorManagerPatch.Disable(_harmony);
        }
    }
}