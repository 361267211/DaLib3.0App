/*********************************************************
 * 名    称：QueryableExtension
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/3 17:56:48
 * 描    述：EF扩展
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Common.Utility
{
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
    }
}
