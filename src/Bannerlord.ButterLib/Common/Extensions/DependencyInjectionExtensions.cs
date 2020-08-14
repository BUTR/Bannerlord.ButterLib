using Microsoft.Extensions.DependencyInjection;

using System;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceProvider GetServiceProvider(this Game game) => SubModule.ServiceProvider;
        public static IServiceCollection GetServiceProvider(this MBSubModuleBase subModule) => SubModule.Services;
        public static IServiceCollection GetServices(this MBSubModuleBase subModule) => SubModule.Services;
    }
}
