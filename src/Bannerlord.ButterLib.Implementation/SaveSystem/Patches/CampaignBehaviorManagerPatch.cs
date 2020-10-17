using Bannerlord.ButterLib.Common.Extensions;

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
    internal class CampaignBehaviorManagerPatch
    {
        private static ILogger _logger = default!;

        // Application:

        internal static void Apply(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<CampaignBehaviorManagerPatch>>() ??
                NullLogger<CampaignBehaviorManagerPatch>.Instance;

            if (OnGameLoadedTargetMethod == null)
                _logger.LogError("OnGameLoadedTargetMethod is null");
            if (OnBeforeSaveTargetMethod == null)
                _logger.LogError("OnBeforeSaveTargetMethod is null");
            if (OnGameLoadedPatchMethod == null)
                _logger.LogError("OnGameLoadedPatchMethod is null");
            if (OnBeforeSavePatchMethod == null)
                _logger.LogError("OnBeforeSavePatchMethod is null");

            if (OnGameLoadedTargetMethod == null || OnBeforeSaveTargetMethod == null ||
                OnGameLoadedPatchMethod == null || OnBeforeSavePatchMethod == null)
            {
                return;
            }

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
            if (____campaignBehaviorDataStore == null)
            {
                _logger.LogError("OnGameLoadedPrefix: ____campaignBehaviorDataStore is null");
                return;
            }

            MBObjectVariableStorageBehavior.Instance?.Dispose();
            MBObjectVariableStorageBehavior.Instance = new MBObjectVariableStorageBehavior();

            LoadBehaviorDataMethod?.Invoke(
                ____campaignBehaviorDataStore,
                new object[] { MBObjectVariableStorageBehavior.Instance });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnBeforeSavePostfix(object? ____campaignBehaviorDataStore)
        {
            if (MBObjectVariableStorageBehavior.Instance == null)
            {
                _logger.LogError("OnBeforeSavePostfix: MBObjectVariableStorageBehavior.Instance is null");
                return;
            }

            if (____campaignBehaviorDataStore == null)
            {
                _logger.LogError("OnBeforeSavePostfix: ____campaignBehaviorDataStore is null");
                return;
            }

            SaveBehaviorDataMethod?.Invoke(
                ____campaignBehaviorDataStore,
                new object[] { MBObjectVariableStorageBehavior.Instance });
        }
    }
}
