﻿using Newtonsoft.Json;
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
            if ((_isContainerDelegate != null && _isContainerDelegate(type)) || (_isContainerDelegate == null && IsContainerFallback(type)))
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
                .Select(x => (FieldInfo: x, Attribute: x.GetCustomAttribute<SaveableFieldAttribute>(true)))
                .Where(t => t.Attribute != null);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(x => (PropertyInfo: x, Attribute: x.GetCustomAttribute<SaveablePropertyAttribute>(true)))
                .Where(t => t.Attribute != null);

            return fields.Select(tuple => new JsonProperty
            {
                PropertyName = $"M{tuple.Attribute.LocalSaveId}",
                PropertyType = tuple.FieldInfo.FieldType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(tuple.FieldInfo)
            }).Concat(properties.Select(tuple => new JsonProperty
            {
                PropertyName = $"M{tuple.Attribute.LocalSaveId}",
                PropertyType = tuple.PropertyInfo.PropertyType,
                Readable = true,
                Writable = true,
                ValueProvider = base.CreateMemberValueProvider(tuple.PropertyInfo)
            })).ToList();
        }
    }
}