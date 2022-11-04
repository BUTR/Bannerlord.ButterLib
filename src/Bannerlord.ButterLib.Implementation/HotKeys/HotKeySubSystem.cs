/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/InputSubModule.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.Implementation.HotKeys.Patches;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

using TaleWorlds.InputSystem;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeySubSystem : ISubSystem
    {
        public static HotKeySubSystem? Instance { get; private set; }

        public string Id => "Hot Keys";
        public string Description => "Provides a better way for mods to create hotkeys";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => false;

        private readonly Harmony _harmony = new("Bannerlord.ButterLib.HotKeySystem");

        public HotKeySubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            if (ButterLibSubModule.Instance is { } instance)
                instance.OnApplicationTickEvent += OnApplicationTick;

            OptionsProviderPatches.Enable(_harmony);
        }

        public void Disable()
        {
            IsEnabled = false;

            if (ButterLibSubModule.Instance is { } instance)
                instance.OnApplicationTickEvent -= OnApplicationTick;

            OptionsProviderPatches.Disable(_harmony);
        }

        private static void OnApplicationTick(float dt)
        {
            for (var i = 0; i < HotKeyManagerImplementation.GlobalHotKeyStorage.Count; i++)
            {
                var hotKey = HotKeyManagerImplementation.GlobalHotKeyStorage[i];
                if (!hotKey.ShouldExecute() || hotKey.GameKey is null) continue;

                if (hotKey.GameKey.KeyboardKey?.InputKey.IsDown() == true || hotKey.GameKey.ControllerKey?.InputKey.IsDown() == true)
                    hotKey.IsDownInternal();
                if (hotKey.GameKey.KeyboardKey?.InputKey.IsPressed() == true || hotKey.GameKey.ControllerKey?.InputKey.IsPressed() == true)
                    hotKey.OnPressedInternal();
                if (hotKey.GameKey.KeyboardKey?.InputKey.IsReleased() == true || hotKey.GameKey.ControllerKey?.InputKey.IsReleased() == true)
                    hotKey.OnReleasedInternal();
            }
        }
    }
}