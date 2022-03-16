using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers
{
    /// <summary>
    /// Wraps several <see cref="MBSubModuleBase"/> so when the game calls any method, it
    /// will be passed to its children.
    /// </summary>
    public class MBSubModuleBaseListWrapper : MBSubModuleBase
    {
        protected List<MBSubModuleBaseWrapper> SubModules { get; } = new();

        public virtual void OnServiceRegistration()
        {
            foreach (var subModule in SubModules)
                subModule.OnServiceRegistration();
        }
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            foreach (var subModule in SubModules)
                subModule.SubModuleLoad();
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            foreach (var subModule in SubModules)
                subModule.SubModuleUnloaded();
        }
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            foreach (var subModule in SubModules)
                subModule.ApplicationTick(dt);
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            foreach (var subModule in SubModules)
                subModule.BeforeInitialModuleScreenSetAsRoot();
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            foreach (var subModule in SubModules)
                subModule.GameStart(game, gameStarterObject);
        }

        /// <exclude/>
        public void SubModuleLoad() => OnSubModuleLoad();
        /// <exclude/>
        public void SubModuleUnloaded() => OnSubModuleUnloaded();
        /// <exclude/>
        public void ApplicationTick(float dt) => OnApplicationTick(dt);
        /// <exclude/>
        public void BeforeInitialModuleScreenSetAsRoot() => OnBeforeInitialModuleScreenSetAsRoot();
        /// <exclude/>
        public void GameStart(Game game, IGameStarter gameStarterObject) => OnGameStart(game, gameStarterObject);
        /// <exclude/>
        public override bool DoLoading(Game game)
        {
            if (!base.DoLoading(game))
                return false;

            return SubModules.All(subModule => subModule.DoLoading(game));
        }
        /// <exclude/>
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            foreach (var subModule in SubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        /// <exclude/>
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            foreach (var subModule in SubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        /// <exclude/>
        public override void BeginGameStart(Game game)
        {
            base.BeginGameStart(game);

            foreach (var subModule in SubModules)
                subModule.BeginGameStart(game);
        }
        /// <exclude/>
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            foreach (var subModule in SubModules)
                subModule.OnGameEnd(game);
        }
        /// <exclude/>
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            foreach (var subModule in SubModules)
                subModule.OnGameInitializationFinished(game);
        }
        /// <exclude/>
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            foreach (var subModule in SubModules)
                subModule.OnMissionBehaviorInitialize(mission);
        }
        /// <exclude/>
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);

            foreach (var subModule in SubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        /// <exclude/>
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);

            foreach (var subModule in SubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
    }
}