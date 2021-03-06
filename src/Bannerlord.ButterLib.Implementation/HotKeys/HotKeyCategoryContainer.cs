﻿/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyCategoryContainer.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.HotKeys;

using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeyCategoryContainer : GameKeyContext
    {
        private static int ListCapacity = 300;

        public HotKeyCategoryContainer(string categoryId, IEnumerable<HotKeyBase> keys) : base(categoryId, ListCapacity)
        {
            var keyName = Module.CurrentModule.GlobalTextManager.AddGameText("str_key_name");
            var keyDesc = Module.CurrentModule.GlobalTextManager.AddGameText("str_key_description");
            var variationString = $"{categoryId}_";
            foreach (var key in keys)
            {
                keyName.AddVariationWithId($"{variationString}{key.Id}", TextObjectHelper.Create(key.DisplayName), new List<GameTextManager.ChoiceTag>());
                keyDesc.AddVariationWithId($"{variationString}{key.Id}", TextObjectHelper.Create(key.Description), new List<GameTextManager.ChoiceTag>());
                RegisterGameKey(new GameKey(key.Id, key.Uid, categoryId, key.DefaultKey, key.Category));
            }
        }
    }
}