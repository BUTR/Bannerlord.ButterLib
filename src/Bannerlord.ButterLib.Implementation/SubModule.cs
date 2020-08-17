using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common;
using Bannerlord.ButterLib.Common.Extensions;
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

namespace Bannerlord.ButterLib.Implementation
{
    public class SubModule : MBSubModuleBase
    {
        private const string SErrorLoading = "{=PZthBmJc9B}ButterLib Campaign Identifier failed to load! See details in the mod log.";

        public Harmony? CampaignIdentifierHarmonyInstance { get; private set; }
        internal bool Patched { get; private set; }

        internal static ILogger? Logger { get; private set; }

        static SubModule()
        {
            Logger = default!;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            
            Logger = this.GetTempServiceProvider().GetRequiredService<ILogger<SubModule>>();
            Logger.LogTrace("OnSubModuleLoad() started tracking.");

            Logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
            Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, this.GetTempServiceProvider());

            var services = this.GetServices();
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();

            DelayedSubModuleLoader.Register<StoryModeSubModule>();
            DelayedSubModuleLoader.OnMethod += (s, e) =>
            {
                if (e.Type != typeof(StoryModeSubModule) || e.MethodName != "OnSubModuleLoad")
                    return;

                if (e.IsBase && e.PatchType == DelayedSubModuleEventArgs.SubModulePatchType.Postfix)
                {
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
            };
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

            if (!Patched)
            {
                Logger.LogError("Failed to execute patches!");
                InformationManager.DisplayMessage(new InformationMessage(new TextObject(SErrorLoading).ToString(), Colors.Red));
            }
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
    }
}