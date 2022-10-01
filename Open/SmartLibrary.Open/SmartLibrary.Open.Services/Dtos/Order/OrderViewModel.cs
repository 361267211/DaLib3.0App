using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class OrderViewModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 授权类型名称
        /// </summary>
        public string AuthTypeDisp { get; set; }
        /// <summary>
        /// 开通类型
        /// </summary>
        public int OpenType { get; set; }
        /// <summary>
        /// 开通类型名称
        /// </summary>
        public string OpenTypeDisp { get; set; }
        /// <summary>
        /// 授权途径
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// 授权途径
        /// </summary>
        public string WayDisp { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 客户联系人
        /// </summary>
        public string ContactMan { get; set; }
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyMan { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
    }
}
