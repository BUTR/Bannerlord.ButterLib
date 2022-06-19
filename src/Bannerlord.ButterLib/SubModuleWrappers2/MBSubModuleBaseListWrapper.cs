using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SubModuleWrappers2
{
#pragma warning disable CS0109 // ReSharper disable VirtualMemberNeverOverridden.Global
    /// <summary>
    /// Wraps several <see cref="MBSubModuleBase"/> so when the game calls any method, it
    /// will be passed to its children.
    /// </summary>
    public class MBSubModuleBaseListWrapper : MBSubModuleBase
    {
        protected readonly List<MBSubModuleBaseWrapper> _subModules = new();
        public IReadOnlyList<MBSubModuleBaseWrapper> SubModules => _subModules.AsReadOnly();

        public new virtual void OnSubModuleLoad()
        {
            foreach (var subModule in SubModules)
                subModule.OnSubModuleLoad();
        }
        public new virtual void OnSubModuleUnloaded()
        {
            foreach (var subModule in SubModules)
                subModule.OnSubModuleUnloaded();
        }
        public new virtual void OnApplicationTick(float dt)
        {
            foreach (var subModule in SubModules)
                subModule.OnApplicationTick(dt);
        }
        public new virtual void OnBeforeInitialModuleScreenSetAsRoot()
        {
            foreach (var subModule in SubModules)
                subModule.OnBeforeInitialModuleScreenSetAsRoot();
        }
        public new virtual void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnGameStart(game, gameStarterObject);
        }
        public new virtual void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            foreach (var subModule in SubModules)
                subModule.InitializeGameStarter(game, starterObject);
        }

        public virtual void OnServiceRegistration()
        {
            foreach (var subModule in SubModules)
                subModule.OnServiceRegistration();
        }

        public new virtual bool DoLoading(Game game)
        {
            return SubModules.All(subModule => subModule.DoLoading(game));
        }
        public new virtual void OnGameLoaded(Game game, object initializerObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        public new virtual void OnCampaignStart(Game game, object starterObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        public new virtual void BeginGameStart(Game game)
        {
            foreach (var subModule in SubModules)
                subModule.BeginGameStart(game);
        }
        public new virtual void OnGameEnd(Game game)
        {
            foreach (var subModule in SubModules)
                subModule.OnGameEnd(game);
        }
        public new virtual void OnGameInitializationFinished(Game game)
        {
            foreach (var subModule in SubModules)
                subModule.OnGameInitializationFinished(game);
        }
        public new virtual void OnBeforeMissionBehaviourInitialize(Mission mission)
        {
            foreach (var subModule in SubModules)
                subModule.OnBeforeMissionBehaviourInitialize(mission);
        }
        public new virtual void OnMissionBehaviourInitialize(Mission mission)
        {
            foreach (var subModule in SubModules)
                subModule.OnMissionBehaviourInitialize(mission);
        }
        public new virtual void OnMultiplayerGameStart(Game game, object starterObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        public new virtual void OnNewGameCreated(Game game, object initializerObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
        public new virtual void RegisterSubModuleObjects(bool isSavedCampaign)
        {
            foreach (var subModule in SubModules)
                subModule.RegisterSubModuleObjects(isSavedCampaign);
        }
        public new virtual void AfterRegisterSubModuleObjects(bool isSavedCampaign)
        {
            foreach (var subModule in SubModules)
                subModule.AfterRegisterSubModuleObjects(isSavedCampaign);
        }
        public new virtual void OnAfterGameInitializationFinished(Game game, object starterObject)
        {
            foreach (var subModule in SubModules)
                subModule.OnAfterGameInitializationFinished(game, starterObject);
        }
        public new virtual void OnConfigChanged()
        {
            foreach (var subModule in SubModules)
                subModule.OnConfigChanged();
        }
        public new virtual void OnInitialState()
        {
            foreach (var subModule in SubModules)
                subModule.OnInitialState();
        }
        
        public new virtual void AfterAsyncTickTick(float dt)
        {
            foreach (var subModule in SubModules)
                subModule.AfterAsyncTickTick(dt);
        }
    }
#pragma warning restore CS0109 // ReSharper restore VirtualMemberNeverOverridden.Global
}