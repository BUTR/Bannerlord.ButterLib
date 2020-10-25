using System.Diagnostics.CodeAnalysis;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem
{
    /// <summary>
    /// Interface to <see cref="MBObjectBase"/>-associated dynamic variable / flag storage
    /// </summary>
    public interface IMBObjectVariableStorage
    {
        /* Variables */

        /**
         * <summary>
         * Gets the <paramref name="value"/> of the variable <paramref name="name"/> stored for <paramref name="object"/>.
         * </summary>
         * <returns>
         * If the variable exists, returns <see langword="true"/>, and <paramref name="value"/> is set to its value.
         * Otherwise, returns <see langword="false"/>, and <paramref name="value"/> is set to a default-valued <typeparamref name="T"/>.
         * </returns>
         * <typeparam name="T">The type of the variable's value.</typeparam>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         */
        public bool TryGetVariable<T>(MBObjectBase @object, string name, [MaybeNull] out T value);

        /**
         * <summary>
         * Set the value of the variable <paramref name="name"/> upon <paramref name="object"/>
         * to <paramref name="data"/>.
         * </summary>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         * <param name="data">The variable's value.</param>
         */
        public void SetVariable(MBObjectBase @object, string name, object? data);

        /// <summary>
        /// Remove the variable <paramref name="name"/> from <paramref name="object"/>, if set.
        /// </summary>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        public void RemoveVariable(MBObjectBase @object, string name);

        /* Flags */

        /// <summary>
        /// Check whether the flag <paramref name="name"/> is set upon <paramref name="object"/>.
        /// </summary>
        /// <returns><see langword="true"/> if the flag is set, else <see langword="false"/>.</returns>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public bool HasFlag(MBObjectBase @object, string name);

        /// <summary>Set the flag <paramref name="name"/> upon <paramref name="object"/>.</summary>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public void SetFlag(MBObjectBase @object, string name);

        /// <summary>Remove the flag <paramref name="name"/> from <paramref name="object"/>, if set.</summary>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public void RemoveFlag(MBObjectBase @object, string name);
    }
}
