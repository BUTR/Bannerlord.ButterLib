using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    // TODO
    internal static class MBObjectBaseExtensions
    {
        public static void AddExtension<T>(this MBObjectBase @object, string key, ref T data)
        {
            MBObjectBaseExtensionStore.AddExtension(@object, key, ref data);
        }

        public static void RemoveExtension(this MBObjectBase @object, string key)
        {
            MBObjectBaseExtensionStore.RemoveExtension(@object, key);
        }

#nullable disable
        public static T GetExtension<T>(this MBObjectBase @object, string key)
        {
            return MBObjectBaseExtensionStore.GetExtension<T>(@object, key);
        }
    }
}