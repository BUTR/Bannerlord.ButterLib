using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.Logging;
using Bannerlord.ButterLib.Implementation.ObjectSystem;
using Bannerlord.ButterLib.Implementation.ObjectSystem.Patches;
using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;
using Bannerlord.ButterLib.ObjectSystem;

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
            Logger.LogTrace("ButterLib.Implementation: OnSubModuleLoad");

            Logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
            Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, this.GetTempServiceProvider()!);

            var services = this.GetServices();
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddSingleton<CampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton<DistanceMatrixStatic, DistanceMatrixStaticImplementation>();
            services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();
            services.AddTransient<ICampaignDescriptorProvider, JsonCampaignDescriptorProvider>();
            services.AddScoped<IMBObjectExtensionDataStore, MBObjectExtensionDataStore>();

            Logger.LogTrace("ButterLib.Implementation: OnSubModuleLoad: Done");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            Logger.LogTrace("ButterLib.Implementation: OnBeforeInitialModuleScreenSetAsRoot");

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

                CampaignBehaviorManagerPatch.Apply(new Harmony("Bannerlord.ButterLib.MBObjectExtensionDataStore"));

                var saveSystemHarmony = new Harmony("Bannerlord.ButterLib.SaveSystem");
                TypeExtensionsPatch.Apply(saveSystemHarmony); // Adds support for saving many more container types
                //DefinitionContextPatch.Apply(saveSystemHarmony); // Fixes save corruption & crashes when duplicate types are defined
            }

            Logger.LogTrace("ButterLib.Implementation: OnBeforeInitialModuleScreenSetAsRoot: Done");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Logger.LogTrace("ButterLib.Implementation: OnGameStart");

            if (game.GameType is Campaign)
            {
                var gameStarter = (CampaignGameStarter)gameStarterObject;

                // Behaviors
                gameStarter.AddBehavior(new CampaignIdentifierBehavior());
                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());
            }

            Logger.LogTrace("ButterLib.Implementation: OnGameStart: Done");
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            Logger.LogTrace("ButterLib.Implementation: OnGameEnd");

            if (game.GameType is Campaign)
            {
                CampaignIdentifierEvents.Instance = null;
            }

            Logger.LogTrace("ButterLib.Implementation: OnGameEnd: Done");
        }
    }
}