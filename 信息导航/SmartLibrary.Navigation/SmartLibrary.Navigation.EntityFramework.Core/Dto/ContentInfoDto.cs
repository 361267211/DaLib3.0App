/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Dto
{
    [Serializable]
    public partial class ContentInfoDto
    {
        private string _content;

        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string TitleColor { get; set; }
        public bool IsStrong { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public string[] CheckboxGroup { get; set; }
        public int PlateId { get; set; }
        public string Author { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Auditor { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher { get; set; }
        public string Content { get; set; }
        public string Keywords { get; set; }
        public string OutUrl { get; set; }
        public string Pic { get; set; }
        public string[] EditItem { get; set; }
        public string SdeptId { get; set; }
        public string[] SdeptId1 { get; set; }
        public int Status { get; set; }
        public DateTime Createtime { get; set; }
        public int DeleteFlag { get; set; }
        public string Operator { get; set; }
        public DateTime? ValidityTime { get; set; }
        public int SiteId { get; set; }
        public int Level { get; set; }
        public int ItemType { get; set; }
        public int ParentId { get; set; }
        public string Path { get; set; }
        public int? IsScore { get; set; }
        public int? Score { get; set; }
        public int? IsFeedBack { get; set; }
        public string ServiceRule { get; set; }
        public string ServiceStep { get; set; }
        public string ManageService { get; set; }
        public string Link { get; set; }
        public string IsShowContents { get; set; }
        public int? IsScore1 { get; set; }
        public string OutUrlSource { get; set; }
        public int? IsInSource { get; set; }
        public int? IsAuthorSource { get; set; }
        public int? JumpType { get; set; }
        public DateTime? Updatetime { get; set; }
        public int? OrderIndex { get; set; }
        public string ActivitySpeaker { get; set; }
        public DateTime? ActivityTime { get; set; }
        public string ActivityLocation { get; set; }
        public string ActivityCommunity { get; set; }
        public int? ActivityParticipantLimit { get; set; }
        public DateTime? ActivityApplyTimeLimit { get; set; }
        public int? ClickNum { get; set; }

        #region 拓展字段
        /// <summary>
        /// 显示时间
        /// </summary>
        public string ShowTime { get; set; }
        public string Description { get; set; }

        public string DisplayCreateTime => Createtime.ToString("yyyy-MM-dd");
        public string DisplayUpdateTime => Updatetime?.ToString("yyyy-MM-dd");
        /// <summary>
        /// 显示标题
        /// </summary>

        public string DisplayTitle { get; set; }
        /// <summary>
        /// 简介
        /// </summary>

        public string Preview { get; set; }

        /// <summary>
        /// 报名人数，外链
        /// </summary>
        public int ApplyCount { get; set; }
        public string DetailUrl { get; set; }
        public int ContentType { get; set; }

        //栏目
        public ContentInfoDto ParContentInfoDto { get; set; }
        #endregion
    }


}
