using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem.Extensions
{
    /// <summary>
    /// <see cref="MBObjectBase"/> extension methods for Object Extension Data (dynamic variable / flag storage)
    /// </summary>
    public static class MBObjectBaseExtensions
    {
        private static IMBObjectExtensionDataStore? _instance;

        private static IMBObjectExtensionDataStore? Instance =>
            _instance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectExtensionDataStore>();

        internal static void OnGameEnd() => _instance = null;

        /* Variables */

        /// <summary>
        /// Check whether a variable <paramref name="name"/> is stored upon <paramref name="object"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if it exists, else <see langword="false"/>.
        /// </returns>
        /// <example>
        /// <code>
        ///  foreach (var skill in SkillObject.All)
        ///      if (!skill.HasVariable("CustomModifiers"))
        ///          RecalculateCustomModifiers(skill);
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        internal static bool HasVariable(this MBObjectBase @object, string name)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Variable name cannot be empty.", nameof(name));

            return Instance is { } instance && instance.HasVariable(@object, name);
        }

#nullable disable
        /// <summary>
        /// Gets the <paramref name="value"/> of the variable <paramref name="name"/> stored for <paramref name="object"/>.
        /// </summary>
        /// <returns>
        /// If the variable exists (including when set to <see langword="null"/>), returns <see langword="true"/>, and <paramref name="value"/> is set to its value.
        /// Otherwise, returns <see langword="false"/>, and <paramref name="value"/> is set to a default-valued <typeparamref name="T"/>.
        /// </returns>
        /// <example>
        /// <code>
        /// if (myTown.TryGetVariable("AppointedMarshal", out Hero marshal))
        ///     DeployMarshal(marshal);
        /// </code>
        /// </example>
        /// <typeparam name="T">The type of the variable's value.</typeparam>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        /// <param name="value">The variable value or <see langword="default"/> if nonexistent.</param>
        internal static bool TryGetVariable<T>(this MBObjectBase @object, string name, out T value)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Variable name cannot be empty.", nameof(name));

            if (typeof(T) == typeof(char))
                throw new NotSupportedException("TaleWorlds.SaveSystem cannot store variable-width basic type `char`. Consider using a string.");

            if (Instance is { } instance && instance.TryGetVariable(@object, name, out T val))
            {
                value = val;
                return true;
            }

            value = default;
            return false;
        }
#nullable restore

        /// <summary>
        ///  Set the value of the variable <paramref name="name"/> upon <paramref name="object"/>
        ///  to <paramref name="data"/>.
        ///  </summary>
        ///  <example>
        ///  <code>
        ///  // Herein, we're going to find potential lovers (non-spouse consorts) for a lover-less player Hero,
        ///  // and then we'll pick out a few and actually put them into the official Lovers variable of the player
        ///  // with SetVariable.
        ///  //
        ///  // We also assume there's already a HashSet&lt;Hero&gt;-type variable called AttractedTo which is
        ///  // already set on all heroes that are theoretically eligible to marry the player.
        ///
        ///  // Example is for setting a variable outright rather than modifying a retrieved one by reference, so this is only
        ///  // for lover-less players. Modifying existing variables directly by reference is shown at the end.
        ///  if (Hero.MainHero.HasVariable("Lovers"))
        ///      return;
        ///
        ///  // Find possible lovers for the player Hero
        ///  var possibleLovers = Hero.All.Where(h =&gt; h.IsAlive &amp;&amp; // Pretty gross otherwise
        ///                                           Math.Abs(Hero.MainHero.Age - h.Age) &lt; 15f &amp;&amp; // Less than 15 year age gap
        ///                                           // By game rules, whether the character could legally marry the player someday:
        ///                                           Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, h) &amp;&amp;
        ///                                           // Only those that are attracted to the player (uses another variable, a set of heroes):
        ///                                           h.TryGetVariable("AttractedTo", out HashSet&lt;Hero&gt; attractionSet) &amp;&amp;
        ///                                           attractionSet.Contains(Hero.MainHero) &amp;&amp;
        ///                                           // The attraction must be mutual:
        ///                                           Hero.MainHero.TryGetVariable("AttractedTo", out HashSet&lt;Hero&gt; playerAttractionSet) &amp;&amp;
        ///                                           playerAttractionSet.Contains(h)
        ///                                           );
        ///
        ///  // Take [up to] the 5 youngest of the possible lovers (results in a List&lt;Hero&gt;)
        ///  var lovers = possibleLovers.OrderBy(h =&gt; h.Age).Take(5).ToList();
        ///
        ///  // Set these lovers into the player's variable named "Lovers"
        ///  Hero.MainHero.SetVariable("Lovers", lovers);
        ///
        ///  // Being lovers goes both ways (i.e., if A is B's lover, then B is A's lover):
        ///  foreach (var lover in lovers)
        ///  {
        ///      // Append the player to any potential preexisting lovers that this new lover may have,
        ///      // else set a new Lovers variable upon them which contains only the player.
        ///
        ///      // NOTE: This demonstrates modification of an existing variable by reference, which
        ///      //       often bypasses the need to actually use SetVariable:
        ///      if (lover.TryGetVariable("Lovers", out List&lt;Hero&gt; theirLovers))
        ///          theirLovers.Add(Hero.MainHero); // Modify existing variable by reference
        ///      else
        ///          lover.SetVariable("Lovers", new List&lt;Hero&gt; { Hero.MainHero }); // If var doesn't exist, forced to SetVariable
        ///  }
        ///  </code>
        ///  </example>
        ///  <typeparam name="T">Type of the variable's value.</typeparam>
        ///  <param name="object">A game object.</param>
        ///  <param name="name">The variable's name.</param>
        ///  <param name="data">The variable's value.</param>
        internal static void SetVariable<T>(this MBObjectBase @object, string name, T data)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Variable name cannot be empty.", nameof(name));

            if (typeof(T) == typeof(char))
                throw new NotSupportedException("TaleWorlds.SaveSystem cannot store type 'char'. Consider using a string.");

            if (Instance is { } instance)
                instance.SetVariable(@object, name, data);
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
        internal static void RemoveVariable(this MBObjectBase @object, string name)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Variable name cannot be empty.", nameof(name));

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
        public static bool HasFlag(this MBObjectBase @object, string name)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Flag name cannot be empty.", nameof(name));

            return Instance is { } instance && instance.HasFlag(@object, name);
        }

        /// <summary>Set the flag <paramref name="name"/> upon <paramref name="object"/>.</summary>
        /// <example>
        /// <code>
        /// myKingdom.SetFlag("IsRevoltFaction");
        /// </code>
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static void SetFlag(this MBObjectBase @object, string name)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Flag name cannot be empty.", nameof(name));

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
        public static void RemoveFlag(this MBObjectBase @object, string name)
        {
            if (name is null!)
                throw new ArgumentNullException(nameof(name));
            else if (name.Length == 0)
                throw new ArgumentException("Flag name cannot be empty.", nameof(name));

            if (Instance is { } instance)
                instance.RemoveFlag(@object, name);
        }
    }
}