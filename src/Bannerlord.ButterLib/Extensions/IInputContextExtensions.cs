using Bannerlord.ButterLib.HotKeys;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.InputSystem;

using HotKeyManager = Bannerlord.ButterLib.HotKeys.HotKeyManager;

namespace Bannerlord.ButterLib.Extensions;

public static class IInputContextExtensions
{
    private sealed class ActionOnDispose : IDisposable
    {
        private readonly Action _action;
        public ActionOnDispose(Action action) => _action = action;
        public void Dispose() => _action?.Invoke();
    }

    /*
    private delegate bool IsUpDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed);
    private delegate bool IsDownDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed, bool checkControllerKey = true);
    private delegate bool IsDownImmediateDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed);
    private delegate bool IsPressedDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed);
    private delegate bool IsReleasedDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed);
    private delegate float GetKeyStateDelegate(GameKey instance, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed);

    private static readonly IsUpDelegate? IsUpMethod = AccessTools2.GetDelegate<IsUpDelegate>(typeof(GameKey), "IsUp");
    private static readonly IsDownDelegate? IsDownMethod = AccessTools2.GetDelegate<IsDownDelegate>(typeof(GameKey), "IsDown");
    private static readonly IsDownImmediateDelegate? IsDownImmediateMethod = AccessTools2.GetDelegate<IsDownImmediateDelegate>(typeof(GameKey), "IsDownImmediate");
    private static readonly IsPressedDelegate? IsPressedMethod = AccessTools2.GetDelegate<IsPressedDelegate>(typeof(GameKey), "IsPressed");
    private static readonly IsReleasedDelegate? IsReleasedMethod = AccessTools2.GetDelegate<IsReleasedDelegate>(typeof(GameKey), "IsReleased");
    private static readonly GetKeyStateDelegate? GetKeyStateMethod = AccessTools2.GetDelegate<GetKeyStateDelegate>(typeof(GameKey), "GetKeyState");

    private static readonly AccessTools.FieldRef<InputContext, List<int>>? _lastFrameDownGameKeyIDs =
        AccessTools2.FieldRefAccess<InputContext, List<int>>("_lastFrameDownGameKeyIDs");
    private static readonly AccessTools.FieldRef<InputContext, List<GameKey>>? _gameKeysToCurrentlyIgnore =
        AccessTools2.FieldRefAccess<InputContext, List<GameKey>>("_gameKeysToCurrentlyIgnore");

    internal static bool IsUp(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && IsUpMethod is not null &&
               IsUpMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed);
    }

    internal static bool IsDown(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && IsDownMethod is not null &&
               IsDownMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed);
    }

    internal static bool IsDownImmediate(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && IsDownImmediateMethod is not null &&
               IsDownImmediateMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed);
    }

    internal static bool IsPressed(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && IsPressedMethod is not null &&
               IsPressedMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed);
    }

    internal static bool IsReleased(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && IsReleasedMethod is not null &&
               IsReleasedMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed);
    }

    internal static float GetKeyState(this IInputContext inputContext, GameKey gameKey)
    {
        return inputContext is InputContext inputContext2 && GetKeyStateMethod is not null
            ? GetKeyStateMethod(gameKey, inputContext2.IsKeysAllowed, inputContext2.IsMouseButtonAllowed && inputContext2.MouseOnMe, inputContext2.IsMouseWheelAllowed, inputContext2.IsControllerAllowed)
            : 0.0f;
    }


    public static bool IsGameKeyDown<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is not { } gameKey) return false;

        return inputContext.IsDown(gameKey);
    }

    public static bool IsGameKeyDownImmediate<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is not { } gameKey) return false;

        return inputContext.IsDownImmediate(gameKey);
    }

    public static bool IsGameKeyPressed<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is not { } gameKey) return false;

        return inputContext.IsPressed(gameKey);
    }

    public static bool IsGameKeyReleased<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is not { } gameKey) return false;

        return inputContext.IsReleased(gameKey);
    }

    public static float GetGameKeyState<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is not { } gameKey) return 0.0f;

        return inputContext.GetKeyState(gameKey);
    }
    */

    public static IDisposable? SubscribeToIsDownEvent<THotKeyBase>(this IInputContext inputContext, Action action) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute()) return null;
        hotKey.IsDownEvent += action;
        return new ActionOnDispose(() => hotKey.IsDownEvent -= action);
    }

    public static IDisposable? SubscribeToOnPressedEvent<THotKeyBase>(this IInputContext inputContext, Action action) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute()) return null;
        hotKey.OnPressedEvent += action;
        return new ActionOnDispose(() => hotKey.OnPressedEvent -= action);
    }

    public static IDisposable? SubscribeToOnReleasedEvent<THotKeyBase>(this IInputContext inputContext, Action action) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute()) return null;
        hotKey.OnReleasedEvent += action;
        return new ActionOnDispose(() => hotKey.OnReleasedEvent -= action);
    }

    public static IDisposable? SubscribeToIsDownAndReleasedEvent<THotKeyBase>(this IInputContext inputContext, Action action) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute()) return null;
        hotKey.IsDownAndReleasedEvent += action;
        return new ActionOnDispose(() => hotKey.IsDownAndReleasedEvent -= action);
    }


    public static bool IsHotKeyDown<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is null) return false;

        return (hotKey.GameKey.KeyboardKey?.InputKey is { } keyboardKey && inputContext.IsKeyDown(keyboardKey)) ||
               (hotKey.GameKey.ControllerKey?.InputKey is { } controllerKey && inputContext.IsKeyDown(controllerKey));
    }

    public static bool IsHotKeyPressed<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is null) return false;

        return (hotKey.GameKey.KeyboardKey?.InputKey is { } keyboardKey && inputContext.IsKeyPressed(keyboardKey)) ||
               (hotKey.GameKey.ControllerKey?.InputKey is { } controllerKey && inputContext.IsKeyPressed(controllerKey));
    }

    public static bool IsHotKeyReleased<THotKeyBase>(this IInputContext inputContext) where THotKeyBase : HotKeyBase
    {
        var hotKey = HotKeyManager.StaticInstance?.HotKeys.FirstOrDefault(x => x.GetType() == typeof(THotKeyBase));

        if (hotKey is null || !hotKey.ShouldExecute() || hotKey.GameKey is null) return false;

        return (hotKey.GameKey.KeyboardKey?.InputKey is { } keyboardKey && inputContext.IsKeyReleased(keyboardKey)) ||
               (hotKey.GameKey.ControllerKey?.InputKey is { } controllerKey && inputContext.IsKeyReleased(controllerKey));
    }
}