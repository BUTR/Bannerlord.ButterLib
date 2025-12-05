using HarmonyLib.BUTR.Extensions;

using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers2;
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

    // Added in v1.2.0
    private delegate void OnNetworkTickDelegate(float dt);

    // Added in v1.3.0
    private delegate void OnNewModuleLoadDelegate();
    private delegate void RegisterSubModuleTypesDelegate();
    private delegate void InitializeSubModuleGameObjectsDelegate(Game game);
    private delegate void OnAfterGameLoadedDelegate(Game game);
    private delegate void OnBeforeGameStartDelegate(MBGameManager mbGameManager, List<string> disabledModules);
    private delegate void OnSubModuleActivatedDelegate();
    private delegate void OnSubModuleDeactivatedDelegate();


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

    // Added in v1.2.0
    private OnNetworkTickDelegate? OnNetworkTickInstance { get; }

    // Added in v1.3.0
    private OnNewModuleLoadDelegate? OnNewModuleLoadInstance { get; }
    private RegisterSubModuleTypesDelegate? RegisterSubModuleTypesInstance { get; }
    private InitializeSubModuleGameObjectsDelegate? InitializeSubModuleGameObjectsInstance { get; }
    private OnAfterGameLoadedDelegate? OnAfterGameLoadedInstance { get; }
    private OnBeforeGameStartDelegate? OnBeforeGameStartInstance { get; }
    private OnSubModuleActivatedDelegate? OnSubModuleActivatedInstance { get; }
    private OnSubModuleDeactivatedDelegate? OnSubModuleDeactivatedInstance { get; }


    public MBSubModuleBase SubModule { get; }

    public MBSubModuleBaseWrapper(MBSubModuleBase subModule)
    {
        SubModule = subModule;

        OnSubModuleLoadInstance = AccessTools2.GetDelegate<OnSubModuleLoadDelegate, MBSubModuleBase>(subModule, nameof(OnSubModuleLoad));
        OnSubModuleUnloadedInstance = AccessTools2.GetDelegate<OnSubModuleUnloadedDelegate, MBSubModuleBase>(subModule, nameof(OnSubModuleUnloaded));
        OnBeforeInitialModuleScreenSetAsRootInstance = AccessTools2.GetDelegate<OnBeforeInitialModuleScreenSetAsRootDelegate, MBSubModuleBase>(subModule, nameof(OnBeforeInitialModuleScreenSetAsRoot));
        OnGameStartInstance = AccessTools2.GetDelegate<OnGameStartDelegate, MBSubModuleBase>(subModule, nameof(OnGameStart));
        OnApplicationTickInstance = AccessTools2.GetDelegate<OnApplicationTickDelegate, MBSubModuleBase>(subModule, nameof(OnApplicationTick));
        InitializeGameStarterInstance = AccessTools2.GetDelegate<InitializeGameStarterDelegate, MBSubModuleBase>(subModule, nameof(InitializeGameStarter));

        OnServiceRegistrationInstance = AccessTools2.GetDelegate<OnServiceRegistrationDelegate, MBSubModuleBase>(subModule, nameof(OnServiceRegistration), logErrorInTrace: false);

        OnConfigChangedInstance = AccessTools2.GetDelegate<OnConfigChangedDelegate, MBSubModuleBase>(subModule, nameof(OnConfigChanged));
        OnGameLoadedInstance = AccessTools2.GetDelegate<OnGameLoadedDelegate, MBSubModuleBase>(subModule, nameof(OnGameLoaded));
        OnNewGameCreatedInstance = AccessTools2.GetDelegate<OnNewGameCreatedDelegate, MBSubModuleBase>(subModule, nameof(OnNewGameCreated));
        BeginGameStartInstance = AccessTools2.GetDelegate<BeginGameStartDelegate, MBSubModuleBase>(subModule, nameof(BeginGameStart));
        OnCampaignStartInstance = AccessTools2.GetDelegate<OnCampaignStartDelegate, MBSubModuleBase>(subModule, nameof(OnCampaignStart));
        RegisterSubModuleObjectsInstance = AccessTools2.GetDelegate<RegisterSubModuleObjectsDelegate, MBSubModuleBase>(subModule, nameof(RegisterSubModuleObjects));
        AfterRegisterSubModuleObjectsInstance = AccessTools2.GetDelegate<AfterRegisterSubModuleObjectsDelegate, MBSubModuleBase>(subModule, nameof(AfterRegisterSubModuleObjects));
        OnMultiplayerGameStartInstance = AccessTools2.GetDelegate<OnMultiplayerGameStartDelegate, MBSubModuleBase>(subModule, nameof(OnMultiplayerGameStart));
        OnGameInitializationFinishedInstance = AccessTools2.GetDelegate<OnGameInitializationFinishedDelegate, MBSubModuleBase>(subModule, nameof(OnGameInitializationFinished));
        OnAfterGameInitializationFinishedInstance = AccessTools2.GetDelegate<OnAfterGameInitializationFinishedDelegate, MBSubModuleBase>(subModule, nameof(OnAfterGameInitializationFinished));
        DoLoadingInstance = AccessTools2.GetDelegate<DoLoadingDelegate, MBSubModuleBase>(subModule, nameof(DoLoading));
        OnGameEndInstance = AccessTools2.GetDelegate<OnGameEndDelegate, MBSubModuleBase>(subModule, nameof(OnGameEnd));
        OnBeforeMissionBehaviourInitializeInstance = AccessTools2.GetDelegate<OnBeforeMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, nameof(OnBeforeMissionBehaviorInitialize));
        OnMissionBehaviourInitializeInstance = AccessTools2.GetDelegate<OnMissionBehaviourInitializeDelegate, MBSubModuleBase>(subModule, nameof(OnMissionBehaviorInitialize));
        OnInitialStateInstance = AccessTools2.GetDelegate<OnInitialStateDelegate, MBSubModuleBase>(subModule, nameof(OnInitialState));
        AfterAsyncTickTickInstance = AccessTools2.GetDelegate<AfterAsyncTickTickDelegate, MBSubModuleBase>(subModule, nameof(AfterAsyncTickTick));

        // Added in v1.2.0
        OnNetworkTickInstance = AccessTools2.GetDelegate<OnNetworkTickDelegate, MBSubModuleBase>(subModule, nameof(OnNetworkTick), logErrorInTrace: false);

        // Added in v1.3.0
        OnNewModuleLoadInstance = AccessTools2.GetDelegate<OnNewModuleLoadDelegate, MBSubModuleBase>(subModule, nameof(OnNewModuleLoad), logErrorInTrace: false);
        RegisterSubModuleTypesInstance = AccessTools2.GetDelegate<RegisterSubModuleTypesDelegate, MBSubModuleBase>(subModule, nameof(RegisterSubModuleTypes), logErrorInTrace: false);
        InitializeSubModuleGameObjectsInstance = AccessTools2.GetDelegate<InitializeSubModuleGameObjectsDelegate, MBSubModuleBase>(subModule, nameof(InitializeSubModuleGameObjects), logErrorInTrace: false);
        OnAfterGameLoadedInstance = AccessTools2.GetDelegate<OnAfterGameLoadedDelegate, MBSubModuleBase>(subModule, nameof(OnAfterGameLoaded), logErrorInTrace: false);
        OnBeforeGameStartInstance = AccessTools2.GetDelegate<OnBeforeGameStartDelegate, MBSubModuleBase>(subModule, nameof(OnBeforeGameStart), logErrorInTrace: false);
        OnSubModuleActivatedInstance = AccessTools2.GetDelegate<OnSubModuleActivatedDelegate, MBSubModuleBase>(subModule, nameof(OnSubModuleActivated), logErrorInTrace: false);
        OnSubModuleDeactivatedInstance = AccessTools2.GetDelegate<OnSubModuleDeactivatedDelegate, MBSubModuleBase>(subModule, nameof(OnSubModuleDeactivated), logErrorInTrace: false);
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
    public new virtual void OnBeforeMissionBehaviorInitialize(Mission mission) => OnBeforeMissionBehaviourInitializeInstance?.Invoke(mission);
    public new virtual void OnMissionBehaviorInitialize(Mission mission) => OnMissionBehaviourInitializeInstance?.Invoke(mission);
    public new virtual void OnMultiplayerGameStart(Game game, object starterObject) => OnMultiplayerGameStartInstance?.Invoke(game, starterObject);
    public new virtual void OnNewGameCreated(Game game, object initializerObject) => OnNewGameCreatedInstance?.Invoke(game, initializerObject);
    public new virtual void RegisterSubModuleObjects(bool isSavedCampaign) => RegisterSubModuleObjectsInstance?.Invoke(isSavedCampaign);
    public new virtual void AfterRegisterSubModuleObjects(bool isSavedCampaign) => AfterRegisterSubModuleObjectsInstance?.Invoke(isSavedCampaign);
    public new virtual void OnAfterGameInitializationFinished(Game game, object starterObject) => OnAfterGameInitializationFinishedInstance?.Invoke(game, starterObject);
    public new virtual void OnConfigChanged() => OnConfigChangedInstance?.Invoke();
    public new virtual void OnInitialState() => OnInitialStateInstance?.Invoke();
    public new virtual void AfterAsyncTickTick(float dt) => AfterAsyncTickTickInstance?.Invoke(dt);

    // Added in v1.2.0
    public new virtual void OnNetworkTick(float dt) => OnNetworkTickInstance?.Invoke(dt);

    // Added in v1.3.0
    public new virtual void OnNewModuleLoad() => OnNewModuleLoadInstance?.Invoke();
    public new virtual void RegisterSubModuleTypes() => RegisterSubModuleTypesInstance?.Invoke();
    public new virtual void InitializeSubModuleGameObjects(Game game) => InitializeSubModuleGameObjectsInstance?.Invoke(game);
    public new virtual void OnAfterGameLoaded(Game game) => OnAfterGameLoadedInstance?.Invoke(game);
    public new virtual void OnBeforeGameStart(MBGameManager mbGameManager, List<string> disabledModules) => OnBeforeGameStartInstance?.Invoke(mbGameManager, disabledModules);
    public new virtual void OnSubModuleActivated() => OnSubModuleActivatedInstance?.Invoke();
    public new virtual void OnSubModuleDeactivated() => OnSubModuleDeactivatedInstance?.Invoke();
}
#pragma warning restore CS0109 // ReSharper restore VirtualMemberNeverOverridden.Global