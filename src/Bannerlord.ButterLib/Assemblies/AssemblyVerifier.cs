using System;

namespace Bannerlord.ButterLib.Assemblies
{
    [Obsolete("Manipulations with AppDomain are not recommended", true)]
    public sealed class AssemblyVerifier : IDisposable
    {
        private readonly IAssemblyLoader _assemblyLoader = new AssemblyLoaderProxy();

        public AssemblyVerifier(string name) { }

        public IAssemblyLoader? GetLoader(out Exception? exception)
        {
            exception = null;
            return _assemblyLoader;
        }

        public void Dispose() { }
    }
}