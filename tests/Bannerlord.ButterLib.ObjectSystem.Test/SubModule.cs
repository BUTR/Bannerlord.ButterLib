using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.ObjectSystem.Test
{
    public sealed class SubModule : MBSubModuleBase
    {
        private const string Version = "Rev. 4";
        private bool _hasLoaded;

        private ILogger _logger = default!;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _logger = this.GetTempServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!_hasLoaded)
            {
                _logger = this.GetServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();

                _logger.LogInformationAndDisplay($"Loaded {typeof(SubModule).Namespace}: {Version}");
                _hasLoaded = true;
            }
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            base.OnGameStart(game, starterObject);

            if (game.GameType is Campaign)
            {
                CampaignGameStarter initializer = (CampaignGameStarter)starterObject;
                initializer.AddBehavior(new TestCampaignBehavior());
                _logger.LogTrace($"Added campaign behavior: {nameof(TestCampaignBehavior)}");
            }
        }
    }
}