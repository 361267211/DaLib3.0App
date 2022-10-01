/*********************************************************
* 名    称：CustomerAppTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：客户应用列表查询条件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos.Customer
{
    /// <summary>
    /// 客户应用列表查询条件
    /// </summary>
    public class CustomerAppTableQuery : TableQueryBase
    {
        /// <summary>
        /// 应用类型0：正常应用，1：即将过期应用，2：过期应用
        /// </summary>
        public int? AppStatus { get; set; }
    }
}
