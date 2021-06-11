using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.HotKeys;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.HotKeys;
using Bannerlord.ButterLib.Implementation.Logging;
using Bannerlord.ButterLib.Implementation.ObjectSystem;
using Bannerlord.ButterLib.Implementation.SaveSystem;
using Bannerlord.ButterLib.ObjectSystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Implementation
{
    public sealed class SubModule : MBSubModuleBase
    {
        internal static ILogger? Logger { get; private set; }

        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServices() is { } services)
            {
                services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
                services.AddSingleton<ICampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();

                services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
                services.AddSingleton<IDistanceMatrixStatic, DistanceMatrixStaticImplementation>();

                services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();
#if e143 || e150 || e151 || e152 || e153
                services.AddTransient<ICampaignDescriptorProvider, JsonCampaignDescriptorProvider>();
#elif e154 || e155 || e156 || e157 || e158 || e159 || e1510 || e160
                services.AddTransient<ICampaignDescriptorProvider, BlankCampaignDescriptorProvider>();
#else
#error ConstGameVersionWithPrefix is not handled!
#endif

                services.AddScoped<IMBObjectExtensionDataStore, MBObjectExtensionDataStore>();

                services.AddScoped<HotKeyManager, HotKeyManagerImplementation>();
                services.AddSingleton<IHotKeyManagerStatic, HotKeyManagerStaticImplementation>();

                services.AddSubSystem<CampaignIdentifierSubSystem>();
                services.AddSubSystem<DistanceMatrixSubSystem>();
                services.AddSubSystem<HotKeySubSystem>();
                services.AddSubSystem<ObjectSystemSubSystem>();
                services.AddSubSystem<SaveSystemSubSystem>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var serviceProvider = ServiceRegistrationWasCalled ? this.GetServiceProvider() : this.GetTempServiceProvider();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();

            Logger = serviceProvider.GetRequiredService<ILogger<SubModule>>();
            Logger.LogTrace("ButterLib.Implementation: OnSubModuleLoad");

            Logger.LogInformation("Wrapping DebugManager of type {type} with DebugManagerWrapper.", Debug.DebugManager.GetType());
            Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, serviceProvider!);

            HotKeySubSystem.Instance?.Enable();
            SaveSystemSubSystem.Instance?.Enable();

            Logger.LogTrace("ButterLib.Implementation: OnSubModuleLoad: Done");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            Logger.LogTrace("ButterLib.Implementation: OnBeforeInitialModuleScreenSetAsRoot");

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                Logger = this.GetServiceProvider().GetRequiredService<ILogger<SubModule>>();

                if (Debug.DebugManager is not DebugManagerWrapper)
                {
                    Logger.LogWarning("DebugManagerWrapper was replaced with {type}! Wrapping it with DebugManagerWrapper.", Debug.DebugManager.GetType());
                    Debug.DebugManager = new DebugManagerWrapper(Debug.DebugManager, this.GetServiceProvider()!);
                }

                CampaignIdentifierSubSystem.Instance?.Enable();
                ObjectSystemSubSystem.Instance?.Enable();
            }

            Logger.LogTrace("ButterLib.Implementation: OnBeforeInitialModuleScreenSetAsRoot: Done");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Logger.LogTrace("ButterLib.Implementation: OnGameStart");

            if (game.GameType is Campaign)
            {
                var gameStarter = (CampaignGameStarter)gameStarterObject;

                gameStarter.AddBehavior(new GeopoliticsCachingBehavior());

#if e143 || e150 || e151 || e152 || e153
                if (game.GameType is StoryMode.CampaignStoryMode)
                    gameStarter.AddBehavior(new CampaignIdentifierBehavior()); // Requires StoryMode
#endif
            }

            Logger.LogTrace("ButterLib.Implementation: OnGameStart: Done");
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            Logger.LogTrace("ButterLib.Implementation: OnGameEnd");

            if (game.GameType is Campaign)
            {
                CampaignIdentifierEvents.Instance = null;
            }

            Logger.LogTrace("ButterLib.Implementation: OnGameEnd: Done");
        }
    }
}