using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.SaveSystem;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches
{
    internal sealed class CampaignBehaviorManagerPatch
    {
        private delegate void SaveBehaviorDataDelegate(CampaignBehaviorBase campaignBehavior);
        private delegate void LoadBehaviorDataDelegate(CampaignBehaviorBase campaignBehavior);

        private static ILogger _logger = default!;

        // Application:

        internal static void Apply(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<CampaignBehaviorManagerPatch>>() ??
                NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (OnGameLoadedTargetMethod == null)
                _logger.LogError($"{nameof(OnGameLoadedTargetMethod)} is null");
            if (OnBeforeSaveTargetMethod == null)
                _logger.LogError($"{nameof(OnBeforeSaveTargetMethod)} is null");
            if (OnGameLoadedPatchMethod == null)
                _logger.LogError($"{nameof(OnGameLoadedPatchMethod)} is null");
            if (OnBeforeSavePatchMethod == null)
                _logger.LogError($"{nameof(OnBeforeSavePatchMethod)} is null");
            if (LoadBehaviorDataMethod == null)
                _logger.LogError($"{nameof(LoadBehaviorDataMethod)} is null");
            if (SaveBehaviorDataMethod == null)
                _logger.LogError($"{nameof(SaveBehaviorDataMethod)} is null");
            if (CampaignBehaviorDataStoreType == null)
                _logger.LogError($"{nameof(CampaignBehaviorDataStoreType)} is null");

            if (OnGameLoadedTargetMethod == null || OnBeforeSaveTargetMethod == null ||
                OnGameLoadedPatchMethod == null  || OnBeforeSavePatchMethod == null  ||
                LoadBehaviorDataMethod == null   || SaveBehaviorDataMethod == null   ||
                CampaignBehaviorDataStoreType == null)
                return;

            harmony.Patch(OnGameLoadedTargetMethod, prefix: new HarmonyMethod(OnGameLoadedPatchMethod));
            harmony.Patch(OnBeforeSaveTargetMethod, postfix: new HarmonyMethod(OnBeforeSavePatchMethod));
        }

        // Target and patch methods:

        private static readonly MethodInfo? OnGameLoadedTargetMethod =
            AccessTools.Method(typeof(CampaignBehaviorManager), "OnGameLoaded");

        private static readonly MethodInfo? OnGameLoadedPatchMethod =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnGameLoadedPrefix));

        private static readonly MethodInfo? OnBeforeSaveTargetMethod =
            AccessTools.Method(typeof(CampaignBehaviorManager), "OnBeforeSave");

        private static readonly MethodInfo? OnBeforeSavePatchMethod =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnBeforeSavePostfix));

        // Necessary reflection:

        private static readonly Type? CampaignBehaviorDataStoreType =
            typeof(Campaign).Assembly.GetType("TaleWorlds.CampaignSystem.CampaignBehaviorDataStore");

        private static readonly MethodInfo? LoadBehaviorDataMethod =
            AccessTools.Method(CampaignBehaviorDataStoreType, "LoadBehaviorData");

        private static readonly MethodInfo? SaveBehaviorDataMethod =
            AccessTools.Method(CampaignBehaviorDataStoreType, "SaveBehaviorData");

        // Patch implementation:

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnGameLoadedPrefix(object? ____campaignBehaviorDataStore)
        {
            var mbObjectVariableStorage = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectVariableStorage>();

            if (mbObjectVariableStorage == null)
            {
                _logger.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(mbObjectVariableStorage)} is null");
                return;
            }

            if (!(mbObjectVariableStorage is CampaignBehaviorBase storageBehavior))
            {
                _logger.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(mbObjectVariableStorage)} is not a CampaignBehaviorBase");
                return;
            }

            if (____campaignBehaviorDataStore == null)
            {
                _logger.LogError($"{nameof(OnGameLoadedPrefix)}: {nameof(____campaignBehaviorDataStore)} is null");
                return;
            }

            var loadBehaviorData = AccessTools2.GetDelegate<LoadBehaviorDataDelegate>(____campaignBehaviorDataStore, LoadBehaviorDataMethod!);
            loadBehaviorData?.Invoke(storageBehavior);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnBeforeSavePostfix(object? ____campaignBehaviorDataStore)
        {
            var mbObjectVariableStorage = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectVariableStorage>();

            if (mbObjectVariableStorage == null)
            {
                _logger.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(mbObjectVariableStorage)} is null");
                return;
            }

            if (!(mbObjectVariableStorage is CampaignBehaviorBase storageBehavior))
            {
                _logger.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(mbObjectVariableStorage)} is not a CampaignBehaviorBase");
                return;
            }

            if (____campaignBehaviorDataStore == null)
            {
                _logger.LogError($"{nameof(OnBeforeSavePostfix)}: {nameof(____campaignBehaviorDataStore)} is null");
                return;
            }

            var saveBehaviorData = AccessTools2.GetDelegate<SaveBehaviorDataDelegate>(____campaignBehaviorDataStore, SaveBehaviorDataMethod!);
            saveBehaviorData?.Invoke(storageBehavior);
        }
    }
}
