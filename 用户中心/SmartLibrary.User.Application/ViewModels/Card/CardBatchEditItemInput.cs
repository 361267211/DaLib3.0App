/*********************************************************
* 名    称：CardBatchEditItemInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：批量修改属性项
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 批量修改属性项
    /// </summary>
    public class CardBatchEditItemInput
    {
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
