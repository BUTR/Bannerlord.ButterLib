using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ObjectSystem;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

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
            _log = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<CampaignBehaviorManagerPatch>>() ??
                NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (OnGameLoadedTargetMI is null)
                _log.LogError($"{nameof(OnGameLoadedTargetMI)} is null");
            if (OnBeforeSaveTargetMI is null)
                _log.LogError($"{nameof(OnBeforeSaveTargetMI)} is null");
            if (OnGameLoadedPatchMI is null)
                _log.LogError($"{nameof(OnGameLoadedPatchMI)} is null");
            if (OnBeforeSavePatchMI is null)
                _log.LogError($"{nameof(OnBeforeSavePatchMI)} is null");
            if (LoadBehaviorDataMI is null)
                _log.LogError($"{nameof(LoadBehaviorDataMI)} is null");
            if (SaveBehaviorDataMI is null)
                _log.LogError($"{nameof(SaveBehaviorDataMI)} is null");
            if (CampaignBehaviorDataStoreT is null)
                _log.LogError($"{nameof(CampaignBehaviorDataStoreT)} is null");

            if (OnGameLoadedTargetMI is null || OnBeforeSaveTargetMI is null ||
                OnGameLoadedPatchMI is null  || OnBeforeSavePatchMI is null  ||
                LoadBehaviorDataMI is null   || SaveBehaviorDataMI is null   ||
                CampaignBehaviorDataStoreT is null)
            {
                return;
            }

            harmony.Patch(OnGameLoadedTargetMI, prefix: new HarmonyMethod(OnGameLoadedPatchMI));
            harmony.Patch(OnBeforeSaveTargetMI, postfix: new HarmonyMethod(OnBeforeSavePatchMI));
        }

        // Target and patch methods:

        private static readonly MethodInfo? OnGameLoadedTargetMI = AccessTools.Method(typeof(CampaignBehaviorManager), "OnGameLoaded");

        private static readonly MethodInfo? OnBeforeSaveTargetMI = AccessTools.Method(typeof(CampaignBehaviorManager), "OnBeforeSave");

        private static readonly MethodInfo? OnGameLoadedPatchMI =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnGameLoadedPrefix));

        private static readonly MethodInfo? OnBeforeSavePatchMI =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnBeforeSavePostfix));

        // Necessary reflection:

        private static readonly Type? CampaignBehaviorDataStoreT =
            typeof(Campaign).Assembly.GetType("TaleWorlds.CampaignSystem.CampaignBehaviorDataStore");

        private static readonly MethodInfo? LoadBehaviorDataMI = AccessTools.Method(CampaignBehaviorDataStoreT, "LoadBehaviorData");

        private static readonly MethodInfo? SaveBehaviorDataMI = AccessTools.Method(CampaignBehaviorDataStoreT, "SaveBehaviorData");

        // Patch implementation:

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnGameLoadedPrefix(object? ____campaignBehaviorDataStore)
        {
            var mbObjectVariableStorage = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectExtensionDataStore>();

            if (mbObjectVariableStorage is null)
            {
                _log.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(mbObjectVariableStorage)} is null");
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                _log.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(mbObjectVariableStorage)} is not a CampaignBehaviorBase");
                return;
            }

            if (____campaignBehaviorDataStore is null)
            {
                _log.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(____campaignBehaviorDataStore)} is null");
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
                _log.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(mbObjectVariableStorage)} is null");
                return;
            }

            if (mbObjectVariableStorage is not CampaignBehaviorBase storageBehavior)
            {
                _log.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(mbObjectVariableStorage)} is not a CampaignBehaviorBase");
                return;
            }

            if (____campaignBehaviorDataStore is null)
            {
                _log.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(____campaignBehaviorDataStore)} is null");
                return;
            }

            var saveBehaviorData = AccessTools2.GetDelegate<SaveBehaviorDataDelegate>(____campaignBehaviorDataStore, SaveBehaviorDataMI!);
            saveBehaviorData?.Invoke(storageBehavior);
        }
    }
}
