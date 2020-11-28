using System;
using System.Reflection;

namespace Bannerlord.ButterLib.Assemblies
{
    /// <summary>
    /// Creates a full copy of the game's AppDomain.
    /// Should be used to check if a loading Assembly will be compatible with the game.
    /// </summary>
    /// <remarks>
    /// Should be replaced with AssemblyLoadContext once Bannerlord switched to .NET Core
    /// </remarks>
    [Obsolete("Manipulations with AppDomain are not recommended", true)]
    public sealed class AssemblyVerifier : IDisposable
    {
        private readonly IAssemblyLoader _assemblyLoader = new AssemblyLoaderProxy();

        public AssemblyVerifier(string name) { }

        /// <summary>
        /// Returns an implementation of <see cref="IAssemblyLoader"/> that will be executing <see cref="Assembly"/> loading from an isolated <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="exception">The exception that occurred when trying to create the <see cref="IAssemblyLoader"/></param>
        public IAssemblyLoader? GetLoader(out Exception? exception)
        {
            exception = null;
            return _assemblyLoader;
        }

        public void Dispose() { }
    }
}