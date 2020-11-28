using Bannerlord.ButterLib.SubSystems;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    internal class DistanceMatrixSubSystem : ISubSystem
    {
        public string Id => "Distance Matrix";
        public string Description => "Provides helpers to calculate distance between objects on map.";
        public bool IsEnabled => true;
        public bool CanBeDisabled => false;
        public bool CanBeSwitchedAtRuntime => false;
        public void Enable() { }
        public void Disable() { }
    }
}