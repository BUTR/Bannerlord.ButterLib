using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System;
using System.Collections.Concurrent;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.DelayedSubModule
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
        // We need a ConcurrentHashSet
        private static ConcurrentDictionary<Type, object?> RegisteredTypes { get; } = new ConcurrentDictionary<Type, object?>();

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
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleSubscriptionType.AfterMethod, "OnSubModuleLoad"));
        }
        private static void BaseOnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleSubscriptionType.AfterMethod, "OnSubModuleUnloaded"));
        }
        private static void BaseOnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleSubscriptionType.AfterMethod, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void BaseOnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleSubscriptionType.AfterMethod, "OnGameStart"));
        }
        private static void BaseOnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), true, DelayedSubModuleSubscriptionType.AfterMethod, "OnGameEnd"));
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
            if (!RegisteredTypes.TryAdd(subModule, null))
                return;

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
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.BeforeMethod, "OnSubModuleLoad"));
        }
        private static void SubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.AfterMethod, "OnSubModuleLoad"));
        }

        private static void OnSubModuleUnloadedPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.BeforeMethod, "OnSubModuleUnloaded"));
        }
        private static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.AfterMethod, "OnSubModuleUnloaded"));
        }

        private static void OnBeforeInitialModuleScreenSetAsRootPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.BeforeMethod, "OnBeforeInitialModuleScreenSetAsRoot"));
        }
        private static void OnBeforeInitialModuleScreenSetAsRootPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.AfterMethod, "OnBeforeInitialModuleScreenSetAsRoot"));
        }

        private static void OnGameStartPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.BeforeMethod, "OnGameStart"));
        }
        private static void OnGameStartPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.AfterMethod, "OnGameStart"));
        }

        private static void OnGameEndPrefix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.BeforeMethod, "OnGameEnd"));
        }
        private static void OnGameEndPostfix(MBSubModuleBase __instance)
        {
            OnMethod?.Invoke(null, new DelayedSubModuleEventArgs(__instance.GetType(), false, DelayedSubModuleSubscriptionType.AfterMethod, "OnGameEnd"));
        }

        public static void Subscribe<T1, T2>(string method, DelayedSubModuleSubscriptionType subscriptionType, EventHandler<DelayedSubModuleEventArgs> @delegate)
            where T1 : MBSubModuleBase
            where T2 : MBSubModuleBase
        {
            Subscribe<T1>(typeof(T2), method, subscriptionType, @delegate);
        }
        public static void Subscribe<T>(MBSubModuleBase caller, string method, DelayedSubModuleSubscriptionType subscriptionType, EventHandler<DelayedSubModuleEventArgs> @delegate)
            where T : MBSubModuleBase
        {
            Subscribe<T>(caller.GetType(), method, subscriptionType, @delegate);
        }
        private static void Subscribe<T>(Type caller, string method, DelayedSubModuleSubscriptionType subscriptionType, EventHandler<DelayedSubModuleEventArgs> @delegate)
            where T : MBSubModuleBase
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules();
            var callerModule = ModuleInfoHelper.GetModuleInfo(caller);
            var destModule = ModuleInfoHelper.GetModuleInfo(typeof(T));

            var callerModulePosition = loadedModules.IndexOf(callerModule!);
            var destModulePosition = loadedModules.IndexOf(destModule!);

            if (callerModulePosition < destModulePosition) // Dest module was still not called
            {
                OnMethod += @delegate;
            }
            if (callerModulePosition == destModulePosition) // This should not happen
            {
                if (subscriptionType == DelayedSubModuleSubscriptionType.BeforeMethod)
                {
                    @delegate.Invoke(caller, new DelayedSubModuleEventArgs(typeof(T), false, subscriptionType, method));
                }
                if (subscriptionType == DelayedSubModuleSubscriptionType.AfterMethod)
                {
                    OnMethod += @delegate;
                }
            }
            if (callerModulePosition > destModulePosition) // Dest module was already called
            {
                @delegate.Invoke(caller, new DelayedSubModuleEventArgs(typeof(T), false, subscriptionType, method));
            }
        }
    }
}