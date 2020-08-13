using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;

using HarmonyLib;

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

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

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

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!Patched)
            {
                InformationManager.DisplayMessage(new InformationMessage(new TextObject(SErrorLoading).ToString(), Colors.Red));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                //Events
                // TODO:
                //CampaignIdentifierEvents.Instance = new CampaignIdentifierEvents();
                //CampaignGameStarter
                CampaignGameStarter gameStarter = (CampaignGameStarter) gameStarterObject;
                //Behaviors
                gameStarter.AddBehavior(new CampaignIdentifierBehavior());
                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());
            }
        }
    }
}