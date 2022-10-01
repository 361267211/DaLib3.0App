/*********************************************************
* 名    称：SearchPropertyDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户查询属性
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户查询属性
    /// </summary>
    public class SearchPropertyDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 非附加属性
        /// </summary>
        public bool External { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int Type { get; set; }
    }
}
