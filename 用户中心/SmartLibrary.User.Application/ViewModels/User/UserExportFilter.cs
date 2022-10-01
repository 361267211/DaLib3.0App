/*********************************************************
* 名    称：UserExportFilter.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者导出查询
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者导出查询
    /// </summary>
    public class UserExportFilter : UserTableQuery
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        public int ExportType { get; set; }
        public UserExportFilter()
        {
            Properties = new List<ExportPropertyInput>();
        }
        /// <summary>
        /// 需要导出的数据列
        /// </summary>
        public List<ExportPropertyInput> Properties { get; set; }
    }

    public class UserEncodeExportFilter : UserExportFilter
    {

    }

    public class UserExportBriefOutput
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
