/*********************************************************
* 名    称：SyncCardConfigItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡同步配置，从一卡通同步读者卡
* 更新历史：
*
* *******************************************************/
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 读者卡同步配置信息
    /// </summary>
    public class SyncCardConfigItemDto
    {
        /// <summary>
        /// 周期表达式
        /// </summary>
        [Required(ErrorMessage = "请填写Cron表达式")]
        public string Cron { get; set; }
        /// <summary>
        /// 执行周期备注
        /// </summary>
        public string CronRemark { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyFullName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string ClassFullName { get; set; }
        /// <summary>
        /// 任务参数
        /// </summary>
        public string TaskParam { get; set; }
        /// <summary>
        /// 适配器程序集名称
        /// </summary>
        public string AdapterAssemblyFullName { get; set; }
        /// <summary>
        /// 适配器名称
        /// </summary>
        public string AdapterClassFullName { get; set; }
        /// <summary>
        /// 适配器参数
        /// </summary>
        public string AdapterParm { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int JobStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
