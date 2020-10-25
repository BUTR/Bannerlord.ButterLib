using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private static string ChunksToString(string[] chunks)
        {
            if (chunks.Length == 0)
                return string.Empty;
            else if (chunks.Length == 1)
                return chunks[0];

            var strBuilder = new StringBuilder(short.MaxValue);

            foreach (var chunk in chunks)
                strBuilder.Append(chunk);

            return strBuilder.ToString();
        }

        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T data, JsonSerializerSettings? settings = null)
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
                var dataJson = JsonConvert.SerializeObject(data, Formatting.None, settings);
                var jsonData = JsonConvert.SerializeObject(new JsonData(2, dataJson));
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
                    var result = dataStore.SyncData(key, ref jsonDataChunks); // try to get as array of JSON string(s)
                    var jsonData = JsonConvert.DeserializeObject<JsonData>(ChunksToString(jsonDataChunks ?? Array.Empty<string>()), settings);
                    data = jsonData.Format switch
                    {
                        2 => JsonConvert.DeserializeObject<T>(jsonData.Data, settings),
                        _ => data
                    };
                    return result;
                }
                catch (Exception e) when (e is InvalidCastException) { }

                try
                {
                    // The first version of SyncDataAsJson stored the string as a single entity
                    var jsonData = "";
                    var result = dataStore.SyncData(key, ref jsonData); // try to get as JSON string
                    data = JsonConvert.DeserializeObject<T>(jsonData, settings);
                    return result;
                }
                catch (Exception ex) when (ex is InvalidCastException) { }

                try
                {
                    // Most likely the save file stores the data with its default binary serialization
                    // We read it as it is, the next save will convert the data to JSON
                    var result = dataStore.SyncData(key, ref data);
                    return result;
                }
                catch (Exception ex) when (ex is InvalidCastException) { }
            }

            return false;
        }

        private sealed class JsonData
        {
            public int Format { get; private set; }
            public string Data { get; private set; }

            public JsonData(int format, string data)
            {
                Format = format;
                Data = data;
            }
        }
    }
}