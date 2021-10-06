﻿using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.MBSubModuleBaseExtended
{
    /// <summary>
    /// A <see cref="T:TaleWorlds.MountAndBlade.MBSubModuleBase" />-derived abstract class, that provides new SubModule events.
    /// </summary>
    public abstract class MBSubModuleBaseEx : MBSubModuleBase, IMBSubModuleBaseEx
    {
        /// <summary>
        /// Event that takes place before the module is loaded into the game. Butterlib calls this inside the <see cref="ButterLibSubModule.OnSubModuleLoad"/> for every <see cref="IMBSubModuleBaseEx"/>-derived MBSubModule loaded by the game.
        /// </summary>
        /// <remarks>Any ServiceCollection registration should go here.</remarks>
        public virtual void OnBeforeSubModuleLoad() { }
        /// <summary>
        /// Event that takes place right after OnSubModuleUnloaded events of all loaded submodules were handled.
        /// </summary>
        public virtual void OnAllSubModulesUnLoaded() { }
        /// <summary>
        /// Event that takes place right after OnBeforeInitialModuleScreenSetAsRoot events of all loaded submodules were handled.
        /// </summary>
        public virtual void OnBeforeInitialModuleScreenSetAsRootDelayed() { }
        /// <summary>
        /// Event that takes place right after OnGameStart events of all loaded submodules were handled.
        /// </summary>
        public virtual void OnGameStartDelayed(Game game, IGameStarter gameStarterObject) { }
        /// <summary>
        /// Event that takes place right after OnGameEnd events of all loaded submodules were handled.
        /// </summary>
        public virtual void OnGameEndDelayed(Game game) { }
    }
}