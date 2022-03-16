using Bannerlord.ModuleManager;

using HarmonyLib;

using System;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule
{
    internal static class TestHelper
    {
        public static readonly Type? OldModuleInfoType = Type.GetType("TaleWorlds.Library.ModuleInfo, TaleWorlds.Library", false);
        public static readonly Type? NewModuleInfoType = Type.GetType("TaleWorlds.ModuleManager.ModuleInfo, TaleWorlds.ModuleManager", false);


        private static ModuleInfoExtended? _moduleInfoCaller;
        public static ModuleInfoExtended ModuleInfoCaller
        {
            get
            {
                if (_moduleInfoCaller is null)
                {
                    _moduleInfoCaller = new ModuleInfoExtended();
                    AccessTools.Property(typeof(ModuleInfoExtended), "Id")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                    AccessTools.Property(typeof(ModuleInfoExtended), "Name")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
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
                    AccessTools.Property(typeof(ModuleInfoExtended), "Id")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                    AccessTools.Property(typeof(ModuleInfoExtended), "Name")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
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
                    var type = OldModuleInfoType ?? NewModuleInfoType;
                    _moduleInfoCaller1 = Activator.CreateInstance(type);
                    AccessTools.Property(type, "Id")?.SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
                    AccessTools.Property(type, "Name")?.SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
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
                    var type = OldModuleInfoType ?? NewModuleInfoType;
                    _moduleInfoTarget1 = Activator.CreateInstance(type);
                    AccessTools.Property(type, "Id")?.SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                    AccessTools.Property(type, "Name")?.SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget1;
            }
        }
    }
}