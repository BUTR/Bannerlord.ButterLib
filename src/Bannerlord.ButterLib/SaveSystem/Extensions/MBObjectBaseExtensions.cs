using Newtonsoft.Json;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public static class MBObjectBaseExtensions
    {
        public static void AddExtension<T>(this MBObjectBase @object, string key, ref T data, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver()
            };

            MBObjectBaseExtensionCampaignBehavior.Instance?.AddExtension<T>(@object, key, ref data, settings);
        }

        public static void RemoveExtension(this MBObjectBase @object, string key, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver()
            };

            MBObjectBaseExtensionCampaignBehavior.Instance?.RemoveExtension(@object, key);
        }

#nullable disable
        public static T GetExtension<T>(this MBObjectBase @object, string key, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver()
            };

            return MBObjectBaseExtensionCampaignBehavior.Instance != null
                ? MBObjectBaseExtensionCampaignBehavior.Instance.GetExtension<T>(@object, key, settings)
                : default;
        }
#nullable restore
    }
}