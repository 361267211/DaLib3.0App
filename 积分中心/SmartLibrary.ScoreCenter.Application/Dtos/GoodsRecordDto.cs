/*********************************************************
* 名    称：GoodsRecordDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 商品记录
    /// </summary>
    public class GoodsRecordDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
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
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 领取方式
        /// </summary>
        public int ObtainWay { get; set; }
        /// <summary>
        /// 领取地址
        /// </summary>
        public string ObtainAddress { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
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
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public string ObtainTime { get; set; }
    }
}
