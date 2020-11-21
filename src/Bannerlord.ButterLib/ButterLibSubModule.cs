using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler;
using Bannerlord.ButterLib.ObjectSystem.Extensions;
using Bannerlord.ButterLib.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Text;

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
        private const string SErrorHarmonyNotFound =
@"{=EEVJa5azpB}Bannerlord.Harmony module was not found!";
        private const string SErrorModuleLoaderNotFound =
@"{=j3DZ87zFMB}Bannerlord.ModuleLoader module was not found!";
        private const string SErrorOfficialModulesLoadedBeforeButterLib =
@"{=GDkjThJcH6}ButterLib is loaded after the official modules!
Make sure ButterLib is loaded before them!";
        private const string SErrorOfficialModules =
@"{=5k4Eqevh53}The following modules were loaded before ButterLib:";

        internal event Action<float>? OnApplicationTickEvent;

        private ILogger _logger = default!;

        private bool DelayedServiceCreation { get; set; }
        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

        public ButterLibSubModule()
        {
            Instance = this;

            CheckLoadOrder();
        }

        public void OnServiceRegistration()
        {
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
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
            {
                OnServiceRegistration();
                DelayedServiceCreation = true;
            }

            _logger = this.GetTempServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();
            _logger.LogTrace("OnSubModuleLoad() started tracking.");

            if (!DelayedServiceCreation)
                InitializeServices();

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

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                if (DelayedServiceCreation)
                    InitializeServices();
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

        private static void CheckLoadOrder()
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules();

            var sb = new StringBuilder();

            var moduleLoaderModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ModuleLoader");
            var moduleLoaderIndex = moduleLoaderModule is not null ? loadedModules.IndexOf(moduleLoaderModule) : -1;
            if (moduleLoaderIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(new TextObject(SErrorModuleLoaderNotFound).ToString());
            }

            var butterLibModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ButterLib");
            var butterLibModuleIndex = butterLibModule is not null ? loadedModules.IndexOf(butterLibModule) : -1;
            if (butterLibModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(new TextObject(SErrorHarmonyNotFound).ToString());
            }

            var officialModules = loadedModules.Where(x => x.IsOfficial).Select(x => (Module: x, Index: loadedModules.IndexOf(x)));
            var modulesLoadedBeforeButterLib = officialModules.Where(tuple => tuple.Index < butterLibModuleIndex).ToList();
            if (modulesLoadedBeforeButterLib.Count > 0)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(new TextObject(SErrorOfficialModulesLoadedBeforeButterLib).ToString());
                sb.AppendLine(new TextObject(SErrorOfficialModules).ToString());
                foreach (var (module, _) in modulesLoadedBeforeButterLib)
                    sb.AppendLine(module.Id);
            }
        }

        private void InitializeServices()
        {
            if (Services is not null)
            {
                GlobalServiceProvider = Services.BuildServiceProvider();
                _logger.LogTrace("Created GlobalServiceProvider.");
                Services = null!;
                _logger.LogTrace("Set Services to null.");

                _logger = ServiceProvider.GetRequiredService<ILogger<ButterLibSubModule>>();
                _logger.LogTrace("Assigned new _logger from GlobalServiceProvider.");
            }
        }
    }
}