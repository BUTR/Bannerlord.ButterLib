/*
 * Original code from https://github.com/sirdoombox/BannerLib/blob/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input/HotKeyBase.cs.
 * Licensed under Unlicense License.
 * Authors: sirdoombox, BUTR.
 */

using System;

using TaleWorlds.InputSystem;

namespace Bannerlord.ButterLib.HotKeys
{
    /// <summary>
    /// Base type for all HotKey definitions to derive from.
    /// </summary>
    public abstract class HotKeyBase
    {
        internal int Id { get; set; }
        internal GameKey? GameKey { get; set; }

        /// <summary>
        /// The unique (to your mod) Id for this hotkey.
        /// </summary>
        protected internal string Uid { get; }

        /// <summary>
        /// The display name for your hotkey that will appear in the options menu.
        /// </summary>
        protected internal virtual string DisplayName { get; }

        /// <summary>
        /// The description text that will appear in the options menu next to your hotkey.
        /// </summary>
        protected internal virtual string Description { get; } = "No Description Set.";

        /// <summary>
        /// The default key for your HotKey, if this is not set in your constructor it will default to `Invalid`
        /// </summary>
        protected internal virtual InputKey DefaultKey { get; } = InputKey.Invalid;

        /// <summary>
        /// The Category in the options menu under which this hotkey will appear.
        /// <see cref="HotKeyManager.Categories"/>
        /// </summary>
        protected internal virtual string Category { get; } = HotKeyManager.Categories[HotKeyCategory.Action];

        /// <summary>
        /// Provide none, one or many functions which all must evaluate to true in order for the key to process input.
        /// This does not need to be set, and can be reset with <see cref="Predicate"/> = null;
        /// </summary>
        public Func<bool>? Predicate { get; set; }

        /// <summary>
        /// Tells the input manager whether or not to process input for this key.
        /// Setting this infrequently is cheaper than using <see cref="Predicate"/> but it is less convenient.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Called once on the frame a key was pressed.
        /// </summary>
        public event Action? OnPressedEvent;
        /// <summary>
        /// Called once on the frame a key was released.
        /// </summary>
        public event Action? OnReleasedEvent;
        /// <summary>
        /// Called once every frame a key remains down.
        /// </summary>
        public event Action? IsDownEvent;

        /// <summary>
        /// The required constructor which has the bare minimum needed to register a key.
        /// </summary>
        /// <param name="uid">The (unique to your mod) id for your hotkey.</param>
        protected internal HotKeyBase(string uid)
        {
            Uid = uid;
            DisplayName = Uid;
        }
        protected internal HotKeyBase(string uid, string displayName, string description, InputKey defaultKey, string category)
        {
            Uid = uid;
            DisplayName = displayName;
            Description = description;
            DefaultKey = defaultKey;
            Category = category;
        }

        /// <summary>
        /// Allows you to supply a HotKeyBase derived class wherever a GameKey might normally be used.
        /// </summary>
        /// <param name="hotKey"><see cref="HotKeyBase"/> to convert.</param>
        /// <returns>The <see cref="GameKey"/> stored internally.</returns>
        public static implicit operator GameKey(HotKeyBase hotKey) => hotKey.GameKey ?? throw new Exception();

        /// <inheritdoc cref="OnPressedEvent"/>
        protected virtual void OnPressed() { }
        /// <inheritdoc cref="OnReleasedEvent"/>
        protected virtual void OnReleased() { }
        /// <inheritdoc cref="IsDownEvent"/>
        protected virtual void IsDown() { }

        internal bool ShouldExecute() => IsEnabled && ((Predicate is null) || (Predicate is not null && Predicate()));

        internal void OnPressedInternal()
        {
            OnPressedEvent?.Invoke();
            OnPressed();
        }

        internal void OnReleasedInternal()
        {
            OnReleasedEvent?.Invoke();
            OnReleased();
        }

        internal void IsDownInternal()
        {
            IsDownEvent?.Invoke();
            IsDown();
        }
    }
}