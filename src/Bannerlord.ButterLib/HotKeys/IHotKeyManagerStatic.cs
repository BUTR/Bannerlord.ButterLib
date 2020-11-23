/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

namespace Bannerlord.ButterLib.HotKeys
{
    internal interface IHotKeyManagerStatic
    {
        public HotKeyManager Create(string modName);
    }
}