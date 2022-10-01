/*********************************************************
* 名    称：InfoPermitReaderDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：******
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.BasicConfigSet
{
    /// <summary>
    /// 读者权限设置
    /// </summary>
    public class InfoPermitReaderDto
    {
        /// <summary>
        /// 权限类型 0:申领读者卡 1:完善个人信息
        /// </summary>
        public int ConfigType { get; set; }
        /// <summary>
        /// 读者类型 0:用户类型 1:用户组 
        /// </summary>
        public int ReaderType { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        public string RefID { get; set; }
    }
}
