/*********************************************************
* 名    称：InfoPermitReaderOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者修改信息权限输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者修改权限输出
    /// </summary>
    public class InfoPermitReaderOutput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 配置类型
        /// </summary>
        public int ConfigType { get; set; }
        /// <summary>
        /// 读者类型，1：用户组 2：用户类型
        /// </summary>
        public int ReaderType { get; set; }
        /// <summary>
        /// 关联ID，可能是用户组Id,也可能是用户类型属性ID
        /// </summary>
        public string RefID { get; set; }
    }
}
