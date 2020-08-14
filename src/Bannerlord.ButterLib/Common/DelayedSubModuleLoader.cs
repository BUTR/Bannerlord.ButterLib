using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common
{
    /// <summary>
    /// Instead of depending on a module in SubModule.xml, just execute some code after its execution.
    /// </summary>
    /// <typeparam name="TSubModule"></typeparam>
    public class DelayedSubModuleLoader<TSubModule> where TSubModule : MBSubModuleBase
    {
        private static event EventHandler GlobalOnSubModuleLoad;
        private static event EventHandler GlobalOnSubModuleUnloaded;
        private static event EventHandler GlobalOnBeforeInitialModuleScreenSetAsRoot;
        private static event EventHandler GlobalOnGameStart;
        private static event EventHandler GlobalOnGameEnd;

        public event EventHandler OnSubModuleLoad { add => GlobalOnSubModuleLoad += value; remove => GlobalOnSubModuleLoad -= value; }
        public event EventHandler OnSubModuleUnloaded { add => GlobalOnSubModuleUnloaded += value; remove => GlobalOnSubModuleUnloaded -= value; }
        public event EventHandler OnBeforeInitialModuleScreenSetAsRoot { add => GlobalOnBeforeInitialModuleScreenSetAsRoot += value; remove => GlobalOnBeforeInitialModuleScreenSetAsRoot -= value; }
        public event EventHandler OnGameStart { add => GlobalOnGameStart += value; remove => GlobalOnGameStart -= value; }
        public event EventHandler OnGameEnd { add => GlobalOnGameEnd += value; remove => GlobalOnGameEnd -= value; }

        public DelayedSubModuleLoader()
        {
            var harmony = new Harmony($"butterlib.delayedsubmoduleloader.{typeof(TSubModule).Name.ToLowerInvariant()}");
            harmony.Patch(
                AccessTools.Method(typeof(TSubModule), "OnSubModuleLoad"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader<TSubModule>), nameof(SubModuleLoadPostfix)));
            harmony.Patch(
                AccessTools.Method(typeof(TSubModule), "OnSubModuleUnloaded"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader<TSubModule>), nameof(OnSubModuleUnloadedPostfix)));
            harmony.Patch(
                AccessTools.Method(typeof(TSubModule), "OnBeforeInitialModuleScreenSetAsRoot"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader<TSubModule>), nameof(OnBeforeInitialModuleScreenSetAsRootPostfix)));
            harmony.Patch(
                AccessTools.Method(typeof(TSubModule), "OnGameStart"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader<TSubModule>), nameof(OnGameStartPostfix)));
            harmony.Patch(
                AccessTools.Method(typeof(TSubModule), "OnGameEnd"),
                postfix: new HarmonyMethod(typeof(DelayedSubModuleLoader<TSubModule>), nameof(OnGameEndPostfix)));

            OnSubModuleLoad += GlobalOnSubModuleLoad;
            OnSubModuleUnloaded += GlobalOnSubModuleUnloaded;
            OnBeforeInitialModuleScreenSetAsRoot += GlobalOnBeforeInitialModuleScreenSetAsRoot;
            OnGameStart += GlobalOnGameStart;
            OnGameEnd += GlobalOnGameEnd;
        }

        private static void SubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is TSubModule))
                return;

            GlobalOnSubModuleLoad?.Invoke(null, EventArgs.Empty);
        }
        private static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is TSubModule))
                return;

            GlobalOnSubModuleUnloaded?.Invoke(null, EventArgs.Empty);
        }
        private static void OnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is TSubModule))
                return;

            GlobalOnBeforeInitialModuleScreenSetAsRoot?.Invoke(null, EventArgs.Empty);
        }
        private static void OnGameStartPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is TSubModule))
                return;

            GlobalOnGameStart?.Invoke(null, EventArgs.Empty);
        }
        private static void OnGameEndPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is TSubModule))
                return;

            GlobalOnGameEnd?.Invoke(null, EventArgs.Empty);
        }
    }
}