using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SmartLibrary.News.Utility
{
    /// <summary>
    /// 名    称：QueryableExtension
    /// 作    者：张泽军
    /// 创建时间：2021/11/16 10:13:50
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// 附加排序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="sortMethod"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string property, bool isAes)
        {
            var type = typeof(T);
            var parameterExp = Expression.Parameter(type);
            var propertyInfo = type.GetProperty(property);
            var propertyExp = Expression.Property(parameterExp, propertyInfo);
            var lambdaExp = Expression.Lambda<Func<T, dynamic>>(propertyExp, parameterExp);

            return isAes ? source.OrderBy(lambdaExp) : source.OrderByDescending(lambdaExp);
        }

        /// <summary>
        /// 根据指定属性名称对序列进行排序
        /// </summary>
        /// <typeparam name="TSource">source中的元素的类型</typeparam>
        /// <param name="source">一个要排序的值序列</param>
        /// <param name="property">属性名称</param>
        /// <param name="isAes">是否升序</param>
        /// <returns></returns>
        public static IQueryable<TSource> ApplyOrderCustomize<TSource>(this IQueryable<TSource> source, string property, bool isAes) where TSource : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
            PropertyInfo pi = typeof(TSource).GetProperty(property);
            MemberExpression selector = Expression.MakeMemberAccess(param, pi);
            LambdaExpression le = Expression.Lambda(selector, param);
            string methodName = (isAes) ? "OrderBy" : "OrderByDescending";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
            return source.Provider.CreateQuery<TSource>(resultExp);
        }
    }
}
