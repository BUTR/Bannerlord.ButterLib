﻿using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.SaveSystem
{
    internal class SaveSystemSubSystem : ISubSystem
    {
        public static SaveSystemSubSystem? Instance { get; private set; }

        public string Id => "Save System";
        public string Description => "Extends and fixes the game's save system";
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

            TypeExtensionsPatch.Enable(_harmony); // Adds support for saving many more container types
            //DefinitionContextPatch.Enable(_harmony); // Fixes save corruption & crashes when duplicate types are defined
        }

        public void Disable()
        {
            IsEnabled = false;

            TypeExtensionsPatch.Disable(_harmony);
            //DefinitionContextPatch.Disable(_harmony);
        }
    }
}