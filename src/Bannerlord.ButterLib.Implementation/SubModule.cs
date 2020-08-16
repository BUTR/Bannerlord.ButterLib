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
    public sealed class SubModule : MBSubModuleBase
    {
        private const string SErrorLoading = "{=PZthBmJc9B}ButterLib Campaign Identifier failed to load! See details in the mod log.";

        public Harmony? CampaignIdentifierHarmonyInstance { get; private set; }
        internal bool Patched { get; private set; }

        private ILogger _logger = default!;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            
            _logger = this.GetTempServiceProvider().GetRequiredService<ILogger<SubModule>>();
            _logger.LogTrace("OnSubModuleLoad() started tracking.");

            _logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
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

                if (e.IsBase && e.PatchType == HarmonyPatchType.Postfix)
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
                        DebugHelper.HandleException(ex, "OnSubModuleLoad", "Initialization error - {0}");
                    }
                }
            };
            _logger.LogTrace("OnSubModuleLoad() finished.");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            _logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() started.");

            var serviceProvider = this.GetServiceProvider();
            _logger = serviceProvider.GetRequiredService<ILogger<SubModule>>();

            if (Debug.DebugManager is DebugManagerWrapper debugManagerWrapper)
            {
                Debug.DebugManager = new DebugManagerWrapper(debugManagerWrapper.OriginalDebugManager, serviceProvider);
            }
            else
            {
                _logger.LogWarning("DebugManagerWrapper was replaced with {type}! Wrapping it with DebugManagerWrapper.", Debug.DebugManager.GetType());
                Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, serviceProvider);
            }

            if (!Patched)
            {
                _logger.LogError("Failed to execute patches!");
                InformationManager.DisplayMessage(new InformationMessage(new TextObject(SErrorLoading).ToString(), Colors.Red));
            }
            _logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() finished.");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            _logger.LogTrace("OnGameStart(Game, IGameStarter) started.");

            if (game.GameType is Campaign)
            {
                //CampaignGameStarter
                CampaignGameStarter gameStarter = (CampaignGameStarter) gameStarterObject;
                //Behaviors
                gameStarter.AddBehavior(new CampaignIdentifierBehavior());
                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());
            }
            _logger.LogTrace("OnGameStart(Game, IGameStarter) finished.");
        }
    }
}