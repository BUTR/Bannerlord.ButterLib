using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ModuleManager;

using HarmonyLib.BUTR.Extensions;

using System;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule;

internal static class TestHelper
{
    public static readonly Type? ModuleInfoType = AccessTools2.TypeByName("TaleWorlds.ModuleManager.ModuleInfo");

    private static ModuleInfoExtendedHelper? _moduleInfoCaller;
    public static ModuleInfoExtendedHelper ModuleInfoCaller
    {
        get
        {
            if (_moduleInfoCaller is null)
            {
                _moduleInfoCaller = new ModuleInfoExtendedHelper(new ModuleInfoExtended(), false, "");
                AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Id")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Name")?.SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
            }

            return _moduleInfoCaller;
        }
    }

    private static ModuleInfoExtendedHelper? _moduleInfoTarget;
    public static ModuleInfoExtendedHelper ModuleInfoTarget
    {
        get
        {
            if (_moduleInfoTarget is null)
            {
                _moduleInfoTarget = new ModuleInfoExtendedHelper(new ModuleInfoExtended(), false, "");
                AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Id")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                AccessTools2.Property("Bannerlord.ModuleManager.ModuleInfoExtended:Name")?.SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
            }

            return _moduleInfoTarget;
        }
    }
}