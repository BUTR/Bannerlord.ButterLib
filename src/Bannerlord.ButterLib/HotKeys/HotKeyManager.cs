/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyManager.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.ButterLib.HotKeys;

/// <summary>
/// Describes a single HotKey group to which you can add individual HotKeyManager before building.
/// </summary>
public abstract class HotKeyManager
{
    private static IHotKeyManagerStatic? _staticInstance;
    internal static IHotKeyManagerStatic? StaticInstance =>
        _staticInstance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IHotKeyManagerStatic>();

    /// <summary>
    /// The available categories in the main menu for your hotkey to appear under.
    /// </summary>
    public static readonly IReadOnlyDictionary<HotKeyCategory, string> Categories = new Dictionary<HotKeyCategory, string>
    {
        { HotKeyCategory.Action, nameof(GameKeyMainCategories.ActionCategory) },
        { HotKeyCategory.Chat, nameof(GameKeyMainCategories.ChatCategory) },
        { HotKeyCategory.CampaignMap,nameof(GameKeyMainCategories.CampaignMapCategory) },
        { HotKeyCategory.MenuShortcut, nameof(GameKeyMainCategories.MenuShortcutCategory) },
        { HotKeyCategory.OrderMenu, nameof(GameKeyMainCategories.OrderMenuCategory) }
    };

    /// <summary>
    /// Create a new HotKey group for your mod.
    /// </summary>
    /// <param name="modName">The name of your mod.</param>
    /// <returns>A HotKeyManager object for you to start adding new HotKeyManager to.</returns>
    /// <exception cref="ArgumentException">Thrown if a mod with the same name has already begun registering hotkeys.</exception>
    public static HotKeyManager? Create(string modName) => StaticInstance?.Create(modName);

    /// <summary>
    /// Create a new HotKey group for your mod with a separate entry on the Options Menu.
    /// </summary>
    /// <param name="modName">The name of your mod.</param>
    /// <param name="categoryName">Category entry name in the Options Menu, supports translation</param>
    /// <returns>A HotKeyManager object for you to start adding new HotKeyManager to.</returns>
    /// <exception cref="ArgumentException">Thrown if a mod with the same name has already begun registering hotkeys.</exception>
    public static HotKeyManager? CreateWithOwnCategory(string modName, string categoryName) => StaticInstance?.CreateWithOwnCategory(modName, categoryName);


    /// <summary>
    /// Adds a hotkey to the manager ready for building.
    /// </summary>
    /// <param name="hotkey">The <see cref="HotKeyBase"/> to add.</param>
    /// <typeparam name="T">The <see cref="HotKeyBase"/> derived type to add.</typeparam>
    /// <returns>The provided <see cref="HotKeyBase"/> (now initialized)</returns>
    /// <exception cref="ArgumentException">Thrown when a hotkey with the same IdName exists.</exception>
    public abstract T Add<T>(T hotkey) where T : HotKeyBase;

    /// <summary>
    /// Adds a hotkey to he manager ready for building.
    /// </summary>
    /// <typeparam name="T">The <see cref="HotKeyBase"/> derived type to add.</typeparam>
    /// <returns>A new instance of <seealso cref="HotKeyBase"/></returns>
    public abstract T Add<T>() where T : HotKeyBase, new();

    /// <summary>
    /// Builds up the hotkeys and registers them with Bannerlord.
    /// </summary>
    /// <returns>Returns all the hotkeys that were built up.</returns>
    public abstract IReadOnlyList<HotKeyBase> Build();
}