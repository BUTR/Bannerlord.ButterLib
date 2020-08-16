using HarmonyLib;

using System;

namespace Bannerlord.ButterLib.Common
{
    public sealed class DelayedSubModuleEventArgs : EventArgs
    {
        public Type Type { get; }
        /// <summary>
        /// When true, can only be a <see cref="HarmonyPatchType.Postfix"/>.
        /// When false, can be a <see cref="HarmonyPatchType.Prefix"/> or <see cref="HarmonyPatchType.Postfix"/>
        /// </summary>
        public bool IsBase { get; }
        public HarmonyPatchType PatchType { get; }
        public string MethodName { get; }

        public DelayedSubModuleEventArgs(Type type, bool isBase, HarmonyPatchType patchType, string methodName)
        {
            Type = type;
            IsBase = isBase;
            PatchType = patchType;
            MethodName = methodName;
        }
    }
}