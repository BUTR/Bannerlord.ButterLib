using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public static class MBObjectBaseExtensions
    {
        private static IMBObjectVariableStorage? _instance;

        private static IMBObjectVariableStorage? Instance =>
            _instance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMBObjectVariableStorage>();

        internal static void OnGameEnd() => _instance = null;

        /* Variables */

#nullable disable

        /// <summary>
        /// Get the value of the variable <paramref name="name"/> stored for <paramref name="object"/>
        /// as a <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// A value of type <typeparamref name="T"/> for the variable.
        /// If no such variable exists, a default-valued <typeparamref name="T"/>.
        /// </returns>
        /// <example>
        /// var marshal = myTown.GetVariable&lt;Hero&gt;("AppointedMarshal");
        ///
        /// if (marshal != null && marshal.IsAlive)
        ///     UseMarshalForSomething(marshal);
        /// </example>
        /// <typeparam name="T">The type of the variable value.</typeparam>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
        public static T GetVariable<T>(this MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
            {
                if (typeof(T) == typeof(char))
                {
                    var str = instance.GetVariable<string>(@object, name);
                    return !string.IsNullOrEmpty(str) && str[0] is T val ? val : default;
                }

                return instance.GetVariable<T>(@object, name);
            }

            return default;
        }

        public static T GetVariableAsJson<T>(this MBObjectBase @object, string name, JsonSerializerSettings settings = null)
        {
            if (Instance is { } instance && instance.GetVariable<string>(@object, name) is { } jsonData)
                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            return default;
        }

#nullable restore

        /// <summary>
        /// Set the value of the variable <paramref name="name"/> upon <paramref name="object"/>
        /// to <paramref name="data"/>.
        /// </summary>
        /// <example>
        /// myHero.SetVariable("Lovers", loverList);
        /// </example>
        /// <param name="object">A game object.</param>
        /// <param name="name">The variable's name.</param>
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

        public static void SetVariableAsJson<T>(this MBObjectBase @object, string name, T data, JsonSerializerSettings? settings = null)
        {
            if (Instance is { } instance)
                instance.SetVariable(@object, name, JsonConvert.SerializeObject(data, settings));
        }

        /// <summary>
        /// Remove the variable <paramref name="name"/> from <paramref name="object"/>.
        /// </summary>
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
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static bool HasFlag(MBObjectBase @object, string name) =>
            (Instance is { } instance) && instance.HasFlag(@object, name);

        /// <summary>Set the flag <paramref name="name"/> upon <paramref name="object"/>.</summary>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static void SetFlag(MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
                instance.SetFlag(@object, name);
        }

        /// <summary>Remove the flag <paramref name="name"/> from <paramref name="object"/>, if set.</summary>
        /// <param name="object">A game object.</param>
        /// <param name="name">A string flag.</param>
        public static void RemoveFlag(MBObjectBase @object, string name)
        {
            if (Instance is { } instance)
                instance.RemoveFlag(@object, name);
        }
    }
}