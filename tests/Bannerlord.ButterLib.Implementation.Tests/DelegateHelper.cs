using System.Reflection;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Implementation.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedCreateFromDelegate(ref CharacterCode __result);
        public static MethodInfo GetMethodInfo(this MockedCreateFromDelegate @delegate) => @delegate.Method;

        public delegate bool MockedCurrentTicksDelegate(ref long __result);
        public static MethodInfo GetMethodInfo(this MockedCurrentTicksDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetBasePathDelegate(ref string __result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetSettlementAllDelegate(ref MBReadOnlyList<Settlement> __result);
        public static MethodInfo GetMethodInfo(this MockedGetSettlementAllDelegate @delegate) => @delegate.Method;
    }
}