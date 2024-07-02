using Bannerlord.ButterLib.SubSystems;
using Bannerlord.ButterLib.SubSystems.Settings;

using System.Collections.Generic;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix;

internal class DistanceMatrixSubSystem : ISubSystem, ISubSystemSettings<DistanceMatrixSubSystem>
{
    public static DistanceMatrixSubSystem? Instance { get; private set; }

    public string Id => "Distance Matrix";
    public string Name => "{=Ox6uK8fZWs}Distance Matrix";
    public string Description => "{=WQ4r2n0mYj}Mod Developer feature! Provides helpers to calculate distance between objects on map.";
    public bool IsEnabled { get; private set; } = false;
    public bool CanBeDisabled => true;
    public bool CanBeSwitchedAtRuntime => false;
    internal bool GameInitialized { get; set; } = false;
    public bool ConsiderVillages { get; set; } = true;

    public DistanceMatrixSubSystem()
    {
        Instance = this;
    }

    public void Enable()
    {
        if (IsEnabled) return;

        if (GameInitialized) return;

        IsEnabled = true;
    }

    public void Disable()
    {
        if (!IsEnabled) return;

        if (GameInitialized) return;

        IsEnabled = false;
    }

    public IReadOnlyCollection<SubSystemSettingsDeclaration<DistanceMatrixSubSystem>> Declarations { get; } = new SubSystemSettingsDeclaration<DistanceMatrixSubSystem>[]
    {
        new SubSystemSettingsPropertyBool<DistanceMatrixSubSystem>(
            "{=MSJe8ih4yp}Consider Villages",
            "{=kvR54SiOFn}Allow villages to be used for built-in distance matrix calculations. May negatively affect performance.",
            x => x.ConsiderVillages),
    };
}