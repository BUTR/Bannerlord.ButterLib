using TaleWorlds.Core;

namespace Bannerlord.ButterLib.MBSubModuleBaseExtended
{
    /// <summary>
    /// An interface to use in your <see cref="T:TaleWorlds.MountAndBlade.MBSubModuleBase" />-derived class. Provides new SubModule events.
    /// </summary>
    public interface IMBSubModuleBaseEx
    {
        /// <summary>
        /// Event that takes place right after OnSubModuleUnloaded events of all loaded submodules were handled.
        /// </summary>
        public void OnAllSubModulesUnLoaded();
        /// <summary>
        /// Event that takes place right after OnBeforeInitialModuleScreenSetAsRoot events of all loaded submodules were handled.
        /// </summary>
        public void OnBeforeInitialModuleScreenSetAsRootDelayed();
        /// <summary>
        /// Event that takes place right after OnGameStart events of all loaded submodules were handled.
        /// </summary>
        public void OnGameStartDelayed(Game game, IGameStarter gameStarterObject);
        /// <summary>
        /// Event that takes place right after OnGameEnd events of all loaded submodules were handled.
        /// </summary>
        public void OnGameEndDelayed(Game game);
    }
}