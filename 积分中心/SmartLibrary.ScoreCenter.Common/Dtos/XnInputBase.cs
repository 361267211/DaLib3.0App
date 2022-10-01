/*********************************************************
* 名    称：XnInputBase.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：通用查询条件，用于构建动态linq语句
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Common.Extensions;
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Common.Dtos
{
    /// <summary>
    /// 通用输入扩展参数（带权限）
    /// </summary>
    public interface IXnInputBase
    {
        /// <summary>
        /// 授权菜单
        /// </summary>
        public List<long> GrantMenuIdList { get; set; }

        /// <summary>
        /// 授权角色
        /// </summary>
        public List<long> GrantRoleIdList { get; set; }

        /// <summary>
        /// 授权数据
        /// </summary>
        public List<long> GrantOrgIdList { get; set; }
    }

    /// <summary>
    /// 通用分页输入参数
    /// </summary>
    public class PageInputBase
    {
        /// <summary>
        /// 搜索值
        /// </summary>
        public virtual string SearchValue { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public virtual int PageNo { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        public virtual int PageSize { get; set; } = 20;

        /// <summary>
        /// 搜索开始时间
        /// </summary>
        public virtual string SearchBeginTime { get; set; }

        /// <summary>
        /// 搜索结束时间
        /// </summary>
        public virtual string SearchEndTime { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string SortField { get; set; }

        /// <summary>
        /// 排序方法,默认升序,否则降序(配合antd前端,约定参数为 Ascend,Dscend)
        /// </summary>
        public virtual string SortOrder { get; set; }

        /// <summary>
        /// 降序排序(不要问我为什么是descend不是desc，前端约定参数就是这样)
        /// </summary>
        public virtual string DescStr { get; set; } = "descend";

        /// <summary>
        /// 复杂查询条件
        /// </summary>
        public virtual List<Condition> SearchParameters { get; set; } = new();
    }
}