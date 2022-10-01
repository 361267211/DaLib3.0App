/*********************************************************
* 名    称：OrderTableViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：授权订单列表视图模型
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common;
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 订单列表显示视图模型
    /// </summary>
    public class OrderTableViewModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public Guid DevID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string DevName { get; set; }
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
        /// 授权类型显示名称
        /// </summary>
        public string AuthTypeDisp
        {
            get
            {
                try
                {
                    return ((EnumOrderAuthType)AuthType).ToString();
                }
                catch { return ""; }

            }
        }
        /// <summary>
        /// 开通类型
        /// </summary>
        public int OpenType { get; set; }
        /// <summary>
        /// 开通类型显示名称
        /// </summary>
        public string OpenTypeDisp
        {
            get
            {
                try
                {
                    return ((EnumOrderOpenType)OpenType).ToString();
                }
                catch { return ""; }

            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态显示名称
        /// </summary>
        public string StatusDisp
        {
            get
            {
                try
                {
                    return ((EnumOrderStatus)Status).ToString();
                }
                catch { return ""; }

            }
        }
        /// <summary>
        /// 途径
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// 途径显示名称
        /// </summary>
        public string WayDisp
        {
            get
            {
                try
                {
                    return ((EnumOrderWay)Way).ToString();
                }
                catch { return ""; }

            }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTimeOffset CommitDate { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 拒绝备注信息
        /// </summary>
        public string Remark { get; set; }
    }
}
