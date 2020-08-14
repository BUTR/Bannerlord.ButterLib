#define Tick

using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib
{
    public class LoaderSubModule : MBSubModuleBase
    {
        private List<(MBSubModuleBase, Type)> ImplementationSubModules { get; } = new List<(MBSubModuleBase, Type)>();

        private void LoadAllImplementationAssemblies()
        {
            var implementationAssemblies = new List<Assembly>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            var thisAssembly = typeof(LoaderSubModule).Assembly;

            var assemblyFile = new FileInfo(thisAssembly.Location);
            if (!assemblyFile.Exists)
                return;

            var assemblyDirectory = assemblyFile.Directory;
            if (assemblyDirectory == null || !assemblyDirectory.Exists)
                return;

            var matches = assemblyDirectory.GetFiles("Bannerlord.ButterLib.Implementation.*.dll");
            if (!matches.Any())
                return;

            foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(m.Name))))
            {
                var assembly = Assembly.ReflectionOnlyLoadFrom(match.FullName);
                
                var gameVersion = ApplicationVersionUtils.TryParse(ApplicationVersionUtils.GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;
                var metadatas = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
                var supportedVersionStr = metadatas.FirstOrDefault(a => a.Key == "GameVersion")?.Value;
                var supportedVersion = !string.IsNullOrEmpty(supportedVersionStr) && ApplicationVersionUtils.TryParse(supportedVersionStr, out var sv)
                    ? sv
                    : (ApplicationVersion?) null;
                if (gameVersion != null && supportedVersion != null && gameVersion.Value.IsSameWithoutRevision(supportedVersion.Value))
                    implementationAssemblies.Add(Assembly.LoadFrom(match.FullName));
            }

            var submodules = implementationAssemblies.SelectMany(assembly => assembly.GetTypes().Where(t =>
                t.FullName != typeof(LoaderSubModule).FullName && typeof(MBSubModuleBase).IsAssignableFrom(t)));
            foreach (var subModuleType in submodules)
            {
                if (subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null)?.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                    ImplementationSubModules.Add((subModule, subModuleType));
            }
        }


        private readonly Dictionary<Type, Dictionary<string, MethodInfo?>> _reflectionCache = new Dictionary<Type, Dictionary<string, MethodInfo?>>();
        private readonly object[] _emptyParams = Array.Empty<object>();
#if Tick
        private readonly object[] _dtParams = new object[1];
#endif

        public LoaderSubModule()
        {
            LoadAllImplementationAssemblies();

            foreach (var (_, subModuleType) in ImplementationSubModules)
            {
                if (!_reflectionCache.ContainsKey(subModuleType))
                    _reflectionCache.Add(subModuleType, new Dictionary<string, MethodInfo?>());

                _reflectionCache[subModuleType]["OnSubModuleLoad"] = AccessTools.Method(subModuleType, "OnSubModuleLoad");
                _reflectionCache[subModuleType]["OnSubModuleUnloaded"] = AccessTools.Method(subModuleType, "OnSubModuleUnloaded");
                _reflectionCache[subModuleType]["OnApplicationTick"] = AccessTools.Method(subModuleType, "OnApplicationTick");
                _reflectionCache[subModuleType]["OnBeforeInitialModuleScreenSetAsRoot"] = AccessTools.Method(subModuleType, "OnBeforeInitialModuleScreenSetAsRoot");
                _reflectionCache[subModuleType]["OnGameStart"] = AccessTools.Method(subModuleType, "OnGameStart");
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            foreach (var (subModule, subModuleType) in ImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleLoad"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            foreach (var (subModule, subModuleType) in ImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleUnloaded"]?.Invoke(subModule, _emptyParams);
        }
#if Tick
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            _dtParams[0] = dt;
            foreach (var (subModule, subModuleType) in ImplementationSubModules)
                _reflectionCache[subModuleType]["OnApplicationTick"]?.Invoke(subModule, _dtParams);
        }
#endif
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            foreach (var (subModule, subModuleType) in ImplementationSubModules)
                _reflectionCache[subModuleType]["OnBeforeInitialModuleScreenSetAsRoot"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            var @params = new object[] { game, gameStarterObject };
            foreach (var (subModule, subModuleType) in ImplementationSubModules)
                _reflectionCache[subModuleType]["OnGameStart"]?.Invoke(subModule, @params);
        }

        /// <exclude/>
        public override bool DoLoading(Game game)
        {
            return base.DoLoading(game) && ImplementationSubModules.All(tuple => tuple.Item1.DoLoading(game));
        }
        /// <exclude/>
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        /// <exclude/>
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        /// <exclude/>
        public override void BeginGameStart(Game game)
        {
            base.BeginGameStart(game);


            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.BeginGameStart(game);
        }
        /// <exclude/>
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnGameEnd(game);
        }
        /// <exclude/>
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnGameInitializationFinished(game);
        }
        /// <exclude/>
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnMissionBehaviourInitialize(mission);
        }
        /// <exclude/>
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        /// <exclude/>
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);

            foreach (var (subModule, _) in ImplementationSubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
    }
}