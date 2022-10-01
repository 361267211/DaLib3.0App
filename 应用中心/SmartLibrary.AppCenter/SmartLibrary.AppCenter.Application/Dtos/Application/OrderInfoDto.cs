using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用订单信息
    /// </summary>
    public class OrderInfoDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 开发商
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 显示授权类型
        /// </summary>
        public string ShowAuthType { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string AuthType { get; set; }

        /// <summary>
        /// 显示开通类型
        /// </summary>
        public string ShowOpenType { get; set; }

        /// <summary>
        /// 开通类型
        /// </summary>
        public string OpenType { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string ShowStatus { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public string CommitDate { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 拒绝状态备注信息
        /// </summary>
        public string Remark { get; set; }
    }
}
