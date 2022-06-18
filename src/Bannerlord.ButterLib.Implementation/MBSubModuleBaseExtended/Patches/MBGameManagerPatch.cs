using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.MBSubModuleBaseExtended;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches
{
    internal sealed class MBGameManagerPatch
    {
        private static ILogger _log = default!;

        private static readonly Type? TargetType = typeof(MBGameManager);

        private static readonly MethodInfo? miTargetMethodGameStart = AccessTools.Method(TargetType, "OnGameStart");
        private static readonly MethodInfo? miTargetMethodGameEnd = AccessTools.Method(TargetType, "OnGameEnd");

        private static readonly MethodInfo? miPatchMethod = AccessTools.Method(typeof(MBGameManagerPatch), nameof(Transpiler));

        private static readonly MethodInfo? miMBSubModuleBaseOnGameStartEvent = AccessTools.Method(typeof(MBSubModuleBase), "OnGameStart");
        private static readonly MethodInfo? miMBSubModuleBaseOnGameEndEvent = AccessTools.Method(typeof(MBSubModuleBase), "OnGameEnd");

        private static readonly MethodInfo? miDelayedOnGameStartEventCaller = AccessTools.Method(typeof(MBGameManagerPatch), nameof(DelayedOnGameStartEvent));
        private static readonly MethodInfo? miDelayedOnGameEndEventCaller = AccessTools.Method(typeof(MBGameManagerPatch), nameof(DelayedOnGameEndEvent));

        internal static bool Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _log = provider?.GetRequiredService<ILogger<ModulePatch>>() ?? NullLogger<ModulePatch>.Instance;

            return
                CheckRequiredMethodInfos()
                && harmony.Patch(miTargetMethodGameStart, transpiler: new HarmonyMethod(miPatchMethod)) is not null
                && harmony.Patch(miTargetMethodGameEnd, transpiler: new HarmonyMethod(miPatchMethod)) is not null;
        }

        internal static bool Disable(Harmony harmony)
        {
            if (CheckRequiredMethodInfos())
            {
                harmony.Unpatch(miTargetMethodGameStart, miPatchMethod);
                harmony.Unpatch(miTargetMethodGameEnd, miPatchMethod);
            }

            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase __originalMethod)
        {
            MethodInfo miToSearchFor;
            CodeInstruction[] ciToAdd;
            var originalMethodName = "TaleWorlds.MountAndBlade.MBGameManager." + __originalMethod.Name;

            switch (__originalMethod.Name)
            {
                case "OnGameStart":
                    miToSearchFor = miMBSubModuleBaseOnGameStartEvent!;
                    ciToAdd = new CodeInstruction[] { new(opcode: OpCodes.Ldarg_1),
                                                      new(opcode: OpCodes.Ldarg_2),
                                                      new(opcode: OpCodes.Call, operand: miDelayedOnGameStartEventCaller) };
                    break;
                case "OnGameEnd":
                    miToSearchFor = miMBSubModuleBaseOnGameEndEvent!;
                    ciToAdd = new CodeInstruction[] { new(opcode: OpCodes.Ldarg_1),
                                                      new(opcode: OpCodes.Call, operand: miDelayedOnGameEndEventCaller) };
                    break;
                default:
                    _log.LogError("Error while applying Harmony transpiler for " + originalMethodName + " - unexpected target method!");
                    return instructions;
            }

            return AddDelayedEvent(instructions, miToSearchFor, ciToAdd, originalMethodName);
        }

        private static bool CheckRequiredMethodInfos() =>
            MBSubModuleBaseExSubSystem.NotNull(_log, TargetType, nameof(TargetType))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miTargetMethodGameStart, nameof(miTargetMethodGameStart))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miTargetMethodGameEnd, nameof(miTargetMethodGameEnd))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miPatchMethod, nameof(miPatchMethod))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miMBSubModuleBaseOnGameStartEvent, nameof(miMBSubModuleBaseOnGameStartEvent))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miDelayedOnGameStartEventCaller, nameof(miDelayedOnGameStartEventCaller));

        private static IEnumerable<CodeInstruction> AddDelayedEvent(IEnumerable<CodeInstruction> instructions, MethodInfo miToSearchFor, CodeInstruction[] ciToAdd, string originalMethodName)
        {
            var codes = new List<CodeInstruction>(instructions);
            try
            {
                int originalCallIndex = -1, finallyIndex = -1;
                for (var i = 0; i < codes.Count; ++i)
                {
                    if (originalCallIndex < 0 && codes[i].Calls(miToSearchFor))
                    {
                        originalCallIndex = i;
                        continue;
                    }
                    if (finallyIndex < 0 && codes[i].opcode == OpCodes.Endfinally)
                    {
                        finallyIndex = i;
                        break;
                    }
                }
                if (originalCallIndex < 0 || finallyIndex < 0)
                {
                    _log.LogDebug("Transpiler for " + originalMethodName + " could not find code hooks!");
                    MBSubModuleBaseExSubSystem.LogNoHooksIssue(_log, originalCallIndex, finallyIndex, codes, MethodBase.GetCurrentMethod()!);
                }
                else
                {
                    codes.InsertRange(finallyIndex + 1, ciToAdd);
                    codes[finallyIndex + 1].MoveLabelsFrom(codes[finallyIndex + ciToAdd.Length + 1]);
                }

                return codes;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error while applying Harmony transpiler for " + originalMethodName);
                return codes;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DelayedOnGameStartEvent(Game game, IGameStarter gameStarter)
        {
            foreach (var submodule in Module.CurrentModule.SubModules.OfType<IMBSubModuleBaseEx>())
            {
                submodule.OnGameStartDelayed(game, gameStarter);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DelayedOnGameEndEvent(Game game)
        {
            foreach (var submodule in Module.CurrentModule.SubModules.OfType<IMBSubModuleBaseEx>())
            {
                submodule.OnGameEndDelayed(game);
            }
        }
    }
}