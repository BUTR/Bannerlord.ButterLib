using Bannerlord.ButterLib.Common.Helpers;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Common.Extensions
{
    /// <summary>Extension class of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent}" /> class and its generic variations.</summary>
    /// <remarks>
    /// For some reason TW hid "Invoke" methods of the MbEvent classes with number of generic arguments greater than one.
    /// This extension class corrects said misconception.
    /// </remarks>
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

        /// <summary>Invokes all the listeners to the specified <see cref="T:TaleWorlds.CampaignSystem.MbEvent`2}" />.</summary>
        /// <typeparam name="T1">The type of the first event argument.</typeparam>
        /// <typeparam name="T2">The type of the second event argument.</typeparam>
        /// <param name="instance">An instance of <see cref="T:TaleWorlds.CampaignSystem.MbEvent`2}" /> to call "Invoke" against.</param>
        /// <param name="arg1">Fist argument of the event.</param>
        /// <param name="arg2">Second argument of the event.</param>
        /// <remarks>
        /// This is accomplished by calling a delegate on the private method called "Invoke" of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent`2}" />, 
        /// which guarantees fast and native execution.
        /// </remarks>
        public static void Invoke<T1, T2>(this MbEvent<T1, T2> instance, T1 arg1, T2 arg2)
        {
            MbEvent2InvokeHandler<T1, T2>.DeInvoke?.Invoke(instance, arg1, arg2);
        }

        /// <summary>Invokes all the listeners to the specified <see cref="T:TaleWorlds.CampaignSystem.MbEvent`3}" />.</summary>
        /// <typeparam name="T1">The type of the first event argument.</typeparam>
        /// <typeparam name="T2">The type of the second event argument.</typeparam>
        /// <typeparam name="T3">The type of the third event argument.</typeparam>
        /// <param name="instance">An instance of <see cref="T:TaleWorlds.CampaignSystem.MbEvent`3}" /> to call "Invoke" against.</param>
        /// <param name="arg1">Fist argument of the event.</param>
        /// <param name="arg2">Second argument of the event.</param>
        /// <param name="arg3">Third argument of the event.</param>
        /// <remarks>
        /// This is accomplished by calling a delegate on the private method called "Invoke" of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent`3}" />, 
        /// which guarantees fast and native execution.
        /// </remarks>
        public static void Invoke<T1, T2, T3>(this MbEvent<T1, T2, T3> instance, T1 arg1, T2 arg2, T3 arg3)
        {
            MbEvent3InvokeHandler<T1, T2, T3>.DeInvoke?.Invoke(instance, arg1, arg2, arg3);
        }

        /// <summary>Invokes all the listeners to the specified <see cref="T:TaleWorlds.CampaignSystem.MbEvent`4}" />.</summary>
        /// <typeparam name="T1">The type of the first event argument.</typeparam>
        /// <typeparam name="T2">The type of the second event argument.</typeparam>
        /// <typeparam name="T3">The type of the third event argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth event argument.</typeparam>
        /// <param name="instance">An instance of <see cref="T:TaleWorlds.CampaignSystem.MbEvent`4}" /> to call "Invoke" against.</param>
        /// <param name="arg1">Fist argument of the event.</param>
        /// <param name="arg2">Second argument of the event.</param>
        /// <param name="arg3">Third argument of the event.</param>
        /// <param name="arg4">Fourth argument of the event.</param>
        /// <remarks>
        /// This is accomplished by calling a delegate on the private method called "Invoke" of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent`4}" />, 
        /// which guarantees fast and native execution.
        /// </remarks>
        public static void Invoke<T1, T2, T3, T4>(this MbEvent<T1, T2, T3, T4> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            MbEvent4InvokeHandler<T1, T2, T3, T4>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4);
        }

        /// <summary>Invokes all the listeners to the specified <see cref="T:TaleWorlds.CampaignSystem.MbEvent`5}" />.</summary>
        /// <typeparam name="T1">The type of the first event argument.</typeparam>
        /// <typeparam name="T2">The type of the second event argument.</typeparam>
        /// <typeparam name="T3">The type of the third event argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth event argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth event argument.</typeparam>
        /// <param name="instance">An instance of <see cref="T:TaleWorlds.CampaignSystem.MbEvent`5}" /> to call "Invoke" against.</param>
        /// <param name="arg1">Fist argument of the event.</param>
        /// <param name="arg2">Second argument of the event.</param>
        /// <param name="arg3">Third argument of the event.</param>
        /// <param name="arg4">Fourth argument of the event.</param>
        /// <param name="arg5">Fifth argument of the event.</param>
        /// <remarks>
        /// This is accomplished by calling a delegate on the private method called "Invoke" of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent`5}" />, 
        /// which guarantees fast and native execution.
        /// </remarks>
        public static void Invoke<T1, T2, T3, T4, T5>(this MbEvent<T1, T2, T3, T4, T5> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            MbEvent5InvokeHandler<T1, T2, T3, T4, T5>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>Invokes all the listeners to the specified <see cref="T:TaleWorlds.CampaignSystem.MbEvent`6}" />.</summary>
        /// <typeparam name="T1">The type of the first event argument.</typeparam>
        /// <typeparam name="T2">The type of the second event argument.</typeparam>
        /// <typeparam name="T3">The type of the third event argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth event argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth event argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth event argument.</typeparam>
        /// <param name="instance">An instance of <see cref="T:TaleWorlds.CampaignSystem.MbEvent`6}" /> to call "Invoke" against.</param>
        /// <param name="arg1">Fist argument of the event.</param>
        /// <param name="arg2">Second argument of the event.</param>
        /// <param name="arg3">Third argument of the event.</param>
        /// <param name="arg4">Fourth argument of the event.</param>
        /// <param name="arg5">Fifth argument of the event.</param>
        /// <param name="arg6">Sixth argument of the event.</param>
        /// <remarks>
        /// This is accomplished by calling a delegate on the private method called "Invoke" of the <see cref="T:TaleWorlds.CampaignSystem.MbEvent`6}" />, 
        /// which guarantees fast and native execution.
        /// </remarks>
        public static void Invoke<T1, T2, T3, T4, T5, T6>(this MbEvent<T1, T2, T3, T4, T5, T6> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            MbEvent6InvokeHandler<T1, T2, T3, T4, T5, T6>.DeInvoke?.Invoke(instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }
}