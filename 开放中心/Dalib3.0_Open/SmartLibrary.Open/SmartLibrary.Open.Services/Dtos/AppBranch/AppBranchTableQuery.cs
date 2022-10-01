/*********************************************************
* 名    称：AppBranchTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用类型查询条件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common.AssemblyBase;
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    public class AppBranchTableQuery:TableQueryBase
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid? AppID { get; set; }
        /// <summary>
        /// 是否主分支
        /// </summary>
        public bool? IsMaster { get; set; }
    }
}
