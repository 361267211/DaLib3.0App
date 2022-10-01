/*********************************************************
* 名    称：BasicConfigService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分规则配置
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分规则配置服务
    /// </summary>
    public class BasicConfigService : IBasicConfigService, IScoped
    {
        private readonly IRepository<BasicConfig> _basicConfigRepository;

        public BasicConfigService(IRepository<BasicConfig> basicConfigRepository)
        {
            _basicConfigRepository = basicConfigRepository;
        }

        /// <summary>
        /// 获取积分规则配置
        /// </summary>
        /// <returns></returns>
        public async Task<BasicConfigEditData> GetBasicConfigSet()
        {
            var basicConfig = await _basicConfigRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag);
            var basicConfigData = basicConfig.Adapt<BasicConfigEditData>();
            return basicConfigData;
        }

        /// <summary>
        /// 修改积分规则内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SaveConfigSet(BasicConfigEditData input)
        {
            var basicConfig = await _basicConfigRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag);
            if (basicConfig == null)
            {
                throw Oops.Oh("未找到配置数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            basicConfig.ShowRule = input.ShowRule;
            basicConfig.RuleContent = input.RuleContent;
            basicConfig.UpdateTime = DateTime.Now;
            await _basicConfigRepository.UpdateAsync(basicConfig);
            return true;
        }
    }
}
