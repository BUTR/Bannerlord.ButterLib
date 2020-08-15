using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common
{
    /// <summary>
    /// Instead of depending on a module in SubModule.xml, just execute some code after its execution.
    /// </summary>
    public class DelayedSubModuleLoader
    {
        public static event EventHandler<DelayedSubModuleEventArgs>? OnMethod;

        static DelayedSubModuleLoader()
        {
            var harmony = new Harmony("butterlib.delayedsubmoduleloader.static");
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(MBSubModuleBase), "OnSubModuleLoad"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader), nameof(BaseSubModuleLoadPostfix)));
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(MBSubModuleBase), "OnSubModuleUnloaded"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader), nameof(BaseOnSubModuleUnloadedPostfix)));
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(MBSubModuleBase), "OnBeforeInitialModuleScreenSetAsRoot"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader), nameof(BaseOnBeforeInitialModuleScreenSetAsRootPostfix)));
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(MBSubModuleBase), "OnGameStart"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader), nameof(BaseOnGameStartPostfix)));
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(MBSubModuleBase), "OnGameEnd"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader), nameof(BaseOnGameEndPostfix)));
        }
        private static void BaseSubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, HarmonyPatchType.Postfix, "OnSubModuleLoad"));
        }
        private static void BaseOnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, HarmonyPatchType.Postfix, "OnSubModuleUnloaded"));
        }
        private static void BaseOnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, HarmonyPatchType.Postfix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void BaseOnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, HarmonyPatchType.Postfix, "OnGameStart"));
        }
        private static void BaseOnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, HarmonyPatchType.Postfix, "OnGameEnd"));
        }
        
        public static void Register<TSubModule>(int priority = -1, string[]? before= null, string[]? after = null) => Register(typeof(TSubModule), priority, before, after);
        public static void Register(Type subModule, int priority = -1, string[]? before= null, string[]? after = null)
        {
            var harmony = new Harmony($"butterlib.delayedsubmoduleloader.{subModule.Name.ToLowerInvariant()}");
            var onSubModuleLoad = AccessTools.DeclaredMethod(subModule, "OnSubModuleLoad");
            if (onSubModuleLoad != null)
                harmony.Patch(
                    onSubModuleLoad,
                    prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(SubModuleLoadPrefix)), priority, before, after),
                    postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(SubModuleLoadPostfix)), priority, before, after));

            var onSubModuleUnloaded = AccessTools.DeclaredMethod(subModule, "OnSubModuleUnloaded");
            if (onSubModuleUnloaded != null)
                harmony.Patch(
                    AccessTools.DeclaredMethod(subModule, "OnSubModuleUnloaded"),
                    prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnSubModuleUnloadedPrefix)), priority, before, after),
                    postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnSubModuleUnloadedPostfix)), priority, before, after));

            var onBeforeInitialModuleScreenSetAsRoot = AccessTools.DeclaredMethod(subModule, "OnBeforeInitialModuleScreenSetAsRoot");
            if (onBeforeInitialModuleScreenSetAsRoot != null)
                harmony.Patch(
                    AccessTools.DeclaredMethod(subModule, "OnBeforeInitialModuleScreenSetAsRoot"),
                    prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnBeforeInitialModuleScreenSetAsRootPrefix)), priority, before, after),
                    postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnBeforeInitialModuleScreenSetAsRootPostfix)), priority, before, after));

            var onGameStart = AccessTools.DeclaredMethod(subModule, "OnGameStart");
            if (onGameStart != null)
                harmony.Patch(
                    AccessTools.DeclaredMethod(subModule, "OnGameStart"),
                    prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnGameStartPrefix)), priority, before, after),
                    postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnGameStartPostfix)), priority, before, after));

            var onGameEnd = AccessTools.DeclaredMethod(subModule, "OnGameEnd");
            if (onGameEnd != null)
                harmony.Patch(
                    AccessTools.DeclaredMethod(subModule, "OnGameEnd"),
                    prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnGameEndPrefix)), priority, before, after),
                    postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(DelayedSubModuleLoader), nameof(OnGameEndPostfix)), priority, before, after));
        }
        private static void SubModuleLoadPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Prefix, "OnSubModuleLoad"));
        }
        private static void SubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Postfix, "OnSubModuleLoad"));
        }

        private static void OnSubModuleUnloadedPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Prefix, "OnSubModuleUnloaded"));
        }
        private static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Postfix, "OnSubModuleUnloaded"));
        }

        private static void OnBeforeInitialModuleScreenSetAsRootPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Prefix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void OnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Postfix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }

        private static void OnGameStartPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Prefix, "OnGameStart"));
        }
        private static void OnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Postfix, "OnGameStart"));
        }

        private static void OnGameEndPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Prefix, "OnGameEnd"));
        }
        private static void OnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, HarmonyPatchType.Postfix, "OnGameEnd"));
        }
    }
}