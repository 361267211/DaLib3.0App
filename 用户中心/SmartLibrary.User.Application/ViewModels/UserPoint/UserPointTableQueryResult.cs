/*********************************************************
* 名    称：UserPointTableQueryResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户积分查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户积分查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserPointTableQueryResult<T> : TableQueryResult<T> where T : class
    {
        /// <summary>
        /// 总积分
        /// </summary>
        public decimal TotalPoints { get; set; }
        /// <summary>
        /// 消费积分
        /// </summary>
        public decimal ConsumePoints { get; set; }
        /// <summary>
        /// 过期积分
        /// </summary>
        public decimal ExpirePoints { get; set; }

    }
}
