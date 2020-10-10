using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using NUnit.Framework;

using System.Runtime.CompilerServices;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Tests
{
    public class ApplicationVersionUtilsTests
    {
        private static readonly string TestAppVersionStr = "e1.4.3.231432";
        private static readonly ApplicationVersion TestAppVersion = ApplicationVersion.FromString("e1.4.3.231432", ApplicationVersionGameType.Singleplayer);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetVersionStr(ref string __result)
        {
            __result = TestAppVersionStr;
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
            Assert.AreEqual(TestAppVersionStr, gameVersionStr);
        }

        [Test]
        public void GameVersion_Test()
        {
            var gameVersion = ApplicationVersionUtils.GameVersion();
            Assert.NotNull(gameVersion);
            Assert.AreEqual(TestAppVersion, gameVersion);
        }

        [Test]
        public void TryParse_Test()
        {
            var result = ApplicationVersionUtils.TryParse(TestAppVersionStr, out var gameVersion);
            Assert.True(result);
            Assert.AreEqual(TestAppVersion, gameVersion);
        }
    }
}