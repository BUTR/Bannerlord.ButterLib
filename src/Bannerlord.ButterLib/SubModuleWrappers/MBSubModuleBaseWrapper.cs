using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers
{
    /// <summary>
    /// Wraps a <see cref="MBSubModuleBase"/> so protected methods could be called
    /// without a performance hit
    /// </summary>
    public class MBSubModuleBaseWrapper : MBSubModuleBase
    {
        private delegate void OnSubModuleLoadDelegate();
        private delegate void OnSubModuleUnloadedDelegate();
        private delegate void OnBeforeInitialModuleScreenSetAsRootDelegate();
        private delegate void OnGameStartDelegate(Game game, IGameStarter gameStarterObject);
        private delegate void OnApplicationTickDelegate(float dt);
        private delegate void OnServiceRegistrationDelegate();


        private OnSubModuleLoadDelegate? OnSubModuleLoadInstance { get; }
        private OnSubModuleUnloadedDelegate? OnSubModuleUnloadedInstance { get; }
        private OnBeforeInitialModuleScreenSetAsRootDelegate? OnBeforeInitialModuleScreenSetAsRootInstance { get; }
        private OnGameStartDelegate? OnGameStartInstance { get; }
        private OnApplicationTickDelegate? OnApplicationTickInstance { get; }
        private OnServiceRegistrationDelegate? OnServiceRegistrationInstance { get; }

        private MBSubModuleBase SubModule { get; }

        public MBSubModuleBaseWrapper(MBSubModuleBase subModule)
        {
            SubModule = subModule;

            OnSubModuleLoadInstance = AccessTools2.GetDelegate<OnSubModuleLoadDelegate, MBSubModuleBase>(subModule, "OnSubModuleLoad");
            OnSubModuleUnloadedInstance = AccessTools2.GetDelegate<OnSubModuleUnloadedDelegate, MBSubModuleBase>(subModule, "OnSubModuleUnloaded");
            OnBeforeInitialModuleScreenSetAsRootInstance = AccessTools2.GetDelegate<OnBeforeInitialModuleScreenSetAsRootDelegate, MBSubModuleBase>(subModule, "OnBeforeInitialModuleScreenSetAsRoot");
            OnGameStartInstance = AccessTools2.GetDelegate<OnGameStartDelegate, MBSubModuleBase>(subModule, "OnGameStart");
            OnApplicationTickInstance = AccessTools2.GetDelegate<OnApplicationTickDelegate, MBSubModuleBase>(subModule, "OnApplicationTick");
            OnServiceRegistrationInstance = AccessTools2.GetDelegate<OnServiceRegistrationDelegate, MBSubModuleBase>(subModule, "OnServiceRegistration");
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            OnSubModuleLoadInstance?.Invoke();
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            OnSubModuleUnloadedInstance?.Invoke();
        }
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            OnApplicationTickInstance?.Invoke(dt);
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            OnBeforeInitialModuleScreenSetAsRootInstance?.Invoke();
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            OnGameStartInstance?.Invoke(game, gameStarterObject);
        }

        /// <exclude/>
        public void OnServiceRegistration() => OnServiceRegistrationInstance?.Invoke();
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

            return SubModule.DoLoading(game);
        }
        /// <exclude/>
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            SubModule.OnGameLoaded(game, initializerObject);
        }
        /// <exclude/>
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            SubModule.OnCampaignStart(game, starterObject);
        }
        /// <exclude/>
        public override void BeginGameStart(Game game)
        {
            base.BeginGameStart(game);

            SubModule.BeginGameStart(game);
        }
        /// <exclude/>
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            SubModule.OnGameEnd(game);
        }
        /// <exclude/>
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            SubModule.OnGameInitializationFinished(game);
        }
        /* They renamed it in e165
        /// <exclude/>
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            SubModule.OnMissionBehaviourInitialize(mission);
        }
        */
        /// <exclude/>
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);

            SubModule.OnMultiplayerGameStart(game, starterObject);
        }
        /// <exclude/>
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);

            SubModule.OnNewGameCreated(game, initializerObject);
        }
    }
}