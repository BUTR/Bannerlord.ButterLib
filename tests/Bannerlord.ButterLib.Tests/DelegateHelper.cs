using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedGetLoadedModulesDelegate(ref List<ModuleInfo> list);
        public static MethodInfo GetMethodInfo(MockedGetLoadedModulesDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetModuleInfoDelegate(Type type, ref ModuleInfo? __result);
        public static MethodInfo GetMethodInfo(MockedGetModuleInfoDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetBasePathDelegate(ref string __result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;
    }
}