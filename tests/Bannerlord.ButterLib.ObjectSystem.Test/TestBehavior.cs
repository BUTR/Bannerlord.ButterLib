using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.ObjectSystem.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.SaveSystem.Test
{
    internal class TestCampaignBehavior : CampaignBehaviorBase
    {
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
                if (!SetOrValidateHeroVars(h, true))
                {
                    _logger.LogTrace($"Errant Hero: {GetHeroTrace(h)}");
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
                if (!SetOrValidateHeroVars(h, false))
                {
                    _logger.LogTrace($"Errant Hero: {GetHeroTrace(h)}");
                    _logger.LogError("Halting execution due to error(s).");
                    _logger.LogWarningAndDisplay("<<<<<  TEST FAILED!  >>>>>");
                    return;
                }
            }

            _logger.LogWarningAndDisplay("<<<<<  TEST PASSED!  >>>>>");
        }

        private bool SetOrValidateHeroVars(Hero h, bool store)
        {
            int age = (int)h.Age;
            Hero? spouse = h.Spouse;
            Kingdom? kingdom = h.Clan?.Kingdom;
            Hero? topLiege = kingdom?.Ruler;
            List<Hero> children = h.Children!;
            char gender = h.IsFemale ? 'F' : 'M';
            Clan[]? fellowClans = kingdom?.Clans.Where(c => c != h.Clan).ToArray();

            bool stopAfterHero = false;

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

                if (h.GetVariable<TestHeroClass>("THC") != thc)
                {
                    _logger.LogError("ERROR: Set THC != Get");
                    stopAfterHero = true;
                }

                if (h.GetVariable<char>("Gender") != gender)
                {
                    _logger.LogError($"ERROR: Set Gender '{gender}' != Get");
                    stopAfterHero = true;
                }

                if (h.GetVariable<Clan[]>("FellowClans") != fellowClans)
                {
                    _logger.LogError("ERROR: Set fellowClans != Get");
                    stopAfterHero = true;
                }
            }
            else
            {
                TestHeroClass? gotThc = h.GetVariable<TestHeroClass>("THC");

                char gotGender = h.GetVariable<char>("Gender");
                Clan[]? gotFellowClans = h.GetVariable<Clan[]>("FellowClans");

                if (gotThc == null)
                {
                    stopAfterHero = true;
                    _logger.LogError("ERROR: gotThc null!");
                }
                else
                {
                    if (gotThc.Age != age)
                    {
                        stopAfterHero = true;
                        _logger.LogError($"ERROR: gotThc.Age ({gotThc.Age}) != age ({age})");
                    }

                    if (spouse != null && gotThc.Spouse != null && gotThc.Spouse != spouse)
                    {
                        stopAfterHero = true;
                        _logger.LogError($"ERROR: gotThc.Spouse ({gotThc.Spouse.Name}) != spouse ({spouse.Name})");
                    }

                    if (kingdom != null && gotThc.Kingdom != null && gotThc.Kingdom != kingdom)
                    {
                        stopAfterHero = true;
                        _logger.LogError($"ERROR: gotThc.Kingdom ({gotThc.Kingdom.Name}) != kingdom ({kingdom.Name})");
                    }

                    if (topLiege != null && gotThc.TopLiege != null && gotThc.TopLiege != topLiege)
                    {
                        stopAfterHero = true;
                        _logger.LogError($"ERROR: gotThc.TopLiege ({gotThc.TopLiege.Name}) != topLiege ({topLiege.Name})");
                    }

                    if (children != null && gotThc.Children == null)
                    {
                        stopAfterHero = true;
                        _logger.LogError("ERROR: gotThc.Children null!");
                        _logger.LogTrace($"children: [{string.Join(", ", children)}]");
                    }
                    else if (children != null && !children.SequenceEqual(gotThc.Children))
                    {
                        stopAfterHero = true;
                        _logger.LogError("ERROR: gotThc.Children sequence != children sequence!");
                        _logger.LogTrace($"gotThc.Children: [{string.Join(", ", gotThc.Children)}]");
                        _logger.LogTrace($"children: [{string.Join(", ", children)}]");
                    }
                }

                if (gotFellowClans == null && fellowClans != null)
                {
                    stopAfterHero = true;
                    _logger.LogError("ERROR: gotFellowClans null!");
                    _logger.LogTrace($"fellowClans: [{string.Join<Clan>(", ", fellowClans)}]");
                }
                else if (fellowClans != null && gotFellowClans != null && !fellowClans.SequenceEqual(gotFellowClans))
                {
                    stopAfterHero = true;
                    _logger.LogError("ERROR: gotFellowClans sequence != fellowClans sequence!");
                    _logger.LogTrace($"gotFellowClans: [{string.Join<Clan>(", ", gotFellowClans)}]");
                    _logger.LogTrace($"fellowClans: [{string.Join<Clan>(", ", fellowClans)}]");
                }

                if (gotGender != gender)
                {
                    stopAfterHero = true;
                    _logger.LogError($"ERROR: gotGender should be '{gender}' but gotGender == '{gotGender}'");
                }
            }

            return !stopAfterHero;
        }

        private string GetHeroTrace(Hero h)
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
