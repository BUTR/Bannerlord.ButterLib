using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.Logging;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StoryMode;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;

namespace Bannerlord.ButterLib.Implementation
{
    public sealed class SubModule : MBSubModuleBase
    {
        private const string SErrorLoading = "{=PZthBmJc9B}ButterLib Campaign Identifier failed to load! See details in the mod log.";

        public Harmony? CampaignIdentifierHarmonyInstance { get; private set; }
        internal bool Patched { get; private set; }

        internal static ILogger? Logger { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Logger = this.GetTempServiceProvider().GetRequiredService<ILogger<SubModule>>();
            Logger.LogTrace("OnSubModuleLoad() started tracking.");

            Logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
            Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, this.GetTempServiceProvider());

            var services = this.GetServices();
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddSingleton<CampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton(typeof(DistanceMatrixStatic<>), typeof(DistanceMatrixStaticImplementation<>));
            services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();

            DelayedSubModuleLoader.Register<StoryModeSubModule>();
            DelayedSubModuleLoader.Subscribe<GauntletUISubModule, SubModule>(
                nameof(OnSubModuleLoad), DelayedSubModuleSubscriptionType.AfterMethod,
                InitializeCampaignIdentifier);

            Logger.LogTrace("OnSubModuleLoad() finished.");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() started.");

            var serviceProvider = this.GetServiceProvider();
            Logger = serviceProvider.GetRequiredService<ILogger<SubModule>>();

            if (Debug.DebugManager is DebugManagerWrapper debugManagerWrapper)
            {
                Debug.DebugManager = new DebugManagerWrapper(debugManagerWrapper.OriginalDebugManager, serviceProvider);
            }
            else
            {
                Logger.LogWarning("DebugManagerWrapper was replaced with {type}! Wrapping it with DebugManagerWrapper.", Debug.DebugManager.GetType());
                Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, serviceProvider);
            }

            DelayedSubModuleLoader.Register<GauntletUISubModule>();
            DelayedSubModuleLoader.Subscribe<GauntletUISubModule, SubModule>(
                nameof(OnBeforeInitialModuleScreenSetAsRoot),
                DelayedSubModuleSubscriptionType.AfterMethod,
                WarnNotPatched);

            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() finished.");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Logger.LogTrace("OnGameStart(Game, IGameStarter) started.");

            if (game.GameType is Campaign)
            {
                //CampaignGameStarter
                CampaignGameStarter gameStarter = (CampaignGameStarter) gameStarterObject;
                //Behaviors
                gameStarter.AddBehavior(new CampaignIdentifierBehavior());
                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());
            }
            Logger.LogTrace("OnGameStart(Game, IGameStarter) finished.");
        }

        private void InitializeCampaignIdentifier(object s, DelayedSubModuleEventArgs e)
        {
            if (!e.IsValid<StoryModeSubModule>(nameof(OnSubModuleLoad), DelayedSubModuleSubscriptionType.AfterMethod))
                return;

            try
            {
                CampaignIdentifierHarmonyInstance ??= new Harmony("Bannerlord.ButterLib.CampaignIdentifier");
                CampaignIdentifierHarmonyInstance.PatchAll();
                Patched = true;
            }
            catch (Exception ex)
            {
                Patched = false;
                DebugHelper.HandleException(ex, "Error in OnSubModuleLoad while initializing CampaignIdentifier.");
            }
        }

        private void WarnNotPatched(object s, DelayedSubModuleEventArgs e)
        {
            if (!e.IsValid<GauntletUISubModule>(nameof(OnBeforeInitialModuleScreenSetAsRoot), DelayedSubModuleSubscriptionType.AfterMethod))
                return;

            if (!Patched)
            {
                Logger.LogError("Failed to execute patches!");
                InformationManager.DisplayMessage(new InformationMessage(new TextObject(SErrorLoading).ToString(), Colors.Red));
            }
        }
    }
}