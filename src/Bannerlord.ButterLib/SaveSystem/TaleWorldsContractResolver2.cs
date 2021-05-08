using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public class TaleWorldsContractResolver2 : TaleWorldsContractResolverBase
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Container are an exception.
            if ((_isContainerDelegate is not null && _isContainerDelegate(type)) || (_isContainerDelegate is null && IsContainerFallback(type)))
                return base.CreateProperties(type, memberSerialization);

            // SaveableRootClassAttribute is not needed
            // SaveableInterfaceAttribute is not used by the game
            if (type.GetMembers().All(m => m.GetCustomAttributes(true).Any(
                att => att.GetType() != typeof(SaveableFieldAttribute) ||
                       att.GetType() != typeof(SaveablePropertyAttribute))))
            {
                return new List<JsonProperty>();
            }

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(x => (FieldInfo: x, Attribute: x.GetCustomAttribute<SaveableFieldAttribute>(true)))
                .Where(t => t.Attribute is not null);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(x => (PropertyInfo: x, Attribute: x.GetCustomAttribute<SaveablePropertyAttribute>(true)))
                .Where(t => t.Attribute is not null);

            return fields.Select(tuple => new JsonProperty
            {
                PropertyName = $"M{tuple.Attribute!.LocalSaveId}",
                PropertyType = tuple.FieldInfo.FieldType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(tuple.FieldInfo)
            }).Concat(properties.Select(tuple => new JsonProperty
            {
                PropertyName = $"M{tuple.Attribute!.LocalSaveId}",
                PropertyType = tuple.PropertyInfo.PropertyType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(tuple.PropertyInfo)
            })).ToList();
        }
    }
}