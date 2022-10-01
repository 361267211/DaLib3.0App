/*********************************************************
 * 名    称：TableQueryBase
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/2 14:15:27
 * 描    述：列表查询请求对象基类
 *
 * 更新历史：
 *
 * *******************************************************/

namespace SmartLibrary.SceneManage.Common.AssemblyBase
{
    /// <summary>
    /// 列表查询请求对象基类
    /// </summary>
    public class TableQueryBase
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 分页数据量
        /// </summary>
        public int PageSize { get; set; } = 50;
        /// <summary>
        /// 关键字，用于模糊查询
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 默认降序,否则升序
        /// </summary>
        public bool IsAsc { get; set; }


    }
}
