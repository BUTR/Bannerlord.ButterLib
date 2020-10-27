using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.ObjectSystem.Test
{
    internal class TestCampaignBehavior : CampaignBehaviorBase
    {
        private bool _stopAfter;
        private readonly ILogger _logger;

        public TestCampaignBehavior()
        {
            _logger = this.GetServiceProvider()?.GetRequiredService<ILogger<TestCampaignBehavior>>()
                      ?? NullLogger<TestCampaignBehavior>.Instance;
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, OnBeforeSave);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        protected void OnBeforeSave()
        {
            _logger.LogInformation(">> STORING VARIABLES...");

            foreach (var h in Hero.All)
            {
                SetOrValidateHeroVars(h, true);

                if (_stopAfter)
                {
                    _logger.LogTrace($"Errant Hero: {GetObjectTrace(h)}");
                    _logger.LogError("Halting execution due to error(s).");
                    _logger.LogWarningAndDisplay("<<<<<  STORE FAILED!  >>>>>");
                    return;
                }
            }

            _logger.LogWarningAndDisplay("<<<<<  STORE PASSED!  >>>>>");
        }

        protected void OnGameLoaded(CampaignGameStarter starter)
        {
            _logger.LogInformation(">> LOADING VARIABLES...");

            foreach (var h in Hero.All)
            {
                SetOrValidateHeroVars(h, false);

                if (_stopAfter)
                {
                    _logger.LogTrace($"Errant Hero: {GetObjectTrace(h)}");
                    _logger.LogError("Halting execution due to error(s).");
                    _logger.LogWarningAndDisplay("<<<<<  TEST FAILED!  >>>>>");
                    return;
                }
            }

            _logger.LogWarningAndDisplay("<<<<<  TEST PASSED!  >>>>>");
        }

        private void SetOrValidateHeroVars(Hero h, bool store)
        {
            var gender = h.IsFemale ? 'F' : 'M';
            var fellowClans = h.Clan?.Kingdom?.Clans.Where(c => c != h.Clan).ToArray();
            var thc = new HeroTest(h);

            if (store)
            {
                h.SetVariable("Gender", gender);
                h.SetVariable("FellowClans", fellowClans);
                h.SetVariable("HeroTest", thc);

                if (h.TryGetVariable("HeroTest", out HeroTest? thc2) && thc2 != thc)
                    Error("Set != Get: HeroTest");

                if (h.TryGetVariable("Gender", out char gender2) && gender2 != gender)
                    Error($"Set != Get: Gender");

                if (h.TryGetVariable("FellowClans", out Clan[]? fellowClans2) && fellowClans2 != fellowClans)
                    Error("Set != Get: FellowClans");
            }
            else
            {
                LoadVar(h, "Gender", out char gotGender);
                LoadVar(h, "FellowClans", out Clan[]? gotFellowClans);
                LoadVar(h, "HeroTest", out HeroTest? gotHeroTest);

                TestSeqVar("FellowClans", fellowClans, gotFellowClans);
                TestValVar("Gender", gender, gotGender);
                gotHeroTest?.Test(this, "HeroTest", thc);
            }
        }

        [SaveableClass(1)]
        private class HeroTest
        {
            internal void Test(TestCampaignBehavior test, string name, HeroTest want)
            {
                var me = name;

                test.TestValVar($"{me}.{nameof(Age)}", want.Age, Age);
                test.TestValVar($"{me}.{nameof(BodyProperties)}", want.BodyProperties, BodyProperties);
                test.TestValVar($"{me}.{nameof(StateEnum)}", want.StateEnum, StateEnum);

                test.TestRefVar($"{me}.{nameof(Spouse)}", want.Spouse, Spouse);
                test.TestRefVar($"{me}.{nameof(Kingdom)}", want.Kingdom, Kingdom);
                test.TestRefVar($"{me}.{nameof(TopLiege)}", want.TopLiege, TopLiege);

                test.TestSeqVar($"{me}.{nameof(Children)}", want.Children, Children);
                test.TestSeqVar($"{me}.{nameof(ParentNames)}", want.ParentNames, ParentNames);

                // disabled for now, because we can't get reference-equality w/ non-MBObjectBase objects
                // test.TestSeqVar($"{me}.{nameof(EquippedItemUsage)}", want.EquippedItemUsage, EquippedItemUsage);

                test.TestRefVarByValue($"{me}.{nameof(NameLink)}", want.NameLink, NameLink);
                test.TestRefVarByValue($"{me}.{nameof(Name)}", want.Name, Name);
            }

            internal HeroTest(Hero h)
            {
                Hero = h;
                Age = (int)h.Age;
                Spouse = h.Spouse;
                Kingdom = h.Clan?.Kingdom;
                TopLiege = h.Clan?.Kingdom?.Ruler;
                Children = h.Children;
                BodyProperties = h.BodyProperties;
                NameLink = h.EncyclopediaLinkWithName;
                StateEnum = h.HeroState;
                Name = h.Name.ToString();
                NameUtf8 = Encoding.UTF8.GetBytes(Name);

                if (h.Father != null && h.Mother != null)
                    ParentNames = new[] { GetHeroTrace(h.Father), GetHeroTrace(h.Mother) };
                else if (h.Father != null && h.Mother == null)
                    ParentNames = new[] { GetHeroTrace(h.Father) };
                else if (h.Father == null && h.Mother != null)
                    ParentNames = new[] { GetHeroTrace(h.Mother) };
                else
                    ParentNames = Array.Empty<string>();

                EquippedItemUsage = new List<ItemTest>();

                foreach (var equipment in new[] { h.BattleEquipment, h.CivilianEquipment })
                {
                    for (EquipmentIndex i = 0; i < EquipmentIndex.NumEquipmentSetSlots; ++i)
                    {
                        var item = equipment[i].Item;

                        if (item == null)
                            continue;

                        if (item.TryGetVariable("ItemTest", out ItemTest? itemTest))
                            itemTest!.Users.Add(this);
                        else
                        {
                            itemTest = new ItemTest(item, this);
                            item.SetVariable("ItemTest", itemTest);
                        }

                        EquippedItemUsage.Add(itemTest);
                    }
                }
            }

            [SaveableField(0)]
            internal readonly Hero Hero; // internal readonly

            [SaveableProperty(1)]
            internal int Age { get; set; } // accessible setter

            [SaveableField(2)]
            readonly Hero? Spouse; // private readonly

            [SaveableProperty(3)]
            internal Kingdom? Kingdom { get; private set; } // private setter

            [SaveableField(4)]
            readonly Hero? TopLiege; // nullable MBObjectBase

            [SaveableField(5)]
            internal IList<Hero> Children; // generic interface where T : MBObjectBase

            [SaveableField(6)]
            readonly BodyProperties BodyProperties; // a struct

            [SaveableField(7)]
            readonly TextObject NameLink; // a complex ref type that's not MBObjectBase

            [SaveableField(8)]
            internal Hero.CharacterStates StateEnum; // an enum

            [SaveableField(9)]
            internal string Name; // a string

            [SaveableField(10)]
            internal byte[] NameUtf8; // a UTF-8 byte array

            [SaveableField(11)]
            internal string[] ParentNames; // a string array

            // This one should exercise reference loops.
            // 1. List<ItemTest> -> ItemTest elements -> HeroTest [ItemTest.FirstUser]
            // 2. List<ItemTest> -> ItemTest elements -> List<HeroTest> [ItemTest.Users] -> HeroTest elements
            [SaveableField(12)]
            internal List<ItemTest> EquippedItemUsage;
        }

        [SaveableClass(2)]
        private class ItemTest : IEquatable<ItemTest>
        {
            internal ItemTest(ItemObject item, HeroTest firstUser)
            {
                Item = item;
                Users = new List<HeroTest> { firstUser };
                FirstUser = firstUser;
            }

            [SaveableProperty(0)]
            internal ItemObject Item { get; set; }

            [SaveableField(1)]
            internal List<HeroTest> Users;

            [SaveableField(2)]
            internal HeroTest FirstUser;

            public bool Equals(ItemTest other) =>
                Item == other.Item &&
                Users.SequenceEqual(other.Users) &&
                FirstUser == other.FirstUser;

            public override bool Equals(object? obj) => obj is ItemTest it && Equals(it);

            public override int GetHashCode() => HashCode.Combine(Item, Users, FirstUser);
        }

        private void LoadVar<T>(MBObjectBase obj, string name, out T value)
        {
            if (obj.TryGetVariable(name, out T val))
            {
                value = val!;
                return;
            }

            value = default!;
            Error($"Variable \"{name}\" not found!");
        }

        private void TestValVar<T>(string name, T want, T got) where T : struct
        {
            if (!want.Equals(got))
                Error($"{name} is incorrect! Got: {ValOrDefault(got)} | Want: {ValOrDefault(want)}");
        }

        private void TestRefVar<T>(string name, T? want, T? got) where T : class
        {
            if (want == null && got == null)
                return;

            if (want != null && got == null)
                Error($"{name} is null!");
            else if (want == null && got != null)
                Error($"{name} is NOT null!");
            else if (want != got)
                Error($"{name} is incorrect! Got: {got} | Want: {want}");
        }

        private void TestRefVarByValue<T>(string name, T? want, T? got) where T : class
        {
            if (want == null && got == null)
                return;

            if (want != null && got == null)
                Error($"{name} is null!");
            else if (want == null && got != null)
                Error($"{name} is NOT null!");
            else if (!want!.Equals(got))
                Error($"{name} is incorrect! Got: {got} | Want: {want}");
        }

        private void TestSeqVar<T>(string name, IEnumerable<T>? want, IEnumerable<T>? got)
        {
            if (want == null && got == null)
                return;

            if (want != null && got == null)
            {
                Error($"{name} is null!");
                _logger.LogTrace($"\tWant: [{string.Join(",", want)}]");
            }
            else if (want == null && got != null)
                Error($"{name} is NOT null!");
            else if (!want.SequenceEqual(got))
            {
                Error($"{name} sequence is incorrect!");
                _logger.LogTrace($"\tGot:  [{string.Join(",", got)}]\n\tWant: [{string.Join(",", want)}]");
            }
        }

        private void Error(string msg)
        {
            _stopAfter = true;
            _logger.LogError("ERROR: " + msg);
        }

        private static string ValOrDefault<T>(T val) where T : struct => val.Equals(default(T)) ? "<default>" : val.ToString();

        private static string GetObjectTrace(MBObjectBase obj) =>
            $"{obj.GetType().FullName}[\"{obj.StringId}\": {obj.Id.InternalValue}]: {obj.GetName()}";

        private static string GetHeroTrace(Hero h)
            => $"{h.Name}{((h.Clan != null) ? $" {h.Clan.Name}" : "")}{((h.Clan?.Kingdom != null) ? $" of {h.Clan.Kingdom.Name}" : "")}";
    }
}
