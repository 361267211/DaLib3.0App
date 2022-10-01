/*********************************************************
* 名    称：UserImportValidateInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220209
* 描    述：读者/读者卡查重校验数据结构
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者有效性验证
    /// </summary>
    public class ReaderValidateInfo
    {
        /// <summary>
        /// 读者ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 读者卡有效性验证
    /// </summary>
    public class CardValidateInfo
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 读者Id
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
