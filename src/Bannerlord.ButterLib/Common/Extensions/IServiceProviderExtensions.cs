using Microsoft.Extensions.DependencyInjection;

using System;

namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static T Create<T>(this IServiceProvider serviceProvider, params object[] parameters) =>
            ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);

        public static object Create(this IServiceProvider serviceProvider, Type type, params object[] parameters) =>
            ActivatorUtilities.CreateInstance(serviceProvider, type, parameters);
    }
}
