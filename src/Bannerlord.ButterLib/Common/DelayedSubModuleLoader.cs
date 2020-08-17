using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common
{
    /// <summary>
    /// Allows you to inject your own code into the load / unload sequence
    /// of other modules without specifying them directly as a dependency
    /// in 'SubModule.xml'.
    /// </summary>
    /// <remarks>
    /// The basic loading order still applies, so while you can technically
    /// patch the 'OnSubModuleLoad' methods of the already loaded modules,
    /// it won't affect them as they will be executed before.
    /// </remarks>
    public static class DelayedSubModuleLoader
    {
        /// <summary>
        /// An event that is raised when the load / unload methods of the
        /// <see cref="MBSubModuleBase"/> or its derived classes are called.
        /// </summary>
        /// <remarks><para>
        /// Allows you to inject your own code into the load / unload sequence
        /// of other modules without specifying them directly as a dependency
        /// in 'SubModule.xml'.
        /// </para><para>
        /// Supported methods are: 'OnSubModuleLoad', 'OnSubModuleUnloaded',
        /// 'OnBeforeInitialModuleScreenSetAsRoot', 'OnGameStart', 'OnGameEnd'.
        /// </para></remarks>
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
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnSubModuleLoad"));
        }
        private static void BaseOnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnSubModuleUnloaded"));
        }
        private static void BaseOnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void BaseOnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnGameStart"));
        }
        private static void BaseOnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnGameEnd"));
        }

        /// <summary>Registers a module to be a target of the <see cref="DelayedSubModuleLoader"/> patching.</summary>        
        /// <typeparam name="TSubModule">The exact type of the module to be patched.</typeparam>
        /// <param name="priority">The <see cref="HarmonyPriority"/> that would be assigned to the patches that would be made.</param>
        /// <param name="before">A list of <see cref="Harmony.Id"/>s that should come after the patches that would be made.</param>
        /// <param name="after">A list of <see cref="Harmony.Id"/>s that should come before the patches that would be made.</param>
        public static void Register<TSubModule>(int priority = -1, string[]? before= null, string[]? after = null) where TSubModule : MBSubModuleBase => Register(typeof(TSubModule), priority, before, after);

        /// <summary>Registers a module to be a target of the <see cref="DelayedSubModuleLoader"/> patching.</summary>        
        /// <param name="subModule">The exact type of the module to be patched.</param>
        /// <param name="priority">The <see cref="HarmonyPriority"/> that would be assigned to the patches that would be made.</param>
        /// <param name="before">A list of <see cref="Harmony.Id"/>s that should come after the patches that would be made.</param>
        /// <param name="after">A list of <see cref="Harmony.Id"/>s that should come before the patches that would be made.</param>
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
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Prefix, "OnSubModuleLoad"));
        }
        private static void SubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnSubModuleLoad"));
        }

        private static void OnSubModuleUnloadedPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Prefix, "OnSubModuleUnloaded"));
        }
        private static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnSubModuleUnloaded"));
        }

        private static void OnBeforeInitialModuleScreenSetAsRootPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Prefix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void OnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnBeforeInitialModuleScreenSetAsRoot"));
        }

        private static void OnGameStartPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Prefix, "OnGameStart"));
        }
        private static void OnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnGameStart"));
        }

        private static void OnGameEndPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Prefix, "OnGameEnd"));
        }
        private static void OnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleEventArgs.SubModulePatchType.Postfix, "OnGameEnd"));
        }
    }
}