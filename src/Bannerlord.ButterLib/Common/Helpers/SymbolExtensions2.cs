using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Bannerlord.ButterLib.Common.Helpers
{
    public static class SymbolExtensions2
    {
        public static FieldInfo GetFieldInfo<T>(Expression<Func<T>> expression)
        {
            return GetFieldInfo((LambdaExpression)expression);
        }
        public static FieldInfo GetFieldInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetFieldInfo((LambdaExpression)expression);
        }
        public static FieldInfo GetFieldInfo(LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression body) || !(body.Member is FieldInfo fieldInfo))
                throw new ArgumentException("Invalid Expression. Expression should consist of a Field return only.");

            return fieldInfo;
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> expression)
        {
            return GetPropertyInfo((LambdaExpression)expression);
        }
        public static PropertyInfo GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetPropertyInfo((LambdaExpression)expression);
        }
        public static PropertyInfo GetPropertyInfo(LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression body) || !(body.Member is PropertyInfo propertyInfo))
                throw new ArgumentException("Invalid Expression. Expression should consist of a Property return only.");

            return propertyInfo;
        }
    }
}