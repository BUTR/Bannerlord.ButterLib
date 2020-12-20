using System.Diagnostics.CodeAnalysis;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem
{
    /**
     * <summary>
     * Storage interface to <see cref="MBObjectBase"/>-associated Object Extension Data (dynamic variables / flags).
     * Used primarily by <see cref="Extensions.MBObjectBaseExtensions"/>.
     * </summary>
     */
    internal interface IMBObjectExtensionDataStore
    {
        /****    V A R I A B L E S    ****/

        /**
         * <summary>
         * Check whether a variable <paramref name="name"/> is stored upon <paramref name="object"/>.
         * </summary>
         * <returns>
         * <see langword="true"/> if it exists, else <see langword="false"/>.
         * </returns>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         */
        public bool HasVariable(MBObjectBase @object, string name);

        /**
         * <summary>
         * Remove the variable <paramref name="name"/> from <paramref name="object"/>, if set.
         * </summary>
         * <returns>
         * <see langword="true"/> if it was set prior to removal, else <see langword="false"/>.
         * </returns>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         */
        public bool RemoveVariable(MBObjectBase @object, string name);

        /**
         * <summary>
         * Set the value of the variable <paramref name="name"/> upon <paramref name="object"/> to <paramref name="data"/>.
         * </summary>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         * <param name="data">The variable's value.</param>
         */
        public void SetVariable(MBObjectBase @object, string name, object? data);

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
        public bool TryGetVariable<T>(MBObjectBase @object, string name, out T value);

        /****    F L A G S    ****/

        /**
         * <summary>
         * Check whether the flag <paramref name="name"/> is set upon <paramref name="object"/>.
         * </summary>
         * <returns>
         * <see langword="true"/> if set, else <see langword="false"/>.
         * </returns>
         * <param name="object">A game object.</param>
         * <param name="name">A string flag.</param>
         */
        public bool HasFlag(MBObjectBase @object, string name);

        /**
         * <summary>
         * Remove the flag <paramref name="name"/> from <paramref name="object"/>, if set.
         * </summary>
         * <returns>
         * <see langword="true"/> if it was set prior to removal, else <see langword="false"/>.
         * </returns>
         * <param name="object">A game object.</param>
         * <param name="name">A string flag.</param>
         */
        public bool RemoveFlag(MBObjectBase @object, string name);

        /**
         * <summary>
         * Set the flag <paramref name="name"/> upon <paramref name="object"/>.
         * </summary>
         * <param name="object">A game object.</param>
         * <param name="name">A string flag.</param>
         */
        public void SetFlag(MBObjectBase @object, string name);
    }
}
