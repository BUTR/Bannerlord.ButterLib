using Bannerlord.ButterLib.ExceptionHandler.DebuggerDetection;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.ExceptionHandler;

// BEW commentary:
// TaleWorlds.DotNet.Managed:ApplicationTick                              -> Replicated
// TaleWorlds.Engine.ScriptComponentBehaviour:OnTick                      -> Called by TaleWorlds.Engine.ManagedScriptHolder:TickComponents
// TaleWorlds.MountAndBlade.Module:OnApplicationTick                      -> Replicated
// TaleWorlds.MountAndBlade.View.Missions.MissionView:OnMissionScreenTick -> Called by TaleWorlds.MountAndBlade.View.Screen.MissionScreen:OnFrameTick
// TaleWorlds.ScreenSystem.ScreenManager:Tick                             -> Replicated
// TaleWorlds.MountAndBlade.Mission:Tick                                  -> Replicated
// TaleWorlds.MountAndBlade.MissionBehaviour:OnMissionTick                -> Called by TaleWorlds.MountAndBlade.Mission:Tick
// TaleWorlds.MountAndBlade.MBSubModuleBase:OnSubModuleLoad               -> Replicated
internal sealed class BEWPatch
{
    private static bool _cachedDebuggerAttached;
    private static int _lastCheckTicks;

    public static bool IsDebuggerAttached()
    {
        var currentTicks = Environment.TickCount;
        if (currentTicks - _lastCheckTicks < 100)
            return _cachedDebuggerAttached;

        _cachedDebuggerAttached = false;

        if (Debugger.IsAttached)
            _cachedDebuggerAttached = true;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            _cachedDebuggerAttached = ProcessDebug.CheckProcessDebugObjectHandle();

        _lastCheckTicks = currentTicks;
        return _cachedDebuggerAttached;
    }

    internal record ExceptionIdentifier(Type Type, string? StackTrace, string Message)
    {
        public static ExceptionIdentifier FromException(Exception e) => new(e.GetType(), e.StackTrace, e.Message);
    }

    internal static readonly HashSet<ExceptionIdentifier> SuppressedExceptions = [];

    private static readonly string[] BEW = ["org.calradia.admiralnelson.betterexceptionwindow"];


    private static readonly MethodInfo? ManagedApplicationTickMethod = AccessTools2.Method("TaleWorlds.DotNet.Managed:ApplicationTick");
    private static readonly MethodInfo? ModuleOnApplicationTickMethod = AccessTools2.Method("TaleWorlds.MountAndBlade.Module:OnApplicationTick");
    private static readonly MethodInfo? ScreenManagerTickMethod = AccessTools2.Method("TaleWorlds.ScreenSystem.ScreenManager:Tick");
    private static readonly MethodInfo? ManagedScriptHolderTickComponentsMethod = AccessTools2.Method("TaleWorlds.Engine.ManagedScriptHolder:TickComponents");
    private static readonly MethodInfo? MissionTickMethod = AccessTools2.Method("TaleWorlds.MountAndBlade.Mission:Tick");
    public static readonly MethodInfo? FinalizerMethod = SymbolExtensions2.GetMethodInfo((Exception x) => Finalizer(x));

    private static void Finalizer(Exception? __exception)
    {
        if (__exception is not null)
        {
            if (ExceptionHandlerSubSystem.Instance?.DisableWhenDebuggerIsAttached == true && IsDebuggerAttached())
                return;

            ExceptionReporter.Show(__exception);
        }
    }

    internal static void Enable(Harmony harmony)
    {
        harmony.Patch(ManagedApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: BEW));
        harmony.Patch(ModuleOnApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: BEW));
        harmony.Patch(ScreenManagerTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: BEW));
        harmony.Patch(ManagedScriptHolderTickComponentsMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: BEW));
        harmony.Patch(MissionTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: BEW));

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
        harmony.Unpatch(ModuleOnApplicationTickMethod, FinalizerMethod);
        harmony.Unpatch(ScreenManagerTickMethod, FinalizerMethod);
        harmony.Unpatch(ManagedScriptHolderTickComponentsMethod, FinalizerMethod);
        harmony.Unpatch(MissionTickMethod, FinalizerMethod);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static IEnumerable<CodeInstruction> BlankTranspiler(IEnumerable<CodeInstruction> instructions) => instructions;
}