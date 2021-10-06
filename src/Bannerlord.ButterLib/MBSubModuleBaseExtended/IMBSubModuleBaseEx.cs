using TaleWorlds.Core;

namespace Bannerlord.ButterLib.MBSubModuleBaseExtended
{
    /// <summary>
    /// An interface to use in your <see cref="T:TaleWorlds.MountAndBlade.MBSubModuleBase" />-derived class. Provides new SubModule events.
    /// </summary>
    public interface IMBSubModuleBaseEx
    {
        /// <summary>
        /// Event that takes place before the module is loaded into the game. Butterlib calls this inside the <see cref="ButterLibSubModule.OnSubModuleLoad"/> for every <see cref="IMBSubModuleBaseEx"/>-derived MBSubModule loaded by the game.
        /// </summary>
        /// <remarks>Any ServiceCollection registration should go here.</remarks>
        public void OnBeforeSubModuleLoad();
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