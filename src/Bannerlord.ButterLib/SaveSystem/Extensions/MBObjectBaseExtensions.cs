using Bannerlord.ButterLib.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    internal static class MBObjectBaseExtensions
    {
        // Backing data store:

        private static IMBObjectVariableStorage? _instance;

        private static IMBObjectVariableStorage? Instance =>
            _instance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectVariableStorage>();

        internal static void OnGameEnd() => _instance = null;

        // Extension method API:

#nullable disable

        public static T GetVariable<T>(this MBObjectBase @object, string key)
        {
            if (Instance is { } instance)
                return instance.GetVariable<T>(@object, key);
            return default;
        }

        public static T GetVariableAsJson<T>(this MBObjectBase @object, string key, JsonSerializerSettings settings = null)
        {
            if (Instance is { } instance && instance.GetVariable<string>(@object, key) is { } jsonData)
                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            return default;
        }

#nullable restore

        public static void SetVariable<T>(this MBObjectBase @object, string key, T data)
        {
            if (Instance is { } instance)
                instance.SetVariable(@object, key, data);
        }

        public static void SetVariableAsJson<T>(this MBObjectBase @object, string key, T data, JsonSerializerSettings? settings = null)
        {
            if (Instance is { } instance)
                instance.SetVariable(@object, key, JsonConvert.SerializeObject(data, settings));
        }

        public static void RemoveVariable(this MBObjectBase @object, string key)
        {
            if (Instance is { } instance)
                instance.RemoveVariable(@object, key);
        }
    }
}