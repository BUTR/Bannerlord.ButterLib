using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.HotKeys.Test
{
    public class SubModule : MBSubModuleBase
    {
        private bool _campaignIsStarted;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            var hkm = HotKeyManager.Create("MyMod");
            if (hkm is not null)
            {
                var hk = hkm.Add<TestKey1>();
                hk.Predicate = () => _campaignIsStarted;
                hk.OnReleasedEvent += () => InformationManager.DisplayMessage(new InformationMessage("Test Key Released!", Colors.Magenta));
                hkm.Build();
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
                _campaignIsStarted = true;
        }
    }
}