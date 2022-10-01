﻿/*********************************************************
* 名    称：ReaderEditDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者编辑数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 读者编辑数据
    /// </summary>
    public class ReaderEditDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddrDetail { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
    }
}
