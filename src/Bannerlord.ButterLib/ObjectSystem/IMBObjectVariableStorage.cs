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

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
        /// as a raw <see cref="object"/>.
        /// </summary>
        /// <returns>
        /// The raw <see cref="object"/> stored in the variable.
        /// If no such variable exists, then <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// It's preferred to use <see cref="GetVariable{T}(MBObjectBase, string)"/>, the generic, strongly-typed
        /// variant. This method is provided for some special cases (e.g., if you don't know the type
        /// of the object stored in the variable).
        /// </remarks>
        /// <seealso cref="GetVariable{T}(MBObjectBase, string)"/>
        /// <param name="object"> A game object.</param>
        /// <param name="name"> The variable's name.</param>
        public object? GetVariable(MBObjectBase @object, string name);

        /**
         * <summary>
         * Get the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
         * as a <typeparamref name="T"/>.
         * </summary>
         * <returns>
         * If the variable exists, its value as a <typeparamref name="T"/>.
         * If the variable doesn't exist, a default-valued <typeparamref name="T"/>.
         * </returns>
         * <seealso cref="GetVariable(MBObjectBase, string)"/>
         * <typeparam name="T">The type of the variable value.</typeparam>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         */
        [return: MaybeNull]
        public T GetVariable<T>(MBObjectBase @object, string name);

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
