using HarmonyLib;

using System;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    [Obsolete("Use package Harmony.Extensions instead!", true)]
    public static class SymbolExtensions2
    {
        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(Expression<Func<TObject, TField>> expression) => null;
        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(LambdaExpression expression) => null;

        public static ConstructorInfo? GetConstructorInfo<T>(Expression<Func<T>> expression) => null;
        public static ConstructorInfo? GetConstructorInfo<T, TResult>(Expression<Func<T, TResult>> expression) => null;
        public static ConstructorInfo? GetConstructorInfo(LambdaExpression expression) => null;

        public static FieldInfo? GetFieldInfo<T>(Expression<Func<T>> expression) => null;
        public static FieldInfo? GetFieldInfo<T, TResult>(Expression<Func<T, TResult>> expression) => null;
        public static FieldInfo? GetFieldInfo(LambdaExpression expression) => null;

        public static PropertyInfo? GetPropertyInfo<T>(Expression<Func<T>> expression) => null;
        public static PropertyInfo? GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression) => null;
        public static PropertyInfo? GetPropertyInfo(LambdaExpression expression) => null;
    }
}