using System;
using System.Reflection;

namespace Bannerlord.ButterLib.Assemblies
{
    [Obsolete("Manipulations with AppDomain are not recommended", true)]
    public interface IAssemblyLoader
    {
        void LoadFile(string path);
        bool LoadFileAndTest(string path);
        bool LoadFileAndTest(string path, out ReflectionTypeLoadException? exception);
    }
}