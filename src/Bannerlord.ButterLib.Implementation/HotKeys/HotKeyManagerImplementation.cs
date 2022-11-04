/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.HotKeys;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.InputSystem;

using HotKeyManager = Bannerlord.ButterLib.HotKeys.HotKeyManager;
using TWHotKeyManager = TaleWorlds.InputSystem.HotKeyManager;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeyManagerImplementation : HotKeyManager
    {
        internal static readonly List<HotKeyCategoryContainer> GlobalContainerStorage = new();
        internal static readonly List<HotKeyBase> GlobalHotKeyStorage = new();

        private int _currentId = 0; // To prevent collision with the game
        private readonly string _modName;
        private readonly string _categoryName;
        private readonly List<HotKeyBase> _instanceHotKeys = new();

        internal HotKeyManagerImplementation(string modName, string categoryName = "")
        {
            _modName = modName;
            _categoryName = categoryName;
        }

        public override T Add<T>(T hotkey)
        {
            if (GlobalHotKeyStorage.Any(x => string.Equals(x.Uid, hotkey.Uid, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"A hotkey called {hotkey.Uid} already exists", nameof(hotkey));

            _instanceHotKeys.Add(hotkey);
            hotkey.Id = _currentId;
            _currentId++;
            return hotkey;
        }

        public override T Add<T>() => Add(new T());

        public override IReadOnlyList<HotKeyBase> Build()
        {
            if (GlobalContainerStorage.Any(x => string.Equals(x.GameKeyCategoryId, _modName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"A hotkey category called {_modName} already exists", nameof(_modName));

            var hotKeyCategoryContainer = new HotKeyCategoryContainer(_modName, _categoryName, _instanceHotKeys);

            TWHotKeyManager.RegisterInitialContexts(new List<GameKeyContext> { hotKeyCategoryContainer }, true);
            GlobalContainerStorage.Add(hotKeyCategoryContainer);

            var keys = hotKeyCategoryContainer.RegisteredGameKeys;
            foreach (var hotKey in _instanceHotKeys)
            {
                foreach (var gameKey in keys.Where(gameKey => gameKey is not null && string.Equals(gameKey.StringId, hotKey.Uid, StringComparison.OrdinalIgnoreCase)))
                {
                    hotKey.GameKey = gameKey;
                }
            }
            GlobalHotKeyStorage.AddRange(_instanceHotKeys);
            return _instanceHotKeys;
        }
    }
}