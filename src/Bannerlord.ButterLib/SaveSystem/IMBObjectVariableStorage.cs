using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    /// <summary>Interface to MBObjectBase-associated dynamic variable/flag storage</summary>
    public interface IMBObjectVariableStorage
    {
        /* Generalized Variables */

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
        /// as a raw <see cref="System.Object"/>.
        /// </summary>
        /// <returns>
        /// The raw <see cref="System.Object"/> stored in the variable.
        /// If no such variable exists, then <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// It's preferred to use the <see cref="GetVariable{T}(MBObjectBase, string)">generic, strongly-typed
        /// version of this method</see>. This is provided for some special cases (e.g., if you don't know the type
        /// of the object stored in the variable).
        /// </remarks>
        /// <seealso cref="GetVariable{T}(MBObjectBase, string)"/>
        /// <param name="object"> A game object.</param>
        /// <param name="name"> The variable's name.</param>
        public object? GetVariable(MBObjectBase @object, string name);

        // Testing the IMHO more readable multi-line comment XMLDoc syntax:
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
        public T GetVariable<T>(MBObjectBase @object, string name);

        // Testing the IMHO more readable multi-line comment XMLDoc syntax:
        /**
        <summary>
        Set the value of the variable <paramref name="name"/> upon <paramref name="object"/>
        to <paramref name="data"/>.
        </summary>
        <param name="object">A game object.</param>
        <param name="name">The variable's name.</param>
        <param name="data">The variable's value.</param>
        */
        public void SetVariable(MBObjectBase @object, string name, object? data);

        /// <summary>
        /// Remove the variable <paramref name="name"/> from <paramref name="object"/>, if set.
        /// </summary>
        /// <example>
        /// <code>
        /// myHero.RemoveVariable("TitlePrefix");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        public void RemoveVariable(MBObjectBase @object, string name);

        /* Flags */

        /// <summary>
        /// Check whether the flag <paramref name="name"/> is set upon <paramref name="object"/>.
        /// </summary>
        /// <returns><see langword="true"/> if the flag is set, else <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// if (thisKingdom.HasFlag("IsRevoltFaction"))
        ///     continue;
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public bool HasFlag(MBObjectBase @object, string name);

        /// <summary>Set the flag <paramref name="name"/> upon <paramref name="object"/>.</summary>
        /// <example>
        /// <code>
        /// myKingdom.SetFlag("IsRevoltFaction");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public void SetFlag(MBObjectBase @object, string name);

        /// <summary>Remove the flag <paramref name="name"/> from <paramref name="object"/>, if set.</summary>
        /// <example>
        /// <code>
        /// myHero.RemoveFlag("IsImmortal");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public void RemoveFlag(MBObjectBase @object, string name);
    }
}
