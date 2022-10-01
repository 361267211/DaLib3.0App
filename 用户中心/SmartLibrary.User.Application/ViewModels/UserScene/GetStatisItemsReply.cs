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

namespace SmartLibrary.User.Application.ViewModels.UserScene
{
    public class GetStatisItemsReply
    {
        public int MyCourrentBorrowCount { get; set; }
        public int MyAchievementsCount { get; set; }
        public int MyTotalScore { get; set; }
        public int MyCollectJournalCount { get; set; }
        public int MyYearComeInLibCount { get; set; }
        public int MyReservedSeatCount { get; set; }
        public int MyVisitPortalCount { get; set; }
        /// <summary>
        /// 当前积分
        /// </summary>
        public int MyRemainScore { get; set; }
    }

    public class StaticsItem
    {
        public string Title { get; set; }

        public string Link { get; set; }
        public int Count { get; set; }
    }

    
}
