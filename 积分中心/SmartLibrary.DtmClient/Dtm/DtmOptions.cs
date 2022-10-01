/*********************************************************
* 名    称：DtmOptions.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm客户端注入配置
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm客户端注入配置
    /// </summary>
    public class DtmOptions
    {
        /// <summary>
        /// 分布式事务协调器地址
        /// </summary>
        public string DtmUrl { get; set; }
        /// <summary>
        /// 数据库地址
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        internal int DbType { get; set; }

        public void UsePgSql()
        {
            this.DbType = (int)EnumDbType.PostgreSQL;
        }
    }
}
