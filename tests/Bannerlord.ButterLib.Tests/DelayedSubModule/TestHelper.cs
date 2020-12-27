using Bannerlord.ButterLib.Common.Helpers;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Tests.DelayedSubModule
{
    public static class TestHelper
    {
        private static ExtendedModuleInfo? _moduleInfoCaller;
        public static ExtendedModuleInfo ModuleInfoCaller
        {
            get
            {
                if (_moduleInfoCaller is null)
                {
                    _moduleInfoCaller = new ExtendedModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoCaller, nameof(TestSubModuleCaller));
                }

                return _moduleInfoCaller;
            }
        }

        private static ExtendedModuleInfo? _moduleInfoTarget;
        public static ExtendedModuleInfo ModuleInfoTarget
        {
            get
            {
                if (_moduleInfoTarget is null)
                {
                    _moduleInfoTarget = new ExtendedModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoTarget, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget;
            }
        }
        
        
        private static ModuleInfo? _moduleInfoCaller1;
        public static ModuleInfo ModuleInfoCaller1
        {
            get
            {
                if (_moduleInfoCaller1 is null)
                {
                    _moduleInfoCaller1 = new ModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoCaller1, nameof(TestSubModuleCaller));
                }

                return _moduleInfoCaller1;
            }
        }

        private static ModuleInfo? _moduleInfoTarget1;
        public static ModuleInfo ModuleInfoTarget1
        {
            get
            {
                if (_moduleInfoTarget1 is null)
                {
                    _moduleInfoTarget1 = new ModuleInfo();
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Id).SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                    SymbolExtensions2.GetPropertyInfo((ModuleInfo m) => m.Name).SetValue(_moduleInfoTarget1, nameof(TestSubModuleTarget));
                }

                return _moduleInfoTarget1;
            }
        }
    }
}