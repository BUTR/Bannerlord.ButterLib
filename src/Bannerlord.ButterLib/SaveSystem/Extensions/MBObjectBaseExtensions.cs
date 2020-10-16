using Newtonsoft.Json;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    // TODO:
    internal static class MBObjectBaseExtensions
    {
#nullable disable

        public static T GetVariable<T>(this MBObjectBase @object, string key)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                return instance.GetVariable<T>(@object, key);
            return default;
        }

        public static T GetVariableAsJson<T>(this MBObjectBase @object, string key, JsonSerializerSettings settings = null)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance && instance.GetVariable<string>(@object, key) is { } jsonData)
                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            return default;
        }

#nullable restore

        public static void SetVariable<T>(this MBObjectBase @object, string key, T data)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.SetVariable(@object, key, data);
        }

        public static void SetVariableAsJson<T>(this MBObjectBase @object, string key, T data, JsonSerializerSettings? settings = null)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.SetVariable(@object, key, JsonConvert.SerializeObject(data, settings));
        }

        public static void RemoveVariable(this MBObjectBase @object, string key)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.RemoveVariable(@object, key);
        }
    }
}