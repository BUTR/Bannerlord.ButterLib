using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DistanceMatrix;
using Bannerlord.ButterLib.ExceptionHandler;
using Bannerlord.ButterLib.Extensions;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.DistanceMatrix;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

using SymbolExtensions2 = HarmonyLib.BUTR.Extensions.SymbolExtensions2;
using TWCommon = TaleWorlds.Library.Common;

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
        private static bool MockedGetConfigPath(ref string __result)
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetModuleNames(ref string[] __result)
        {
            __result = Array.Empty<string>();
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
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigPath)));
            harmony.Patch(SymbolExtensions2.GetPropertyInfo(() => TWCommon.ConfigName!).GetMethod,
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigName)));
            var engineUtilitiesType = Type.GetType("TaleWorlds.Engine.Utilities, TaleWorlds.Engine", false);
            harmony.Patch(engineUtilitiesType?.GetMethod("GetModulesNames", BindingFlags.Public | BindingFlags.Static),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetModuleNames)));

            var subModule = new ButterLibSubModule();
            var subModuleWrapper = new MBSubModuleBaseWrapper(subModule);
            subModuleWrapper.SubModuleLoad();
            ExceptionHandlerSubSystem.Instance?.Disable();

            var services = ButterLibSubModule.Instance!.GetServices()!;
            services.AddScoped<CampaignDescriptor, CampaignDescriptorImplementation>();
            services.AddSingleton<ICampaignDescriptorStatic, CampaignDescriptorStaticImplementation>();
            services.AddScoped(typeof(DistanceMatrix<>), typeof(DistanceMatrixImplementation<>));
            services.AddSingleton<IDistanceMatrixStatic, DistanceMatrixStaticImplementation>();
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
            using var harmony = new HarmonyDisposable($"{nameof(DependencyInjectionTests)}.{nameof(CampaignDescriptor_Test)}");
            harmony.Patch(AccessTools.DeclaredPropertyGetter(typeof(CampaignTime), "CurrentTicks"),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedCurrentTicks)));
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => CharacterCode.CreateFrom((BasicCharacterObject) null!)),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedCreateFrom)));


            var hero = (Hero) FormatterServices.GetUninitializedObject(typeof(Hero));
#if e143 || e150 || e151 || e152 || e153 || e154 || e155 || e156 || e157 || e158 || e159
            AccessTools.Field(typeof(Hero), "Name").SetValue(hero, new TextObject("TestHero"));
#elif e1510 || e160
            AccessTools.Property(typeof(Hero), "Name").SetValue(hero, new TextObject("TestHero"));
#else
#error ConstGameVersionWithPrefix is not handled!
#endif
            AccessTools.Field(typeof(Hero), "_birthDay").SetValue(hero, CampaignTime.YearsFromNow(18));

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