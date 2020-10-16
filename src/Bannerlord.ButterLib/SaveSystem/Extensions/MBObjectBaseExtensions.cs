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
                return instance.GetExtension<T>(@object, key);
            return default;
        }
        public static T GetExtensionAsJson<T>(this MBObjectBase @object, string key, JsonSerializerSettings settings = null)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance && instance.GetExtension<string>(@object, key) is { } jsonData)
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
                instance.AddExtension(@object, key, JsonConvert.SerializeObject(data, settings));
        }

        public static void RemoveExtension(this MBObjectBase @object, string key)
        {
            if (MBObjectBaseExtensionCampaignBehavior.Instance is { } instance)
                instance.RemoveExtension(@object, key);
        }
    }
}