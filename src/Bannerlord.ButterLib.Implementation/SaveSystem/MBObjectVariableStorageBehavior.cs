using Bannerlord.ButterLib.SaveSystem;
using Bannerlord.ButterLib.SaveSystem.Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.SaveSystem
{
    internal sealed class MBObjectVariableStorageBehavior : CampaignBehaviorBase, IMBObjectVariableStorage
    {
        private ConcurrentDictionary<StorageKey, object?> _variables = new ConcurrentDictionary<StorageKey, object?>();

        // JsonConvert won't do a ConcurrentBag, so another ConcurrentDictionary is easy.
        private ConcurrentDictionary<StorageKey, bool> _flags = new ConcurrentDictionary<StorageKey, bool>();

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                // Cache known-expired object IDs, as MBObjectManager.GetObject can be slow.
                var expiredIdCache = new Dictionary<uint, bool>();

                // Remove entries in data store that refer to now-nonexistent/untracked objects.
                ReleaseOrphanedEntries(_variables, expiredIdCache);
                ReleaseOrphanedEntries(_flags, expiredIdCache);
            }

            dataStore.SyncDataAsJson("ButterLib.MBObjectVariableStorage.Vars", ref _variables);
            dataStore.SyncDataAsJson("ButterLib.MBObjectVariableStorage.Flags", ref _flags);
        }

        private void ReleaseOrphanedEntries<ValueT>(ConcurrentDictionary<StorageKey, ValueT> dict, Dictionary<uint, bool> expiredIdCache)
        {
            foreach (var sk in dict.Keys)
            {
                if (expiredIdCache.ContainsKey(sk.ObjectId))
                    dict.TryRemove(sk, out _);
                else if (MBObjectManager.Instance.GetObject(sk) == default)
                {
                    expiredIdCache[sk.ObjectId] = true;
                    dict.TryRemove(sk, out _);
                }
            }
         }

        /* Variables Implementation */

        public void SetVariable(MBObjectBase @object, string key, object? data) =>
            _variables[StorageKey.Make(@object, key)] = data;

        public void RemoveVariable(MBObjectBase @object, string key) =>
            _variables.TryRemove(StorageKey.Make(@object, key), out _);

        public object? GetVariable(MBObjectBase @object, string key) =>
            _variables.TryGetValue(StorageKey.Make(@object, key), out var value) ? value : null;

#nullable disable

        public T GetVariable<T>(MBObjectBase @object, string key) =>
            (_variables.TryGetValue(StorageKey.Make(@object, key), out var val) && val is T value) ? value : default;

#nullable restore

        /* Flags Implementation */

        public bool HasFlag(MBObjectBase @object, string name) => _flags.ContainsKey(StorageKey.Make(@object, name));

        public void SetFlag(MBObjectBase @object, string name) => _flags[StorageKey.Make(@object, name)] = true;

        public void RemoveFlag(MBObjectBase @object, string name) => _flags.TryRemove(StorageKey.Make(@object, name), out _);

        /* StorageKey Implementation */

        private sealed class StorageKeyConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(StorageKey) || objectType == typeof(StorageKey?);

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                if (value is StorageKey storageKey)
                {
                    serializer.Serialize(writer, storageKey.ObjectId);
                    serializer.Serialize(writer, storageKey.Key);
                    return;
                }

                serializer.Serialize(writer, null);
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (serializer.Deserialize<uint?>(reader) is { } objectId && reader.Read() && serializer.Deserialize<string>(reader) is { } key)
                {
                    return new StorageKey(objectId, key);
                }
                return null;
            }
        }

        [JsonConverter(typeof(StorageKeyConverter))]
        private readonly struct StorageKey : IEquatable<StorageKey>
        {
            public static implicit operator MBGUID(StorageKey sk) => new MBGUID(sk.ObjectId);

            public readonly uint ObjectId;
            public readonly string Key;

            internal StorageKey(uint objectId, string key) => (ObjectId, Key) = (objectId, key);

            public static StorageKey Make(MBObjectBase obj, string key) => new StorageKey(obj.Id.InternalValue, key);

            public bool Equals(StorageKey other) => ObjectId == other.ObjectId && Key != null! && other.Key != null! && Key.Equals(other.Key);

            public override bool Equals(object? obj) => obj is StorageKey sk && Equals(sk);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }
    }
}