using Bannerlord.ButterLib.SaveSystem.Extensions;

using Newtonsoft.Json;

using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public sealed class MBObjectBaseExtensionCampaignBehavior : CampaignBehaviorBase
    {
        public static MBObjectBaseExtensionCampaignBehavior? Instance { get; set; }

        private Dictionary<MBGUID, Dictionary<string, string?>>? _extensions;

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncDataAsJson("_descriptorManager", ref _extensions);

            if (dataStore.IsLoading && _extensions == null)
            {
                _extensions = new Dictionary<MBGUID, Dictionary<string, string?>>();
            }
        }

        public override void RegisterEvents() { }

        public void AddExtension<T>(MBObjectBase @object, string key, ref T data, JsonSerializerSettings? settings = null)
        {
            if (_extensions == null)
                return;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, string?>();

            _extensions[@object.Id][key] = JsonConvert.SerializeObject(data, settings);
        }

        public void RemoveExtension(MBObjectBase @object, string key)
        {
            if (_extensions == null)
                return;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, string?>();

            if (_extensions[@object.Id].TryGetValue(key, out _))
                _extensions[@object.Id].Remove(key);
        }

#nullable disable
        public T GetExtension<T>(MBObjectBase @object, string key, JsonSerializerSettings? settings = null)
        {
            if (_extensions == null)
                return default;

            if (!_extensions.TryGetValue(@object.Id, out _))
                _extensions[@object.Id] = new Dictionary<string, string>();

            if (_extensions[@object.Id].TryGetValue(key, out var val) && val is { } value)
                return JsonConvert.DeserializeObject<T>(value, settings);

            return default;
        }
#nullable restore
    }
}