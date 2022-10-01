using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model.Reader;

namespace TaskManager.Adapters
{
    public interface IReaderInfoAdapter
    {
        /// <summary>
        /// 获取所有读者信息
        /// </summary>
        /// <returns></returns>
        List<ReaderInfo> GetReaderInfos();
    }
}
