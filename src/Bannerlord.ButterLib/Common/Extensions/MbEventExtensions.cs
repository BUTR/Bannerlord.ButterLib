using Bannerlord.ButterLib.Common.Helpers;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Common.Extensions
{
    public static class MbEventExtensions
    {
        internal static class MbEvent2InvokeHandler<T1, T2>
        {
            internal delegate void InvokeDelegate(MbEvent<T1, T2> instance, T1 t1, T2 t2);
            internal static readonly InvokeDelegate? DeInvoke = AccessTools2.GetDeclaredDelegate<InvokeDelegate>(typeof(MbEvent<T1, T2>), "Invoke");
        }
        internal static class MbEvent3InvokeHandler<T1, T2, T3>
        {
            internal delegate void InvokeDelegate(MbEvent<T1, T2, T3> instance, T1 t1, T2 t2, T3 t3);
            internal static readonly InvokeDelegate? DeInvoke = AccessTools2.GetDeclaredDelegate<InvokeDelegate>(typeof(MbEvent<T1, T2, T3>), "Invoke");
        }
        internal static class MbEvent4InvokeHandler<T1, T2, T3, T4>
        {
            internal delegate void InvokeDelegate(MbEvent<T1, T2, T3, T4> instance, T1 t1, T2 t2, T3 t3, T4 t4);
            internal static readonly InvokeDelegate? DeInvoke = AccessTools2.GetDeclaredDelegate<InvokeDelegate>(typeof(MbEvent<T1, T2, T3, T4>), "Invoke");
        }
        internal static class MbEvent5InvokeHandler<T1, T2, T3, T4, T5>
        {
            internal delegate void InvokeDelegate(MbEvent<T1, T2, T3, T4, T5> instance, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
            internal static readonly InvokeDelegate? DeInvoke = AccessTools2.GetDeclaredDelegate<InvokeDelegate>(typeof(MbEvent<T1, T2, T3, T4, T5>), "Invoke");
        }
        internal static class MbEvent6InvokeHandler<T1, T2, T3, T4, T5, T6>
        {
            internal delegate void InvokeDelegate(MbEvent<T1, T2, T3, T4, T5, T6> instance, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
            internal static readonly InvokeDelegate? DeInvoke = AccessTools2.GetDeclaredDelegate<InvokeDelegate>(typeof(MbEvent<T1, T2, T3, T4, T5, T6>), "Invoke");
        }

        public static void Invoke<T1, T2>(this MbEvent<T1, T2> instance, T1 arg1, T2 arg2)
        {
            MbEvent2InvokeHandler<T1, T2>.DeInvoke?.Invoke(instance, arg1, arg2);
        }

        public static void Invoke<T1, T2, T3>(this MbEvent<T1, T2, T3> instance, T1 arg1, T2 arg2, T3 arg3)
        {
            MbEvent3InvokeHandler<T1, T2, T3>.DeInvoke?.Invoke(instance, arg1, arg2, arg3);
        }

        public static void Invoke<T1, T2, T3, T4>(this MbEvent<T1, T2, T3, T4> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            MbEvent4InvokeHandler<T1, T2, T3, T4>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4);
        }

        public static void Invoke<T1, T2, T3, T4, T5>(this MbEvent<T1, T2, T3, T4, T5> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            MbEvent5InvokeHandler<T1, T2, T3, T4, T5>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4, arg5);
        }

        public static void Invoke<T1, T2, T3, T4, T5, T6>(this MbEvent<T1, T2, T3, T4, T5, T6> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            MbEvent6InvokeHandler<T1, T2, T3, T4, T5, T6>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }
}