using Bannerlord.BUTRLoader;
using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using Module = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    // BEW commentary:
    // TaleWorlds.DotNet.Managed:ApplicationTick                              -> Replicated
    // TaleWorlds.Engine.ScriptComponentBehaviour:OnTick                      -> Called by TaleWorlds.Engine.ManagedScriptHolder:TickComponents
    // TaleWorlds.MountAndBlade.Module:OnApplicationTick                      -> Replicated
    // TaleWorlds.MountAndBlade.View.Missions.MissionView:OnMissionScreenTick -> Called by TaleWorlds.MountAndBlade.View.Screen.MissionScreen:OnFrameTick
    // TaleWorlds.Engine.Screens.ScreenManager:Tick                           -> Replicated
    // TaleWorlds.MountAndBlade.Mission:Tick                                  -> Replicated
    // TaleWorlds.MountAndBlade.MissionBehaviour:OnMissionTick                -> Called by TaleWorlds.MountAndBlade.Mission:Tick

    [BUTRLoaderInterceptor]
    internal sealed class BEWPatch
    {
        internal record ExceptionIdentifier(Type Type, string StackTrace, string Message)
        {
            public static ExceptionIdentifier FromException(Exception e) => new(e.GetType(), e.StackTrace, e.Message);
        }

        internal static readonly HashSet<ExceptionIdentifier> SuppressedExceptions = new();

        private static readonly string[] bew = { "org.calradia.admiralnelson.betterexceptionwindow" };

        private static readonly MethodInfo? ManagedApplicationTickMethod = AccessTools2.Method("TaleWorlds.DotNet.Managed:ApplicationTick");
        private static readonly MethodInfo? ModuleOnApplicationTickMethod = AccessTools2.Method("TaleWorlds.MountAndBlade.Module:OnApplicationTick");
        private static readonly MethodInfo? ScreenManagerTickMethod = AccessTools2.Method("TaleWorlds.Engine.Screens.ScreenManager:Tick");
        private static readonly MethodInfo? ManagedScriptHolderTickComponentsMethod = AccessTools2.Method("TaleWorlds.Engine.ManagedScriptHolder:TickComponents");
        private static readonly MethodInfo? MissionTickMethod = AccessTools2.Method("TaleWorlds.MountAndBlade.Mission:Tick");
        private static readonly MethodInfo? FinalizerMethod = AccessTools2.Method(typeof(BEWPatch), nameof(Finalizer));

        private static readonly AccessTools.FieldRef<Module, Dictionary<string, Type>>? LoadedSubModuleTypes =
            AccessTools2.FieldRefAccess<Module, Dictionary<string, Type>>("_loadedSubmoduleTypes");

        private static bool _wasButrLoaderInterceptorCalled = false;

        private static void Finalizer(Exception? __exception)
        {
            if (__exception is not null && !SuppressedExceptions.Contains(ExceptionIdentifier.FromException(__exception)))
            {
                SuppressedExceptions.Add(ExceptionIdentifier.FromException(__exception));
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }

        internal static void Enable(Harmony harmony)
        {
            harmony.Patch(ManagedApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));
            harmony.Patch(ModuleOnApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));
            harmony.Patch(ScreenManagerTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));
            harmony.Patch(ManagedScriptHolderTickComponentsMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));
            harmony.Patch(MissionTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));

            if (!_wasButrLoaderInterceptorCalled)
                PatchSubModules(harmony);

            // Managed.ApplicationTick
            harmony.TryPatch(
                AccessTools2.Method("ManagedCallbacks.LibraryCallbacksGenerated:Managed_ApplicationTick"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
            // ScreenManager.Tick
            harmony.TryPatch(
                AccessTools2.Method("ManagedCallbacks.EngineCallbacksGenerated.ScreenManager_Tick"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
            // ManagedScriptHolder.TickComponents
            harmony.TryPatch(
                AccessTools2.Method("ManagedCallbacks.EngineCallbacksGenerated.ManagedScriptHolder_TickComponents"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
            // Mission.Tick
            harmony.TryPatch(
                AccessTools2.Method("TaleWorlds.MountAndBlade.MissionState.FinishMissionLoading"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
            harmony.TryPatch(
                AccessTools2.Method("TaleWorlds.MountAndBlade.MissionState.TickMissionAux"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
            harmony.TryPatch(
                AccessTools2.Method("TaleWorlds.MountAndBlade.MissionState.TickMission"),
                transpiler: AccessTools2.Method(typeof(BEWPatch), nameof(BlankTranspiler)));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(ManagedApplicationTickMethod, FinalizerMethod);
            harmony.Unpatch(ScreenManagerTickMethod, FinalizerMethod);
            harmony.Unpatch(ManagedScriptHolderTickComponentsMethod, FinalizerMethod);
            harmony.Unpatch(MissionTickMethod, FinalizerMethod);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static IEnumerable<CodeInstruction> BlankTranspiler(IEnumerable<CodeInstruction> instructions) => instructions;


        /// <summary>
        /// We need to patch MBSubModuleBase.OnSubModuleLoad because they generally can't be catched.
        /// The reason is, Harmony can't patch a method it's running in.
        /// The exception handling (this one) is started within the
        /// Module.Initialize() -> Module.LoadSubModules() -> Module.InitializeSubModules() -> MBSubModuleBase.OnSubModuleLoad()
        /// We start the exception interception within this scope, so if anything is throwing while within Module.Initialize(),
        /// we will lose that. It's easier to intercept every exception in MBSubModuleBase.OnSubModuleLoad() instead.
        /// </summary>
        private static bool PatchSubModules(Harmony harmony)
        {
            if (LoadedSubModuleTypes is null)
                return false;

            var dict = LoadedSubModuleTypes(Module.CurrentModule);
            foreach (var (_, type) in dict)
            {
                var method = AccessTools2.Method(type, "OnSubModuleLoad");
                if (method is null)
                    continue;

                harmony.Patch(method, finalizer: new HarmonyMethod(FinalizerMethod, before: bew));
            }
            return true;
        }

        // BUTRLoader gives un the ability to intercept every exception call.
        // We will use the earlier entrypoint instead
        private static void OnInitializeSubModulesPrefix()
        {
            _wasButrLoaderInterceptorCalled = true;
            PatchSubModules(new Harmony("Bannerlord.ButterLib.ExceptionHandler.BUTRLoadingInterceptor"));
        }
    }
}