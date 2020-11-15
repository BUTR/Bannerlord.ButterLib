using Bannerlord.ButterLib.Common.Helpers;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule
{
    public static class TestHelper
    {
        private static ModuleInfo? _moduleInfoCaller;

        public static ModuleInfo ModuleInfoCaller
        {
            get
            {
                if (_moduleInfoCaller is null)
                {
                    _moduleInfoCaller = new ModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                }

                return _moduleInfoCaller;
            }
        }

        private static ModuleInfo? _moduleInfoTarget;
        public static ModuleInfo ModuleInfoTarget
        {
            get
            {
                if (_moduleInfoTarget is null)
                {
                    _moduleInfoTarget = new ModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget;
            }
        }
    }
}