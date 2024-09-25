using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;
using Bannerlord.ButterLib.Options;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.SaveSystem;

internal class SaveSystemSubSystem : ISubSystem
{
    public static SaveSystemSubSystem? Instance { get; private set; }

    public string Id => "Save System";
    public string Name => "{=66A5N9278w}Save System";
    public string Description => @"{=9ybOxGpWb5}Extends and fixes the game's save system:{NL}
* Fixes possible collision with save names;{NL}
* Fixes save corruption & crashes when duplicate types are defined;{NL}
* Adds support for saving many more container types;{NL}
This might alter the save file, disabling the feature might render the save file unloadable!";
    public bool IsEnabled { get; private set; }
    public bool CanBeDisabled => true;
    public bool CanBeSwitchedAtRuntime => false;

    private readonly Harmony _harmony = new("Bannerlord.ButterLib.SaveSystem");

    private bool _wasInitialized;

    public SaveSystemSubSystem()
    {
        Instance = this;
    }

    public void Enable()
    {
        if (!_wasInitialized)
        {
            _wasInitialized = true;
            var isEnabledViaSettings = SettingsProvider.PopulateSubSystemSettings(this) ?? true;
            if (!isEnabledViaSettings) return;
        }

        if (IsEnabled) return;
        IsEnabled = true;

        BehaviourNamePatch.Enable(_harmony); // Fixes possible collision with save names
        TypeExtensionsPatch.Enable(_harmony); // Adds support for saving many more container types
        DefinitionContextPatch.Enable(_harmony); // Fixes save corruption & crashes when duplicate types are defined
    }

    public void Disable()
    {
        if (!IsEnabled) return;
        IsEnabled = false;

        BehaviourNamePatch.Disable(_harmony);
        TypeExtensionsPatch.Disable(_harmony);
        DefinitionContextPatch.Disable(_harmony);
    }
}