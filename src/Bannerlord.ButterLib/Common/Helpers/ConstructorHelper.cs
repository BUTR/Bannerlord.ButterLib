using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bannerlord.ButterLib.Common.Helpers
{
    internal static class ConstructorHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public static TDelegate? Delegate<TDelegate>(ConstructorInfo constructorInfo) where TDelegate : Delegate
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 6)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<TDelegate>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when(e is ArgumentException)
            {
                return null;
            }
        }


        public static Func<T1>? Func<T1>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 0)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2>? Func<T1, T2>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 1)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2, T3>? Func<T1, T2, T3>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 2)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2, T3>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2, T3, T4>? Func<T1, T2, T3, T4>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 3)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2, T3, T4>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2, T3, T4, T5>? Func<T1, T2, T3, T4, T5>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 4)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2, T3, T4, T5>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2, T3, T4, T5, T6>? Func<T1, T2, T3, T4, T5, T6>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 5)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7>? Func<T1, T2, T3, T4, T5, T6, T7>(ConstructorInfo constructorInfo)
        {
            var parameterTypes = constructorInfo.GetParameters();
            if (parameterTypes.Length != 6)
                return null;

            var parameters = parameterTypes.Select((t, i) => Expression.Parameter(t.ParameterType, "p" + i)).ToArray();
            var newExpression = Expression.New(constructorInfo, parameters);
            try
            {
                var lambda = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7>>(newExpression, parameters);
                return lambda.Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
    }
}