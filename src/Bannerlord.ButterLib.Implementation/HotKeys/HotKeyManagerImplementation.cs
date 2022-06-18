/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.HotKeys;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using HotKeyManager = Bannerlord.ButterLib.HotKeys.HotKeyManager;
using TWHotKeyManager = TaleWorlds.InputSystem.HotKeyManager;

namespace Bannerlord.ButterLib.Implementation.HotKeys
{
    internal sealed class HotKeyManagerImplementation : HotKeyManager
    {
        internal static readonly List<HotKeyBase> HotKeys = new();

        private int _currentId = 150; // should be enough no prevent collision with the game
        private readonly string _subModName;
        private readonly List<HotKeyBase> _hotKeys = new();

        internal HotKeyManagerImplementation(string subModName)
        {
            _subModName = subModName;
        }

        public override T Add<T>(T hotkey)
        {
            if (_hotKeys.Any(x => string.Equals(x.Uid, hotkey.Uid, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"A hotkey called {hotkey.Uid} already exists", nameof(hotkey));
            _hotKeys.Add(hotkey);
            hotkey.Id = _currentId;
            _currentId++;
            return hotkey;
        }

        public override T Add<T>() => Add(new T());

        public override IReadOnlyList<HotKeyBase> Build()
        {
            var hotKeyCategoryContainer = new HotKeyCategoryContainer(_subModName, _hotKeys);

#if e172
            TWHotKeyManager.Initialize(new PlatformFilePath(EngineFilePaths.ConfigsPath, "BannerlordGameKeys.xml"), new List<GameKeyContext> { hotKeyCategoryContainer }, true);
#elif e180
            TWHotKeyManager.Initialize(new PlatformFilePath(EngineFilePaths.ConfigsPath, "BannerlordGameKeys.xml"), true);
#endif
            var keys = hotKeyCategoryContainer.RegisteredGameKeys;
            foreach (var hotKey in _hotKeys)
            {
                foreach (var gameKey in keys.Where(gameKey => gameKey is not null && string.Equals(gameKey.StringId, hotKey.Uid, StringComparison.OrdinalIgnoreCase)))
                {
                    hotKey.GameKey = gameKey;
                }
            }
            HotKeys.AddRange(_hotKeys);
            return _hotKeys;
        }
    }
}