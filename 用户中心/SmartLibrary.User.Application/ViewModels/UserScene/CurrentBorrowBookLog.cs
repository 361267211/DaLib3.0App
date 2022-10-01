/*********************************************************
 * 名    称：IndexStatisticsViewModel
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

    /// <summary>
    /// 当前借阅列表
    /// </summary>
    public class CurrentBorrowBookLog
    {
        /// <summary>
        /// 图书列表
        /// </summary>
        public List<BorrowBook> BorrowBookList { get; set; }

        /// <summary>
        /// 可借
        /// </summary>
        public int BorrowLimitCount { get; set; } = 0;

        /// <summary>
        /// 已借
        /// </summary>
        public int HasBorrowCount { get; set; } = 0;
    }

    /// <summary>
    /// 图书借阅日志信息
    /// </summary>
    public class BorrowBook
    {
        /// <summary>
        /// 图书ID
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        /// 中央库文献ID
        /// </summary>
        public string LiteratureId { get; set; }

        /// <summary>
        /// 图书标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图书索书号
        /// </summary>
        public string CallNo { get; set; }

        /// <summary>
        /// 图书条码号
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 图书馆ID
        /// </summary>
        public string LibraryId { get; set; }

        /// <summary>
        /// 图书馆名称
        /// </summary>
        public string LibraryName { get; set; }

        /// <summary>
        /// 图书馆地址
        /// </summary>
        public string LibraryLocation { get; set; }

        /// <summary>
        /// 借阅时间
        /// </summary>
        public string BorrowTime { get; set; }

        /// <summary>
        /// 应归还时间
        /// </summary>
        public string ShouldReturnTime { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public int Cq
        {
            get
            {
                try
                {

                    DateTime.TryParse(ShouldReturnTime, out DateTime stime);

                    if (DateTime.Compare(DateTime.Now, stime) > 0)
                    {
                        return 1;
                    }
                }
                catch
                { }
                return 0;
            }
        }

        /// <summary>
        /// 归还时间
        /// </summary>
        public string ReturnTime { get; set; }

        /// <summary>
        /// 续借次数
        /// </summary>
        public int RenewCount { get; set; }

        /// <summary>
        /// 是否可以续借(true可以续借，false不能续借)
        /// </summary>
        public bool Renewflag { get; set; }
    }
}
