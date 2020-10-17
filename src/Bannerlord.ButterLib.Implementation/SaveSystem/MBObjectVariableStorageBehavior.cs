using Bannerlord.ButterLib.SaveSystem;
using Bannerlord.ButterLib.SaveSystem.Extensions;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.SaveSystem
{
    internal sealed class MBObjectVariableStorageBehavior : CampaignBehaviorBase, IMBObjectVariableStorage, IDisposable
    {
        public static MBObjectVariableStorageBehavior? Instance { get; set; }

        private ConcurrentDictionary<StorageKey, object?>? _variables;

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                // Cleanup variables in data store that refer to now-nonexistent/untracked objects.
                // This is also periodically done due to autosaves to address any leak concerns.

                // Cache known-expired object IDs, as MBObjectManager.GetObject can be slow,
                // particularly for the types likely to have expired.
                var expiredIdCache = new Dictionary<uint, bool>();

                foreach (var sk in _variables!.Keys)
                {
                    if (expiredIdCache.ContainsKey(sk.ObjectId))
                        _variables.TryRemove(sk, out _);
                    else if (MBObjectManager.Instance.GetObject(sk) == default)
                    {
                        expiredIdCache[sk.ObjectId] = true;
                        _variables.TryRemove(sk, out _);
                    }
                }
            }

            dataStore.SyncDataAsJson("ButterLib.MBObjectVariableStorage", ref _variables);

            if (dataStore.IsLoading && _variables == null)
                _variables = new ConcurrentDictionary<StorageKey, object?>();
        }

        public void SetVariable(MBObjectBase @object, string key, object? data)
        {
            if (_variables == null)
                return;

            _variables[StorageKey.Make(@object, key)] = data;
        }

        public void RemoveVariable(MBObjectBase @object, string key)
        {
            if (_variables == null)
                return;

            _variables.TryRemove(StorageKey.Make(@object, key), out _);
        }

        public object? GetVariable(MBObjectBase @object, string key)
        {
            if (_variables == null)
                return null;

            if (_variables.TryGetValue(StorageKey.Make(@object, key), out var value))
                return value;

            return null;
        }

#nullable disable

        public T GetVariable<T>(MBObjectBase @object, string key)
        {
            if (_variables == null)
                return default;

            if (_variables.TryGetValue(StorageKey.Make(@object, key), out var val) && val is T value)
                return value;

            return default;
        }

#nullable restore

        private readonly struct StorageKey : IEquatable<StorageKey>
        {
            public static implicit operator MBGUID(StorageKey sk) => new MBGUID(sk.ObjectId);

            public readonly uint ObjectId;
            public readonly string Key;

            private StorageKey(uint objectId, string key) => (ObjectId, Key) = (objectId, key);

            public static StorageKey Make(MBObjectBase obj, string key) => new StorageKey(obj.Id.InternalValue, key);

            public bool Equals(StorageKey other) => ObjectId == other.ObjectId && Key.Equals(other.Key);

            public override bool Equals(object obj) => obj is StorageKey sk && Equals(sk);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }

        public void Dispose()
        {
            _variables?.Clear();
        }
    }
}