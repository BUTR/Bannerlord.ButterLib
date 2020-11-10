﻿using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.ObjectSystem.Test
{
    internal class TestCampaignBehavior : CampaignBehaviorBase
    {
        private const bool SimpleMode = true;
        private bool _hitError;
        private readonly ILogger _log;

        public TestCampaignBehavior()
        {
            _log = this.GetServiceProvider()?.GetRequiredService<ILogger<TestCampaignBehavior>>()
                   ?? NullLogger<TestCampaignBehavior>.Instance;

            _log.LogTrace($"{nameof(TestCampaignBehavior)} initialized.");
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, OnBeforeSave);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnBeforeSave()
        {
            _log.LogInformation(">> STORING VARIABLES...");

            if (SimpleMode)
            {
                OnBeforeSaveSimple();
                return;
            }

#pragma warning disable CS0162 // Unreachable code detected
            foreach (var h in Hero.All)
#pragma warning restore CS0162 // Unreachable code detected
            {
                SetOrValidateHeroVars(h, true);

                if (_hitError)
                {
                    _log.LogTrace($"Errant Hero: {GetHeroTrace(h)} / Object: {GetObjectTrace(h)}");
                    _log.LogError("Halting execution due to error(s).");
                    break;
                }
            }

            LogAndDisplayOutcome(true);
        }

        private void OnGameLoaded(CampaignGameStarter starter)
        {
            _log.LogInformation(">> LOADING VARIABLES...");

            if (SimpleMode)
            {
                OnGameLoadedSimple();
                return;
            }

#pragma warning disable CS0162 // Unreachable code detected
            foreach (var h in Hero.All)
#pragma warning restore CS0162 // Unreachable code detected
            {
                SetOrValidateHeroVars(h, false);

                if (_hitError)
                {
                    _log.LogTrace($"Errant Hero: {GetHeroTrace(h)} / Object: {GetObjectTrace(h)}");
                    _log.LogError("Halting execution due to error(s).");
                    break;
                }
            }

            LogAndDisplayOutcome(false);
        }

        private const string SimpleId = "vlandia";
        private const string SimpleKey = "SimpleMode.StringId";

        private void OnBeforeSaveSimple()
        {
            //var realm = MBObjectManager.Instance.GetObject<Kingdom>(SimpleId);

            //if (realm is null)
            //    Error($"Kingdom object \"{SimpleId}\" not found!");
            //else
            //{
            //    realm.SetVariable(SimpleKey, SimpleId);

            //    if (LoadObjectVar(realm, SimpleKey, out string? id2) && id2 is { } && !SimpleId.Equals(id2))
            //        Error($"Incorrect value for variable \"{SimpleKey}\" immediately after setting it. Got value \"{id2}\".");
            //}

            var h = Hero.MainHero;
            h.SetFlag("IsPlayer");

            if (!h.HasFlag("IsPlayer"))
                Error("Set IsPlayer flag on MainHero but didn't have it immediately afterward!");

            LogAndDisplayOutcome(true);
        }

        private void OnGameLoadedSimple()
        {
            //var realm = MBObjectManager.Instance.GetObject<Kingdom>(SimpleId);

            //if (realm is null)
            //    Error($"Kingdom object \"{SimpleId}\" not found!");
            //else if (LoadObjectVar(realm, SimpleKey, out string? id2) && id2 is { } && !SimpleId.Equals(id2))
            //    Error($"Incorrect value for variable \"{SimpleKey}\" stored upon object {GetObjectTrace(realm)}. Got value \"{id2}\".");

            if (!Hero.MainHero.HasFlag("IsPlayer"))
                Error("IsPlayer flag not set upon MainHero!");

            LogAndDisplayOutcome(false);
        }

        private void LogAndDisplayOutcome(bool store, bool? passed = null)
        {
            var ok = passed ?? !_hitError;
            var phase = store ? "STORAGE" : "LOADING";
            var outcome = ok ? "PASSED" : "FAILED";
            var simple = SimpleMode ? "SIMPLE-" : string.Empty;
            var output = $"  <<  {phase} {simple}TEST {outcome}!  >>";

            if (ok)
                _log.LogInformationAndDisplay(output);
            else
                _log.LogErrorAndDisplay(output);
        }

        private void SetOrValidateHeroVars(Hero h, bool store)
        {
            var gender = h.IsFemale ? 'F' : 'M';
            var fellowClans = h.Clan?.Kingdom?.Clans.Where(c => c != h.Clan).ToArray();
            var ht = new HeroTest(h);

            if (store)
            {
                h.SetVariable("Gender", gender);
                h.SetVariable("FellowClans", fellowClans);
                h.SetVariable("HeroTest", ht);

                if (h.TryGetVariable("HeroTest", out HeroTest? ht2) && ht2 != ht)
                    Error("Set != Get: HeroTest");

                if (h.TryGetVariable("Gender", out char gender2) && gender2 != gender)
                    Error("Set != Get: Gender");

                if (h.TryGetVariable("FellowClans", out Clan[]? fellowClans2) && fellowClans2 != fellowClans)
                    Error("Set != Get: FellowClans");
            }
            else
            {
                LoadObjectVar(h, "Gender", out char gotGender);
                LoadObjectVar(h, "FellowClans", out Clan[]? gotFellowClans);
                LoadObjectVar(h, "HeroTest", out HeroTest? gotHeroTest);

                TestSeqVar("FellowClans", fellowClans, gotFellowClans);
                TestValVar("Gender", gender, gotGender);
                gotHeroTest?.Test(this, "HeroTest", ht);
            }
        }

        //public class SavedTypeDefiner : SaveableCampaignBehaviorTypeDefiner
        //{
        //    public SavedTypeDefiner() : base(222_444_600) { }

        //    protected override void DefineClassTypes()
        //    {
        //        AddClassDefinition(typeof(HeroTest), 1);
        //        //AddClassDefinition(typeof(WrappedDictionary), 3);
        //    }

        //    protected override void DefineEnumTypes() => AddEnumDefinition(typeof(ElectionCandidate), 2);

        //    protected override void DefineContainerDefinitions()
        //    {
        //        // Apparently this throws a duplicate key exception in one of SaveSystem.DefinitionContext's type dictionaries, because !SaveSystem.IsUserFriendly():
        //        // ConstructContainerDefinition(typeof(Clan[]));

        //        // This has never been uncommented but would've done the same:
        //        // ConstructContainerDefinition(typeof(Dictionary<string, string>));
        //    }
        //}

        //[SaveableClass(1)]
        private class HeroTest
        {
            internal void Test(TestCampaignBehavior test, string name, HeroTest want)
            {
                var me = name;

                test.TestValVar($"{me}.{nameof(Age)}", want.Age, Age);
                test.TestValVar($"{me}.{nameof(BodyProperties)}", want.BodyProperties, BodyProperties);
                test.TestValVar($"{me}.{nameof(StateEnum)}", want.StateEnum, StateEnum);
                test.TestValVar($"{me}.{nameof(CustomEnum)}", want.CustomEnum, CustomEnum);

                test.TestRefVar($"{me}.{nameof(Spouse)}", want.Spouse, Spouse);
                test.TestRefVar($"{me}.{nameof(Kingdom)}", want.Kingdom, Kingdom);

                // Disabled for now, because we can't get reference-equality w/ non-MBObjectBase objects:
                // test.TestSeqVar($"{me}.{nameof(EquippedItemUsage)}", want.EquippedItemUsage, EquippedItemUsage);

                test.TestRefVarByValue($"{me}.{nameof(NameLink)}", want.NameLink, NameLink);
            }

            // only explicit constructor, parameter name doesn't match any fields/props but type does
            internal HeroTest(Hero h)
            {
                Hero = h;
                Age = (int)h.Age;
                Spouse = h.Spouse;
                Kingdom = h.Clan?.Kingdom;
                BodyProperties = h.BodyProperties;
                NameLink = h.EncyclopediaLinkWithName;
                StateEnum = h.HeroState;
                CustomEnum = (ElectionCandidate)(h.Id.InternalValue % 3);

                #region DisabledCircularReferenceTest
                //EquippedItemUsage = new List<ItemTest>();

                //foreach (var equipment in new[] { h.BattleEquipment, h.CivilianEquipment })
                //{
                //    for (EquipmentIndex i = 0; i < EquipmentIndex.NumEquipmentSetSlots; ++i)
                //    {
                //        var item = equipment[i].Item;

                //        if (item is null)
                //            continue;

                //        if (item.TryGetVariable("ItemTest", out ItemTest? itemTest))
                //            itemTest!.Users.Add(this);
                //        else
                //        {
                //            itemTest = new ItemTest(item, this);
                //            item.SetVariable("ItemTest", itemTest);
                //        }

                //        EquippedItemUsage.Add(itemTest);
                //    }
                //}
                #endregion DisabledCircularReferenceTest
            }

            [SaveableField(0)]
            internal readonly Hero Hero; // internal readonly ref

            [SaveableProperty(1)]
            internal int Age { get; set; } // uh, if THIS doesn't work...

            [SaveableField(2)]
            private readonly Hero? Spouse; // private readonly ref

            [SaveableProperty(3)]
            internal Kingdom? Kingdom { get; private set; } // private setter

            [SaveableField(4)]
            internal readonly BodyProperties BodyProperties; // a readonly struct

            [SaveableField(5)]
            internal TextObject NameLink; // a complex ref type that's not MBObjectBase

            [SaveableField(6)]
            internal Hero.CharacterStates StateEnum; // a game enum

            [SaveableField(7)]
            internal ElectionCandidate CustomEnum; // custom enum

            #region DisabledCircularReferenceTest
            // This one should exercise reference loops.
            // 1. List<ItemTest> -> ItemTest elements -> HeroTest [ItemTest.FirstUser]
            // 2. List<ItemTest> -> ItemTest elements -> List<HeroTest> [ItemTest.Users] -> HeroTest elements
            //[SaveableField(12)]
            //internal List<ItemTest> EquippedItemUsage;
            #endregion DisabledCircularReferenceTest
        }

        //[SaveableEnum(2)]
        private enum ElectionCandidate { Trump = 0, Biden = 1, Kanye = 2 };

        //[SaveableClass(3)]
        //public class WrappedDictionary
        //{
        //    [SaveableProperty(0)]
        //    public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();
        //}

        #region DisabledCircularReferenceTest
        //[SaveableClass(4)]
        //private class ItemTest : IEquatable<ItemTest>
        //{
        //    internal ItemTest(ItemObject item, HeroTest firstUser)
        //    {
        //        Item = item;
        //        Users = new List<HeroTest> { firstUser };
        //        FirstUser = firstUser;
        //    }

        //    [SaveableProperty(0)]
        //    internal ItemObject Item { get; set; }

        //    [SaveableField(1)]
        //    internal List<HeroTest> Users;

        //    [SaveableField(2)]
        //    internal HeroTest FirstUser;

        //    public bool Equals(ItemTest other) =>
        //        Item == other.Item &&
        //        Users.SequenceEqual(other.Users) &&
        //        FirstUser == other.FirstUser;

        //    public override bool Equals(object? obj) => obj is ItemTest it && Equals(it);

        //    public override int GetHashCode() => HashCode.Combine(Item, Users, FirstUser);
        //}
        #endregion DisabledCircularReferenceTest

        private bool LoadObjectVar<T>(MBObjectBase obj, string name, out T value)
        {
            if (obj.TryGetVariable<T>(name, out var val))
            {
                value = data!;
                return true;
            }

            value = default!;
            Error($"Object's variable \"{name}\" not found! Object: {GetObjectTrace(obj)}");
            return false;
        }

        private bool TestValVar<T>(string id, T want, T got) where T : struct =>
            Assert(want.Equals(got), "WRONG VALUE!", id, $"GOT: {ValueToStringOrDefault(got)}  //  WANT: {ValueToStringOrDefault(want)}");

        private bool TestRefVarNullCases<T>(string id, T? want, T? got) where T : class => AllNull(want, got) || NorNull(want, got) ||
            AssertNot(OnlyGotIsNull(want, got), "NULL!", id) && AssertNot(OnlyGotIsNotNull(want, got), "NOT NULL!", id);

        private bool TestRefVar<T>(string id, T? want, T? got) where T : class => TestRefVarNullCases(id, want, got) &&
            Assert(NorNull(want, got) && ReferenceEquals(want, got), "REF-INEQUALITY!", id, $"GOT: {got}  //  WANT: {want}");

        private bool TestRefVarByValue<T>(string id, T? want, T? got) where T : class => TestRefVarNullCases(id, want, got) &&
            Assert(NorNull(want, got) && want!.Equals(got), "WRONG VALUE!", id, $"GOT: {got}  //  WANT: {want}");

        private bool TestSeqVar<T>(string id, IEnumerable<T>? want, IEnumerable<T>? got) => TestRefVarNullCases(id, want, got) &&
            Assert(NorNull(want, got) && want.SequenceEqual(got), "WRONG SEQUENCE!", id, $"GOT: [{string.Join(" | ", got)}]  //  WANT: [{string.Join(" | ", want)}]");

        private bool AssertNot(bool condition, string msg, string? id = null, string? trace = null) => !condition || Error(msg, id, trace);
        private bool Assert(bool condition, string msg, string? id = null, string? trace = null) => condition || Error(msg, id, trace);

        private bool Error(string msg, string? id = null, string? trace = null)
        {
            _log.LogError($"ERROR: {(id is null ? msg : $"{id}: {msg}")}");

            if (trace != null)
                _log.LogTrace(trace);

            _hitError = true;
            return false;
        }

        private static bool NorNull<T>(T? a, T? b) where T : class => !(a is null || b is null);
        private static bool AllNull<T>(T? a, T? b) where T : class => a is null && b is null;
        private static bool OnlyGotIsNull<T>(T? want, T? got) where T : class => want is { } && got is null;
        private static bool OnlyGotIsNotNull<T>(T? want, T? got) where T : class => want is null && got is { };

        private static string ValueToStringOrDefault<T>(T val) where T : struct => val.Equals(default(T)) ? $"default({typeof(T).Name})" : val.ToString() ?? string.Empty;

        private static string GetObjectTrace(MBObjectBase obj) =>
            $"{obj.GetType().FullName}[\"{obj.StringId}\": {obj.Id.InternalValue}]: {obj.GetName()}";

        private static string GetHeroTrace(Hero h)
            => $"{h.Name}{((h.Clan is not null) ? $" {h.Clan.Name}" : "")}{((h.Clan?.Kingdom is not null) ? $" of {h.Clan.Kingdom.Name}" : "")}";
    }
}
