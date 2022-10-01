/*********************************************************
* 名    称：ICustomerService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211108
* 描    述：客户管理接口
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<CustomerTableViewModel>> QueryTableData(CustomerTableQuery queryFilter);
        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CustomerTableViewModel> GetById(Guid customerId);
        /// <summary>
        /// 通过id获取客户凭据信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CustomerCredentialViewModel> GetCredentialById(Guid customerId);
        /// <summary>
        /// 获取所有客户凭据信息，用于认证中心验证
        /// </summary>
        /// <returns></returns>
        Task<List<CustomerCredentialViewModel>> GetCustomerCredentials();
        /// <summary>
        /// 根据Code获取客户信息
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        Task<CustomerTableViewModel> GetByCode(string customerCode);
    }
}
