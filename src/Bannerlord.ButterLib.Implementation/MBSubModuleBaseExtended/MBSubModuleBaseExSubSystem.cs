using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches;
using Bannerlord.ButterLib.MBSubModuleBaseExtended;
using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using TaleWorlds.MountAndBlade;

using TWModule = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended
{
    class MBSubModuleBaseExSubSystem : ISubSystem
    {
        public static MBSubModuleBaseExSubSystem? Instance { get; private set; }
        public string Id => "MBSubModuleBase extended";
        public string Description => "Introduces a MBSubModuleBase-derived abstract class, that provides new SubModule events.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => false;

        private readonly Harmony _harmony = new("Bannerlord.ButterLib.MBSubModuleBaseEx");

        public MBSubModuleBaseExSubSystem()
        {
            Instance = this;
        }
        public void Enable()
        {
            IsEnabled = true;

            ModulePatch.Enable(_harmony);
            MBGameManagerPatch.Enable(_harmony);

            //There is no way to patch Module.InitializeSubModules to add OnAllSubModulesLoaded.
            //We tried to patch the last loaded submodule instead, but that caused problems if it had PatchAll() called in it.
            //So for now we removed OnAllSubModulesLoaded event entirely.
        }
        public void Disable()
        {
            IsEnabled = false;
            ModulePatch.Disable(_harmony);
            MBGameManagerPatch.Disable(_harmony);
            //Think about DelayedSubModuleManager.Unregister
        }

        internal static void LogNoHooksIssue(ILogger logger, int originalCallIndex, int finallyIndex, List<CodeInstruction> codes, MethodBase currentMethod)
        {
            StringBuilder issueInfo = new("Indexes: ");
            issueInfo.Append($"\n\toriginalCallIndex = {originalCallIndex}.\n\tfinallyIndex={finallyIndex}.");
            issueInfo.Append($"\nIL:");
            for (int i = 0; i < codes.Count; ++i)
            {
                issueInfo.Append($"\n\t{i:D4}:\t{codes[i]}");
            }
            // get info about other transpilers on OriginalMethod
            HarmonyLib.Patches patches = Harmony.GetPatchInfo(currentMethod);
            if (patches != null)
            {
                issueInfo.Append($"\nOther transpilers:");
                foreach (Patch patch in patches.Transpilers)
                {
                    issueInfo.Append(patch.GetDebugString());
                }
            }
            logger.LogDebug(issueInfo.ToString());
        }

        internal static bool NotNull<T>(ILogger logger, T obj, string name) where T : class?
        {
            if (obj is null)
            {
                logger.LogError($"{name} is null!");
                return false;
            }
            return true;
        }
    }
}