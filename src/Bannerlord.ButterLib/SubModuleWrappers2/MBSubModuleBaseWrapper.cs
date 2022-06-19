using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers2
{
#pragma warning disable CS0109 // ReSharper disable VirtualMemberNeverOverridden.Global
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
        private delegate void InitializeGameStarterDelegate(Game game, IGameStarter starterObject);

        private delegate void OnServiceRegistrationDelegate();

        private delegate void OnConfigChangedDelegate();
        private delegate void OnGameLoadedDelegate(Game game, object initializerObject);
        private delegate void OnNewGameCreatedDelegate(Game game, object initializerObject);
        private delegate void BeginGameStartDelegate(Game game);
        private delegate void OnCampaignStartDelegate(Game game, object starterObject);
        private delegate void RegisterSubModuleObjectsDelegate(bool isSavedCampaign);
        private delegate void AfterRegisterSubModuleObjectsDelegate(bool isSavedCampaign);
        private delegate void OnMultiplayerGameStartDelegate(Game game, object starterObject);
        private delegate void OnGameInitializationFinishedDelegate(Game game);
        private delegate void OnAfterGameInitializationFinishedDelegate(Game game, object starterObject);
        private delegate bool DoLoadingDelegate(Game game);
        private delegate void OnGameEndDelegate(Game game);
        private delegate void OnBeforeMissionBehaviourInitializeDelegate(Mission mission);
        private delegate void OnMissionBehaviourInitializeDelegate(Mission mission);
        private delegate void OnInitialStateDelegate();
        private delegate void AfterAsyncTickTickDelegate(float dt);


        private OnSubModuleLoadDelegate? OnSubModuleLoadInstance { get; }
        private OnSubModuleUnloadedDelegate? OnSubModuleUnloadedInstance { get; }
        private OnBeforeInitialModuleScreenSetAsRootDelegate? OnBeforeInitialModuleScreenSetAsRootInstance { get; }
        private OnGameStartDelegate? OnGameStartInstance { get; }
        private OnApplicationTickDelegate? OnApplicationTickInstance { get; }
        private InitializeGameStarterDelegate? InitializeGameStarterInstance { get; }

        private OnServiceRegistrationDelegate? OnServiceRegistrationInstance { get; }

        private OnConfigChangedDelegate? OnConfigChangedInstance { get; }
        private OnGameLoadedDelegate? OnGameLoadedInstance { get; }
        private OnNewGameCreatedDelegate? OnNewGameCreatedInstance { get; }
        private BeginGameStartDelegate? BeginGameStartInstance { get; }
        private OnCampaignStartDelegate? OnCampaignStartInstance { get; }
        private RegisterSubModuleObjectsDelegate? RegisterSubModuleObjectsInstance { get; }
        private AfterRegisterSubModuleObjectsDelegate? AfterRegisterSubModuleObjectsInstance { get; }
        private OnMultiplayerGameStartDelegate? OnMultiplayerGameStartInstance { get; }
        private OnGameInitializationFinishedDelegate? OnGameInitializationFinishedInstance { get; }
        private OnAfterGameInitializationFinishedDelegate? OnAfterGameInitializationFinishedInstance { get; }
        private DoLoadingDelegate? DoLoadingInstance { get; }
        private OnGameEndDelegate? OnGameEndInstance { get; }
        private OnBeforeMissionBehaviourInitializeDelegate? OnBeforeMissionBehaviourInitializeInstance { get; }
        private OnMissionBehaviourInitializeDelegate? OnMissionBehaviourInitializeInstance { get; }
        private OnInitialStateDelegate? OnInitialStateInstance { get; }
        private AfterAsyncTickTickDelegate? AfterAsyncTickTickInstance { get; }


        public MBSubModuleBase SubModule { get; }

        public MBSubModuleBaseWrapper(MBSubModuleBase subModule)
        {
            SubModule = subModule;

            OnSubModuleLoadInstance = AccessTools2.GetDelegate<OnSubModuleLoadDelegate, MBSubModuleBase>(subModule, "OnSubModuleLoad");
            OnSubModuleUnloadedInstance = AccessTools2.GetDelegate<OnSubModuleUnloadedDelegate, MBSubModuleBase>(subModule, "OnSubModuleUnloaded");
            OnBeforeInitialModuleScreenSetAsRootInstance = AccessTools2.GetDelegate<OnBeforeInitialModuleScreenSetAsRootDelegate, MBSubModuleBase>(subModule, "OnBeforeInitialModuleScreenSetAsRoot");
            OnGameStartInstance = AccessTools2.GetDelegate<OnGameStartDelegate, MBSubModuleBase>(subModule, "OnGameStart");
            OnApplicationTickInstance = AccessTools2.GetDelegate<OnApplicationTickDelegate, MBSubModuleBase>(subModule, "OnApplicationTick");
            InitializeGameStarterInstance = AccessTools2.GetDelegate<InitializeGameStarterDelegate, MBSubModuleBase>(subModule, "InitializeGameStarter");

            OnServiceRegistrationInstance = AccessTools2.GetDelegate<OnServiceRegistrationDelegate, MBSubModuleBase>(subModule, "OnServiceRegistration");

            OnConfigChangedInstance = AccessTools2.GetDelegate<OnConfigChangedDelegate, MBSubModuleBase>(subModule, "OnConfigChanged");
            OnGameLoadedInstance = AccessTools2.GetDelegate<OnGameLoadedDelegate, MBSubModuleBase>(subModule, "OnGameLoaded");
            OnNewGameCreatedInstance = AccessTools2.GetDelegate<OnNewGameCreatedDelegate, MBSubModuleBase>(subModule, "OnNewGameCreated");
            BeginGameStartInstance = AccessTools2.GetDelegate<BeginGameStartDelegate, MBSubModuleBase>(subModule, "BeginGameStart");
            OnCampaignStartInstance = AccessTools2.GetDelegate<OnCampaignStartDelegate, MBSubModuleBase>(subModule, "OnCampaignStart");
            RegisterSubModuleObjectsInstance = AccessTools2.GetDelegate<RegisterSubModuleObjectsDelegate, MBSubModuleBase>(subModule, "RegisterSubModuleObjects");
            AfterRegisterSubModuleObjectsInstance = AccessTools2.GetDelegate<AfterRegisterSubModuleObjectsDelegate, MBSubModuleBase>(subModule, "AfterRegisterSubModuleObjects");
            OnMultiplayerGameStartInstance = AccessTools2.GetDelegate<OnMultiplayerGameStartDelegate, MBSubModuleBase>(subModule, "OnMultiplayerGameStart");
            OnGameInitializationFinishedInstance = AccessTools2.GetDelegate<OnGameInitializationFinishedDelegate, MBSubModuleBase>(subModule, "OnGameInitializationFinished");
            OnAfterGameInitializationFinishedInstance = AccessTools2.GetDelegate<OnAfterGameInitializationFinishedDelegate, MBSubModuleBase>(subModule, "OnAfterGameInitializationFinished");
            DoLoadingInstance = AccessTools2.GetDelegate<DoLoadingDelegate, MBSubModuleBase>(subModule, "DoLoading");
            OnGameEndInstance = AccessTools2.GetDelegate<OnGameEndDelegate, MBSubModuleBase>(subModule, "OnGameEnd");
            OnBeforeMissionBehaviourInitializeInstance = AccessTools2.GetDelegate<OnBeforeMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, "OnBeforeMissionBehaviorInitialize")
                                                         ?? AccessTools2.GetDelegate<OnBeforeMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, "OnBeforeMissionBehaviourInitialize");
            OnMissionBehaviourInitializeInstance = AccessTools2.GetDelegate<OnMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, "OnMissionBehaviorInitialize")
                                                  ?? AccessTools2.GetDelegate<OnMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, "OnMissionBehaviourInitialize");
            OnInitialStateInstance = AccessTools2.GetDelegate<OnInitialStateDelegate, MBSubModuleBase>(subModule, "OnInitialState");
            AfterAsyncTickTickInstance = AccessTools2.GetDelegate<AfterAsyncTickTickDelegate, MBSubModuleBase>(subModule, "AfterAsyncTickTick");
        }

        public new virtual void OnSubModuleLoad() => OnSubModuleLoadInstance?.Invoke();
        public new virtual void OnSubModuleUnloaded() => OnSubModuleUnloadedInstance?.Invoke();
        public new virtual void OnApplicationTick(float dt) => OnApplicationTickInstance?.Invoke(dt);
        public new virtual void OnBeforeInitialModuleScreenSetAsRoot() => OnBeforeInitialModuleScreenSetAsRootInstance?.Invoke();
        public new virtual void OnGameStart(Game game, IGameStarter gameStarterObject) => OnGameStartInstance?.Invoke(game, gameStarterObject);
        public new virtual void InitializeGameStarter(Game game, IGameStarter starterObject) => InitializeGameStarterInstance?.Invoke(game, starterObject);

        public new virtual void OnServiceRegistration() => OnServiceRegistrationInstance?.Invoke();

        public new virtual bool DoLoading(Game game) => DoLoadingInstance?.Invoke(game) ?? false;
        public new virtual void OnGameLoaded(Game game, object initializerObject) => OnGameLoadedInstance?.Invoke(game, initializerObject);
        public new virtual void OnCampaignStart(Game game, object starterObject) => OnCampaignStartInstance?.Invoke(game, starterObject);
        public new virtual void BeginGameStart(Game game) => BeginGameStartInstance?.Invoke(game);
        public new virtual void OnGameEnd(Game game) => OnGameEndInstance?.Invoke(game);
        public new virtual void OnGameInitializationFinished(Game game) => OnGameInitializationFinishedInstance?.Invoke(game);
        public new virtual void OnBeforeMissionBehaviourInitialize(Mission mission) => OnBeforeMissionBehaviourInitializeInstance?.Invoke(mission);
        public new virtual void OnMissionBehaviourInitialize(Mission mission) => OnMissionBehaviourInitializeInstance?.Invoke(mission);
        public new virtual void OnMultiplayerGameStart(Game game, object starterObject) => OnMultiplayerGameStartInstance?.Invoke(game, starterObject);
        public new virtual void OnNewGameCreated(Game game, object initializerObject) => OnNewGameCreatedInstance?.Invoke(game, initializerObject);
        public new virtual void RegisterSubModuleObjects(bool isSavedCampaign) => RegisterSubModuleObjectsInstance?.Invoke(isSavedCampaign);
        public new virtual void AfterRegisterSubModuleObjects(bool isSavedCampaign) => AfterRegisterSubModuleObjectsInstance?.Invoke(isSavedCampaign);
        public new virtual void OnAfterGameInitializationFinished(Game game, object starterObject) => OnAfterGameInitializationFinishedInstance?.Invoke(game, starterObject);
        public new virtual void OnConfigChanged() => OnConfigChangedInstance?.Invoke();
        public new virtual void OnInitialState() => OnInitialStateInstance?.Invoke();
        public new virtual void AfterAsyncTickTick(float dt) => AfterAsyncTickTickInstance?.Invoke(dt);
    }
#pragma warning restore CS0109 // ReSharper restore VirtualMemberNeverOverridden.Global
}