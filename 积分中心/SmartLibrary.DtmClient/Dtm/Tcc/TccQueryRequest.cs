/*********************************************************
* 名    称：TccQueryRequest.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Tcc事务查询参数
* 更新历史：
*
* *******************************************************/
namespace SmartLibrary.DtmClient.Dtm.Tcc
{
    /// <summary>
    /// Tcc事务查询，目前没提供查询接口
    /// </summary>
    public class TccQueryRequest
    {
        /// <summary>
        /// GlobalTransationId
        /// </summary>
        public string gid { get; set; }
        /// <summary>
        /// 事务类型
        /// </summary>
        public string trans_type { get; set; }
        /// <summary>
        /// 子分支Id
        /// </summary>
        public string branch_id { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string op { get; set; }

    }
}
