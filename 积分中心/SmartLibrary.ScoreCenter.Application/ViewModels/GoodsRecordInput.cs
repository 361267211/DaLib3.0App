/*********************************************************
* 名    称：GoodsRecordInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品管理
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 商品管理
    /// </summary>
    public class GoodsRecordInput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入商品名称")]
        [MaxLength(20, ErrorMessage = "商品名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 类型 0:虚拟 1:实物
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前库存量
        /// </summary>
        public int CurrentCount { get; set; }
        /// <summary>
        /// 锁定量
        /// </summary>
        public int FreezeCount { get; set; }
        /// <summary>
        /// 所需积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [MaxLength(200, ErrorMessage = "图片地址超长")]
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 领取方式
        /// </summary>
        public int ObtainWay { get; set; }
        /// <summary>
        /// 领取地址
        /// </summary>
        [MaxLength(500, ErrorMessage = "领取地址最多输入500个字符")]
        public string ObtainAddress { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [MaxLength(200, ErrorMessage = "联系方式最多输入200个字符")]
        public string ObtainContact { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string DetailInfo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public string ObtainTime { get; set; }
    }
}
