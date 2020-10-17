using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.Logging;
using Bannerlord.ButterLib.Implementation.SaveSystem;
using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;
using Bannerlord.ButterLib.SaveSystem;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Implementation
{
    public sealed class SubModule : MBSubModuleBase
    {
        private bool FirstInit { get; set; } = true;

        internal static ILogger? Logger { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Logger = this.GetTempServiceProvider().GetRequiredService<ILogger<SubModule>>();
            Logger.LogTrace("OnSubModuleLoad() started tracking.");

            Logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
            Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, this.GetTempServiceProvider()!);

            var services = this.GetServices();
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddSingleton<CampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton<DistanceMatrixStatic, DistanceMatrixStaticImplementation>();
            services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();
            services.AddTransient<ICampaignDescriptorProvider, JsonCampaignDescriptorProvider>();
            services.AddScoped<IMBObjectVariableStorage, MBObjectVariableStorageBehavior>();

            Logger.LogTrace("OnSubModuleLoad() finished.");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() started.");

            if (FirstInit)
            {
                FirstInit = false;

                var serviceProvider = this.GetServiceProvider();
                Logger = serviceProvider.GetRequiredService<ILogger<SubModule>>();

                if (Debug.DebugManager is DebugManagerWrapper debugManagerWrapper)
                {
                    Debug.DebugManager = new DebugManagerWrapper(debugManagerWrapper.OriginalDebugManager, serviceProvider!);
                }
                else
                {
                    Logger.LogWarning("DebugManagerWrapper was replaced with {type}! Wrapping it with DebugManagerWrapper.", Debug.DebugManager.GetType());
                    Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, serviceProvider!);
                }

                var campaignIdentifierHarmony = new Harmony("Bannerlord.ButterLib.CampaignIdentifier");
                CharacterCreationContentApplyCulturePatch.Apply(campaignIdentifierHarmony);
                ClanInitializeClanPatch.Apply(campaignIdentifierHarmony);

                // Selectively apply Harmony patches for MBObjectVariableStorageBehavior load/save
                // Moved to OnBeforeInitialModuleScreenSetAsRoot so we'll get a final logger
                CampaignBehaviorManagerPatch.Apply(new Harmony("Bannerlord.ButterLib.MBObjectVariableStorage"));
            }

            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() finished.");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Logger.LogTrace("OnGameStart(Game, IGameStarter) started.");

            if (game.GameType is Campaign)
            {
                var gameStarter = (CampaignGameStarter)gameStarterObject;

                // Behaviors
                gameStarter.AddBehavior(new CampaignIdentifierBehavior());
                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());

                // Pseudo-Behavior for MBObjectBase extension variable storage in CampaignBehaviorDataStore
                MBObjectVariableStorageBehavior.Instance = new MBObjectVariableStorageBehavior();
            }

            Logger.LogTrace("OnGameStart(Game, IGameStarter) finished.");
        }
    }
}