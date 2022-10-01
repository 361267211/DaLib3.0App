/*********************************************************
* 名    称：GoodsManageTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品查询条件
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 商品查询条件
    /// </summary>
    public class GoodsManageTableQuery : TableQueryBase
    {
        /// <summary>
        /// 商品类型
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Name { get; set; }
    }
}
