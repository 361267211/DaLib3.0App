/*********************************************************
 * 名    称：AppAvailibleSortField
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用模板可用排序字段
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using Furion.DatabaseAccessor;

namespace SmartLibrary.Open.Services.Dtos
{

    /// <summary>
    /// 积分事件注册
    /// </summary>
    public class AppAvailibleSortFieldViewModel : Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string SortFieldName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string SortFieldValue { get; set; }

    }
}