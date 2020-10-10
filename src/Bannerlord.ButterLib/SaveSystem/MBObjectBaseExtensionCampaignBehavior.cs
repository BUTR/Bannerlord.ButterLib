using Bannerlord.ButterLib.SaveSystem.Extensions;

using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    // TODO:
    internal sealed class MBObjectBaseExtensionCampaignBehavior : CampaignBehaviorBase
    {
        public static MBObjectBaseExtensionCampaignBehavior? Instance { get; set; }

        private Dictionary<MBGUID, Dictionary<string, object?>>? _extensions;

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncDataAsJson("_descriptorManager", ref _extensions);

            if (dataStore.IsLoading && _extensions == null)
            {
                _extensions = new Dictionary<MBGUID, Dictionary<string, object?>>();
            }
        }

        public override void RegisterEvents() { }

        public void AddExtension(MBObjectBase @object, string key, object? data)
        {
            if (_extensions == null)
                return;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, object?>();

            _extensions[@object.Id][key] = data;
        }

        public void RemoveExtension(MBObjectBase @object, string key)
        {
            if (_extensions == null)
                return;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, object?>();

            if (_extensions[@object.Id].TryGetValue(key, out _))
                _extensions[@object.Id].Remove(key);
        }

#nullable disable
        public object GetExtension(MBObjectBase @object, string key)
        {
            if (_extensions == null)
                return default;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, object>();

            if (_extensions[@object.Id].TryGetValue(key, out var value))
                return value;

            return default;
        }
#nullable restore
    }
}