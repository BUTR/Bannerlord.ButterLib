using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib
{
    /// <summary>
    /// Main SubModule. Performs initialization of all 3 stages.
    /// </summary>
    public sealed partial class ButterLibSubModule : MBSubModuleBase
    {
        private ILogger _logger = default!;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Instance = this;
            CanBeConfigured = false;

            Services = new ServiceCollection();
            Services.AddOptions();
            Services.Configure<ButterLibOptions>(o =>
            {
                var defaultJsonOptions = new JsonButterLibOptionsModel();
                o.MinLogLevel = defaultJsonOptions.MinLogLevel;
            });
            foreach (var action in BeforeInitialization)
                action?.Invoke(Services);

            this.AddSerilogLogger();
            this.AddSerilogLoggerProvider("butterlib.txt", new[] { "Bannerlord.ButterLib.*" });

            _logger = this.GetTempServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();
            _logger.LogTrace("OnSubModuleLoad() started tracking.");


            _logger.LogTrace("OnSubModuleLoad() finished.");
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            _logger.LogTrace("OnSubModuleUnloaded() started.");

            Instance = null!;
            
            _logger.LogTrace("OnSubModuleUnloaded() finished.");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            _logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() started.");

            GlobalServiceProvider = Services.BuildServiceProvider();
            Services = null!;

            _logger = ServiceProvider.GetRequiredService<ILogger<ButterLibSubModule>>();
            
            _logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() finished.");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            _logger.LogTrace("OnGameStart(Game, IGameStarter) started.");

            GameScope = ServiceProvider.CreateScope();
            _logger.LogInformation("Created GameScope..");

            if (game.GameType is Campaign)
            {
                //Events
                CampaignIdentifierEvents.Instance = new CampaignIdentifierEvents();
            }

            _logger.LogTrace("OnGameStart(Game, IGameStarter) finished.");
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            _logger.LogTrace("OnGameEnd(Game) started.");

            GameScope = null;

            _logger.LogTrace("OnGameEnd(Game) finished.");
        }
    }
}