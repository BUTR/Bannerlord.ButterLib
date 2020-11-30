using HarmonyLib;

using System.Linq;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class MBCallbackPatches
    {
        private static MethodInfo[]? _methods;
        private static MethodInfo[] Methods => _methods ??= Enumerable.Empty<MethodInfo>()
            .Concat(BasePatch.GetMethods<Agent, MBCallback>())
            .Concat(BasePatch.GetMethods<MBCallback>(typeof(BannerlordTableauManager)))
            .Concat(BasePatch.GetMethods<CoreManaged, MBCallback>())
            .Concat(BasePatch.GetMethods<MBCallback>(typeof(GameNetwork)))
            .Concat(BasePatch.GetMethods<ManagedOptions, MBCallback>())
            .Concat(BasePatch.GetMethods<MBEditor, MBCallback>())
            .Concat(BasePatch.GetMethods<MBMultiplayerData, MBCallback>())
            .Concat(BasePatch.GetMethods<Mission, MBCallback>())
            .Concat(BasePatch.GetMethods<TaleWorlds.MountAndBlade.Module, MBCallback>())
            .Concat(BasePatch.GetMethods<MBCallback>(typeof(WeaponComponentMissionExtensions)))
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