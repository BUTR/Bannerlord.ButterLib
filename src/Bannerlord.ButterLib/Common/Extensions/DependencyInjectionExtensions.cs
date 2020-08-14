using Bannerlord.ButterLib.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceProvider GetServiceProvider(this Game game) => SubModule.ServiceProvider;
        public static IServiceCollection GetServiceProvider(this MBSubModuleBase subModule) => SubModule.Services;

        public static IServiceCollection GetServices(this MBSubModuleBase subModule) => SubModule.Services;

        internal static IServiceCollection AddDefaultLogger(this MBSubModuleBase subModule)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(System.IO.Path.Combine(Utilities.GetConfigsPath(), "Mods Log", "default.log"))
                .CreateLogger();

            var services = subModule.GetServices();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));
            return services;
        }
        public static IServiceCollection AddLogger(this MBSubModuleBase subModule, string filename, IEnumerable<Assembly> filter)
        {
            return subModule.AddLogger(filename, filter.Select(x => x.GetName().Name));
        }
        public static IServiceCollection AddLogger(this MBSubModuleBase subModule, string filename, IEnumerable<string>? filter = null, Func<ILoggingBuilder, ILoggingBuilder>? builder = null)
        {
            filter ??= new[] { subModule.GetType().Assembly.GetName().Name };

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Assemblies", JsonConvert.SerializeObject(filter))
                .Filter.With(new AssemblyFilter(filter))
                .WriteTo.File(System.IO.Path.Combine(Utilities.GetConfigsPath(), "Mods Log", filename))
                .CreateLogger();

            var services = subModule.GetServices();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(logger, dispose: true);
                loggingBuilder = builder?.Invoke(loggingBuilder);
            });
            return services;
        }
    }
}