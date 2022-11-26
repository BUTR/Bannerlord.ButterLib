using HarmonyLib.BUTR.Extensions;

using System;

namespace Bannerlord.ButterLib.DynamicAPI
{
    /// <summary>
    /// Represents a wrapper of an object marked with <see cref="DynamicAPIClassAttribute"/>.
    /// </summary>
    public class DynamicAPIInstance
    {
        private readonly object _instance;

        internal DynamicAPIInstance(object instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <summary>
        /// Return an instance API method, see <see cref="DynamicAPIMethodAttribute"/>.
        /// We recommend to save the delegate instead of calling this function multiple times.
        /// </summary>
        public TDelegate? RequestMethod<TDelegate>(string? method) where TDelegate : Delegate
        {
            if (method is null)
                return null;

            if (!DynamicAPIProvider.APIClassMethods.TryGetValue(_instance.GetType(), out var apiMethods))
                return null;

            if (!apiMethods.TryGetValue($"0instance_{method}", out var methodInfo))
                return null;

            return AccessTools2.GetDelegate<TDelegate>(_instance, methodInfo);
        }
    }
}