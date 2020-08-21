using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using NUnit.Framework;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Tests
{
    public class ApplicationVersionUtilsTests
    {
        private static string TestAppVerstionStr = "e1.4.3.231432";
        private static ApplicationVersion TestAppVerstion = ApplicationVersion.FromString("e1.4.3.231432", ApplicationVersionGameType.Singleplayer);

        private static bool MockedGetVersionStr(ref string __result)
        {
            __result = TestAppVerstionStr;
            return false;
        }

        [SetUp]
        public void Setup()
        {
            var harmony = new Harmony($"{nameof(ApplicationVersionUtilsTests)}.{nameof(Setup)}");
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => Managed.GetVersionStr(null!)),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetVersionStr)));
        }

        [Test]
        public void GameVersionStr_Test()
        {
            var gameVersionStr = ApplicationVersionUtils.GameVersionStr();
            Assert.NotNull(gameVersionStr);
            Assert.AreEqual(TestAppVerstionStr, gameVersionStr);
        }

        [Test]
        public void GameVersion_Test()
        {
            var gameVersion = ApplicationVersionUtils.GameVersion();
            Assert.NotNull(gameVersion);
            Assert.AreEqual(TestAppVerstion, gameVersion);
        }

        [Test]
        public void TryParse_Test()
        {
            var result = ApplicationVersionUtils.TryParse(TestAppVerstionStr, out var gameVersion);
            Assert.True(result);
            Assert.AreEqual(TestAppVerstion, gameVersion);
        }
    }
}