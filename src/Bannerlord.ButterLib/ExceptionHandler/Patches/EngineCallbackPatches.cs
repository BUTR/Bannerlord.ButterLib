using HarmonyLib;

using System.Linq;
using System.Reflection;

using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;

namespace Bannerlord.ButterLib.ExceptionHandler.Patches
{
    internal sealed class EngineCallbackPatches
    {
        private static MethodInfo[]? _methods;
        private static MethodInfo[] Methods => _methods ??= Enumerable.Empty<MethodInfo>()
            .Concat(BasePatch.GetMethods<EngineManaged, EngineCallback>())
            .Concat(BasePatch.GetMethods<EngineCallback>(typeof(ManagedExtensions)))
            .Concat(BasePatch.GetMethods<ManagedScriptHolder, EngineCallback>())
            .Concat(BasePatch.GetDerivedMethods<MessageManagerBase, EngineCallback>())
            .Concat(BasePatch.GetMethods<RenderTargetComponent, EngineCallback>())
            .Concat(BasePatch.GetMethods<EngineCallback>(typeof(ScreenManager)))
            .Concat(BasePatch.GetDerivedMethods<ScriptComponentBehaviour, EngineCallback>())
            .Concat(BasePatch.GetMethods<ThumbnailCreatorView, EngineCallback>())
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