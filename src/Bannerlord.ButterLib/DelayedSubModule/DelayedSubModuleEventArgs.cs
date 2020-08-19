using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.DelayedSubModule
{
    /// <summary>
    /// A container class for the arguments of the events,
    /// that are added to the <see cref="MBSubModuleBase"/> public and protected method calls.
    /// </summary>
    /// <remarks>
    /// These Events are injected via Harmony patching of the respective methods
    /// with <see cref="HarmonyPatchType.Prefix"/> and <see cref="HarmonyPatchType.Postfix"/>.
    /// </remarks>
    public class DelayedSubModuleEventArgs : EventArgs
    {
        /// <summary>The type of submodule for which the method was called.</summary>
        /// <value>Exact <see cref="System.Type"/> of the <see cref="MBSubModuleBase"/> derived class. </value>
        public Type Type { get; }

        /// <summary>
        /// An indicator that the underlying virtual implementation of the <see cref="MBSubModuleBase"/>
        /// method was called, not a derived class override.
        /// </summary>
        /// <value><para>
        /// <see langword="true"/>, if base <see cref="MBSubModuleBase"/> virtual method was called.
        /// <see cref="SubscriptionType"/> can only be a <see cref="DelayedSubModuleSubscriptionType.AfterMethod"/> in that case.
        /// </para><para>
        /// <see langword="false"/>, if corresponding override method of the derived class specified
        /// in <see cref="Type"/> was called. <see cref="SubscriptionType"/> could be both
        /// <see cref="DelayedSubModuleSubscriptionType.BeforeMethod"/> and <see cref="DelayedSubModuleSubscriptionType.AfterMethod"/> in that case.
        /// </para></value>
        public bool IsBase { get; }

        /// <summary>A type of the Harmony patch that was used to raise the event.</summary>
        /// <value>
        /// <see cref="DelayedSubModuleSubscriptionType.BeforeMethod"/>, when event is raised before the execution of the method;
        /// <see cref="DelayedSubModuleSubscriptionType.AfterMethod"/>, when event is raised after the execution of the method.
        /// </value>
        public DelayedSubModuleSubscriptionType SubscriptionType { get; }

        /// <summary>A method of the <see cref="MBSubModuleBase"/> derived class that was used to raise the event.</summary>
        /// <value>A string containing the name of the method.</value>
        public string MethodName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayedSubModuleEventArgs"/> class with the supplied values.
        /// </summary>
        /// <param name="type">The type of submodule for which the method was called.</param>
        /// <param name="isBase">
        /// An indicator that the underlying virtual implementation of the <see cref="MBSubModuleBase"/>
        /// method was called, not a derived class override.
        /// </param>
        /// <param name="subscriptionType">A type of the Harmony patch that was used to raise the event.</param>
        /// <param name="methodName">A method of the <see cref="MBSubModuleBase"/> derived class that was used to raise the event.</param>
        /// <exception cref="T:System.ArgumentException">Thrown when <paramref name="type"/>
        /// does not point to a subclass of the <see cref="MBSubModuleBase"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Thrown when <paramref name="subscriptionType"/>
        /// is not a valid <see cref="DelayedSubModuleSubscriptionType"/> enum.</exception>
        public DelayedSubModuleEventArgs(Type type, bool isBase, DelayedSubModuleSubscriptionType subscriptionType, string methodName)
        {
            if (!typeof(MBSubModuleBase).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{type.FullName} is not supported type.", nameof(type));
            }
            if (!Enum.IsDefined(typeof(DelayedSubModuleSubscriptionType), subscriptionType))
            {
                throw new ArgumentOutOfRangeException(nameof(subscriptionType), subscriptionType, $"DelayedSubModuleEventArgs .ctor is supplied with not supported {nameof(DelayedSubModuleSubscriptionType)} value.");
            }
            Type = type;
            IsBase = isBase;
            SubscriptionType = subscriptionType;
            MethodName = methodName;
        }

        public bool IsValid<T>(string methodName, DelayedSubModuleSubscriptionType subscriptionType) where T: MBSubModuleBase
        {
            return Type == typeof(T) && MethodName == methodName && !IsBase && SubscriptionType == subscriptionType;
        }
        public bool IsValidBase<T>(string methodName, DelayedSubModuleSubscriptionType subscriptionType) where T: MBSubModuleBase
        {
            return Type == typeof(T) && MethodName == methodName && IsBase && SubscriptionType == subscriptionType;
        }
    }
}