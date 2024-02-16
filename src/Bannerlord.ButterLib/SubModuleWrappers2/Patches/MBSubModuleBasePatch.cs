using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers2.Patches;

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
                wrapper.OnBeforeMissionBehaviorInitialize(mission);
                break;
            case MBSubModuleBaseListWrapper listWrapper:
                listWrapper.OnBeforeMissionBehaviorInitialize(mission);
                break;
        }
    }
    private static void OnMissionBehaviourInitializePostfix(MBSubModuleBase __instance, Mission mission)
    {
        switch (__instance)
        {
            case MBSubModuleBaseWrapper wrapper:
                wrapper.OnMissionBehaviorInitialize(mission);
                break;
            case MBSubModuleBaseListWrapper listWrapper:
                listWrapper.OnMissionBehaviorInitialize(mission);
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
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnSubModuleLoadPostfix(x)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnSubModuleUnloaded"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnSubModuleUnloadedPostfix(x)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnApplicationTick"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, float y) => OnApplicationTickPostfix(x, ref y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnBeforeInitialModuleScreenSetAsRoot"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnBeforeInitialModuleScreenSetAsRootPostfix(x)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameStart"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, IGameStarter z) => OnGameStartPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:InitializeGameStarter"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, IGameStarter z) => InitializeGameStarterPostfix(x, y, z)))

                // TODO: Won't work
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnServiceRegistration", logErrorInTrace: false),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnServiceRegistrationPostfix(x)))

                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:DoLoading"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, bool y, Game z) => DoLoadingPostfix(x, ref y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameLoaded"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, object z) => OnGameLoadedPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnCampaignStart"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, object z) => OnCampaignStartPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:BeginGameStart"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y) => BeginGameStartPostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameEnd"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y) => OnGameEndPostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameInitializationFinished"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y) => OnGameInitializationFinishedPostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnBeforeMissionBehaviorInitialize"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Mission y) => OnBeforeMissionBehaviourInitializePostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnMissionBehaviorInitialize"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Mission y) => OnMissionBehaviourInitializePostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnMultiplayerGameStart"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, object z) => OnMultiplayerGameStartPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnNewGameCreated"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, object z) => OnNewGameCreatedPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:RegisterSubModuleObjects"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, bool y) => RegisterSubModuleObjectsPostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:AfterRegisterSubModuleObjects"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, bool y) => AfterRegisterSubModuleObjectsPostfix(x, y)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnAfterGameInitializationFinished"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, Game y, object z) => OnAfterGameInitializationFinishedPostfix(x, y, z)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnConfigChanged"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnConfigChangedPostfix(x)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnInitialState"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x) => OnInitialStatePostfix(x)))
                & harmony.TryPatch(
                    AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:AfterAsyncTickTick"),
                    postfix: SymbolExtensions2.GetMethodInfo((MBSubModuleBase x, float y) => AfterAsyncTickTickPostfix(x, y)));
#pragma warning restore format // @formatter:on
    }
}