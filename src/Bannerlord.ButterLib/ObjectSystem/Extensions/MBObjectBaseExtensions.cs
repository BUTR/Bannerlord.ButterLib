using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    /// <summary>
    /// <see cref="TaleWorlds.ObjectSystem.MBObjectBase"/> extension methods for dynamic variable / flag storage
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
         * Gets the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
         * as a raw <see cref="System.Object"/>.
         * </summary>
         * <returns>
         * The raw <see cref="System.Object"/> stored in the variable.
         * If no such variable exists, then <see langword="null"/>.
         * </returns>
         * <remarks>
         * It's preferred to use <see cref="GetVariable{T}(MBObjectBase, string)"/>, the generic, strongly-typed
         * variant. This method is provided for some special cases (e.g., if you don't know the type
         * of the object stored in the variable).
         * </remarks>
         * <example>
         * <code>
         * var obj = hero.GetVariable("LifeContextData");
         *
         * if (obj != null)
         *     return JsonConvert.SerializeObject(obj);
         * </code>
         * </example>
         * <seealso cref="GetVariable{T}(MBObjectBase, string)"/>
         * <param name="object"> A game object.</param>
         * <param name="name"> The variable's name.</param>
         */
        public static object? GetVariable(this MBObjectBase @object, string name) =>
            (Instance is { } instance) ? instance.GetVariable(@object, name) : null;

        /// <summary>
        /// Get the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
        /// as a <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// A stored value of type <typeparamref name="T"/> for the variable.
        /// If no such variable exists, a default-valued <typeparamref name="T"/>.
        /// </returns>
        /// <example>
        /// <code>
        /// var marshal = myTown.GetVariable&lt;Hero&gt;("AppointedMarshal");
        ///
        /// if (marshal != null &amp;&amp; marshal.IsAlive)
        ///     UseMarshal(myTown, marshal);
        /// </code>
        /// </example>
        /// <seealso cref="GetVariable(MBObjectBase, string)"/>
        /// <typeparam name="T">The type of the variable value.</typeparam>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        [return: MaybeNull]
        public static T GetVariable<T>(this MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
            {
                if (typeof(T) == typeof(char))
                    return instance.GetVariable<string>(@object, name) is { } str && str.Length == 1 && str[0] is T val ? val : default;

                return instance.GetVariable<T>(@object, name);
            }

            return default;
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
        /// <typeparam name="T">Type of variable's value.</typeparam>
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