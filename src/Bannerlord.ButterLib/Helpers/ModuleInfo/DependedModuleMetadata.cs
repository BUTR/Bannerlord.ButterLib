using System;

namespace Bannerlord.ButterLib.Helpers.ModuleInfo
{
    public readonly struct DependedModuleMetadata
    {
        public readonly string Id;
        public readonly LoadType LoadType;
        public readonly bool IsOptional;

        public DependedModuleMetadata(string id, LoadType loadType, bool isOptional)
        {
            Id = id;
            LoadType = loadType;
            IsOptional = isOptional;
        }

        /*
        public override bool Equals(object obj) => obj is DependedModuleMetadata objStr && Equals(objStr);
        public bool Equals(DependedModuleMetadata obj) => Id.Equals(obj.Id) && LoadType.Equals(obj.LoadType) && IsOptional.Equals(obj.IsOptional);

        public override int GetHashCode() => HashCode.Combine(Id, LoadType, IsOptional);

        public static bool operator ==(DependedModuleMetadata left, DependedModuleMetadata right) => left.Equals(right);
        public static bool operator !=(DependedModuleMetadata left, DependedModuleMetadata right) => !(left == right);
        */
    }
}