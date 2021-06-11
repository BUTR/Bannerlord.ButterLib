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

        private readonly Harmony _harmony = new("Bannerlord.ButterLib.CampaignIdentifier");

        public CampaignIdentifierSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

#if e143 || e150 || e151 || e152 || e153
            Patches.CharacterCreationContentApplyCulturePatch.Enable(_harmony);
            Patches.ClanInitializeClanPatch.Enable(_harmony);
#elif e154 || e155 || e156 || e157 || e158 || e159 || e1510 || e160
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
        }

        public void Disable()
        {
            IsEnabled = false;

#if e143 || e150 || e151 || e152 || e153
            //CharacterCreationContentApplyCulturePatch.Disable(_harmony);
            //ClanInitializeClanPatch.Disable(_harmony);
#elif e154 || e155 || e156 || e157 || e158 || e159 || e1510 || e160
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
        }
    }
}