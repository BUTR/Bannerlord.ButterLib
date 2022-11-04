/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using System.Collections.Generic;

namespace Bannerlord.ButterLib.HotKeys
{
    internal interface IHotKeyManagerStatic
    {
        IList<HotKeyBase> HotKeys { get; }

        HotKeyManager Create(string modName);

        HotKeyManager CreateWithOwnCategory(string modName, string categoryName);
    }
}