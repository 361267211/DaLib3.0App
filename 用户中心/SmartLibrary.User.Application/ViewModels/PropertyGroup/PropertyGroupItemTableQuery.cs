/*********************************************************
* 名    称：PropertyGroupItemTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组选项查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组选项查询
    /// </summary>
    public class PropertyGroupItemTableQuery : TableQueryBase
    {
        public string GroupCode { get; set; }
    }
}
