/*********************************************************
* 名    称：CustomerService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211108
* 描    述：客户管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class CustomerService : ICustomerService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<MicroApplication> _appRepository;
        private readonly IRepository<Order> _orderRepository;
        public CustomerService(IRepository<Customer> customerRepository
            , IDistributedIDGenerator idGenerator
            , IRepository<MicroApplication> appRepository
            , IRepository<Order> orderRepository)
        {
            _customerRepository = customerRepository;
            _idGenerator = idGenerator;
            _appRepository = appRepository;
            _orderRepository = orderRepository;
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<CustomerTableViewModel>> QueryTableData(CustomerTableQuery queryFilter)
        {
            var nowTime = DateTime.Now;
            var dueTime = nowTime.AddDays(30);
            var orderQuery = from order in _orderRepository.Where(x => !x.DeleteFlag) select order;
            var customerQuery = from customer in _customerRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.Name.Contains(queryFilter.Keyword))
                                select new CustomerTableViewModel
                                {
                                    ID = customer.Id,
                                    Name = customer.Name,
                                    Owner = customer.Owner,
                                    PlatformVersion = customer.PlatformVersion,
                                    AppCount = orderQuery.Where(x => nowTime < x.ExpireDate && x.CustomerId == customer.Id.ToString()).Select(x => x.AppId).Distinct().Count(),
                                    AppDueSoonCount = orderQuery.Where(x => nowTime < x.ExpireDate && dueTime > x.ExpireDate && x.CustomerId == customer.Id.ToString()).Select(x => x.AppId).Distinct().Count(),
                                    AppExpiredCount = orderQuery.Where(x => nowTime > x.ExpireDate && x.CustomerId == customer.Id.ToString()).Select(x => x.AppId).Distinct().Count(),
                                    CreateTime = customer.CreatedTime
                                };
            var pageList = await customerQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerTableViewModel> GetById(Guid customerId)
        {
            var orderQuery = from order in _orderRepository.Where(x => !x.DeleteFlag) select order;
            var customerEntity = await _customerRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == customerId);
            if (customerEntity == null)
            {
                throw Oops.Oh("未找到客户信息");
            }
            var nowTime = DateTime.Now;
            var dueTime = nowTime.AddDays(30);
            var customerInfo = customerEntity.Adapt<CustomerTableViewModel>();
            customerInfo.AppCount = orderQuery.Where(x => nowTime < x.ExpireDate && x.CustomerId == customerId.ToString()).Select(x => x.AppId).Distinct().Count();
            customerInfo.AppDueSoonCount = orderQuery.Where(x => nowTime < x.ExpireDate && dueTime > x.ExpireDate && x.CustomerId == customerId.ToString()).Select(x => x.AppId).Distinct().Count();
            customerInfo.AppExpiredCount = orderQuery.Where(x => nowTime > x.ExpireDate && x.CustomerId == customerId.ToString()).Select(x => x.AppId).Distinct().Count();
            return customerInfo;
        }

        /// <summary>
        /// 获取客户凭据
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerCredentialViewModel> GetCredentialById(Guid customerId)
        {
            var customerEntity = await _customerRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == customerId);
            if (customerEntity == null)
            {
                throw Oops.Oh("未找到客户信息");
            }
            var credentialData = new CustomerCredentialViewModel
            {
                ID = customerId,
                Key = customerEntity.Key,
                Secret = customerEntity.Secret
            };
            return credentialData;
        }

        /// <summary>
        /// 获取所有客户凭据信息，用于认证中心验证
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerCredentialViewModel>> GetCustomerCredentials()
        {
            var result = new List<CustomerCredentialViewModel>();

            var customerList = await _customerRepository.DetachedEntities.Where(c => !c.DeleteFlag).ToListAsync();

            customerList?.ForEach(c =>
            {
                result.Add(new CustomerCredentialViewModel
                {
                    ID = c.Id,
                    Key = c.Key,
                    Secret = c.Secret
                });
            });

            return result;
        }


        /// <summary>
        /// 根据Code获取客户信息
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public async Task<CustomerTableViewModel> GetByCode(string customerCode)
        {
            var orderQuery = from order in _orderRepository.Where(x => !x.DeleteFlag) select order;
            var customerEntity = await _customerRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Owner == customerCode);
            if (customerEntity == null)
            {
                throw Oops.Oh("未找到客户信息");
            }
            var nowTime = DateTime.Now;
            var dueTime = nowTime.AddDays(30);
            var customerInfo = customerEntity.Adapt<CustomerTableViewModel>();
            return customerInfo;
        }
    }
}
