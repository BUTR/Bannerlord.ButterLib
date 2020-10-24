using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem.Extensions
{
    /// <summary>
    /// <see cref="MBObjectBase"/> extension methods for dynamic variable / flag storage
    /// </summary>
    public static class MBObjectBaseExtensions
    {
        private static IMBObjectVariableStorage? _instance;

        private static IMBObjectVariableStorage? Instance =>
            _instance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectVariableStorage>();

        internal static void OnGameEnd() => _instance = null;

        /* Variables */

        /**
         * <summary>
         * Gets the <paramref name="value"/> of the variable <paramref name="name"/> stored for <paramref name="object"/>.
         * </summary>
         * <returns>
         * If the variable exists, returns <see langword="true"/>, and <paramref name="value"/> is set to its value.
         * Otherwise, returns <see langword="false"/>, and <paramref name="value"/> is set to a default-valued <typeparamref name="T"/>.
         * </returns>
         * <example>
         * <code>
         * if (myTown.TryGetVariable("AppointedMarshal", out Hero marshal))
         *     DeployMarshal(marshal);
         * </code>
         * </example>
         * <seealso cref="TryGetVariable(MBObjectBase, string, out object?)"/>
         * <typeparam name="T">The type of the variable's value.</typeparam>
         * <param name="object">A game object.</param>
         * <param name="name">The variable's name.</param>
         */
        public static bool TryGetVariable<T>(this MBObjectBase @object, string name, [MaybeNull] out T value)
        {
            if (Instance is { } instance && instance.TryGetVariable(@object, name, out T val) && val is T typedVal)
            {
                value = typedVal;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Set the value of the variable <paramref name="name"/> upon <paramref name="object"/>
        /// to <paramref name="data"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// myHero.SetVariable("Lovers", new List&lt;Hero&gt; { Hero.MainHero });
        /// </code>
        /// </example>
        /// <typeparam name="T">Type of the variable's value.</typeparam>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        /// <param name="data">The variable's value.</param>
        public static void SetVariable<T>(this MBObjectBase @object, string name, T data)
        {
            if (Instance is { } instance)
            {
                if (data is char @char)
                {
                    instance.SetVariable(@object, name, @char.ToString());
                    return;
                }

                instance.SetVariable(@object, name, data);
            }
        }

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
        public static void RemoveVariable(this MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
                instance.RemoveVariable(@object, name);
        }

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
        public static bool HasFlag(MBObjectBase @object, string name) => (Instance is { } instance) && instance.HasFlag(@object, name);

        /// <summary>Set the flag <paramref name="name"/> upon <paramref name="object"/>.</summary>
        /// <example>
        /// <code>
        /// myKingdom.SetFlag("IsRevoltFaction");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static void SetFlag(MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
                instance.SetFlag(@object, name);
        }

        /// <summary>Remove the flag <paramref name="name"/> from <paramref name="object"/>, if set.</summary>
        /// <example>
        /// <code>
        /// myHero.RemoveFlag("IsImmortal");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static void RemoveFlag(MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
                instance.RemoveFlag(@object, name);
        }
    }
}