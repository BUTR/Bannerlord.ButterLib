using Newtonsoft.Json;

using System;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public static class IDataStoreExtensions
    {
#nullable disable
        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T data, JsonSerializerSettings settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver()
            };

            if (dataStore.IsSaving)
            {
                var jsonData = JsonConvert.SerializeObject(data, Formatting.None, settings);
                return dataStore.SyncData(key, ref jsonData);
            }

            if (dataStore.IsLoading)
            {
                try
                {
                    var jsonData = "";
                    var @return = dataStore.SyncData(key, ref jsonData); /* try to get as JSON string */
                    data = JsonConvert.DeserializeObject<T>(jsonData, settings);
                    return @return;
                }
                catch (Exception e) when (e is InvalidCastException)
                {
                    return dataStore.SyncData(key, ref data); /* fallback to the default behavior, save will fix this */
                }
            }

            return false;
        }
#nullable restore
    }
}