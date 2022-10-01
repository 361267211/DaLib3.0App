using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model.Reader;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    public interface IReaderDataAdapter
    {
        /// <summary>
        /// 同步用户组编码
        /// </summary>
        List<SourceGroupItemDto> GetGroupCodeItems();

        /// <summary>
        /// 获取待处理用户总量 (读者同步服务 getPaged设置为True使用)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        List<ReaderSource> GetReaderInfoListTotalNum(DateTime updateFlag, out MessageHand message);

        /// <summary>
        /// 读者同步分页可选 （配合GetReaderInfoListTotalNum使用  getPaged设置为True使用）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        List<ReaderInfo> GetReaderInfoList(int pageIndex, int pageSize, ReaderSource readerSource, DateTime updateFlag, out MessageHand message);

        /// <summary>
        /// 获取所有读者的userkey和原始密码
        /// </summary>
        /// <returns></returns>
        List<ReaderInfo> GetReaderPasswordInfo();
    }
}
