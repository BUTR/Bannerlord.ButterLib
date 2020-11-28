using Bannerlord.ButterLib.SubSystems;

namespace Bannerlord.ButterLib.DelayedSubModule
{
    internal class DelayedSubModuleSubSystem : ISubSystem
    {
        public string Id => "Delayed SubModule";
        public string Description => "Provides helpers to run methods after SubModule events.";
        public bool IsEnabled => true;
        public bool CanBeDisabled => false;
        public bool CanBeSwitchedAtRuntime => false;
        public void Enable() { }
        public void Disable() { }
    }
}