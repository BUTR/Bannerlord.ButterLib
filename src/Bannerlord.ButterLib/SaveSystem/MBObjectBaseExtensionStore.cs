using System.Collections.Generic;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    // TODO
    internal static class MBObjectBaseExtensionStore
    {
        // Cleared on MBSubModuleBase.OnGameStart
        public static Dictionary<MBGUID, Dictionary<string, object?>> Extensions = new Dictionary<MBGUID, Dictionary<string, object?>>();

        public static void AddExtension<T>(MBObjectBase @object, string key, ref T data)
        {
            if (!Extensions.TryGetValue(@object.Id, out _))
                Extensions[@object.Id] = new Dictionary<string, object?>();

            Extensions[@object.Id][key] = data;
        }

        public static void RemoveExtension(MBObjectBase @object, string key)
        {
            if (!Extensions.TryGetValue(@object.Id, out _))
                Extensions[@object.Id] = new Dictionary<string, object?>();

            if (Extensions[@object.Id].TryGetValue(key, out _))
                Extensions[@object.Id].Remove(key);
        }

#nullable disable
        public static T GetExtension<T>(MBObjectBase @object, string key)
        {
            if (!Extensions.TryGetValue(@object.Id, out _))
                Extensions[@object.Id] = new Dictionary<string, object>();

            if (Extensions[@object.Id].TryGetValue(key, out var val) && val is T value)
                return value;

            return default;
        }
#nullable restore
    }
}