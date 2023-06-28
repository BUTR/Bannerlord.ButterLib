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

        private static ILogger _log = default!;

        // Application:

        internal static void Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _log = provider?.GetRequiredService<ILogger<CampaignBehaviorManagerPatch>>() ?? NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (OnGameLoadedTargetMI is null)
                _log.LogError("{Method} is null", nameof(OnGameLoadedTargetMI));
            if (OnBeforeSaveTargetMI is null)
                _log.LogError("{Method} is null", nameof(OnBeforeSaveTargetMI));
            if (OnGameLoadedPatchMI is null)
                _log.LogError("{Method} is null", nameof(OnGameLoadedPatchMI));
            if (OnBeforeSavePatchMI is null)
                _log.LogError("{Method} is null", nameof(OnBeforeSavePatchMI));
            if (LoadBehaviorDataMI is null)
                _log.LogError("{Method} is null", nameof(LoadBehaviorDataMI));
            if (SaveBehaviorDataMI is null)
                _log.LogError("{Method} is null", nameof(SaveBehaviorDataMI));
            if (CampaignBehaviorDataStoreT is null)
                _log.LogError("{Method} is null", nameof(CampaignBehaviorDataStoreT));

            if (OnGameLoadedTargetMI is null || OnBeforeSaveTargetMI is null ||
                OnGameLoadedPatchMI is null || OnBeforeSavePatchMI is null ||
                LoadBehaviorDataMI is null || SaveBehaviorDataMI is null ||
                CampaignBehaviorDataStoreT is null)
            {
                return;
            }

            harmony.Patch(OnGameLoadedTargetMI, prefix: new HarmonyMethod(OnGameLoadedPatchMI));
            harmony.Patch(OnBeforeSaveTargetMI, postfix: new HarmonyMethod(OnBeforeSavePatchMI));
        }

        internal static void Disable(Harmony harmony) { }

        // Target and patch methods:

        private static readonly MethodInfo? OnGameLoadedTargetMI =
            AccessTools2.Method("TaleWorlds.CampaignSystem.CampaignBehaviors.CampaignBehaviorManager:OnGameLoaded");

        private static readonly MethodInfo? OnBeforeSaveTargetMI =
            AccessTools2.Method("TaleWorlds.CampaignSystem.CampaignBehaviors.CampaignBehaviorManager:OnBeforeSave");

        private static readonly MethodInfo? OnGameLoadedPatchMI =
            AccessTools2.Method("Bannerlord.ButterLib.Implementation.ObjectSystem.Patches.CampaignBehaviorManagerPatch:OnGameLoadedPrefix");

        private static readonly MethodInfo? OnBeforeSavePatchMI =
            AccessTools2.Method("Bannerlord.ButterLib.Implementation.ObjectSystem.Patches.CampaignBehaviorManagerPatch:OnBeforeSavePostfix");

        // Necessary reflection:

        private static readonly Type? CampaignBehaviorDataStoreT =
            typeof(Campaign).Assembly.GetType("TaleWorlds.CampaignSystem.CampaignBehaviorDataStore");

        private static readonly MethodInfo? LoadBehaviorDataMI = AccessTools2.Method(CampaignBehaviorDataStoreT!, "LoadBehaviorData");

        private static readonly MethodInfo? SaveBehaviorDataMI = AccessTools2.Method(CampaignBehaviorDataStoreT!, "SaveBehaviorData");

        // Patch implementation:

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnGameLoadedPrefix(object? ____campaignBehaviorDataStore)
        {
            var mbObjectVariableStorage = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectExtensionDataStore>();

            if (mbObjectVariableStorage is null)
            {
                _log.LogError("{Method}: {Variable} is null", nameof(OnGameLoadedPrefix), nameof(mbObjectVariableStorage));
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                _log.LogError("{Method}: {Variable} is not a CampaignBehaviorBase", nameof(OnGameLoadedPrefix), nameof(mbObjectVariableStorage));
                return;
            }

            if (____campaignBehaviorDataStore is null)
            {
                _log.LogError("{Method}: {Variable} is null", nameof(OnGameLoadedPrefix), nameof(____campaignBehaviorDataStore));
                return;
            }

            var loadBehaviorData = AccessTools2.GetDelegate<LoadBehaviorDataDelegate>(____campaignBehaviorDataStore, LoadBehaviorDataMI!);
            loadBehaviorData?.Invoke(storageBehavior);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnBeforeSavePostfix(object? ____campaignBehaviorDataStore)
        {
            var mbObjectVariableStorage = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectExtensionDataStore>();

            if (mbObjectVariableStorage is null)
            {
                _log.LogError("{Method}: {Variable} is null", nameof(OnBeforeSavePostfix), nameof(mbObjectVariableStorage));
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                _log.LogError("{Method}: {Variable} is not a CampaignBehaviorBase", nameof(OnBeforeSavePostfix), nameof(mbObjectVariableStorage));
                return;
            }

            if (____campaignBehaviorDataStore is null)
            {
                _log.LogError("{Method}: {Variable} is null", nameof(OnBeforeSavePostfix), nameof(____campaignBehaviorDataStore));
                return;
            }

            var saveBehaviorData = AccessTools2.GetDelegate<SaveBehaviorDataDelegate>(____campaignBehaviorDataStore, SaveBehaviorDataMI!);
            saveBehaviorData?.Invoke(storageBehavior);
        }
    }
}