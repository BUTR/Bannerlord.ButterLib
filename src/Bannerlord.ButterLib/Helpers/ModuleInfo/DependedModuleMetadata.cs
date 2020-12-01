using Bannerlord.ButterLib.Common.Extensions;

using System;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Helpers.ModuleInfo
{
    public readonly struct DependedModuleMetadata
    {
        public readonly string Id;
        public readonly LoadType LoadType;
        public readonly bool IsOptional;
        public readonly ApplicationVersion Version;

        public DependedModuleMetadata(string id, LoadType loadType, bool isOptional, ApplicationVersion version)
        {
            Id = id;
            LoadType = loadType;
            IsOptional = isOptional;
            Version = version;
        }

        /*
        public override bool Equals(object obj) => obj is DependedModuleMetadata objStr && Equals(objStr);
        public bool Equals(DependedModuleMetadata obj) => Id.Equals(obj.Id) && LoadType.Equals(obj.LoadType) && IsOptional.Equals(obj.IsOptional);

        public override int GetHashCode() => HashCode.Combine(Id, LoadType, IsOptional);

        public static bool operator ==(DependedModuleMetadata left, DependedModuleMetadata right) => left.Equals(right);
        public static bool operator !=(DependedModuleMetadata left, DependedModuleMetadata right) => !(left == right);
        */

        internal static string GetLoadType(LoadType loadType) => loadType switch
        {
            LoadType.LoadAfterThis  => "Before ",
            LoadType.LoadBeforeThis => "After  ",
            _                       => "ERROR  "
        };
        private static string GetVersion(ApplicationVersion av) => av.IsSameWithRevision(ApplicationVersion.Empty) ? "" : $" {av}";
        private static string GetOptional(bool isOptional) => isOptional ? " Optional" : "";
        public override string ToString() => $"{GetLoadType(LoadType)}{Id}{GetVersion(Version)}{GetOptional(IsOptional)}";
    }
}