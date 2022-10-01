/*********************************************************
 * 名    称：UserInfo
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/2 21:13:47
 * 描    述：用户信息
 *
 * 更新历史：
 *
 * *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 学院编码
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public string Grade  { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string Type  { get; set; }
        /// <summary>
        /// 用户类型名称
        /// </summary>
        public string TypeName  { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string ShowStatus { get; set; }
        /// <summary>
        /// 所属分组
        /// </summary>
        public List<string> GroupIds { get; set; }
}
}
