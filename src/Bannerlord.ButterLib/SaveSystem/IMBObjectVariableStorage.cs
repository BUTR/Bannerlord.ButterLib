using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    // Interface to MBObjectBase-associated extension variables
    public interface IMBObjectVariableStorage
    {
        public object? GetVariable(MBObjectBase @object, string key);
        public T GetVariable<T>(MBObjectBase @object, string key);
        public void SetVariable(MBObjectBase @object, string key, object? data);
        public void RemoveVariable(MBObjectBase @object, string key);
    }
}
