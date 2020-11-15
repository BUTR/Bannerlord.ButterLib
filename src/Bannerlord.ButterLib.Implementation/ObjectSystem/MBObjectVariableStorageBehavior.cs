using Bannerlord.ButterLib.ObjectSystem;
using Bannerlord.ButterLib.SaveSystem.Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal sealed class MBObjectVariableStorageBehavior : CampaignBehaviorBase, IMBObjectVariableStorage
    {
        private ConcurrentDictionary<StorageKey, object?> _variables = new ConcurrentDictionary<StorageKey, object?>();
        private ConcurrentDictionary<StorageKey, uint> _flags = new ConcurrentDictionary<StorageKey, uint>();

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

        private static void ReleaseOrphanedEntries<TValue>(ConcurrentDictionary<StorageKey, TValue> dict, Dictionary<uint, bool> expiredIds)
        {
            foreach (var sk in dict.Keys)
            {
                if (expiredIds.ContainsKey(sk.ObjectId))
                {
                    dict.TryRemove(sk, out _);
                }
                else if (MBObjectManager.Instance.GetObject(sk) == default)
                {
                    expiredIds[sk.ObjectId] = true;
                    dict.TryRemove(sk, out _);
                }
            }
        }

        /* Variables Implementation */

        public bool TryGetVariable<T>(MBObjectBase @object, string key, [MaybeNull] out T value)
        {
            if (_variables.TryGetValue(StorageKey.Make(@object, key), out var val) && val is T typedVal)
            {
                value = typedVal;
                return true;
            }

            value = default;
            return false;
        }

        public void RemoveVariable(MBObjectBase @object, string key) => _variables.TryRemove(StorageKey.Make(@object, key), out _);

        public void SetVariable(MBObjectBase @object, string key, object? data) => _variables[StorageKey.Make(@object, key)] = data;

        /* Flags Implementation */

        public bool HasFlag(MBObjectBase @object, string name) => _flags.ContainsKey(StorageKey.Make(@object, name));

        public void RemoveFlag(MBObjectBase @object, string name) => _flags.TryRemove(StorageKey.Make(@object, name), out _);

        public void SetFlag(MBObjectBase @object, string name) => _flags[StorageKey.Make(@object, name)] = 1;

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
                    return new StorageKey(objectId, key);

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

            public bool Equals(StorageKey other) => ObjectId == other.ObjectId && Key is not null! && other.Key is not null! && Key.Equals(other.Key);

            public override bool Equals(object? obj) => obj is StorageKey sk && Equals(sk);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }
    }
}