using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public class TaleWorldsContractResolver : DefaultContractResolver
    {
        private delegate bool IsContainerDelegate(Type type);
        private static readonly IsContainerDelegate? _isContainerDelegate =
            AccessTools2.GetDelegate<IsContainerDelegate>(
                AccessTools.Method(
                    typeof(SaveableRootClassAttribute).Assembly.GetType("TaleWorlds.SaveSystem.TypeExtensions"), "IsContainer", new [] { typeof(Type) }));

        private static bool IsContainerFallback(Type type)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<, >))
                    return true;
                if (genericTypeDefinition == typeof(List<>))
                    return true;
                if (genericTypeDefinition == typeof(Queue<>))
                    return true;
            }
            else if (type.IsArray)
                return true;

            return false;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Container are an exception.
            if ((_isContainerDelegate != null && _isContainerDelegate(type) || (_isContainerDelegate == null && IsContainerFallback(type))))
                return base.CreateProperties(type, memberSerialization);

            // SaveableRootClassAttribute is not needed
            // SaveableInterfaceAttribute is not used by the game
            if (type.GetCustomAttributes(true).All(a =>
                a.GetType() != typeof(SaveableClassAttribute) && a.GetType() != typeof(SaveableStructAttribute) &&
                a.GetType() != typeof(SaveableEnumAttribute)))
            {
                return new List<JsonProperty>();
            }

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(true).Any(a =>
                    a.GetType() == typeof(SaveableFieldAttribute) || a.GetType() == typeof(SaveablePropertyAttribute)));
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(true).Any(a =>
                    a.GetType() == typeof(SaveableFieldAttribute) || a.GetType() == typeof(SaveablePropertyAttribute)));

            return fields.Select(p => new JsonProperty
            {
                PropertyName = p.Name,
                PropertyType = p.FieldType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(p)
            }).Concat(properties.Select(p => new JsonProperty
            {
                PropertyName = p.Name,
                PropertyType = p.PropertyType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(p)
            })).ToList();
        }
    }
}