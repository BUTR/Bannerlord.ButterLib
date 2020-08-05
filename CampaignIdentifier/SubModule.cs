using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using CampaignIdentifier.CampaignBehaviors;
using CampaignIdentifier.Helpers;

namespace CampaignIdentifier
{
  public class SubModule : MBSubModuleBase
  {
    private const string SErrorLoading = "{=PZthBmJc9B}Campaign Identifier failed to load! See details in the mod log.";

    public Harmony CampaignIdentifierHarmonyInstance { get; private set; }
    internal bool Patched { get; private set; }

    protected override void OnSubModuleLoad()
    {
      base.OnSubModuleLoad();
      try
      {
        if (CampaignIdentifierHarmonyInstance is null)
          CampaignIdentifierHarmonyInstance = new Harmony("Bannerlord.CampaignIdentifier");
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
        CampaignIdentifierEvents.Instance = new CampaignIdentifierEvents();
        //CampaignGameStarter
        CampaignGameStarter gameStarter = (CampaignGameStarter)gameStarterObject;
        //Behaviors
        gameStarter.AddBehavior(new CampaignIdentifierBehavior());
      }
    }
  }
}
