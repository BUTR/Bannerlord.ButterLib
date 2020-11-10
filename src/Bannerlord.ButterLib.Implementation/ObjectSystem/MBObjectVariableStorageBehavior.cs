using Bannerlord.ButterLib.ObjectSystem;

using Newtonsoft.Json;

using System;
//using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal sealed class MBObjectVariableStorageBehavior : CampaignBehaviorBase, IMBObjectVariableStorage
    {
        public const int SaveBaseId = 222_444_700; // 10 reserved, should probably base this one the ButterLib range

        private Dictionary<DataKey, object?> _vars = new Dictionary<DataKey, object?>();
        private Dictionary<DataKey, bool>   _flags = new Dictionary<DataKey, bool>();

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                // Cache known-expired object IDs, as MBObjectManager.GetObject can be slow.
                var expiredIdCache = new Dictionary<uint, bool>();

                // Remove entries in data store that refer to now-nonexistent/untracked objects.
                ReleaseOrphanedEntries(_vars, expiredIdCache);
                ReleaseOrphanedEntries(_flags, expiredIdCache);
            }

            dataStore.SyncData("Vars", ref _vars);
            dataStore.SyncData("Flags", ref _flags);
        }

        private static void ReleaseOrphanedEntries<TVal>(IDictionary<StorageKey, TVal> dict, IDictionary<uint, bool> expiredIds)
        {
            foreach (var sk in dict.Keys)
            {
                if (expiredIds.ContainsKey(sk.ObjectId))
                    dict.Remove(sk); // dict.TryRemove(sk, out _);
                else if (MBObjectManager.Instance.GetObject(new MBGUID(sk.ObjectId)) == default)
                {
                    expiredIds[sk.ObjectId] = true;
                    dict.Remove(sk); // dict.TryRemove(sk, out _);
                }
            }
        }

        /* Variables Implementation */
        public bool HasVariable(MBObjectBase @object, string name) => _vars.ContainsKey(StorageKey.Make(@object, name));

        // public bool RemoveVariable(MBObjectBase @object, string key) => _vars.TryRemove(StorageKey.Make(@object, key), out _);

        public bool RemoveVariable(MBObjectBase @object, string key) => _vars.Remove(StorageKey.Make(@object, key));

        public void SetVariable(MBObjectBase @object, string key, object? data) => _vars[StorageKey.Make(@object, key)] = data;

        public bool TryGetVariable<T>(MBObjectBase @object, string key, [MaybeNull] out T value)
        {
            if (_vars.TryGetValue(StorageKey.Make(@object, key), out var val) && val is T typedVal)
            {
                value = typedVal;
                return true;
            }

            value = default;
            return false;
        }

        /* Flags Implementation */

        public bool HasFlag(MBObjectBase @object, string name) => _flags.ContainsKey(StorageKey.Make(@object, name));

        // public bool RemoveFlag(MBObjectBase @object, string name) => _flags.TryRemove(StorageKey.Make(@object, name), out _);

        public bool RemoveFlag(MBObjectBase @object, string name) => _flags.Remove(StorageKey.Make(@object, name));

        public void SetFlag(MBObjectBase @object, string name) => _flags[StorageKey.Make(@object, name)] = true;

        /* StorageKey Implementation */

        #region StorageKeyConverter
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
        #endregion StorageKeyConverter

        //[JsonConverter(typeof(StorageKeyConverter))]
        public /* readonly */ struct StorageKey : IEquatable<StorageKey>
        {
            //public static implicit operator MBGUID(StorageKey sk) => new MBGUID(sk.ObjectId);

            [SaveableField(0)]
            public readonly uint ObjectId;

            [SaveableField(1)]
            public readonly string Key;

            public StorageKey(uint objectId, string key) => (ObjectId, Key) = (objectId, key);

            public static StorageKey Make(MBObjectBase obj, string key) => new StorageKey(obj.Id.InternalValue, key);

            public bool Equals(StorageKey other) => ObjectId == other.ObjectId && Key is not null! && other.Key is not null! && Key.Equals(other.Key);

            public override bool Equals(object? obj) => obj is StorageKey sk && Equals(sk);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }

        public class SavedTypeDefiner : SaveableCampaignBehaviorTypeDefiner
        {
            public SavedTypeDefiner() : base(SaveBaseId) { }

            protected override void DefineStructTypes()
            {
                AddStructDefinition(typeof(StorageKey), 1);
            }

            protected override void DefineGenericStructDefinitions()
            {
                ConstructGenericStructDefinition(typeof(KeyValuePair<StorageKey, object>));
                ConstructGenericStructDefinition(typeof(KeyValuePair<StorageKey, bool>));
            }

            protected override void DefineContainerDefinitions()
            {
                ConstructContainerDefinition(typeof(Dictionary<StorageKey, object>));
                ConstructContainerDefinition(typeof(Dictionary<StorageKey, bool>));
            }
        }
    }
}