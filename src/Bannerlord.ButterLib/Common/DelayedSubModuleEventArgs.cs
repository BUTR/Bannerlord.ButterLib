using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.Common
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
        /// <see cref="PatchType"/> can only be a <see cref="SubModulePatchType.Postfix"/> in that case.
        /// </para><para>
        /// <see langword="false"/>, if corresponding override method of the derived class specified
        /// in <see cref="Type"/> was called. <see cref="PatchType"/> could be both
        /// <see cref="SubModulePatchType.Prefix"/> and <see cref="SubModulePatchType.Postfix"/> in that case.
        /// </para></value>
        public bool IsBase { get; }

        /// <summary>A type of the Harmony patch that was used to raise the event.</summary>
        /// <value>
        /// <see cref="SubModulePatchType.Prefix"/>, when event is raised before the execution of the method;
        /// <see cref="SubModulePatchType.Postfix"/>, when event is raised after the execution of the method.
        /// </value>
        public SubModulePatchType PatchType { get; }

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
        /// <param name="patchType">A type of the Harmony patch that was used to raise the event.</param>
        /// <param name="methodName">A method of the <see cref="MBSubModuleBase"/> derived class that was used to raise the event.</param>
        /// <exception cref="T:System.ArgumentException">Thrown when <paramref name="type"/>
        /// does not point to a subclass of the <see cref="MBSubModuleBase"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Thrown when <paramref name="patchType"/>
        /// is not a valid <see cref="SubModulePatchType"/> enum.</exception>
        public DelayedSubModuleEventArgs(Type type, bool isBase, SubModulePatchType patchType, string methodName)
        {
            if (!typeof(MBSubModuleBase).IsAssignableFrom(type))
            {
                throw new ArgumentException(string.Format("{0} is not supported type.", type.FullName), nameof(type));
            }
            if (!Enum.IsDefined(typeof(SubModulePatchType), patchType))
            {
                throw new ArgumentOutOfRangeException(nameof(patchType), patchType, $"DelayedSubModuleEventArgs ctor is supplied with not supported {typeof(SubModulePatchType).Name} value.");
            }
            Type = type;
            IsBase = isBase;
            PatchType = patchType;
            MethodName = methodName;
        }

        public enum SubModulePatchType : byte
        {
            Prefix = 0,
            Postfix = 1
        }
    }
}