using Bannerlord.ButterLib.Implementation.ObjectSystem.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem;

internal class ObjectSystemSubSystem : ISubSystem
{
    public static ObjectSystemSubSystem? Instance { get; private set; }

    public string Id => "Object System";
    public string Name => "{=IA0mVgHJgo}Object System";
    public string Description => "{=mFZTv1nwOx}Mod Developer feature!";
    public bool IsEnabled { get; private set; }
    public bool CanBeDisabled => true;
    public bool CanBeSwitchedAtRuntime => false;

    private readonly Harmony _harmony = new("Bannerlord.ButterLib.ObjectSystem");

    public ObjectSystemSubSystem()
    {
            Instance = this;
        }

    public void Enable()
    {
            IsEnabled = true;

            CampaignBehaviorManagerPatch.Enable(_harmony);
        }

    public void Disable()
    {
            IsEnabled = false;

            CampaignBehaviorManagerPatch.Disable(_harmony);
        }
}