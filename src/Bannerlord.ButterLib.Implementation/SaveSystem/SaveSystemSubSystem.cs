using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.SaveSystem
{
    internal class SaveSystemSubSystem : ISubSystem
    {
        public static SaveSystemSubSystem? Instance { get; private set; }

        public string Id => "Save System";
        public string Description => @"{=ButterlibLoggingHint}Extends and fixes the game's save system:
* Fixes possible collision with save names;
* Fixes save corruption & crashes when duplicate types are defined;
* Adds support for saving many more container types;
This might alter the save file, disabling the feature might render the save file unloadable!";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => false;

        private readonly Harmony _harmony = new("Bannerlord.ButterLib.SaveSystem");

        public SaveSystemSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            BehaviourNamePatch.Enable(_harmony); // Fixes possible collision with save names
            TypeExtensionsPatch.Enable(_harmony); // Adds support for saving many more container types
            DefinitionContextPatch.Enable(_harmony); // Fixes save corruption & crashes when duplicate types are defined
        }

        public void Disable()
        {
            IsEnabled = false;

            BehaviourNamePatch.Disable(_harmony);
            TypeExtensionsPatch.Disable(_harmony);
            DefinitionContextPatch.Disable(_harmony);
        }
    }
}