using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem;

/// <summary>
/// Finds MBObjectBase objects based on their MBGUID
/// </summary>
public interface IMBObjectFinder
{
    MBObjectBase? Find(MBGUID id, Type? type = null);
}