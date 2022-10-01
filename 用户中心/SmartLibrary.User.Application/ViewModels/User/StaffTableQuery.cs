/*********************************************************
* 名    称：StaffTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：馆员查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 馆员查询
    /// </summary>
    public class StaffTableQuery : TableQueryBase
    {
        /// <summary>
        /// 读者姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 主卡卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Depart { get; set; }
    }

    public class StaffEncodeTableQuery : StaffTableQuery
    {

    }
}
