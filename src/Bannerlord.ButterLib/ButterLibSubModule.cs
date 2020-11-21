using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;
using Bannerlord.ButterLib.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib
{
    /// <summary>
    /// Main SubModule. Performs initialization of all 3 stages.
    /// </summary>
    public sealed partial class ButterLibSubModule : MBSubModuleBase
    {
        private const string SErrorOfficialLoadedBeforeButterLib = "{=GDkjThJcH6}ButterLib is loaded after the official modules! " +
            "Make sure ButterLib is loaded before them!";

        internal event Action<float>? OnApplicationTickEvent;

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

            this.AddDefaultSerilogLogger();
            this.AddSerilogLoggerProvider("butterlib.txt", new[] { "Bannerlord.ButterLib.*" });

            _logger = this.GetTempServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();
            _logger.LogTrace("OnSubModuleLoad() started tracking.");

            ExceptionHandlerSubSystem.Enable();

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

            if (Services is not null) // First init.
            {
                GlobalServiceProvider = Services.BuildServiceProvider();
                _logger.LogTrace("Created GlobalServiceProvider.");
                Services = null!;
                _logger.LogTrace("Set Services to null.");

                _logger = ServiceProvider.GetRequiredService<ILogger<ButterLibSubModule>>();
                _logger.LogTrace("Assigned new _logger from GlobalServiceProvider.");


                var loadedModules = ModuleInfoHelper.GetLoadedModules();
                var butterLibModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ButterLib");
                var butterLibModuleIndex = butterLibModule is not null ? loadedModules.IndexOf(butterLibModule) : -1;
                if (butterLibModuleIndex == -1)
                    _logger.LogError("ButterLib module was not found!");
                var officialModules = loadedModules.Where(x => x.IsOfficial).Select(x => (Module: x, Index: loadedModules.IndexOf(x)));
                var modulesLoadedBeforeButterLib = officialModules.Where(tuple => tuple.Index < butterLibModuleIndex).ToList();

                if (modulesLoadedBeforeButterLib.Count > 0)
                    _logger.LogErrorAndDisplay(new TextObject(SErrorOfficialLoadedBeforeButterLib).ToString());

                foreach (var (module, _) in modulesLoadedBeforeButterLib)
                    _logger.LogError("ButterLib is loaded after an official module: {module}!", module.Id);
            }

            _logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot() finished.");
        }

        protected override void OnApplicationTick(float dt) => OnApplicationTickEvent?.Invoke(dt);

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            _logger.LogTrace("OnGameStart(Game, IGameStarter) started.");

            GameScope = ServiceProvider.CreateScope();
            _logger.LogInformation("Created GameScope...");

            if (game.GameType is Campaign)
                CampaignIdentifierEvents.Instance = new CampaignIdentifierEvents();

            _logger.LogTrace("OnGameStart(Game, IGameStarter) finished.");
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            _logger.LogTrace("OnGameEnd(Game) started.");

            GameScope = null;

            if (game.GameType is Campaign)
            {
                MBObjectBaseExtensions.OnGameEnd();
                CampaignIdentifierEvents.Instance = null;
            }

            _logger.LogTrace("OnGameEnd(Game) finished.");
        }
    }
}