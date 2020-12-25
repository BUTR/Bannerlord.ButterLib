using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    public readonly struct DependedModule
    {
        public string ModuleId { get; }
        public ApplicationVersion Version { get; }

        public DependedModule(string moduleId, ApplicationVersion version)
        {
            ModuleId = moduleId;
            Version = version;
        }
    }
}