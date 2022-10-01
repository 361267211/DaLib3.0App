/*********************************************************
* 名    称：ReaderMedalOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者勋章获取信息
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 读者勋章信息
    /// </summary>
    public class ReaderMedalOutput
    {
        public ReaderMedalOutput()
        {
            UserMedal = new List<MedalInfoListItemOutput>();
            AllMedals = new List<MedalInfoListItemOutput>();
        }
        /// <summary>
        /// 读者已有勋章
        /// </summary>
        public List<MedalInfoListItemOutput> UserMedal { get; set; }
        /// <summary>
        /// 所有勋章
        /// </summary>
        public List<MedalInfoListItemOutput> AllMedals { get; set; }
    }


    public class MedalInfoListItemOutput
    {
        /// <summary>
        /// 勋章ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 持有人数量
        /// </summary>
        public int ReaderCount { get; set; }
    }
}
