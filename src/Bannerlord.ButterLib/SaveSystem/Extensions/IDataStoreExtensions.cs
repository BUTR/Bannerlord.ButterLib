using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public static class IDataStoreExtensions
    {
        private static IEnumerable<string> ToChunks(string str, int maxChunkSize)
        {
            for (var i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length-i));
        }

#nullable disable
        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T data, JsonSerializerSettings settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.Auto,
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            if (dataStore.IsSaving)
            {
                var jsonData = JsonConvert.SerializeObject(data, Formatting.None, settings);
                var chunks = ToChunks(jsonData, short.MaxValue - 1024).ToArray();
                return dataStore.SyncData(key, ref chunks);
            }

            if (dataStore.IsLoading)
            {
                try
                {
                    // The game's save system limits the string to be of size of short.MaxValue
                    // We avoid this limitation by splitting the string into chunks.
                    var jsonDataChunks = Array.Empty<string>();
                    var @return = dataStore.SyncData(key, ref jsonDataChunks); // try to get as JSON string
                    var jsonData = string.Concat(jsonDataChunks);
                    data = JsonConvert.DeserializeObject<T>(jsonData, settings);
                    return @return;
                }
                catch (Exception e) when (e is InvalidCastException)
                {
                    try
                    {
                        // The first version of SyncDataAsJson stored the string as a single entity
                        var jsonData = "";
                        var @return = dataStore.SyncData(key, ref jsonData); // try to get as JSON string
                        data = JsonConvert.DeserializeObject<T>(jsonData, settings);
                        return @return;
                    }
                    catch (Exception ex) when (ex is InvalidCastException)
                    {
                        // Most likely the save file stores the data with its default binary serialization
                        // We read it as it is, the next save will convert the data to JSON
                        return dataStore.SyncData(key, ref data);
                    }
                }
            }

            return false;
        }
#nullable restore
    }
}