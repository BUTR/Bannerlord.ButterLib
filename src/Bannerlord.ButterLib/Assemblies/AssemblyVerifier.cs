﻿using System;
using System.Reflection;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.Assemblies
{
    /// <summary>
    /// Creates a full copy of the game's AppDomain.
    /// Should be used to check if a loading Assembly will be compatible with the game.
    /// </summary>
    /// <remarks>
    /// Should be replaced with AssemblyLoadContext once Bannerlord switched to .NET Core
    /// </remarks>
    public sealed class AssemblyVerifier : IDisposable
    {
        private readonly AppDomain? _domain;

        private readonly Exception? _assemblyLoaderException;
        private readonly IAssemblyLoader? _assemblyLoader;

        public AssemblyVerifier(string name)
        {
            // We create an AppDomain to load the implementation and see if it's compatible with the game
            // This is done so we will not load an incompatible assembly into the main AppDomain, thus breaking the game.
            var domainName = $"{name}_Assembly_Verifier";
            _domain = AppDomain.CreateDomain(
                domainName,
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup
                {
                    ApplicationName = domainName,
                    ApplicationBase = Path.Combine(Utilities.GetBasePath(), "Modules", "Bannerlord.ButterLib", "bin", TaleWorlds.Library.Common.ConfigName),
                    AppDomainManagerAssembly = typeof(AssemblyVerifier).Assembly.FullName,
                    AppDomainManagerType = typeof(EmptyAppDomainManager).FullName
                });
            try
            {
                if (typeof(AssemblyLoaderProxy).Assembly.FullName is { } str && typeof(AssemblyLoaderProxy).FullName is { } str2)
                    _assemblyLoader = _domain?.CreateInstanceAndUnwrap(str, str2) as AssemblyLoaderProxy;
            }
            catch (Exception e)
            {
                _assemblyLoader = null;
                _assemblyLoaderException = e;
            }
        }

        /// <summary>
        /// Returns an implementation of <see cref="IAssemblyLoader"/> that will be executing <see cref="Assembly"/> loading from an isolated <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="exception">The exception that occurred when trying to create the <see cref="IAssemblyLoader"/></param>
        public IAssemblyLoader? GetLoader(out Exception? exception)
        {
            exception = _assemblyLoaderException;
            return _assemblyLoader;
        }

        private void ReleaseUnmanagedResources()
        {
            AppDomain.Unload(_domain);
        }
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
        ~AssemblyVerifier()
        {
            ReleaseUnmanagedResources();
        }
    }
}