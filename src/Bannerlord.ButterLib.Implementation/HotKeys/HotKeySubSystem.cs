/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/InputSubModule.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using TaleWorlds.InputSystem;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal static class HotKeySubSystem
    {
        public static void Enable()
        {
            if (ButterLibSubModule.Instance is { } instance)
                instance.OnApplicationTickEvent += OnApplicationTick;
        }

        public static void Disable()
        {
            if (ButterLibSubModule.Instance is { } instance)
                instance.OnApplicationTickEvent -= OnApplicationTick;
        }


        private static void OnApplicationTick(float dt)
        {
            foreach (var hotKey in HotKeyManagerImplementation.HotKeys)
            {
                if (!hotKey.ShouldExecute() || hotKey.GameKey is null) continue;

                if (hotKey.GameKey.PrimaryKey.InputKey.IsDown())
                    hotKey.IsDownInternal();
                if (hotKey.GameKey.PrimaryKey.InputKey.IsPressed())
                    hotKey.OnPressedInternal();
                if (hotKey.GameKey.PrimaryKey.InputKey.IsReleased())
                    hotKey.OnReleasedInternal();
            }
        }
    }
}