using Bannerlord.ButterLib.SaveSystem.Extensions;
using System;
using System.Collections.Concurrent;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    internal sealed class MBObjectBaseExtensionCampaignBehavior : CampaignBehaviorBase
    {
        public static MBObjectBaseExtensionCampaignBehavior? Instance { get; set; }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncDataAsJson("_descriptorManager", ref _extensions);

            if (dataStore.IsLoading && _extensions == null)
                _extensions = new ConcurrentDictionary<StorageKey, object?>();
        }

        public override void RegisterEvents() { }

        public void AddExtension(MBObjectBase @object, string key, object? data)
        {
            if (_extensions == null)
                return;

            _extensions[StorageKey.Make(@object, key)] = data;
        }

        public void RemoveExtension(MBObjectBase @object, string key)
        {
            if (_extensions == null)
                return;

            _extensions.TryRemove(StorageKey.Make(@object, key), out _);
        }

#nullable disable
        public object GetExtension(MBObjectBase @object, string key)
        {
            if (_extensions == null)
                return default;

            if (_extensions.TryGetValue(StorageKey.Make(@object, key), out var value))
                return value;

            return default;
        }
#nullable restore

        class StorageKey : IEquatable<StorageKey>
        {
            public uint ObjectId;
            public string Key;

            public StorageKey(uint objectId, string key) => (ObjectId, Key) = (objectId, key);

            public static StorageKey Make(MBObjectBase obj, string key) => new StorageKey(obj.Id.InternalValue, key);

            public bool Equals(StorageKey other) => ObjectId == other.ObjectId && Key.Equals(other.Key);

            public override bool Equals(object obj) => obj is StorageKey sk && Equals(sk);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }

        private ConcurrentDictionary<StorageKey, object?>? _extensions;
    }
}