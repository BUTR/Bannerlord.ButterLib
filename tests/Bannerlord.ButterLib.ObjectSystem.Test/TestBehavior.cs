using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.SaveSystem.Test
{
    internal class TestCampaignBehavior : CampaignBehaviorBase
    {
        private bool _stopAfter = false;
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

        public override void SyncData(IDataStore dataStore) {}

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
            var age = (int)h.Age;
            var spouse = h.Spouse;
            var kingdom = h.Clan?.Kingdom;
            var topLiege = kingdom?.Ruler;
            var children = h.Children;
            var gender = h.IsFemale ? 'F' : 'M';
            var fellowClans = kingdom?.Clans.Where(c => c != h.Clan).ToArray();

            if (store)
            {
                var thc = new TestHeroClass
                {
                    Age = age,
                    Spouse = spouse,
                    Kingdom = kingdom,
                    TopLiege = topLiege,
                    Children = children
                };

                h.SetVariable("THC", thc);
                h.SetVariable("Gender", gender);
                h.SetVariable("FellowClans", fellowClans);

                if (h.TryGetVariable("THC", out TestHeroClass? thc2) && thc2 != thc)
                    Error("Set THC != Get THC");

                if (h.TryGetVariable("Gender", out char gender2) && gender2 != gender)
                    Error($"Set Gender '{gender}' != Get Gender '{ValOrDefault(gender2)}'");

                if (h.TryGetVariable("FellowClans", out Clan[]? fellowClans2) && fellowClans2 != fellowClans)
                    Error("Set FellowClans != Get FellowClans");
            }
            else
            {
                LoadVar(h, "THC", out TestHeroClass? gotThc);
                LoadVar(h, "Gender", out char gotGender);
                LoadVar(h, "FellowClans", out Clan[]? gotFellowClans);

                if (gotThc != null)
                {
                    TestValVar("gotThc.Age", age, gotThc.Age);
                    TestRefVar("gotThc.Spouse", spouse, gotThc.Spouse);
                    TestRefVar("gotThc.Kingdom", kingdom, gotThc.Kingdom);
                    TestRefVar("gotThc.TopLiege", topLiege, gotThc.TopLiege);
                    TestSeqVar("gotThc.Children", children, gotThc.Children);
                }

                TestSeqVar("gotFellowClans", fellowClans, gotFellowClans);
                TestValVar("gotGender", gender, gotGender);
            }
        }

        private void LoadVar<T>(MBObjectBase obj, string name, out T value)
        {
            if (obj.TryGetVariable(name, out T val))
            {
                value = val;
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

        private void TestRefVar<T>(string name, T want, T got) where T : class?
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
                _logger.LogTrace($"Got: [{string.Join(",", got)}]\n\tWant: [{string.Join(",", want)}]");
            }
        }

        private void Error(string msg)
        {
            _stopAfter = true;
            _logger.LogError("ERROR: " + msg);
        }


        private static string ValOrDefault<T>(T val) where T : struct => val.Equals(default(T)) ? "<default>" : val.ToString();

        private static string GetObjectTrace(MBObjectBase obj) => $"{obj.GetType().Name}[{obj.Id.InternalValue}]: {obj.GetName()}";

        private static string GetHeroTrace(Hero h)
        {
            var trace = h.Name.ToString();

            if (h.Clan != null)
                trace += $" {h.Clan.Name}";

            if (h.Clan?.Kingdom != null)
                trace += $" of {h.Clan.Kingdom.Name}";

            return trace;
        }

        [SaveableClass(1)]
        private class TestHeroClass
        {
            [SaveableProperty(1)]
            internal int Age { get; set; } = -1;

            [SaveableField(2)]
            public Hero? Spouse;

            [SaveableProperty(3)]
            internal Kingdom? Kingdom { get; set; }

            [SaveableField(4)]
            internal Hero? TopLiege;

            [SaveableField(5)]
            internal List<Hero> Children = new List<Hero>();
        }
    }
}
