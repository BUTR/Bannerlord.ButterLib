using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public class TaleWorldsContractResolver : TaleWorldsContractResolverBase
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Container are an exception.
            if ((_isContainerDelegate is not null && _isContainerDelegate(type)) || (_isContainerDelegate is null && IsContainerFallback(type)))
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
                .Where(x => x.GetCustomAttribute<SaveableFieldAttribute>(true) is not null);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<SaveablePropertyAttribute>(true) is not null);

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