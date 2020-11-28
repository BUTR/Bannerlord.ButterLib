using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    internal sealed class CampaignIdentifierSubSystem : ISubSystem
    {
        public static CampaignIdentifierSubSystem? Instance { get; private set; }

        public string Id => "CampaignIdentifier";
        public string Description => "Provides a way to identify campaigns across multiple saves";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => false;

        private readonly Harmony _harmony = new Harmony("Bannerlord.ButterLib.CampaignIdentifier");

        public CampaignIdentifierSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            CharacterCreationContentApplyCulturePatch.Enable(_harmony);
            ClanInitializeClanPatch.Enable(_harmony);
        }

        public void Disable()
        {
            IsEnabled = false;

            //CharacterCreationContentApplyCulturePatch.Disable(_harmony);
            //ClanInitializeClanPatch.Disable(_harmony);
        }
    }
}