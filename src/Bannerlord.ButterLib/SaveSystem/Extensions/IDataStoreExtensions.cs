using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        private static string ChunksToString(IReadOnlyList<string> chunks)
        {
            if (chunks.Count == 0) return string.Empty;
            if (chunks.Count == 1) return chunks[0];

            var strBuilder = new StringBuilder(short.MaxValue);
            foreach (var chunk in chunks)
                strBuilder.Append(chunk);
            return strBuilder.ToString();
        }

        private static string Serialize<T>(ref T? data, JsonSerializer serializer)
        {
            var sb = new StringBuilder(256);
            using var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.Formatting = Formatting.None;
            serializer.Serialize(jsonWriter, data, typeof(T));
            return sb.ToString();
        }

        private static T? Deserialize<T>(string json, JsonSerializer serializer)
        {
            using var reader = new JsonTextReader(new StringReader(json));
            return serializer.Deserialize<T>(reader);
        }

        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T? data, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.Auto,
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            return SyncDataAsJson(dataStore, key, ref data, JsonSerializer.Create(settings));
        }

        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T? data, JsonSerializer serializer)
        {
            // If the type we're synchronizing is a string or string array, then it's ambiguous
            // with our own internal storage types, which imply that the strings contain valid
            // JSON. Standard binary serialization can handle these types just fine, so we avoid
            // the ambiguity by passing this data straight to the game's binary serializer.
            if (typeof(T) == typeof(string) || typeof(T) == typeof(string[]))
                return dataStore.SyncData(key, ref data);

            if (dataStore.IsSaving)
            {
                var dataJson = Serialize(ref data, serializer);
                var jsonDataJson = JsonConvert.SerializeObject(new JsonData(2, dataJson));
                var chunks = ToChunks(jsonDataJson, short.MaxValue - 1024).ToArray();
                return dataStore.SyncData(key, ref chunks);
            }

            if (dataStore.IsLoading)
            {
                try
                {
                    // The game's save system limits the string to be of size of short.MaxValue
                    // We avoid this limitation by splitting the string into chunks.
                    var jsonDataChunks = Array.Empty<string>();
                    if (dataStore.SyncData(key, ref jsonDataChunks))
                    {
                        var (format, jsonData) = JsonConvert.DeserializeObject<JsonData?>(ChunksToString(jsonDataChunks ?? Array.Empty<string>())) ?? new(-1, string.Empty);
                        data = format switch
                        {
                            2 => Deserialize<T>(jsonData, serializer),
                            _ => data
                        };
                        return true;
                    }
                }
                catch (Exception e) when (e is InvalidCastException) { }

                try
                {
                    // The first version of SyncDataAsJson stored the string as a single entity
                    var jsonData = "";
                    if (dataStore.SyncData(key, ref jsonData)) // try to get as JSON string
                    {
                        data = Deserialize<T>(jsonData, serializer);
                        return true;
                    }
                }
                catch (Exception ex) when (ex is InvalidCastException) { }

                try
                {
                    // Most likely the save file stores the data with its default binary serialization
                    // We read it as it is, the next save will convert the data to JSON
                    return dataStore.SyncData(key, ref data);
                }
                catch (Exception ex) when (ex is InvalidCastException) { }
            }

            return false;
        }

        private record JsonData(int Format, string Data);
    }
}