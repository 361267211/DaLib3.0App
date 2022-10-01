using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：AutoMapperExtension
    /// 作    者：张泽军
    /// 创建时间：2021/9/8 16:38:29
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// model/dto转entity
        /// </summary>
        /// <typeparam name="TSource">model/dto类型</typeparam>
        /// <typeparam name="TDestination">entity类型</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TDestination ToModel<TDestination>(this object model)
        {
            return Mapper.Map<TDestination>(model);
        }

        /// <summary>
        /// List映射
        /// </summary>
        /// <typeparam name="TSource">entity类型</typeparam>
        /// <typeparam name="TDestination">model/dto类型</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<TDestination> ToModelList<TDestination>(this IEnumerable<object> entities)
        {
            return Mapper.Map<List<TDestination>>(entities);
        }
    }
}
