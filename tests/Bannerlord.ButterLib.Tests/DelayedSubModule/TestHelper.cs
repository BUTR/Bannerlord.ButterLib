using Bannerlord.ModuleManager;

using HarmonyLib.BUTR.Extensions;

using System;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule
{
    internal static class TestHelper
    {
        public static readonly Type? ModuleInfoType = AccessTools2.TypeByName("TaleWorlds.ModuleManager.ModuleInfo");

        private static ModuleInfoExtended? _moduleInfoCaller;
        public static ModuleInfoExtended ModuleInfoCaller
        {
            get
            {
                if (_moduleInfoCaller is null)
                {
                    _moduleInfoCaller = new ModuleInfoExtended();
                    AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Id")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                    AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Name")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                }

                return _moduleInfoCaller;
            }
        }

        private static ModuleInfoExtended? _moduleInfoTarget;
        public static ModuleInfoExtended ModuleInfoTarget
        {
            get
            {
                if (_moduleInfoTarget is null)
                {
                    _moduleInfoTarget = new ModuleInfoExtended();
                    AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Id")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                    AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Name")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget;
            }
        }


        private static object? _moduleInfoCaller1;
        public static object ModuleInfoCaller1
        {
            get
            {
                if (_moduleInfoCaller1 is null)
                {
                    _moduleInfoCaller1 = Activator.CreateInstance(ModuleInfoType);
                    AccessTools2.Property(ModuleInfoType, "Id")?.SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
                    AccessTools2.Property(ModuleInfoType, "Name")?.SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
                }

                return _moduleInfoCaller1;
            }
        }

        private static object? _moduleInfoTarget1;
        public static object ModuleInfoTarget1
        {
            get
            {
                if (_moduleInfoTarget1 is null)
                {
                    _moduleInfoTarget1 = Activator.CreateInstance(ModuleInfoType);
                    AccessTools2.Property(ModuleInfoType, "Id")?.SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                    AccessTools2.Property(ModuleInfoType, "Name")?.SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget1;
            }
        }
    }
}