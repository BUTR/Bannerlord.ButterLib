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
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnSubModuleLoad"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnSubModuleLoadPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnSubModuleUnloaded"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnSubModuleUnloadedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnApplicationTick"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnApplicationTickPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnBeforeInitialModuleScreenSetAsRoot"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnBeforeInitialModuleScreenSetAsRootPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameStart"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnGameStartPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:InitializeGameStarter"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:InitializeGameStarterPostfix"))

                // TODO: Won't work
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnServiceRegistration"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnServiceRegistrationPostfix"))

                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:DoLoading"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:DoLoadingPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameLoaded"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnGameLoadedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnCampaignStart"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnCampaignStartPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:BeginGameStart"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:BeginGameStartPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameEnd"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnGameEndPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameInitializationFinished"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnGameInitializationFinishedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnBeforeMissionBehaviorInitialize"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnBeforeMissionBehaviourInitializePostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnMissionBehaviorInitialize"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnMissionBehaviourInitializePostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnMultiplayerGameStart"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnMultiplayerGameStartPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnNewGameCreated"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnNewGameCreatedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:RegisterSubModuleObjects"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:RegisterSubModuleObjectsPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:AfterRegisterSubModuleObjects"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:AfterRegisterSubModuleObjectsPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnAfterGameInitializationFinished"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnAfterGameInitializationFinishedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnConfigChanged"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnConfigChangedPostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnInitialState"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:OnInitialStatePostfix"))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:AfterAsyncTickTick"),
                    postfix: AccessTools2.Method("Bannerlord.ButterLib.SubModuleWrappers2.Patches.MBSubModuleBasePatch:AfterAsyncTickTickPostfix"));
#pragma warning restore format // @formatter:on
        }
    }
}