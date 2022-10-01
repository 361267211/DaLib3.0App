/*********************************************************
* 名    称：AppTableViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：App表格视图模型
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// App表格视图
    /// </summary>
    public class AppTableViewModel
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public IEnumerable<string> ServiceType { get; set; }
        /// <summary>
        /// 适用终端
        /// </summary>
        public string Terminal { get; set; }
        /// <summary>
        /// 适用终端
        /// </summary>
        public IEnumerable<string> TerminalList { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusDisp { get; set; }
        /// <summary>
        /// 正式客户数量
        /// </summary>
        public int FormalCount { get; set; }
        /// <summary>
        /// 试用客户数量
        /// </summary>
        public int TryCount { get; set; }
        /// <summary>
        /// 分支数量
        /// </summary>
        public int BranchCount { get; set; }
    }
}
