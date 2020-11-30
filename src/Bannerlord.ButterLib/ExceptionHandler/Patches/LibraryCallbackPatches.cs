using HarmonyLib;

using System.Linq;
using System.Reflection;

using TaleWorlds.DotNet;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class LibraryCallbackPatches
    {
        private static MethodInfo[]? _methods;
        private static MethodInfo[] Methods => _methods ??= Enumerable.Empty<MethodInfo>()
            .Concat(BasePatch.GetMethods<DotNetObject, LibraryCallback>())
            .Concat(BasePatch.GetMethods<LibraryCallback>(typeof(Managed)))
            .Concat(BasePatch.GetDerivedMethods<ManagedObject, LibraryCallback>())
            .Concat(BasePatch.GetDerivedMethods<NativeObject, LibraryCallback>())
            .ToArray();

        internal static void Enable(Harmony harmony)
        {
            foreach (var engineCallbackMethod in Methods)
            {
                harmony.Patch(
                    engineCallbackMethod,
                    finalizer: new HarmonyMethod(BasePatch.FinalizerMethod, before: new [] { "org.calradia.admiralnelson.betterexceptionwindow" }));
            }
        }

        internal static void Disable(Harmony harmony)
        {
            foreach (var engineCallbackMethod in Methods)
            {
                harmony.Unpatch(engineCallbackMethod, BasePatch.FinalizerMethod);
            }
        }
    }
}