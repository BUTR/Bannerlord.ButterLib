using Bannerlord.ButterLib.SubSystems;

namespace Bannerlord.ButterLib.DelayedSubModule;

internal class DelayedSubModuleSubSystem : ISubSystem
{
    public static DelayedSubModuleSubSystem? Instance { get; private set; }

    public string Id => "Delayed SubModule";
    public string Name => "{=joCJ9xpDvM}Delayed SubModule";
    public string Description => "{=Gznum6kuzv}Mod Developer feature! Provides helpers to run methods after SubModule events.";
    public bool IsEnabled => true;
    public bool CanBeDisabled => false;
    public bool CanBeSwitchedAtRuntime => false;

    public DelayedSubModuleSubSystem()
    {
        Instance = this;
    }

    public void Enable() { }
    public void Disable() { }
}