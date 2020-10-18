using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    // Interface to MBObjectBase-associated extension variables
    public interface IMBObjectVariableStorage
    {
        /* Generalized Variables */
        public object? GetVariable(MBObjectBase @object, string key);
        public T GetVariable<T>(MBObjectBase @object, string key);
        public void SetVariable(MBObjectBase @object, string key, object? data);
        public void RemoveVariable(MBObjectBase @object, string key);

        /* Flags */
        public bool HasFlag(MBObjectBase @object, string name);
        public void SetFlag(MBObjectBase @object, string name);
        public void RemoveFlag(MBObjectBase @object, string name);
    }
}
