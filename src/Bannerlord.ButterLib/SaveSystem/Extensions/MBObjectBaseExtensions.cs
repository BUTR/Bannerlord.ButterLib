using Newtonsoft.Json;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    // TODO:
    internal static class MBObjectBaseExtensions
    {
#nullable disable
        public static T GetExtension<T>(this MBObjectBase @object, string key)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                return instance.GetExtension(@object, key) is T val ? val : default;
            return default;
        }
        public static T GetExtensionAsJson<T>(this MBObjectBase @object, string key, JsonSerializerSettings settings = null)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance && instance.GetExtension(@object, key) is string jsonData)
                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            return default;
        }
#nullable restore

        public static void SetExtension<T>(this MBObjectBase @object, string key, T data)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.AddExtension(@object, key, data);
        }
        public static void SetExtensionAsJson<T>(this MBObjectBase @object, string key, T data, JsonSerializerSettings? settings = null)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
            {
                var jsonData = JsonConvert.SerializeObject(data, settings);
                instance.AddExtension(@object, key, jsonData);
            }
        }

        public static void RemoveExtension(this MBObjectBase @object, string key)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.RemoveExtension(@object, key);
        }
    }
}