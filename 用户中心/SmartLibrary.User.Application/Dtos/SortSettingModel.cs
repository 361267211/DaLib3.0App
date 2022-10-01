/*********************************************************
* 名    称：SortSettingModel.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：排序信息
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos
{
    /// <summary>
    /// 排序配置
    /// </summary>
    public class SortSettingModel
    {
        /// <summary>
        /// 序号，正序排列，第一个Orderby后面Thenby
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { get; set; }
    }
}
