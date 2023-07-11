using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.MBSubModuleBaseExtended;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

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

using Module = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches
{
    internal sealed class MBGameManagerPatch
    {
        private static ILogger _log = default!;

        private static readonly MethodInfo? miTargetMethodGameStart = AccessTools2.Method("TaleWorlds.MountAndBlade.MBGameManager:OnGameStart");
        private static readonly MethodInfo? miTargetMethodGameEnd = AccessTools2.Method("TaleWorlds.MountAndBlade.MBGameManager:OnGameEnd");

        private static readonly MethodInfo? miPatchMethod = AccessTools2.Method("Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches.MBGameManagerPatch:Transpiler");

        private static readonly MethodInfo? miMBSubModuleBaseOnGameStartEvent = AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameStart");
        private static readonly MethodInfo? miMBSubModuleBaseOnGameEndEvent = AccessTools2.Method("TaleWorlds.MountAndBlade.MBSubModuleBase:OnGameEnd");

        private static readonly MethodInfo? miDelayedOnGameStartEventCaller = AccessTools2.Method("Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches.MBGameManagerPatch:DelayedOnGameStartEvent");
        private static readonly MethodInfo? miDelayedOnGameEndEventCaller = AccessTools2.Method("Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches.MBGameManagerPatch:DelayedOnGameEndEvent");

        internal static bool Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _log = provider?.GetService<ILogger<ModulePatch>>() ?? NullLogger<ModulePatch>.Instance;

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
                    _log.LogError("Error while applying Harmony transpiler for {Method} - unexpected target method!", originalMethodName);
                    return instructions;
            }

            return AddDelayedEvent(instructions, miToSearchFor, ciToAdd, originalMethodName);
        }

        private static bool CheckRequiredMethodInfos() =>
            MBSubModuleBaseExSubSystem.NotNull(_log, miTargetMethodGameStart, nameof(miTargetMethodGameStart))
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
                    _log.LogDebug("Transpiler for {Method} could not find code hooks!", originalMethodName);
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
                _log.LogError(ex, "Error while applying Harmony transpiler for {Method}", originalMethodName);
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