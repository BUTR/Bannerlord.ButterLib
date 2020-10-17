using Bannerlord.ButterLib.SaveSystem;

using Newtonsoft.Json;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.Tests.SaveSystem
{
    public class JsonSerializationTests
    {
        private static readonly FieldRef<FlattenedTroopRoster, Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>>? ElementDictionary =
            FieldRefAccess<FlattenedTroopRoster, Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>>("_elementDictionary");

        [SetUp]
        public void Setup()
        {
            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var d1 = Directory.GetFiles(binFolder, "TaleWorlds*.dll");
            var d2 = Directory.GetFiles(binFolder, "StoryMode*.dll");
            var d3 = Directory.GetFiles(binFolder, "SandBox*.dll");

            foreach (string dll in d1.Concat(d2).Concat(d3))
                Assembly.LoadFile(dll);
        }

        [Test]
        public void CheckForUnsupportedSaveableClassTypes_Test()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.All
            };

            var saveableClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes().Where(t => !t.IsAbstract && !t.IsGenericType && t.GetCustomAttribute<SaveableClassAttribute>(true) != null);
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .ToList();

            var saveableClassInstances = saveableClassTypes.Select(FormatterServices.GetUninitializedObject).ToList();

            var unsupportedTypes = new List<Type>();
            foreach (var saveableClassInstance in saveableClassInstances)
            {
                // Workaround
                if (saveableClassInstance is FlattenedTroopRoster flattenedTroopRoster && ElementDictionary != null)
                    ElementDictionary(flattenedTroopRoster) = new Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>();

                try
                {
                    JsonConvert.SerializeObject(saveableClassInstance, settings);
                }
                catch (Exception e)
                {
                    TestContext.Out.WriteLine($"{saveableClassInstance.GetType()}: {e}");
                    unsupportedTypes.Add(saveableClassInstance.GetType());
                }
            }
            Assert.True(unsupportedTypes.Count == 0, "Unsupported types detected");
        }

        [Test]
        public void CheckForUnsupportedSaveableMembers_Test()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.All
            };

            var saveableClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes().Where(t => !t.IsAbstract && !t.IsGenericType && t.GetCustomAttribute<SaveableClassAttribute>(true) != null);
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .ToList();

            var saveableMembers = saveableClassTypes
                .SelectMany(t =>
                {
                    var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x => x.GetCustomAttributes(true).Any(a =>
                            a.GetType() == typeof(SaveableFieldAttribute) || a.GetType() == typeof(SaveablePropertyAttribute)));
                    var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x => x.GetCustomAttributes(true).Any(a =>
                            a.GetType() == typeof(SaveableFieldAttribute) || a.GetType() == typeof(SaveablePropertyAttribute)));
                    return fields.Select(f => f.FieldType).Concat(properties.Select(p => p.PropertyType));
                })
                .Distinct()
                .ToList();

            var saveableMemberInstances = saveableMembers
                .Where(t => !t.IsAbstract && !t.IsGenericType && !t.IsArray && t != typeof(string))
                .Select(FormatterServices.GetUninitializedObject)
                .ToList();

            var unsupportedTypes = new List<Type>();
            foreach (var saveableMemberInstance in saveableMemberInstances)
            {
                // Workaround
                if (saveableMemberInstance is FlattenedTroopRoster flattenedTroopRoster && ElementDictionary != null)
                    ElementDictionary(flattenedTroopRoster) = new Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>();

                try
                {
                    JsonConvert.SerializeObject(saveableMemberInstance, settings);
                }
                catch (Exception e)
                {
                    TestContext.Out.WriteLine($"{saveableMemberInstance.GetType()}: {e}");
                    unsupportedTypes.Add(saveableMemberInstance.GetType());
                }
            }
            Assert.True(unsupportedTypes.Count == 0, "Unsupported types detected");
        }
    }
}