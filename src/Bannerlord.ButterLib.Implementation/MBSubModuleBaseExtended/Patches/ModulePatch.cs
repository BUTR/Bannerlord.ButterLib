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

using TaleWorlds.MountAndBlade;
using TWModule = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches
{
    internal sealed class ModulePatch
    {
        private static ILogger _log = default!;

        private static readonly Type? TargetType = typeof(TWModule);

        private static readonly MethodInfo? miTargetMethodUnLoad = AccessTools.Method(TargetType, "FinalizeSubModules");
        private static readonly MethodInfo? miPatchMethodUnLoad = AccessTools.Method(typeof(ModulePatch), nameof(FinalizeSubModulesPostfix));

        private static readonly MethodInfo? miTargetMethodScreenAsRoot = AccessTools.Method(TargetType, "SetInitialModuleScreenAsRootScreen");
        private static readonly MethodInfo? miPatchMethodScreenAsRoot = AccessTools.Method(typeof(ModulePatch), nameof(Transpiler));

        private static readonly MethodInfo? miMBSubModuleBaseScreenAsRootEvent = AccessTools.Method(typeof(MBSubModuleBase), "OnBeforeInitialModuleScreenSetAsRoot");
        private static readonly MethodInfo? miDelayedScreenAsRootEventCaller = AccessTools.Method(typeof(ModulePatch), nameof(DelayedScreenAsRootEvent));

        internal static bool Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _log = provider?.GetRequiredService<ILogger<ModulePatch>>() ?? NullLogger<ModulePatch>.Instance;

            return CheckRequiredMethodInfos()
                   && harmony.Patch(miTargetMethodUnLoad, postfix: new HarmonyMethod(miPatchMethodUnLoad)) is not null
                   && harmony.Patch(miTargetMethodScreenAsRoot, transpiler: new HarmonyMethod(miPatchMethodScreenAsRoot)) is not null;
        }

        internal static bool Disable(Harmony harmony)
        {
            if (CheckRequiredMethodInfos())
            {
                harmony.Unpatch(miTargetMethodUnLoad, miPatchMethodUnLoad);
                harmony.Unpatch(miTargetMethodScreenAsRoot, miPatchMethodScreenAsRoot);
            }

            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            try
            {
                int screenAsRootEventIndex = -1, finallyIndex = -1;
                List<CodeInstruction> codes = new(instructions);
                for (int i = 0; i < codes.Count; ++i)
                {
                    if (screenAsRootEventIndex < 0 && codes[i].Calls(miMBSubModuleBaseScreenAsRootEvent))
                    {
                        screenAsRootEventIndex = i;
                        continue;
                    }
                    if (finallyIndex < 0 && codes[i].opcode == OpCodes.Endfinally)
                    {
                        finallyIndex = i;
                        break;
                    }
                }
                if (screenAsRootEventIndex < 0 || finallyIndex < 0)
                {
                    _log.LogDebug("Transpiler for TaleWorlds.MountAndBlade.Module.SetInitialModuleScreenAsRootScreen could not find code hooks!");
                    MBSubModuleBaseExSubSystem.LogNoHooksIssue(_log, screenAsRootEventIndex, finallyIndex, codes, MethodBase.GetCurrentMethod());
                }
                else
                {
                    codes.InsertRange(finallyIndex + 1, new CodeInstruction[] { new CodeInstruction(opcode: OpCodes.Ldarg_0),
                                                                                new CodeInstruction(opcode: OpCodes.Call, operand: miDelayedScreenAsRootEventCaller) });
                    codes[finallyIndex + 1].MoveLabelsFrom(codes[finallyIndex + 3]);
                }
                
                return codes.AsEnumerable();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error while applying Harmony transpiler for TaleWorlds.MountAndBlade.Module.SetInitialModuleScreenAsRootScreen");
                return instructions;
            }
        }

        private static bool CheckRequiredMethodInfos() =>
            MBSubModuleBaseExSubSystem.NotNull(_log, TargetType, nameof(TargetType))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miTargetMethodUnLoad, nameof(miTargetMethodUnLoad))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miPatchMethodUnLoad, nameof(miPatchMethodUnLoad))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miTargetMethodScreenAsRoot, nameof(miTargetMethodScreenAsRoot))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miPatchMethodScreenAsRoot, nameof(miPatchMethodScreenAsRoot))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miMBSubModuleBaseScreenAsRootEvent, nameof(miMBSubModuleBaseScreenAsRootEvent))
            & MBSubModuleBaseExSubSystem.NotNull(_log, miDelayedScreenAsRootEventCaller, nameof(miDelayedScreenAsRootEventCaller));

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void FinalizeSubModulesPostfix(TWModule __instance)
        {
            foreach (MBSubModuleBaseEx submodule in __instance.SubModules.Where(s => s is MBSubModuleBaseEx))
            {
                submodule.OnAllSubModulesUnLoaded();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DelayedScreenAsRootEvent(TWModule instance)
        {
            foreach (MBSubModuleBaseEx submodule in instance.SubModules.Where(s => s is MBSubModuleBaseEx))
            {
                submodule.OnBeforeInitialModuleScreenSetAsRootDelayed();
            }
        }
    }
}
