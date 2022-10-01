/*********************************************************
* 名    称：CardTableQueryItem.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：查询项（本来打算用动态linq拼接的方式，考虑到性能调优，弃用）
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 查询项（本来打算用动态linq拼接的方式，考虑到性能调优，弃用）
    /// </summary>
    public class CardTableQueryItem
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
