using Bannerlord.ButterLib.ObjectSystem;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal sealed class MBObjectExtensionDataStore : CampaignBehaviorBase, IMBObjectExtensionDataStore
    {
        private ConcurrentDictionary<DataKey, object?> _vars = new ConcurrentDictionary<DataKey, object?>();
        private ConcurrentDictionary<DataKey, bool>   _flags = new ConcurrentDictionary<DataKey, bool>();

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                // Cache known-expired object IDs, as MBObjectManager.GetObject can be slow.
                var cache = new HashSet<MBGUID>();
                ReleaseOrphanedEntries(_vars, cache);
                ReleaseOrphanedEntries(_flags, cache);
            }

            dataStore.SyncData("Vars", ref _vars);
            _vars ??= new();

            dataStore.SyncData("Flags", ref _flags);
            _flags ??= new();
        }

        private void ReleaseOrphanedEntries<T>(IDictionary<DataKey, T> dict, ISet<MBGUID> cache)
        {
            foreach (var k in dict.Keys)
            {
                if (cache.Contains(k.ObjectId))
                    dict.Remove(k);
                else if (MBObjectManager.Instance.GetObject(k.ObjectId) == default)
                {
                    cache.Add(k.ObjectId);
                    dict.Remove(k);
                }
            }
        }

        /* Variables Implementation */
        public bool HasVariable(MBObjectBase @object, string name) => _vars.ContainsKey(DataKey.Make(@object, name));

        public bool RemoveVariable(MBObjectBase @object, string name) => _vars.TryRemove(DataKey.Make(@object, name), out _);

        //public bool RemoveVariable(MBObjectBase @object, string name) => _vars.Remove(DataKey.Make(@object, name));

        public void SetVariable(MBObjectBase @object, string name, object? data) => _vars[DataKey.Make(@object, name)] = data;

        public bool TryGetVariable<T>(MBObjectBase @object, string name, [MaybeNullWhen(false)][NotNullWhen(true)] out T value)
        {
            if (_vars.TryGetValue(DataKey.Make(@object, name), out var val) && val is T concreteVal)
            {
                value = concreteVal;
                return true;
            }

            value = default;
            return false;
        }

        /* Flags Implementation */

        public bool HasFlag(MBObjectBase @object, string name) => _flags.ContainsKey(DataKey.Make(@object, name));

        public bool RemoveFlag(MBObjectBase @object, string name) => _flags.TryRemove(DataKey.Make(@object, name), out _);

        //public bool RemoveFlag(MBObjectBase @object, string name) => _flags.Remove(DataKey.Make(@object, name));

        public void SetFlag(MBObjectBase @object, string name) => _flags[DataKey.Make(@object, name)] = true;

        /* DataKey Implementation */

        private sealed class DataKey : IEquatable<DataKey>
        {
            [SaveableField(0)]
            internal readonly MBGUID ObjectId;

            [SaveableField(1)]
            internal readonly string Key;

            private DataKey(MBGUID objectId, string key) => (ObjectId, Key) = (objectId, key);

            internal static DataKey Make(MBObjectBase obj, string key) => new DataKey(obj.Id, key);

            public bool Equals(DataKey other) => ObjectId == other.ObjectId && !(Key is null || other.Key is null) && Key.Equals(other.Key);

            public override bool Equals(object? obj) => obj is DataKey k && Equals(k);

            public override int GetHashCode() => HashCode.Combine(ObjectId, Key);
        }

        private sealed class SavedTypeDefiner : SaveableCampaignBehaviorTypeDefiner
        {
            public SavedTypeDefiner() : base(222_444_700) { }
            protected override void DefineClassTypes() => AddClassDefinition(typeof(DataKey), 1);
            protected override void DefineContainerDefinitions()
            {
                ConstructContainerDefinition(typeof(ConcurrentDictionary<DataKey, object?>));
                ConstructContainerDefinition(typeof(ConcurrentDictionary<DataKey, bool>));
            }
        }
    }
}