using Bannerlord.ModuleLoader.Extensions;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

using TWModule = TaleWorlds.MountAndBlade.Module;

namespace Bannerlord.ModuleLoader
{
    public class SubModule : MBSubModuleBase
    {
        private delegate void OnSubModuleLoadDelegate(MBSubModuleBase instance);
        private delegate void OnServiceRegistrationDelegate();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var onSubModuleLoad =
                (OnSubModuleLoadDelegate) Delegate.CreateDelegate(
                    typeof(OnSubModuleLoadDelegate),
                    typeof(MBSubModuleBase).GetMethod("OnSubModuleLoad", BindingFlags.Instance | BindingFlags.NonPublic)!);

            var loadedSubmoduleTypesField = typeof(TWModule).GetField("_loadedSubmoduleTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            var submodulesField = typeof(TWModule).GetField("_submodules", BindingFlags.Instance | BindingFlags.NonPublic);
            var loadedSubmoduleTypes = loadedSubmoduleTypesField.GetValue(TWModule.CurrentModule) as Dictionary<string, Type>;
            var submodules = submodulesField.GetValue(TWModule.CurrentModule) as List<MBSubModuleBase>;
            var dictVersionField = loadedSubmoduleTypes.GetType().GetField("version", BindingFlags.Instance | BindingFlags.NonPublic);
            var dictVersion = dictVersionField.GetValue(loadedSubmoduleTypes);

            var loadedSubmoduleTypesCopy = new Dictionary<string, Type>(loadedSubmoduleTypes);

            loadedSubmoduleTypes.Clear();
            dictVersionField.SetValue(loadedSubmoduleTypes, dictVersion);

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;
            var subModules = new Dictionary<string, MBSubModuleBase>();
            foreach (var (name, type) in loadedSubmoduleTypesCopy)
            {
                if (type?.GetConstructor(flags, null, Array.Empty<Type>(), null)?.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                {
                    subModules.Add(name, subModule);
                }
            }
            foreach (var (name, instance) in subModules)
            {
                if (instance.GetType().GetMethod("OnServiceRegistration", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } methodInfo)
                {
                    var onServiceRegistration = (OnServiceRegistrationDelegate) Delegate.CreateDelegate(typeof(OnServiceRegistrationDelegate), instance, methodInfo);
                    onServiceRegistration();
                }
            }
            foreach (var (name, instance) in subModules)
            {
                onSubModuleLoad(instance);
            }
            submodules.AddRange(subModules.Values);
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }

        private static IEnumerable<ModuleInfo> GetLoadedModulesEnumerable()
        {
            var modulesNames = Utilities.GetModulesNames();
            for (var i = 0; i < modulesNames.Length; i++)
            {
                var moduleInfo = new ModuleInfo();
                moduleInfo.Load(modulesNames[i]);
                yield return moduleInfo;
            }
        }
    }
}