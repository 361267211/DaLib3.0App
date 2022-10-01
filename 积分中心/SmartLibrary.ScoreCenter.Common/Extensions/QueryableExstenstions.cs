/*********************************************************
* 名    称：QueryableExstenstions.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：动态表达式扩展
* 更新历史：
*
* *******************************************************/
using Furion.LinqBuilder;
using SmartLibrary.ScoreCenter.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SmartLibrary.ScoreCenter.Common.Extensions
{
    /// <summary>
    /// 查询扩展
    /// </summary>
    public static class QueryableExstenstions
    {
        /// <summary>
        /// 根据分页查询参数为IQueryable附加过滤条件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="searchParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, PageInputBase searchParameters)
        {
            if (searchParameters.SearchParameters == null)
                return source;

            var results = source.Where(LambdaExpressionBuilder.BuildLambda<T>(searchParameters.SearchParameters));

            //无排序字段
            if (searchParameters.SortField.IsNullOrEmpty())
                return results;

            return results.ApplyOrder(searchParameters.SortField, searchParameters.SortOrder);
        }

        /// <summary>
        /// 附加排序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="sortMethod"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string property, string sortMethod)
        {
            var type = typeof(T);
            var parameterExp = Expression.Parameter(type, "x");
            var propertyInfo = type.GetProperty(property);
            var propertyExp = Expression.Property(parameterExp, propertyInfo);
            var lambdaExp = Expression.Lambda<Func<T, dynamic>>(propertyExp, parameterExp);

            return sortMethod == "descend" ? source.OrderByDescending(lambdaExp) : source.OrderBy(lambdaExp);
        }

        /// <summary>
        /// 附加排序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="orderDicts"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<T> ApplyMultipleOrder<T>(this IQueryable<T> source, Dictionary<string, string> orderDicts)
        {
            var type = typeof(T);
            var parameterExp = Expression.Parameter(type, "x");
            IOrderedQueryable<T> targetSource = null;
            var orderIndex = 0;
            foreach (var orderItem in orderDicts)
            {
                var propertyInfo = type.GetProperty(orderItem.Key);
                var propertyExp = Expression.Property(parameterExp, propertyInfo);
                var lambdaExp = Expression.Lambda<Func<T, dynamic>>(Expression.Convert(propertyExp, typeof(object)), parameterExp);
                if (orderIndex == 0)
                {
                    targetSource = orderItem.Value == "descend" ? source.OrderByDescending(lambdaExp) : source.OrderBy(lambdaExp);
                }
                else
                {
                    targetSource = orderItem.Value == "descend" ? targetSource.ThenByDescending(lambdaExp) : targetSource.ThenBy(lambdaExp);
                }
                orderIndex++;
            }
            return targetSource;
        }
    }
}