using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.ObjectSystem;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem.Patches
{
    internal sealed class CampaignBehaviorManagerPatch
    {
        private delegate void SaveBehaviorDataDelegate(CampaignBehaviorBase campaignBehavior);
        private delegate void LoadBehaviorDataDelegate(CampaignBehaviorBase campaignBehavior);

        private static readonly MethodInfo? OnGameLoadedTargetMI =
            AccessTools2.Method("TaleWorlds.CampaignSystem.CampaignBehaviors.CampaignBehaviorManager:OnGameLoaded") ??
            AccessTools2.Method("TaleWorlds.CampaignSystem.CampaignBehaviors.CampaignBehaviorManager:LoadBehaviorData");

        private static readonly MethodInfo? OnBeforeSaveTargetMI =
            AccessTools2.Method("TaleWorlds.CampaignSystem.CampaignBehaviors.CampaignBehaviorManager:OnBeforeSave");

        private static readonly MethodInfo? OnGameLoadedPatchMI = SymbolExtensions2.GetMethodInfo((object? x) => OnGameLoadedPrefix(x));
        private static readonly MethodInfo? OnBeforeSavePatchMI = SymbolExtensions2.GetMethodInfo((object? x) => OnBeforeSavePostfix(x));

        internal static void Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            var log = provider?.GetService<ILogger<CampaignBehaviorManagerPatch>>() ?? NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (OnGameLoadedTargetMI is null)
                log.LogError("{Method} is null", nameof(OnGameLoadedTargetMI));
            if (OnBeforeSaveTargetMI is null)
                log.LogError("{Method} is null", nameof(OnBeforeSaveTargetMI));
            if (OnGameLoadedPatchMI is null)
                log.LogError("{Method} is null", nameof(OnGameLoadedPatchMI));
            if (OnBeforeSavePatchMI is null)
                log.LogError("{Method} is null", nameof(OnBeforeSavePatchMI));

            if (OnGameLoadedTargetMI is null || OnBeforeSaveTargetMI is null || OnGameLoadedPatchMI is null || OnBeforeSavePatchMI is null)
                return;

            harmony.Patch(OnGameLoadedTargetMI, prefix: new HarmonyMethod(OnGameLoadedPatchMI));
            harmony.Patch(OnBeforeSaveTargetMI, postfix: new HarmonyMethod(OnBeforeSavePatchMI));
        }

        internal static void Disable(Harmony harmony) { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnGameLoadedPrefix(object? ____campaignBehaviorDataStore)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            var log = provider?.GetService<ILogger<CampaignBehaviorManagerPatch>>() ?? NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (____campaignBehaviorDataStore is null)
            {
                log.LogError("{Method}: {Variable} is null", nameof(OnGameLoadedPrefix), nameof(____campaignBehaviorDataStore));
                return;
            }

            if (ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IMBObjectExtensionDataStore>() is not { } mbObjectVariableStorage)
            {
                log.LogError("{Method}: {Variable} is null", nameof(OnGameLoadedPrefix), nameof(mbObjectVariableStorage));
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                log.LogError("{Method}: {Variable} is not a CampaignBehaviorBase", nameof(OnGameLoadedPrefix), nameof(mbObjectVariableStorage));
                return;
            }

            if (AccessTools2.GetDelegate<LoadBehaviorDataDelegate>(____campaignBehaviorDataStore, ____campaignBehaviorDataStore.GetType(), "LoadBehaviorData") is not { } loadBehaviorData)
            {
                log.LogError("{Method}: {Variable} is not a SaveBehaviorDataDelegate", nameof(OnGameLoadedPrefix), nameof(loadBehaviorData));
                return;
            }

            try
            {
                loadBehaviorData(storageBehavior);
            }
            catch (Exception e)
            {
                log.LogError(e, "{Method}", nameof(OnGameLoadedPrefix));
                return;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnBeforeSavePostfix(object? ____campaignBehaviorDataStore)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            var log = provider?.GetService<ILogger<CampaignBehaviorManagerPatch>>() ?? NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (____campaignBehaviorDataStore is null)
            {
                log.LogError("{Method}: {Variable} is null", nameof(OnBeforeSavePostfix), nameof(____campaignBehaviorDataStore));
                return;
            }

            if (ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IMBObjectExtensionDataStore>() is not { } mbObjectVariableStorage)
            {
                log.LogError("{Method}: {Variable} is null", nameof(OnBeforeSavePostfix), nameof(mbObjectVariableStorage));
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                log.LogError("{Method}: {Variable} is not a CampaignBehaviorBase", nameof(OnBeforeSavePostfix), nameof(mbObjectVariableStorage));
                return;
            }

            if (AccessTools2.GetDelegate<SaveBehaviorDataDelegate>(____campaignBehaviorDataStore, ____campaignBehaviorDataStore.GetType(), "SaveBehaviorData") is not { } saveBehaviorData)
            {
                log.LogError("{Method}: {Variable} is not a SaveBehaviorDataDelegate", nameof(OnBeforeSavePostfix), nameof(saveBehaviorData));
                return;
            }

            try
            {
                saveBehaviorData(storageBehavior);
            }
            catch (Exception e)
            {
                log.LogError(e, "{Method}", nameof(OnBeforeSavePostfix));
                return;
            }
        }
    }
}