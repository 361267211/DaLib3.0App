using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SmartLibrary.Navigation.Utility
{
    /// <summary>
    /// 名    称：QueryableExtension
    /// 作    者：张泽军
    /// 创建时间：2021/11/16 9:42:43
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


        // string fieldExpr(T row) - function returning multiple value string field to test
        // delimiter - string separator between values in test field
        // value - string value to find in values of test field
        // dbq.Where(r => fieldExpr(r).Split(delimiter).Contains(value))
        public static IQueryable<T> WhereSplitContains<T>(this IQueryable<T> dbq,
            Expression<Func<T, string>> fieldExpr, string delimiter, string value)
        {
            var pred = PredicateBuilder.New<T>(r => fieldExpr.Invoke(r) == value);
            pred = pred.Or(r => fieldExpr.Invoke(r).StartsWith(value + delimiter));
            pred = pred.Or(r => fieldExpr.Invoke(r).EndsWith(delimiter + value));
            pred = pred.Or(r => fieldExpr.Invoke(r).Contains(delimiter + value + delimiter));

            return dbq.Where((Expression<Func<T, bool>>)pred.Expand());
        }

        // values - string values, one of which to find in values of test field
        // string fieldExpr(T row) - function returning multiple value string field to test
        // delimiter - string separator between values in test field
        // dbq.Where(r => values.Any(value => fieldExpr(r).Split(delimiter).Contains(value)))
        public static IQueryable<T> WhereAnySplitContains<T>(this IQueryable<T> dbq,
            IEnumerable<string> values, Expression<Func<T, string>> fieldExpr, string delimiter)
        {
            var pred = PredicateBuilder.New<T>();
            foreach (var value in values)
            {
                pred = pred.Or(r => fieldExpr.Invoke(r) == value);
                pred = pred.Or(r => fieldExpr.Invoke(r).StartsWith(value + delimiter));
                pred = pred.Or(r => fieldExpr.Invoke(r).EndsWith(delimiter + value));
                pred = pred.Or(r => fieldExpr.Invoke(r).Contains(delimiter + value + delimiter));
            }

            return dbq.Where((Expression<Func<T, bool>>)pred.Expand());
        }

        public static IQueryable<T> WhereSplitContainsAny<T>(this IQueryable<T> dbq,
            Expression<Func<T, string>> fieldExpr, string delimiter, IEnumerable<string> values) =>
            dbq.WhereAnySplitContains(values, fieldExpr, delimiter);

        public static IQueryable<T> WhereSplitContainsAny<T>(this IQueryable<T> dbq,
            Expression<Func<T, string>> fieldExpr, string delimiter, params string[] values) =>
            dbq.WhereAnySplitContains(values, fieldExpr, delimiter);

        // values - string values, all of which to find in values of test field
        // string fieldExpr(T row) - function returning multiple value string field to test
        // delimiter - string separator between values in test field
        // dbq.Where(r => values.All(value => fieldExpr(r).Split(delimiter).Contains(value)))
        public static IQueryable<T> WhereAllSplitContains<T>(this IQueryable<T> dbq, IEnumerable<string> values, Expression<Func<T, string>> fieldExpr, string delimiter)
        {
            var pred = PredicateBuilder.New<T>();
            foreach (var value in values)
            {
                var subPred = PredicateBuilder.New<T>(r => fieldExpr.Invoke(r) == value);
                subPred = subPred.Or(r => fieldExpr.Invoke(r).StartsWith(value + delimiter));
                subPred = subPred.Or(r => fieldExpr.Invoke(r).EndsWith(delimiter + value));
                subPred = subPred.Or(r => fieldExpr.Invoke(r).Contains(delimiter + value + delimiter));
                pred = pred.And(subPred);
            }

            return dbq.Where((Expression<Func<T, bool>>)pred.Expand());
        }

        public static IQueryable<T> WhereSplitContainsAll<T>(this IQueryable<T> dbq, Expression<Func<T, string>> fieldExpr, string delimiter, IEnumerable<string> values) =>
            dbq.WhereAllSplitContains(values, fieldExpr, delimiter);

        public static IQueryable<T> WhereSplitContainsAll<T>(this IQueryable<T> dbq, Expression<Func<T, string>> fieldExpr, string delimiter, params string[] values) =>
            dbq.WhereAllSplitContains(values, fieldExpr, delimiter);
    }
}
