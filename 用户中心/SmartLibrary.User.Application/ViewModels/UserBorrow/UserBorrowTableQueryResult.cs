/*********************************************************
* 名    称：UserBorrowTableQueryResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户借阅记录
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户借阅记录结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserBorrowTableQueryResult<T> : TableQueryResult<T> where T : class
    {
        public int TotalBorrowCount { get; set; }
        public int NowBorrowCount { get; set; }
    }
}
