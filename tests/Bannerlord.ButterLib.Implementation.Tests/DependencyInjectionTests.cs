using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.Tests
{
    public class DependencyInjectionTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedCreateFrom(ref CharacterCode __result)
        {
            __result = CharacterCode.CreateEmpty();
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedCurrentTicks(ref long __result)
        {
            __result = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetConfigsPath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetLoadedModules(ref List<ModuleInfo> __result)
        {
            __result = new List<ModuleInfo>();
            return false;
        }

        private static bool MockedGetSettlementAll(ref MBReadOnlyList<Settlement> __result)
        {
            var settlement = (Settlement) FormatterServices.GetUninitializedObject(typeof(Settlement));
            var village = (Village) FormatterServices.GetSafeUninitializedObject(typeof(Village));
            SymbolExtensions2.GetPropertyInfo((Settlement s) => s.Id).SetValue(settlement, new MBGUID(1));
            SymbolExtensions2.GetPropertyInfo((Settlement s) => s.IsInitialized).SetValue(settlement, true);
            SymbolExtensions2.GetFieldInfo((Settlement s) => s.Village).SetValue(settlement, village);

            __result = new MBReadOnlyList<Settlement>(new List<Settlement> { settlement });
            return false;
        }


        [SetUp]
        public void Setup()
        {
            var harmony = new Harmony($"{nameof(DependencyInjectionTests)}.{nameof(Setup)}");
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => Utilities.GetConfigsPath()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigsPath)));
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetLoadedModules)));

            var subModule = new ButterLibSubModule();
            var subModuleWrapper = new MBSubModuleBaseWrapper(subModule);
            subModuleWrapper.SubModuleLoad();

            var services = ButterLibSubModule.Instance!.GetServices()!;
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddSingleton<CampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton<DistanceMatrixStatic, DistanceMatrixStaticImplementation>();
            services.AddSingleton<ICampaignExtensions, CampaignExtensionsImplementation>();
            subModuleWrapper.BeforeInitialModuleScreenSetAsRoot();
        }

        [Test]
        public void ICampaignExtensions_Test()
        {
            var serviceProvider = ButterLibSubModule.Instance!.GetServiceProvider()!;

            var campaignExtensions = serviceProvider.GetRequiredService<ICampaignExtensions>();
            Assert.NotNull(campaignExtensions);
            Assert.True(campaignExtensions is CampaignExtensionsImplementation);
        }

        [Test]
        public void CampaignDescriptor_Test()
        {
            using var harmony = new HarmonyDisposable($"{nameof(DependencyInjectionTests)}.{nameof(DistanceMatrix_Test)}");
            harmony.Patch(AccessTools.DeclaredPropertyGetter(typeof(CampaignTime), "CurrentTicks"),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedCurrentTicks)));
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => CharacterCode.CreateFrom((BasicCharacterObject) null!)),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedCreateFrom)));


            var hero = (Hero) FormatterServices.GetUninitializedObject(typeof(Hero));
            SymbolExtensions2.GetFieldInfo((Hero h) => h.Name).SetValue(hero, new TextObject("TestHero"));
            SymbolExtensions2.GetPropertyInfo((Hero h) => h.BirthDay).SetValue(hero, CampaignTime.YearsFromNow(18));

            var campaignDescriptor = CampaignDescriptor.Create(hero);
            Assert.NotNull(campaignDescriptor);
            Assert.True(campaignDescriptor is CampaignDescriptorImplementation);
        }

        [Test]
        public void DistanceMatrix_Test()
        {
            using var harmony = new HarmonyDisposable($"{nameof(DependencyInjectionTests)}.{nameof(DistanceMatrix_Test)}");
            harmony.Patch(SymbolExtensions2.GetPropertyInfo(() => Settlement.All).GetMethod,
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetSettlementAll)));


            var serviceProvider = ButterLibSubModule.Instance!.GetServiceProvider()!;
            var scope = serviceProvider.CreateScope();

            var distanceMatrix = scope.ServiceProvider.GetRequiredService<DistanceMatrix<Settlement>>();
            Assert.NotNull(distanceMatrix);
            Assert.True(distanceMatrix.GetType().GetGenericTypeDefinition() == typeof(DistanceMatrixImplementation<>));
            Assert.AreEqual(distanceMatrix.GetType().GenericTypeArguments, new[] { typeof(Settlement) });


            var distanceMatrix1 = Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix.Create<Settlement>()!;
            Assert.NotNull(distanceMatrix1);
            Assert.True(distanceMatrix1.GetType().GetGenericTypeDefinition() == typeof(DistanceMatrixImplementation<>));
            Assert.AreEqual(distanceMatrix1.GetType().GenericTypeArguments, new[] { typeof(Settlement) });

            var distanceMatrix2 = Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix<Settlement>.Create()!;
            Assert.NotNull(distanceMatrix2);
            Assert.True(distanceMatrix2.GetType().GetGenericTypeDefinition() == typeof(DistanceMatrixImplementation<>));
            Assert.AreEqual(distanceMatrix2.GetType().GenericTypeArguments, new[] { typeof(Settlement) });
        }
    }
}