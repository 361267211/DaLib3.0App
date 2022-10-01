/*********************************************************
* 名    称：ExportCardListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡导出行
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 读者卡导出行
    /// </summary>
    public class ExportCardListItemDto : CardListItemDto
    {
    }
    //读者卡导出简要信息
    public class CardExportBriefDto
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
    }
}
