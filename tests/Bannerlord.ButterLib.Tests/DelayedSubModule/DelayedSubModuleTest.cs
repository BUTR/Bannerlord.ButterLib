using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule
{
    public class DelayedSubModuleTest
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetModuleInfo(Type type, ref object? __result)
        {
            if (type == typeof(TestSubModuleCaller))
            {
                __result = TestHelper.ModuleInfoCaller1;
            }
            if (type == typeof(TestSubModuleTarget))
            {
                __result = TestHelper.ModuleInfoTarget1;
            }

            return false;
        }

        [SetUp]
        public void Setup()
        {
            var harmony = new Harmony($"{nameof(DelayedSubModuleTest)}.{nameof(Setup)}");
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => ModuleInfoHelper.GetModuleInfo(null!)),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetModuleInfo)));

            DelayedSubModuleManager.Register<TestSubModuleCaller>();
            DelayedSubModuleManager.Register<TestSubModuleTarget>();
        }

        [Test]
        public void SubscribeBeforeTargetLoad_Test()
        {
            // Initialization
            //ModuleInfoHelper.LoadedModules = new List<ModuleInfo> { TestHelper.ModuleInfoCaller1, TestHelper.ModuleInfoTarget1 };
            ModuleInfoHelper.LoadedExtendedModules = new List<ExtendedModuleInfo> { TestHelper.ModuleInfoCaller, TestHelper.ModuleInfoTarget };
            ModuleInfoHelper.PastInitialization = true;
            // Initialization

            // Because the Target SubModule loads after Caller, Subscribe should not instacall the delegate
            var delegateWasCalled = false;
            void Delegate(object? sender, SubscriptionEventArgs e) => delegateWasCalled = true;
            DelayedSubModuleManager.Subscribe<TestSubModuleTarget, TestSubModuleCaller>("OnSubModuleLoad", SubscriptionType.AfterMethod, Delegate);
            Assert.False(delegateWasCalled);
        }

        [Test]
        public void SubscribeBeforeTargetLoad_CallTargetManually_Test()
        {
            // Initialization
            //ModuleInfoHelper.LoadedModules = new List<ModuleInfo> { TestHelper.ModuleInfoCaller1, TestHelper.ModuleInfoTarget1 };
            ModuleInfoHelper.LoadedExtendedModules = new List<ExtendedModuleInfo> { TestHelper.ModuleInfoCaller, TestHelper.ModuleInfoTarget };
            ModuleInfoHelper.PastInitialization = true;
            // Initialization

            // Because the Target SubModule loads after Caller, Subscribe should not instacall the delegate
            var delegateWasCalled = false;
            void Delegate(object? sender, SubscriptionEventArgs e) => delegateWasCalled = true;
            DelayedSubModuleManager.Subscribe<TestSubModuleTarget, TestSubModuleCaller>("OnSubModuleLoad", SubscriptionType.AfterMethod, Delegate);
            Assert.False(delegateWasCalled);

            // Manually trigger OnSubModuleLoad and confirm that the delegate is now called
            var module = new MBSubModuleBaseWrapper(new TestSubModuleTarget());
            module.SubModuleLoad();
            Assert.True(delegateWasCalled);
        }

        [Test]
        public void SubscribeAfterTargetLoad_Test()
        {
            // Initialization
            //ModuleInfoHelper.LoadedModules = new List<ModuleInfo> { TestHelper.ModuleInfoTarget1, TestHelper.ModuleInfoCaller1 };
            ModuleInfoHelper.LoadedExtendedModules = new List<ExtendedModuleInfo> { TestHelper.ModuleInfoTarget, TestHelper.ModuleInfoCaller };
            ModuleInfoHelper.PastInitialization = true;
            // Initialization

            // Because the Target SubModule loads before Caller, Subscribe should instacall the delegate
            var delegateWasCalled = false;
            void Delegate(object? sender, SubscriptionEventArgs e) => delegateWasCalled = true;
            DelayedSubModuleManager.Subscribe<TestSubModuleTarget, TestSubModuleCaller>("OnSubModuleLoad", SubscriptionType.AfterMethod, Delegate);
            Assert.True(delegateWasCalled);
        }
    }
}