using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.SubModuleWrappers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib
{
    public class ImplementationLoaderSubModule : MBSubModuleBaseListWrapper
    {
        private static IEnumerable<MBSubModuleBase> LoadAllImplementationAssemblies(ILogger? logger)
        {
            logger?.LogInformation("Loading implementations...");

            var implementationAssemblies = new List<Assembly>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            var thisAssembly = typeof(ImplementationLoaderSubModule).Assembly;

            var assemblyFile = new FileInfo(thisAssembly.Location);
            if (!assemblyFile.Exists)
            {
                logger?.LogError("Assembly file does not exists!");
                yield break;
            }

            var assemblyDirectory = assemblyFile.Directory;
            if (assemblyDirectory == null || !assemblyDirectory.Exists)
            {
                logger?.LogError("Assembly directory does not exists!");
                yield break;
            }

            var matches = assemblyDirectory.GetFiles("Bannerlord.ButterLib.Implementation.*.dll");
            if (!matches.Any())
            {
                logger?.LogError("No implementations found.");
                yield break;
            }

            var gameVersion = ApplicationVersionUtils.TryParse(ApplicationVersionUtils.GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;
            if (gameVersion == null)
            {
                logger?.LogError("Failed to get Game version!");
                yield break;
            }

            foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(m.Name))))
            {
                logger?.LogInformation("Found implementation {name}.", match.Name);

                var assembly = Assembly.ReflectionOnlyLoadFrom(match.FullName);
                
                var metadatas = assembly.GetCustomAttributesData();
                var supportedVersionStr = (string?) metadatas.FirstOrDefault(x => x.ConstructorArguments.Count == 2 && (string?) x.ConstructorArguments[0].Value == "GameVersion").ConstructorArguments[1].Value;
                if (string.IsNullOrEmpty(supportedVersionStr))
                {
                    logger?.LogError("Implementation {name} is missing GameVersion AssemblyMetadataAttribute!", match.Name);
                    continue;
                }

                var supportedVersion = !string.IsNullOrEmpty(supportedVersionStr) && ApplicationVersionUtils.TryParse(supportedVersionStr, out var sv)
                    ? sv
                    : (ApplicationVersion?) null;
                if (supportedVersion == null)
                {
                    logger?.LogError("Implementation {name} has invalid GameVersion AssemblyMetadataAttribute!", match.Name);
                    continue;
                }

                if (gameVersion.Value.IsSameWithoutRevision(supportedVersion.Value))
                {
                    logger?.LogInformation("Implementation {name} is loaded.", match.Name);
                    implementationAssemblies.Add(Assembly.LoadFrom(match.FullName));
                }
                else
                {
                    logger?.LogInformation("Implementation {name} is skipped.", match.Name);
                }
            }

            var submodules = implementationAssemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(t =>
                    t.FullName != typeof(ImplementationLoaderSubModule).FullName && typeof(MBSubModuleBase).IsAssignableFrom(t)));
            foreach (var subModuleType in submodules)
            {
                var constructor = subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null);
                if (constructor == null)
                {
                    logger?.LogError("SubModule {subModuleType} is missing a default constructor!", subModuleType);
                    continue;
                }

                if (constructor.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                    yield return subModule;
            }

            logger?.LogInformation("Finished loading implementations.");
        }

        private ILogger _logger = default!;

        protected override void OnSubModuleLoad()
        {
            _logger = ButterLibSubModule.Instance.GetTempServiceProvider().GetRequiredService<ILogger<ImplementationLoaderSubModule>>();

            SubModules.AddRange(LoadAllImplementationAssemblies(_logger).Select(x => new MBSubModuleBaseWrapper(x)).ToList());

            base.OnSubModuleLoad();
        }
    }
}