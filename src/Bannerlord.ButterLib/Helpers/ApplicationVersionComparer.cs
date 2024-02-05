using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;

using TaleWorlds.Library;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers;

public class ApplicationVersionComparer : IComparer<ApplicationVersion>
{
    private readonly ref struct VersionGameTypeWrapper
    {
        private delegate IComparable GetVersionGameTypeDelegate();
        private delegate void SetVersionGameTypeDelegate(IComparable? value);

        private readonly GetVersionGameTypeDelegate? _getVersionGameTypeDelegate;
        private readonly SetVersionGameTypeDelegate? _setVersionGameTypeDelegate;

        public IComparable? VersionGameType
        {
            get => _getVersionGameTypeDelegate?.Invoke();
            set => _setVersionGameTypeDelegate?.Invoke(value);
        }

        public VersionGameTypeWrapper(object? @object)
        {
                var type = @object?.GetType();

                _getVersionGameTypeDelegate = type is not null
                    ? AccessTools2.GetPropertyGetterDelegate<GetVersionGameTypeDelegate>(@object, type, nameof(VersionGameType))
                    : null;
                _setVersionGameTypeDelegate = type is not null
                    ? AccessTools2.GetPropertySetterDelegate<SetVersionGameTypeDelegate>(@object, type, nameof(VersionGameType))
                    : null;
            }
    }

    public int Compare(ApplicationVersion x, ApplicationVersion y)
    {
            var applicationVersionTypeComparison = x.ApplicationVersionType.CompareTo(y.ApplicationVersionType);
            if (applicationVersionTypeComparison != 0) return applicationVersionTypeComparison;

            var majorComparison = x.Major.CompareTo(y.Major);
            if (majorComparison != 0) return majorComparison;

            var minorComparison = x.Minor.CompareTo(y.Minor);
            if (minorComparison != 0) return minorComparison;

            var revisionComparison = x.Revision.CompareTo(y.Revision);
            if (revisionComparison != 0) return revisionComparison;

            var changeSetComparison = x.ChangeSet.CompareTo(y.ChangeSet);
            if (changeSetComparison != 0) return changeSetComparison;

            var versionGameTypeComparison = new VersionGameTypeWrapper(x).VersionGameType?.CompareTo(new VersionGameTypeWrapper(y).VersionGameType) ?? 0;
            if (versionGameTypeComparison != 0) return versionGameTypeComparison;

            return 0;
        }
}