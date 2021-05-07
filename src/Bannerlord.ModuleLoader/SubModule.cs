using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TaleWorlds.MountAndBlade;

using TWModule = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ModuleLoader
{
    public class SubModule : MBSubModuleBase
    {
        private const string SWarningTitle =
@"{=yu4cP89RWb}Warning from Bannerlord.ModuleLoader!";
        private const string SErrorHarmonyNotFound =
@"{=EEVJa5azpB}Bannerlord.Harmony module was not found!";
        private const string SErrorModuleLoaderNotFound =
@"{=YjsGP3mUaj}Bannerlord.ModuleLoader module was not found!";
        private const string SErrorModuleLoaderNotFirst =
@"{=m8XpFLx9Q7}Bannerlord.ModuleLoader is not second in loading order!
It should load right after Harmony!";
        private const string SErrorOfficialModulesLoadedBefore =
@"{=5tDjYVah2q}ModuleLoader is loaded after the official modules!
Make sure ModuleLoader is loaded before them!";
        private const string SErrorOfficialModules =
@"{=3ys4VQT7CQ}The following modules were loaded before ModuleLoader:";
        private const string SMessageContinue =
@"{=eXs6FLm5DP}It's strongly recommended to terminate the game now. Do you wish to terminate it?";

        private delegate void OnSubModuleLoadDelegate(MBSubModuleBase instance);
        private delegate void OnServiceRegistrationDelegate();
        private delegate MBSubModuleBase ConstructorDelegate();

        public SubModule()
        {
            CheckLoadOrder();
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            ReplaceSubModuleCreation();
        }

        private static void ReplaceSubModuleCreation()
        {
            var onSubModuleLoad = AccessTools2.GetDelegate<OnSubModuleLoadDelegate>(typeof(MBSubModuleBase), "OnSubModuleLoad");

            var loadedSubmoduleTypesField = AccessTools2.FieldRefAccess<TWModule, Dictionary<string, Type>>("_loadedSubmoduleTypes");
            var submodulesField = AccessTools2.FieldRefAccess<TWModule, List<MBSubModuleBase>>("_submodules");
            var dictVersionField = AccessTools2.FieldRefAccess<Dictionary<string, Type>, int>("version");

            if (onSubModuleLoad is null || loadedSubmoduleTypesField is null || submodulesField is null || dictVersionField is null)
                return;

            var loadedSubmoduleTypes = loadedSubmoduleTypesField(TWModule.CurrentModule);
            var submodules = submodulesField(TWModule.CurrentModule);
            var dictVersion = dictVersionField(loadedSubmoduleTypes);

            var loadedSubmoduleTypesCopy = new Dictionary<string, Type>(loadedSubmoduleTypes);
            var preLoadedSubModules = submodules.ToDictionary(k => k.GetType().FullName, v => v.GetType());

            loadedSubmoduleTypes.Clear();
            dictVersionField(loadedSubmoduleTypes) = dictVersion;

            var subModules = new Dictionary<string, MBSubModuleBase>();
            foreach (var (name, type) in loadedSubmoduleTypesCopy.Except(preLoadedSubModules))
            {
                var constructor = AccessTools2.GetConstructorDelegate<ConstructorDelegate>(type, Array.Empty<Type>());
                if (constructor is null) continue;

                subModules.Add(name, constructor());
            }
            submodules.AddRange(subModules.Values);
            foreach (var (name, instance) in subModules)
            {
                var onServiceRegistration = AccessTools2.GetDelegate<OnServiceRegistrationDelegate>(instance, AccessTools.Method(instance.GetType(), "OnServiceRegistration"));
                if (onServiceRegistration is null) continue;

                onServiceRegistration();
            }
            foreach (var (name, instance) in subModules)
            {
                onSubModuleLoad(instance);
            }

            loadedSubmoduleTypesCopy.Clear();
            subModules.Clear();
        }

        private static void CheckLoadOrder()
        {
            var loadedModules = BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().ToList();

            var sb = new StringBuilder();

            var harmonyModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.Harmony");
            var harmonyModuleIndex = harmonyModule is not null ? loadedModules.IndexOf(harmonyModule) : -1;
            if (harmonyModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorHarmonyNotFound)?.ToString());
            }

            var moduleLoaderModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ModuleLoader");
            var moduleLoaderModuleIndex = moduleLoaderModule is not null ? loadedModules.IndexOf(moduleLoaderModule) : -1;
            if (moduleLoaderModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorModuleLoaderNotFound)?.ToString());
            }
            if (moduleLoaderModuleIndex != 1) // Straight after Bannerlord.Harmony
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorModuleLoaderNotFirst)?.ToString());
            }

            var officialModules = loadedModules.Where(x => x.IsOfficial).Select(x => (Module: x, Index: loadedModules.IndexOf(x)));
            var modulesLoadedBefore = officialModules.Where(tuple => tuple.Index < moduleLoaderModuleIndex).ToList();
            if (modulesLoadedBefore.Count > 0)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModulesLoadedBefore)?.ToString());
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModules)?.ToString());
                foreach (var (module, _) in modulesLoadedBefore)
                    sb.AppendLine(module.Id);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SMessageContinue)?.ToString());

                switch (MessageBox.Show(sb.ToString(), TextObjectHelper.Create(SWarningTitle)?.ToString(), MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}