using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class BEWPatch
    {
        internal static readonly HashSet<Exception> SuppressedExceptions = new();

        private static readonly MethodInfo? ManagedApplicationTickMethod = AccessTools2.Method(typeof(Managed), "ApplicationTick");
        private static readonly MethodInfo? ScreenManagerTickMethod = AccessTools2.Method(typeof(ScreenManager), "Tick");
        private static readonly MethodInfo? ManagedScriptHolderTickComponentsMethod = AccessTools2.Method(typeof(ManagedScriptHolder), "TickComponents");
        private static readonly MethodInfo? MissionTickMethod = AccessTools2.Method(typeof(Mission), "Tick");
        public static readonly MethodInfo? FinalizerMethod = AccessTools2.Method(typeof(BEWPatch), nameof(Finalizer));

        private static void Finalizer(Exception? __exception)
        {
            if (__exception is not null && !SuppressedExceptions.Contains(__exception))
            {
                SuppressedExceptions.Add(__exception);
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }

        internal static void Enable(Harmony harmony)
        {
            var array = new[] { "org.calradia.admiralnelson.betterexceptionwindow" };

            harmony.Patch(ManagedApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(ScreenManagerTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(ManagedScriptHolderTickComponentsMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(MissionTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));

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
    }
}