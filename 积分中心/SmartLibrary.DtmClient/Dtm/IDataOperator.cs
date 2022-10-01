/*********************************************************
* 名    称：IDataOperator.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：DtmClient用到的数据操作接口
* 更新历史：
*
* *******************************************************/
using SqlSugar;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// DtmClient用到的数据操作接口
    /// </summary>
    public interface IDataOperator
    {
        /// <summary>
        /// 初始化数据结构
        /// </summary>
        /// <returns></returns>
        Task<bool> InitData(SqlSugarClient dbClient);
        /// <summary>
        /// 插入屏障数据
        /// </summary>
        /// <param name="dbClient"></param>
        /// <param name="paramObj"></param>
        /// <returns></returns>
        Task<int> InsertBarrier(SqlSugarClient dbClient, dynamic paramObj);
    }
}
