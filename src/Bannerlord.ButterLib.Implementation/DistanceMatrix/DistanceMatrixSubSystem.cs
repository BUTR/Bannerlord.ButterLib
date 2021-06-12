using Bannerlord.ButterLib.SubSystems;

using System;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    internal class DistanceMatrixSubSystem : ISubSystem
    {
        public static DistanceMatrixSubSystem? Instance { get; private set; }

        public string Id => "Distance Matrix";
        public string Description => "Provides helpers to calculate distance between objects on map.";
        public bool IsEnabled { get; private set; } = false;
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => false;
        internal bool GameInitialized { get; set; } = false;

        public DistanceMatrixSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            if (GameInitialized)
                throw new Exception("Distance Matrix SubSystem can't be enabled after campaign start!");

            IsEnabled = true;
        }

        public void Disable()
        {
            if (GameInitialized)
                throw new Exception("Distance Matrix SubSystem can't be disabled after campaign start!");

            IsEnabled = false;
        }
    }
}