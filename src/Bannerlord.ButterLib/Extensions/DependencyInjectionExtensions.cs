﻿using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Options;
using Bannerlord.ButterLib.SubSystems;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Filters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// For Stage 3.
        /// </summary>
        public static IServiceProvider? GetServiceProvider(this Game _) => ButterLibSubModule.ServiceProvider;
        /// <summary>
        /// For Stage 3.
        /// </summary>
        public static IServiceProvider? GetServiceProvider(this CampaignBehaviorBase _) => ButterLibSubModule.ServiceProvider;
        /// <summary>
        /// For Stage 3.
        /// </summary>
        public static IServiceProvider? GetServiceProvider(this MBSubModuleBase _) => ButterLibSubModule.ServiceProvider;
        /// <summary>
        /// For Stage 2.
        /// </summary>
        public static IServiceCollection? GetServices(this MBSubModuleBase _) => ButterLibSubModule.Services;
        /// <summary>
        /// For Stage 2.
        /// </summary>
        public static IServiceProvider? GetTempServiceProvider(this MBSubModuleBase _) => ButterLibSubModule.Services?.BuildServiceProvider();

        private static readonly string ModLogsPath = Path.Combine(FSIOHelper.GetConfigPath(), "ModLogs");
        private static readonly string OutputTemplate = "[{Timestamp:HH:mm:ss.fff}] [{SourceContext}] [{Level:u3}]: {Message:lj}{NewLine}{Exception}";

        internal static IServiceCollection? AddDefaultSerilogLogger(this MBSubModuleBase subModule)
        {
            var services = subModule.GetServices();

            var serviceProvider = services.BuildServiceProvider();
            var butterLibOptions = serviceProvider.GetService<IOptions<ButterLibOptions>>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Is((LogEventLevel) butterLibOptions.Value.MinLogLevel)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    outputTemplate: OutputTemplate,
                    path: Path.Combine(ModLogsPath, "default.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .CreateLogger();


            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));
            return services;
        }
        /// <summary>
        /// Don't forget to get a new ILogger after adding a new ILoggerProvider
        /// </summary>
        public static IServiceCollection AddSerilogLoggerProvider(this MBSubModuleBase subModule, string filename, IEnumerable<Assembly> filter) =>
            subModule.AddSerilogLoggerProvider(filename, filter.Select(x => x.GetName().Name ?? string.Empty));
        /// <summary>
        /// Don't forget to get a new ILogger after adding a new ILoggerProvider
        /// </summary>
        public static IServiceCollection AddSerilogLoggerProvider(this MBSubModuleBase subModule, string filename, IEnumerable<string>? filter = null, Action<LoggerConfiguration>? configure = null)
        {
            filter ??= new List<string> { $"{subModule.GetType().Assembly.GetName().Name}.*" };
            var filterList = filter.ToList();

            var services = subModule.GetServices();
            if (services is null)
                throw new Exception("Past Configuration stage.");

            var builder = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Filter.ByIncludingOnly(FromSources(filterList))
                .WriteTo.File(
                    outputTemplate: OutputTemplate,
                    path: Path.Combine(ModLogsPath, filename),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);
            configure?.Invoke(builder);
            var logger = builder.CreateLogger();

            services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>(_ => new SerilogLoggerProvider(logger, true));
            return services;
        }

        public static Func<LogEvent, bool> FromSources(IEnumerable<string> sources)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            return Matching.WithProperty<string>(Constants.SourceContextPropertyName, s => s is not null && sources.Any(x => MatchWildcardString(x, s)));
        }
        private static bool MatchWildcardString(string pattern, string input)
        {
            string regexPattern = pattern.Aggregate("^", (current, c) => current + c switch
            {
                '*' => ".*",
                '?' => ".",
                _ => $"[{c}]"
            });
            // Lets hope that the Regex cache is sufficient.
            return Regex.IsMatch(input, $"{regexPattern}$");
        }

        public static IServiceCollection AddSubSystem<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISubSystem, new()
        {
            var instance = new TImplementation();
            services.AddSingleton<TImplementation>(sp => instance);
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISubSystem, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            return services;
        }

        public static ISubSystem? GetSubSystem(this IServiceProvider sp, string id) => sp.GetServices<ISubSystem>().FirstOrDefault(s => s.Id == id);
    }
}