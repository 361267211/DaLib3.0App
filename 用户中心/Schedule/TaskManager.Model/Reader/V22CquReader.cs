using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Reader
{
    public class V22CquReader
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 站点类型，无用
        /// </summary>
        public int site_type { get; set; }
        /// <summary>
        /// 对应owner标识
        /// </summary>
        public int site_id { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string user_key { get; set; }
        /// <summary>
        /// 读者名称
        /// </summary>
        public string reader_name { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string login_name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 读者id
        /// </summary>
        public string reader_id { get; set; }
        /// <summary>
        /// 读者号
        /// </summary>
        public string reader_number { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string reader_barcode { get; set; }
        /// <summary>
        /// 磁码
        /// </summary>
        public string reader_iccode { get; set; }
        /// <summary>
        /// 读者类型
        /// </summary>
        public string reader_type { get; set; }
        /// <summary>
        /// 读者职务
        /// </summary>
        public string reader_title { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string department_name { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string specialty_name { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public string grade_name { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string class_name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string reader_gender { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string identity_card { get; set; }
        /// <summary>
        /// 读者电话
        /// </summary>
        public string reader_phone { get; set; }
        /// <summary>
        /// 读者邮箱
        /// </summary>
        public string reader_email { get; set; }
        /// <summary>
        /// 读者生日
        /// </summary>
        public DateTime reader_birthday { get; set; }
        /// <summary>
        /// 读者地址
        /// </summary>
        public string reader_address { get; set; }
        /// <summary>
        /// 教育背景
        /// </summary>
        public string edu_background { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int is_active { get; set; }
        /// <summary>
        /// 是否停用
        /// </summary>
        public int is_stoped { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 离校时间
        /// </summary>
        public DateTime stop_time { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime last_login_time { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int source_flag { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int delete_flag { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string field10 { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        //public DateTime versioned { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_flag { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal deposit { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string image_url { get; set; }
        /// <summary>
        /// sessionid
        /// </summary>
        public string user_session_id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string backpassword { get; set; }
        /// <summary>
        /// 读者类型2
        /// </summary>
        public int backreadertype { get; set; }
        public int SiteID { get; set; }
        public string qq_openid { get; set; }
        public string qq_access_token { get; set; }
        /// <summary>
        /// 锁定状态
        /// </summary>
        public int? islocked { get; set; }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime active_time { get; set; }
        /// <summary>
        /// 密码2
        /// </summary>
        public string virgin_password { get; set; }
        /// <summary>
        /// thesis_status
        /// </summary>
        public int? thesis_status { get; set; }
        public string library_branch { get; set; }
        public string userTemplate { get; set; }
        public DateTime? GraduationLeavingTime { get; set; }
        /// <summary>
        /// 超级管理员标识
        /// </summary>
        public int is_supervisor { get; set; }
    }
}
