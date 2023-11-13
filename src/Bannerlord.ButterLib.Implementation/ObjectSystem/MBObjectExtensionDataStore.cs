using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.ObjectSystem;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal sealed class MBObjectExtensionDataStore : CampaignBehaviorBase, IMBObjectExtensionDataStore
    {
        private ConcurrentDictionary<DataKey, object?> _vars = new();
        private ConcurrentDictionary<DataKey, bool> _flags = new();

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            var keeper = ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IMBObjectKeeper>();
            keeper?.Sync(dataStore);

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

        private static void ReleaseOrphanedEntries<T>(IDictionary<DataKey, T> dict, ISet<MBGUID> cache)
        {
            static bool DoesNotExist(MBGUID objectId)
            {
                try
                {
                    return MBObjectManager.Instance.GetObject(objectId) == default;
                }
                catch (Exception e) when (e is MBTypeNotRegisteredException)
                {
                    return true;
                }
            }

            foreach (var k in dict.Keys)
            {
                if (cache.Contains(k.ObjectId))
                    dict.Remove(k);
                else if (DoesNotExist(k.ObjectId))
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

        public bool TryGetVariable<T>(MBObjectBase @object, string name, out T? value)
        {
            if (_vars.TryGetValue(DataKey.Make(@object, name), out var val) && val is T or null)
            {
                value = (T?) val;
                return true;
            }

            value = default;
            return false;
        }

        /* Flags Implementation */

        public bool HasFlag(MBObjectBase @object, string name) => _flags.TryGetValue(DataKey.Make(@object, name), out var flag) && flag;

        public bool RemoveFlag(MBObjectBase @object, string name) => _flags.TryRemove(DataKey.Make(@object, name), out _);

        //public bool RemoveFlag(MBObjectBase @object, string name) => _flags.Remove(DataKey.Make(@object, name));

        public void SetFlag(MBObjectBase @object, string name) => _flags[DataKey.Make(@object, name)] = true;

        /* DataKey Implementation */

        private sealed record DataKey([field: SaveableField(0)] MBGUID ObjectId, [field: SaveableField(1)] string? Key)
        {
            internal static DataKey Make(MBObjectBase obj, string key) => new(obj.Id, key);

            public override string ToString() => $"{ObjectId}::{Key}";
        }

#if v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115
        private sealed class SavedTypeDefiner : SaveableCampaignBehaviorTypeDefiner
#elif v120 || v121 || v122 || v123 || v124
        private sealed class SavedTypeDefiner : SaveableTypeDefiner
#else
#error DEFINE
#endif
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