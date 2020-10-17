using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches
{
    internal static class CampaignBehaviorManagerPatch
    {
        // Application:

        internal static void Apply(Harmony harmony)
        {
            harmony.Patch(OnGameLoadedTargetMethod, prefix: new HarmonyMethod(OnGameLoadedPatchMethod));
            harmony.Patch(OnBeforeSaveTargetMethod, postfix: new HarmonyMethod(OnBeforeSavePatchMethod));
        }

        // Target and patch methods:

        private static readonly MethodInfo OnGameLoadedTargetMethod =
            AccessTools.Method(typeof(CampaignBehaviorManager), "OnGameLoaded");

        private static readonly MethodInfo OnGameLoadedPatchMethod =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnGameLoadedPrefix));

        private static readonly MethodInfo OnBeforeSaveTargetMethod =
            AccessTools.Method(typeof(CampaignBehaviorManager), "OnBeforeSave");

        private static readonly MethodInfo OnBeforeSavePatchMethod =
            AccessTools.Method(typeof(CampaignBehaviorManagerPatch), nameof(OnBeforeSavePostfix));

        // Necessary reflection:

        private static readonly Type CampaignBehaviorDataStoreType =
            typeof(Campaign).Assembly.GetType("TaleWorlds.CampaignSystem.CampaignBehaviorDataStore");

        private static readonly MethodInfo LoadBehaviorDataMethod =
            AccessTools.Method(CampaignBehaviorDataStoreType, "LoadBehaviorData");

        private static readonly MethodInfo SaveBehaviorDataMethod =
            AccessTools.Method(CampaignBehaviorDataStoreType, "SaveBehaviorData");

        // Patch implementation:

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnGameLoadedPrefix(object ____campaignBehaviorDataStore)
        {
            if (MBObjectVariableStorageBehavior.Instance == null)
                return;

            LoadBehaviorDataMethod.Invoke(
                ____campaignBehaviorDataStore,
                new object[] { MBObjectVariableStorageBehavior.Instance });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnBeforeSavePostfix(object ____campaignBehaviorDataStore)
        {
            if (MBObjectVariableStorageBehavior.Instance == null)
                return;

            SaveBehaviorDataMethod.Invoke(
                ____campaignBehaviorDataStore,
                new object[] { MBObjectVariableStorageBehavior.Instance });
        }
    }
}
