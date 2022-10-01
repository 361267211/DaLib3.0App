/*********************************************************
* 名    称：IRegionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：组织机构服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 机构服务
    /// </summary>
    public interface IRegionService
    {
        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        Task<List<RegionOutput>> GetRegionTree();
    }
}
