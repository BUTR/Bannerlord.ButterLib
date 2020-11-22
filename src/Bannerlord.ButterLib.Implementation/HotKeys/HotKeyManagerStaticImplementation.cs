/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.HotKeys;

using System;
using System.Linq;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeyManagerStaticImplementation : IHotKeyManagerStatic
    {
        public HotKeyManager Create(string modName)
        {
            var doesModAlreadyHaveRegisteredKeys = TaleWorlds.InputSystem.HotKeyManager.GetAllCategories()
                .Any(x => string.Equals(x.GameKeyCategoryId, modName, StringComparison.OrdinalIgnoreCase));
            if (doesModAlreadyHaveRegisteredKeys)
                throw new ArgumentException("Hotkeys For Mod With This Name Already Exists.", nameof(modName));
            var idMax = 0;
            foreach (var category in TaleWorlds.InputSystem.HotKeyManager.GetAllCategories())
            {
                foreach (var gameKey in category.RegisteredGameKeys.Where(gameKey => gameKey is not null && gameKey.Id > idMax))
                {
                    idMax = gameKey.Id + 1;
                }
            }
            if (idMax < 73)
                idMax = 73;

            return new HotKeyManagerImplementation(idMax, modName);
        }
    }
}