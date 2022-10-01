/*********************************************************
* 名    称：CardExportFilter.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡导出过滤条件
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡导出过滤
    /// </summary>
    public class CardExportFilter : CardTableQuery
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        public int ExportType { get; set; }
        public CardExportFilter()
        {
            Properties = new List<ExportPropertyInput>();
        }
        /// <summary>
        /// 需要导出的数据列
        /// </summary>
        public List<ExportPropertyInput> Properties { get; set; }
    }

    public class CardEncodeExportFilter : CardExportFilter
    {

    }

    public class CardExportBriefOutput
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
