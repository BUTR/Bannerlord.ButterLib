using Bannerlord.ButterLib.SaveSystem;

using HarmonyLib;

using Newtonsoft.Json;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Tests.SaveSystem
{
    public class JsonSerializationTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetConfigPath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        private static readonly AccessTools.FieldRef<FlattenedTroopRoster, Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>>? ElementDictionary =
            AccessTools.FieldRefAccess<FlattenedTroopRoster, Dictionary<UniqueTroopDescriptor, FlattenedTroopRosterElement>>("_elementDictionary");

        [SetUp]
        public void Setup()
        {
            //var harmony = new Harmony($"{nameof(JsonSerializationTests)}.{nameof(Setup)}");
            //harmony.Patch(SymbolExtensions.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
            //    prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigPath)));

            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var d1 = Directory.GetFiles(binFolder, "TaleWorlds*.dll");
            var d2 = Directory.GetFiles(binFolder, "StoryMode*.dll");
            var d3 = Directory.GetFiles(binFolder, "SandBox*.dll");

            foreach (var dll in d1.Concat(d2).Concat(d3))
                Assembly.LoadFile(dll);
        }

        [Test]
        public void CheckForUnsupportedSaveableClassTypes_Test()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.Auto
            };

            var saveableClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    IEnumerable<Type> Filter(IEnumerable<Type> types)
                    {
                        return types.Where(t =>
                        {
                            try
                            {
                                return t is { IsAbstract: false, IsGenericType: false } &&
                                       t.GetMembers().Any(m => m.GetCustomAttributes(true).Any(
                                           att => att.GetType() == typeof(SaveableFieldAttribute) ||
                                                  att.GetType() == typeof(SaveablePropertyAttribute)));
                            }
                            catch (FileNotFoundException)
                            {
                                return false;
                            }
                        });
                    }
                    try
                    {
                        return Filter(a.GetTypes());
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        return Filter(e.Types);
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .ToList();

            var saveableClassInstances = saveableClassTypes.ConvertAll(FormatterServices.GetUninitializedObject);

            var unsupportedTypes = new List<Type>();
            foreach (var saveableClassInstance in saveableClassInstances)
            {
                // Workaround
                if (saveableClassInstance is FlattenedTroopRoster flattenedTroopRoster && ElementDictionary is not null)
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
                TypeNameHandling = TypeNameHandling.Auto
            };

            var saveableClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    IEnumerable<Type> Filter(Type[] types)
                    {
                        return types.Where(t =>
                        {
                            try
                            {
                                return t is { IsAbstract: false, IsGenericType: false } &&
                                       t.GetMembers().Any(m => m.GetCustomAttributes(true).Any(
                                           att => att.GetType() == typeof(SaveableFieldAttribute) ||
                                                  att.GetType() == typeof(SaveablePropertyAttribute)));
                            }
                            catch (FileNotFoundException)
                            {
                                return false;
                            }
                        });
                    }
                    try
                    {
                        return Filter(a.GetTypes());
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        return Filter(e.Types);
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
                .Where(t => !t.IsAbstract && t is { IsGenericType: false, IsArray: false } && t != typeof(string))
                .Select(FormatterServices.GetUninitializedObject)
                .ToList();

            var unsupportedTypes = new List<Type>();
            foreach (var saveableMemberInstance in saveableMemberInstances)
            {
                // Workaround
                if (saveableMemberInstance is FlattenedTroopRoster flattenedTroopRoster && ElementDictionary is not null)
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