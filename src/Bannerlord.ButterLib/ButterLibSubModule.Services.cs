using Bannerlord.ButterLib.Options;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

namespace Bannerlord.ButterLib
{
    public sealed partial class ButterLibSubModule
    {
        public static bool CanBeConfigured { get; private set; } = true;
        private static List<Action<IServiceCollection>> BeforeInitialization { get; } = new();

        /// <summary>
        /// The only way to inject your stuff before ButterLib will start it's initialization
        /// Can be used to inject a custom <see cref="ButterLibOptions"/> provider, but it needs to load before ButterLib
        /// </summary>
        public static void ConfigureBeforeInitialization(Action<IServiceCollection> action)
        {
            if (CanBeConfigured)
                BeforeInitialization.Add(action);
        }


        public static ButterLibSubModule? Instance { get; set; }

        private static IServiceProvider? GlobalServiceProvider { get; set; }
        private static IServiceProvider? GameScopeServiceProvider => GameScope?.ServiceProvider;
        private static IServiceScope? GameScope { get; set; }

        // DI workflow
        // OnSubModuleLoad - register services via GetServices(), use GetTempServiceProvider() to get the default logger.
        // OnBeforeInitialModuleScreenSetAsRoot - register disabled, GetServiceProvider() available. Re-init your ILogger.
        internal static IServiceProvider? ServiceProvider => GameScopeServiceProvider ?? GlobalServiceProvider;
        internal static IServiceCollection? Services { get; set; }
    }
}