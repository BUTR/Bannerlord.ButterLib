using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers2.Patches
{
    internal sealed class MBSubModuleBasePatch
    {
        private static void OnSubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnSubModuleLoad();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnSubModuleLoad();
                    break;
            }
        }
        private static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnSubModuleUnloaded();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnSubModuleUnloaded();
                    break;
            }
        }
        private static void OnApplicationTickPostfix(MBSubModuleBase __instance, ref float dt)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnApplicationTick(dt);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnApplicationTick(dt);
                    break;
            }
        }
        private static void OnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnBeforeInitialModuleScreenSetAsRoot();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnBeforeInitialModuleScreenSetAsRoot();
                    break;
            }
        }
        private static void OnGameStartPostfix(MBSubModuleBase __instance, Game game, IGameStarter gameStarterObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnGameStart(game, gameStarterObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnGameStart(game, gameStarterObject);
                    break;
            }
        }
        private static void InitializeGameStarterPostfix(MBSubModuleBase __instance, Game game, IGameStarter starterObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.InitializeGameStarter(game, starterObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.InitializeGameStarter(game, starterObject);
                    break;
            }
        }
        private static void OnServiceRegistrationPostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnServiceRegistration();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnServiceRegistration();
                    break;
            }
        }
        private static void DoLoadingPostfix(MBSubModuleBase __instance, ref bool __result, Game game)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    __result = wrapper.DoLoading(game);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    __result = listWrapper.DoLoading(game);
                    break;
            }
        }
        private static void OnGameLoadedPostfix(MBSubModuleBase __instance, Game game, object initializerObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnGameLoaded(game, initializerObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnGameLoaded(game, initializerObject);
                    break;
            }
        }
        private static void OnCampaignStartPostfix(MBSubModuleBase __instance, Game game, object starterObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnCampaignStart(game, starterObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnCampaignStart(game, starterObject);
                    break;
            }
        }
        private static void BeginGameStartPostfix(MBSubModuleBase __instance, Game game)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.BeginGameStart(game);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.BeginGameStart(game);
                    break;
            }
        }
        private static void OnGameEndPostfix(MBSubModuleBase __instance, Game game)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnGameEnd(game);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnGameEnd(game);
                    break;
            }
        }
        private static void OnGameInitializationFinishedPostfix(MBSubModuleBase __instance, Game game)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnGameInitializationFinished(game);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnGameInitializationFinished(game);
                    break;
            }
        }
        private static void OnBeforeMissionBehaviourInitializePostfix(MBSubModuleBase __instance, Mission mission)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnBeforeMissionBehaviourInitialize(mission);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnBeforeMissionBehaviourInitialize(mission);
                    break;
            }
        }
        private static void OnMissionBehaviourInitializePostfix(MBSubModuleBase __instance, Mission mission)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnMissionBehaviourInitialize(mission);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnMissionBehaviourInitialize(mission);
                    break;
            }
        }
        private static void OnMultiplayerGameStartPostfix(MBSubModuleBase __instance, Game game, object starterObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnMultiplayerGameStart(game, starterObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnMultiplayerGameStart(game, starterObject);
                    break;
            }
        }
        private static void OnNewGameCreatedPostfix(MBSubModuleBase __instance, Game game, object initializerObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnNewGameCreated(game, initializerObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnNewGameCreated(game, initializerObject);
                    break;
            }
        }
        private static void RegisterSubModuleObjectsPostfix(MBSubModuleBase __instance, bool isSavedCampaign)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.RegisterSubModuleObjects(isSavedCampaign);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.RegisterSubModuleObjects(isSavedCampaign);
                    break;
            }
        }
        private static void AfterRegisterSubModuleObjectsPostfix(MBSubModuleBase __instance, bool isSavedCampaign)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.AfterRegisterSubModuleObjects(isSavedCampaign);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.AfterRegisterSubModuleObjects(isSavedCampaign);
                    break;
            }
        }
        private static void OnAfterGameInitializationFinishedPostfix(MBSubModuleBase __instance, Game game, object starterObject)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnAfterGameInitializationFinished(game, starterObject);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnAfterGameInitializationFinished(game, starterObject);
                    break;
            }
        }
        private static void OnConfigChangedPostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnConfigChanged();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnConfigChanged();
                    break;
            }
        }
        private static void OnInitialStatePostfix(MBSubModuleBase __instance)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.OnInitialState();
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.OnInitialState();
                    break;
            }
        }
        private static void AfterAsyncTickTickPostfix(MBSubModuleBase __instance, float dt)
        {
            switch (__instance)
            {
                case MBSubModuleBaseWrapper wrapper:
                    wrapper.AfterAsyncTickTick(dt);
                    break;
                case MBSubModuleBaseListWrapper listWrapper:
                    listWrapper.AfterAsyncTickTick(dt);
                    break;
            }
        }

        internal static bool Enable(Harmony harmony)
        {
#pragma warning disable format // @formatter:off
            return true
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnSubModuleLoad)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnSubModuleLoadPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnSubModuleUnloaded)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnSubModuleUnloadedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnApplicationTick)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnApplicationTickPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnBeforeInitialModuleScreenSetAsRoot)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnBeforeInitialModuleScreenSetAsRootPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnGameStart)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnGameStartPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.InitializeGameStarter)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(InitializeGameStarterPostfix)))

                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnServiceRegistration)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnServiceRegistrationPostfix)))

                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.DoLoading)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(DoLoadingPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnGameLoaded)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnGameLoadedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnCampaignStart)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnCampaignStartPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.BeginGameStart)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(BeginGameStartPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnGameEnd)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnGameEndPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnGameInitializationFinished)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnGameInitializationFinishedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnBeforeMissionBehaviourInitialize)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnBeforeMissionBehaviourInitializePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), "OnBeforeMissionBehaviorInitialize"),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnBeforeMissionBehaviourInitializePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnBeforeMissionBehaviourInitialize)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnBeforeMissionBehaviourInitializePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), "OnMissionBehaviorInitialize"),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnMissionBehaviourInitializePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnMissionBehaviourInitialize)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnMissionBehaviourInitializePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnMultiplayerGameStart)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnMultiplayerGameStartPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnNewGameCreated)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnNewGameCreatedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.RegisterSubModuleObjects)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(RegisterSubModuleObjectsPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.AfterRegisterSubModuleObjects)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(AfterRegisterSubModuleObjectsPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnAfterGameInitializationFinished)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnAfterGameInitializationFinishedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnConfigChanged)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnConfigChangedPostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.OnInitialState)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(OnInitialStatePostfix)))
                & harmony.TryPatch(
                    AccessTools2.Method(typeof(MBSubModuleBase), nameof(MBSubModuleBaseWrapper.AfterAsyncTickTick)),
                    postfix: AccessTools2.Method(typeof(MBSubModuleBasePatch), nameof(AfterAsyncTickTickPostfix)));
#pragma warning restore format // @formatter:on
        }
    }
}