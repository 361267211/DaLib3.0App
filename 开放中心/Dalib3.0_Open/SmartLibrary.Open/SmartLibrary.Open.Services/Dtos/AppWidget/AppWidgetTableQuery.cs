/*********************************************************
* 名    称：AppWidgetTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210913
* 描    述：应用组件查询条件
* 更新历史：
*
* *******************************************************/

using System;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用组件查询条件
    /// </summary>
    public class AppWidgetTableQuery : TableQueryBase
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
    }
}
