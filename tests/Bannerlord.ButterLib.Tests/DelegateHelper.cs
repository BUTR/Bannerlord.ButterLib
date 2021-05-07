using Bannerlord.ButterLib.Common.Helpers;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bannerlord.ButterLib.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedGetModuleInfoDelegate(Type type, ref object? __result);
        public static MethodInfo GetMethodInfo(MockedGetModuleInfoDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetBasePathDelegate(ref string __result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetLoadedModulesDelegate(ref IEnumerable<ExtendedModuleInfo> __result);
        public static MethodInfo GetMethodInfo(MockedGetLoadedModulesDelegate @delegate) => @delegate.Method;
    }
}