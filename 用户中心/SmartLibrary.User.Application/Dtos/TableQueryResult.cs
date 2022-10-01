/*********************************************************
* 名    称：TableQueryResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：Table查询结果定义，基本没使用
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos
{
    /// <summary>
    /// Table结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableQueryResult<T> where T : class
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<T> Items { get; set; }
    }

}
