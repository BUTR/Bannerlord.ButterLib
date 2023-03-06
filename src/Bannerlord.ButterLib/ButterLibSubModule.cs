using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.CrashUploader;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.ButterLib.DynamicAPI;
using Bannerlord.ButterLib.ExceptionHandler;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;
using Bannerlord.ButterLib.Options;
using Bannerlord.ButterLib.SubModuleWrappers2;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Diagnostics.Logger;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using DialogResult = System.Windows.Forms.DialogResult;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxDefaultButton = System.Windows.Forms.MessageBoxDefaultButton;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;
using Path = System.IO.Path;

namespace Bannerlord.ButterLib
{
    /// <summary>
    /// Main SubModule. Performs initialization of all 3 stages.
    /// </summary>
    public sealed partial class ButterLibSubModule : MBSubModuleBase
    {
        // We can't rely on EN since the game assumes that the default locale is always English
        private const string SWarningTitle =
@"{=BguqytVG3q}Warning from Bannerlord.ButterLib!";
        private const string SMessageContinue =
@"{=eXs6FLm5DP}It's strongly recommended to terminate the game now. Do you wish to terminate it?";
        private const string SMessageWrongGameVersion =
@"{=aGg5Gh64gH}This version of ButterLib is intended for e1.7.2 and higher! You are running {GAMEVERSION}!";

        internal event Action<float>? OnApplicationTickEvent;

        private ILogger Logger { get; set; } = default!;
        private bool DelayedServiceCreation { get; set; }
        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

        private TextWriterTraceListener? TextWriterTraceListener { get; set; }

        public ButterLibSubModule()
        {
            Instance = this;

            ValidateLoadOrder();
        }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

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
            this.AddSerilogLoggerProvider("trace.txt", new[] { "System.Diagnostics.Logger.*" });

            Services.AddSubSystem<DelayedSubModuleSubSystem>();
            Services.AddSubSystem<ExceptionHandlerSubSystem>();
            Services.AddSubSystem<CrashUploaderSubSystem>();
            Services.AddSubSystem<SubModuleWrappers2SubSystem>();

            Services.AddSingleton<ICrashUploader, BUTRCrashUploader>();
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            PerformMigration001();

            IServiceProvider serviceProvider;

            if (!ServiceRegistrationWasCalled)
            {
                OnServiceRegistration();
                DelayedServiceCreation = true;
                serviceProvider = this.GetTempServiceProvider()!;
            }
            else
            {
                serviceProvider = this.GetServiceProvider()!;
            }

            Logger = serviceProvider.GetRequiredService<ILogger<ButterLibSubModule>>();
            Logger.LogTrace("OnSubModuleLoad: Logging started...");

            if (!DelayedServiceCreation)
                InitializeServices();

            ExceptionHandlerSubSystem.Instance?.Enable();
            CrashUploaderSubSystem.Instance?.Enable();

            Trace.Listeners.Add(TextWriterTraceListener = new TextWriterTraceListener(new StreamWriter(new MemoryStream(), Encoding.UTF8, 1024, true)));
            Trace.AutoFlush = true;
            Logger.LogTrace("Added System.Diagnostics.Trace temporary listener");

            Logger.LogTrace("OnSubModuleLoad: Done");
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            Logger.LogTrace("OnSubModuleUnloaded: Started...");

            Instance = null!;

            Logger.LogTrace("OnSubModuleUnloaded: Done");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot: Started...");

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                if (DelayedServiceCreation)
                {
                    InitializeServices();
                }
            }

            DynamicAPIProvider.Initialize();

            Logger.LogTrace("OnBeforeInitialModuleScreenSetAsRoot: Done");
        }

        protected override void OnApplicationTick(float dt) => OnApplicationTickEvent?.Invoke(dt);

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            Logger.LogTrace("OnGameStart: Started");

            GameScope = ServiceProvider.CreateScope();
            Logger.LogInformation("Created GameScope");

            Logger.LogTrace("OnGameStart: Done");
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            Logger.LogTrace("OnGameEnd: Started");

            GameScope?.Dispose();
            GameScope = null;

            if (game.GameType is Campaign)
            {
                MBObjectBaseExtensions.OnGameEnd();
            }

            Logger.LogTrace("OnGameEnd: Done");
        }

        private static void ValidateLoadOrder()
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules().ToList();
            if (loadedModules.Count == 0) return;

            var sb = new StringBuilder();
            if (!ModuleInfoHelper.ValidateLoadOrder(typeof(ButterLibSubModule), out var report))
            {
                sb.AppendLine(report);
                sb.AppendLine();
                sb.AppendLine(new TextObject(SMessageContinue).ToString());
                switch (MessageBox.Show(sb.ToString(), new TextObject(SWarningTitle).ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0x40000))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }

        private void InitializeServices()
        {
            if (Services is not null)
            {
                GlobalServiceProvider = Services.BuildServiceProvider();
                Logger.LogTrace("Created GlobalServiceProvider");
                Services = null!;
                Logger.LogTrace("Set Services to null");

                Logger = this.GetServiceProvider().GetRequiredService<ILogger<ButterLibSubModule>>();
                Logger.LogTrace("Assigned new _logger from GlobalServiceProvider");

                var logger = this.GetServiceProvider().GetRequiredService<ILogger<LoggerTraceListener>>();
                Trace.Listeners.Add(new LoggerTraceListener(logger));
                Logger.LogTrace("Added System.Diagnostics.Trace main listener");

                if (TextWriterTraceListener is not null)
                {
                    try
                    {
                        Trace.Flush(); // In case AutoFlush was set to false
                        Trace.Listeners.Remove(TextWriterTraceListener);
                        if (TextWriterTraceListener.Writer is StreamWriter { BaseStream: MemoryStream ms })
                        {
                            ms.Seek(0, SeekOrigin.Begin);
                            using var reader = new StreamReader(ms, Encoding.UTF8, true, 1024, true);
                            while (reader.Peek() >= 0)
                            {
                                Trace.WriteLine(reader.ReadLine());
                            }
                            Logger.LogTrace("Flushed logs from the System.Diagnostics.Trace temp listener");
                        }
                    }
                    finally
                    {
                        TextWriterTraceListener.Dispose();
                    }
                }
            }
        }

        private static void PerformMigration001()
        {
            try
            {
                var configPath = PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath);
                if (string.IsNullOrEmpty(configPath)) return;

                var oldConfigPath = Path.GetFullPath("Configs");
                var oldPath = Path.Combine(oldConfigPath, "ModLogs");
                var newPath = Path.Combine(configPath, "ModLogs");
                if (Directory.Exists(oldPath) && Directory.Exists(newPath))
                {
                    foreach (var filePath in Directory.GetFiles(oldPath))
                    {
                        var fileName = Path.GetFileName(filePath);
                        var newFilePath = Path.Combine(newPath, fileName);
                        try
                        {
                            File.Copy(filePath, newFilePath, true);
                            File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }

                    if (Directory.GetFiles(oldPath) is { Length: 0 } && Directory.GetDirectories(oldPath) is { Length: 0 })
                        Directory.Delete(oldPath, true);
                    if (Directory.GetFiles(oldConfigPath) is { Length: 0 } && Directory.GetDirectories(oldConfigPath) is { Length: 0 })
                        Directory.Delete(oldConfigPath, true);
                }
            }
            catch (Exception) { }
        }
    }
}