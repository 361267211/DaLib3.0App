/*********************************************************
* 名    称：VersionTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用版本记录查询条件
* 更新历史：
*
* *******************************************************/
using System;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用版本变更查询条件
    /// </summary>
    public class VersionTableQuery : TableQueryBase
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid? AppID { get; set; }
    }
}
