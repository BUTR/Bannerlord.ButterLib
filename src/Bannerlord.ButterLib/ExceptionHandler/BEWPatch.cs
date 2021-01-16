using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class BEWPatch
    {
        internal static readonly HashSet<Exception> SuppressedExceptions = new();

        private static readonly MethodInfo? ManagedApplicationTickMethod = Method(typeof(Managed), "ApplicationTick");
        private static readonly MethodInfo? ScriptComponentBehaviourOnTickMethod = Method(typeof(ScriptComponentBehaviour), "OnTick");
        private static readonly MethodInfo? ScreenManagerTickMethod = Method(typeof(ScreenManager), "Tick");
        private static readonly MethodInfo? ModuleOnApplicationTickMethod = Method(typeof(TaleWorlds.MountAndBlade.Module), "OnApplicationTick");
        private static readonly MethodInfo? MissionTickMethod = Method(typeof(Mission), "Tick");
        private static readonly MethodInfo? MissionBehaviourTickMethod = Method(typeof(MissionBehaviour), "OnMissionTick");
        private static readonly MethodInfo? MissionViewOnMissionScreenTickMethod = Method(typeof(MissionView), "OnMissionScreenTick");
        private static readonly MethodInfo? FinalizerMethod = Method(typeof(BEWPatch), nameof(Finalizer));

        private static void Finalizer(Exception? __exception)
        {
            if (__exception is not null && !SuppressedExceptions.Contains(__exception))
            {
                SuppressedExceptions.Add(__exception);
                HtmlBuilder.BuildAndShow(new CrashReport(__exception));
            }
        }
        private static void FinalizerSuppress(ref Exception? __exception)
        {
            __exception = null;
        }

        internal static void Enable(Harmony harmony)
        {
            var array = new [] { "org.calradia.admiralnelson.betterexceptionwindow" };

            harmony.Patch(ManagedApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(ScriptComponentBehaviourOnTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(ScreenManagerTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));

            harmony.Patch(ModuleOnApplicationTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(MissionTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(MissionBehaviourTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
            harmony.Patch(MissionViewOnMissionScreenTickMethod, finalizer: new HarmonyMethod(FinalizerMethod, before: array));
        }

        internal static void Disable(Harmony harmony)
        {
            harmony.Unpatch(ManagedApplicationTickMethod, FinalizerMethod);
            harmony.Unpatch(ScriptComponentBehaviourOnTickMethod, FinalizerMethod);
            harmony.Unpatch(ScreenManagerTickMethod, FinalizerMethod);

            harmony.Unpatch(ModuleOnApplicationTickMethod, FinalizerMethod);
            harmony.Unpatch(MissionTickMethod, FinalizerMethod);
            harmony.Unpatch(MissionBehaviourTickMethod, FinalizerMethod);
            harmony.Unpatch(MissionViewOnMissionScreenTickMethod, FinalizerMethod);
        }
    }
}