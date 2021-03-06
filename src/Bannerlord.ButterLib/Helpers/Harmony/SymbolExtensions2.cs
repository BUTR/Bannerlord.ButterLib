using HarmonyLib;

using System;
using System.Linq.Expressions;
using System.Reflection;

using SymbolExtensions3 = HarmonyLib.BUTR.Extensions.SymbolExtensions2;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use package Harmony.Extensions instead!", false)]
    public static class SymbolExtensions2
    {
        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(Expression<Func<TObject, TField>> expression)
            => SymbolExtensions3.FieldRefAccess<TObject, TField>(expression);

        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(LambdaExpression expression)
            => SymbolExtensions3.FieldRefAccess<TObject, TField>(expression);


        public static ConstructorInfo GetConstructorInfo<T>(Expression<Func<T>> expression)
            => SymbolExtensions3.GetConstructorInfo<T>(expression);

        public static ConstructorInfo GetConstructorInfo<T, TResult>(Expression<Func<T, TResult>> expression)
            => SymbolExtensions3.GetConstructorInfo<T, TResult>(expression);

        public static ConstructorInfo GetConstructorInfo(LambdaExpression expression)
            => SymbolExtensions3.GetConstructorInfo(expression);

        public static FieldInfo GetFieldInfo<T>(Expression<Func<T>> expression)
            => SymbolExtensions3.GetFieldInfo<T>(expression);


        public static FieldInfo GetFieldInfo<T, TResult>(Expression<Func<T, TResult>> expression)
            => SymbolExtensions3.GetFieldInfo<T, TResult>(expression);

        public static FieldInfo GetFieldInfo(LambdaExpression expression)
            => SymbolExtensions3.GetFieldInfo(expression);

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> expression)
            => SymbolExtensions3.GetPropertyInfo<T>(expression);

        public static PropertyInfo GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression)
            => SymbolExtensions3.GetPropertyInfo<T, TResult>(expression);

        public static PropertyInfo GetPropertyInfo(LambdaExpression expression)
            => SymbolExtensions3.GetPropertyInfo(expression);
    }
}