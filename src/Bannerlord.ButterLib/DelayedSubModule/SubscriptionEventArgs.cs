using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.DelayedSubModule;

/// <summary>
/// A container class for the arguments of the events,
/// that are added to the <see cref="MBSubModuleBase"/> public and protected method calls.
/// </summary>
/// <remarks>
/// These Events are injected via Harmony patching of the respective methods
/// with <see cref="HarmonyPatchType.Prefix"/> and <see cref="HarmonyPatchType.Postfix"/>.
/// </remarks>
public class SubscriptionEventArgs : EventArgs
{
    /// <summary>
    /// An indicator that the underlying virtual implementation of the <see cref="MBSubModuleBase"/>
    /// method was called, not a derived class override.
    /// </summary>
    /// <value><para>
    /// <see langword="true"/>, if base <see cref="MBSubModuleBase"/> virtual method was called.
    /// </para><para>
    /// <see langword="false"/>, if corresponding override method of the derived class specified
    /// in <see cref="Type"/> was called.
    /// </para></value>
    public bool IsBase { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionGlobalEventArgs"/> class with the supplied values.
    /// </summary>
    /// <param name="isBase">
    /// An indicator that the underlying virtual implementation of the <see cref="MBSubModuleBase"/>
    /// method was called, not a derived class override.
    /// </param>
    public SubscriptionEventArgs(bool isBase)
    {
        IsBase = isBase;
    }
}