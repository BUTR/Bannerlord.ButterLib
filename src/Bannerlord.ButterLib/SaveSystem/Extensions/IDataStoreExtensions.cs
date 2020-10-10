using Newtonsoft.Json;

using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public static class IDataStoreExtensions
    {
#nullable disable
        // TODO
        internal static bool SyncMBObjectExtensions(this IDataStore dataStore, ref MBObjectBase @object)
        {
            if (dataStore.IsSaving)
            {
                var extensions = MBObjectBaseExtensionStore.Extensions.TryGetValue(@object.Id, out var val) ? val : new Dictionary<string, object>();
                return dataStore.SyncData($"{@object.Id}_extensions", ref extensions);
            }

            if (dataStore.IsLoading)
            {
                var extensions = new Dictionary<string, object>();
                var @return = dataStore.SyncData($"{@object.Id}_extensions", ref extensions);
                MBObjectBaseExtensionStore.Extensions[@object.Id] = extensions;
                return @return;
            }

            return false;
        }

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