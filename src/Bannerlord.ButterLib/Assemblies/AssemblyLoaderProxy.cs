using System;
using System.Reflection;

namespace Bannerlord.ButterLib.Assemblies
{
    /// <summary>
    /// Proxy for calling <see cref="Assembly.LoadFile(string)"/> of a non-default <see cref="AppDomain"/>.
    /// </summary>
    [Obsolete("Manipulations with AppDomain are not recommended", true)]
    internal sealed class AssemblyLoaderProxy : IAssemblyLoader
    {
        public void LoadFile(string path) { }

        public bool LoadFileAndTest(string path) => true;
        public bool LoadFileAndTest(string path, out ReflectionTypeLoadException? exception)
        {
            exception = null;
            return true;
        }
    }
}