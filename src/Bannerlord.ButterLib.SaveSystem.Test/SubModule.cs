using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.SaveSystem.Test
{
    public sealed class SubModule : MBSubModuleBase
	{
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

                _logger.LogInformationAndDisplay($"Loaded {typeof(SubModule).Namespace}");
				_hasLoaded = true;
			}
		}

		protected override void OnGameStart(Game game, IGameStarter starterObject)
		{
			base.OnGameStart(game, starterObject);

			if (game.GameType is Campaign)
			{
				CampaignGameStarter initializer = (CampaignGameStarter)starterObject;
				AddBehaviors(initializer);
			}
		}

		private void AddBehaviors(CampaignGameStarter gameInitializer)
        {
            gameInitializer.AddBehavior(new TestCampaignBehavior());
        }
    }
}