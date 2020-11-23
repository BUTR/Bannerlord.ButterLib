using Bannerlord.ButterLib.Assemblies;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Bannerlord.ButterLib.Common.Helpers;
using TaleWorlds.Engine;

using Path = System.IO.Path;
using TWCommon = TaleWorlds.Library.Common;

namespace Bannerlord.ButterLib.Tests.Assemblies
{
    public class AssemblyVerifierTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetBasePath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetConfigName(ref string __result)
        {
            __result = "Win64_Shipping_Client";
            return false;
        }

        private static string BasePath => AppDomain.CurrentDomain.BaseDirectory;
        private static string ButterLib => "Bannerlord.ButterLib.dll";
        private static string ButterLibModule => Path.Combine(BasePath, "Modules", "Bannerlord.ButterLib", "bin", TWCommon.ConfigName);
        private static string ButterLibFile => Path.Combine(ButterLibModule, ButterLib);
        private static string OldBaseLibrary => Path.Combine(BasePath, "_TestData", "BaseLibrary.dll");
        private static string NewBaseLibrary => Path.Combine(BasePath, "_TestData", "BaseLibrary.Incompatible.dll");
        private static string ImplementationLibrary => Path.Combine(BasePath, "_TestData", "ImplementationLibrary.dll");

        [SetUp]
        public void Setup()
        {
            var harmony = new Harmony($"{nameof(AssemblyVerifierTests)}.{nameof(Setup)}");
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => Utilities.GetBasePath()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetBasePath)));
            harmony.Patch(SymbolExtensions2.GetPropertyInfo(() => TWCommon.ConfigName).GetMethod,
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigName)));

            // Copy the ButterLib.dll which contains the AssemblyLoaderProxy.
            var directory = new DirectoryInfo(ButterLibModule);
            if (!directory.Exists)
                directory.Create();
            var file = new FileInfo(ButterLibFile);
            if (!file.Exists)
                File.Copy(Path.Combine(BasePath, ButterLib), Path.Combine(ButterLibModule, ButterLib));
        }

        [Test]
        public void LoadCompatibleAssembly_Test()
        {
            using var verifier = new AssemblyVerifier("Test");
            var loader = verifier.GetLoader(out var exception);
            Assert.Null(exception);
            Assert.NotNull(loader);
            loader!.LoadFile(OldBaseLibrary);
            var result = loader.LoadFileAndTest(ImplementationLibrary);
            Assert.True(result);
        }

        [Test]
        public void LoadIncompatibleAssembly_Test()
        {
            using var verifier = new AssemblyVerifier("Test");
            var loader = verifier.GetLoader(out var exception);
            Assert.Null(exception);
            Assert.NotNull(loader);
            loader!.LoadFile(NewBaseLibrary);
            var result = loader.LoadFileAndTest(ImplementationLibrary);
            Assert.False(result);
        }

        [Test]
        public void LoadWithOutExceptionIncompatibleAssembly_Test()
        {
            using var verifier = new AssemblyVerifier("Test");
            var loader = verifier.GetLoader(out var exception);
            Assert.Null(exception);
            Assert.NotNull(loader);
            loader!.LoadFile(NewBaseLibrary);
            var result = loader.LoadFileAndTest(ImplementationLibrary, out var testException);
            Assert.NotNull(testException);
            Assert.IsInstanceOf<ReflectionTypeLoadException>(testException);
            Assert.False(result);
        }
    }
}