/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyCategoryContainer.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.HotKeys;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeyCategoryContainer : GameKeyContext
    {
        private static readonly int ListCapacity = Enum.GetValues(typeof(GameKeyDefinition)).Cast<int>().Max() + 200;

        public HotKeyCategoryContainer(string categoryId, string categoryName, IEnumerable<HotKeyBase> keys) : base(categoryId, ListCapacity)
        {
            var keyCategoryName = Module.CurrentModule.GlobalTextManager.AddGameText("str_key_category_name");
            keyCategoryName.AddVariationWithId(categoryId, new TextObject(categoryName), new List<GameTextManager.ChoiceTag>());

            var keyName = Module.CurrentModule.GlobalTextManager.AddGameText("str_key_name");
            var keyDesc = Module.CurrentModule.GlobalTextManager.AddGameText("str_key_description");
            foreach (var key in keys)
            {
                var variationId = $"{categoryId}_{(GameKeyDefinition) key.Id}";
                keyName.AddVariationWithId(variationId, new TextObject(key.DisplayName), new List<GameTextManager.ChoiceTag>());
                keyDesc.AddVariationWithId(variationId, new TextObject(key.Description), new List<GameTextManager.ChoiceTag>());
                RegisterGameKey(new GameKey(key.Id, key.Uid, categoryId, key.DefaultKey, key.Category));
            }
        }
    }
}