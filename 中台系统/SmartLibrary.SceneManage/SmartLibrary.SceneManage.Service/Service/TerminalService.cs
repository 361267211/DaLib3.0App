/*********************************************************
 * 名    称：Terminal
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 22:06:42
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Common.Utility;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Service
{
    public class TerminalService : IScoped, ITerminalService
    {
       

        /// <summary>
        /// 终端实例数据仓储
        /// </summary>
        private readonly IRepository<TerminalInstance> _terminalInstanceRepository;

        /// <summary>
        /// 场景数据仓储
        /// </summary>
        private readonly IRepository<Scene> _sceneRepository;

        /// <summary>
        /// 场景分屏数据仓储
        /// </summary>
        private readonly IRepository<SceneScreen> _sceneScreenRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="terminalInstanceRepository"></param>
        /// <param name="themeColorRepository"></param>
        /// <param name="sceneScreenRepository"></param>
        public TerminalService(IRepository<TerminalInstance> terminalInstanceRepository
            , IRepository<Scene> sceneRepository
            , IRepository<SceneScreen> sceneScreenRepository)
        {
            _terminalInstanceRepository = terminalInstanceRepository;
            _sceneRepository = sceneRepository;
            _sceneScreenRepository = sceneScreenRepository;
        }

        /// <summary>
        /// 终端实例列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TerminalInstanceDto>> GetTerminalInstanceList()
        {
            var scene = _sceneRepository.Where(p => !p.DeleteFlag && (p.UserKey==null || p.UserKey.ToLower()=="default")).AsNoTracking();
            var queryTerminalInstance = _terminalInstanceRepository.Where(p => !p.DeleteFlag).Select(p => new TerminalInstanceDto
            {
                Id = p.Id.ToString(),
                Description = p.Description,
                Icon = p.Icon,
                IsSystemInstance = p.IsSystemInstance,
                KeyWords = p.KeyWords,
                Logo = p.Logo,
                Name = p.Name,
                Remark = p.Remark,
                Status = p.Status,
                TerminalType = p.TerminalType,
                VisitUrl = p.VisitUrl,
                SceneCount = scene.Count(s=>s.TerminalInstanceId==p.Id.ToString())
            });

            var table = await queryTerminalInstance.ToListAsync();
            return table;
        }

        /// <summary>
        /// 获取终端实例详情
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public async Task<TerminalInstanceDto> GetTerminalInstanceDetail(string terminalId)
        {
            var temrminal = await _terminalInstanceRepository.FirstOrDefaultAsync(p => p.Id.ToString() == terminalId && !p.DeleteFlag);

            var result = new TerminalInstanceDto
            {
                Id = temrminal.Id.ToString(),
                Description = temrminal.Description,
                Icon = temrminal.Icon,
                IsSystemInstance = temrminal.IsSystemInstance,
                KeyWords = temrminal.KeyWords,
                Logo = temrminal.Logo,
                Name = temrminal.Name,
                Remark = temrminal.Remark,
                Status = temrminal.Status,
                TerminalType = temrminal.TerminalType,
                VisitUrl = temrminal.VisitUrl
            };
            return result;
        }


        /// <summary>
        /// 添加终端实例
        /// </summary>
        /// <param name="temrminal">终端实例</param>
        /// <returns></returns>
        public async Task<Guid> CreateTerminalInstance(TerminalInstanceDto temrminal)
        {
            var terminalInstance = new TerminalInstance
            {
                Id = Guid.NewGuid(),
                Description = temrminal.Description,
                Icon = temrminal.Icon,
                IsSystemInstance = temrminal.IsSystemInstance,
                KeyWords = temrminal.KeyWords,
                Logo = temrminal.Logo,
                Name = temrminal.Name,
                Remark = temrminal.Remark,
                Status = temrminal.Status,
                TerminalType = temrminal.TerminalType,
                VisitUrl = temrminal.VisitUrl,
                CreatedTime = DateTimeOffset.UtcNow
            };
            var result = await _terminalInstanceRepository.InsertAsync(terminalInstance);


            return result.Entity.Id;
        }

        /// <summary>
        /// 更新终端实例
        /// </summary>
        /// <param name="temrminalDto">终端实例</param>
        /// <returns></returns>
        public async Task<Guid> UpdateTerminalInstance(TerminalInstanceDto temrminalDto)
        {
            var temrminal = await _terminalInstanceRepository.FirstOrDefaultAsync(p => p.Id.ToString() == temrminalDto.Id && !p.DeleteFlag);

            temrminal.Description = temrminalDto.Description;
            temrminal.Icon = temrminalDto.Icon;
            temrminal.IsSystemInstance = temrminalDto.IsSystemInstance;
            temrminal.KeyWords = temrminalDto.KeyWords;
            temrminal.Logo = temrminalDto.Logo;
            temrminal.Name = temrminalDto.Name;
            temrminal.Remark = temrminalDto.Remark;
            temrminal.Status = temrminalDto.Status;
            temrminal.TerminalType = temrminalDto.TerminalType;
            temrminal.VisitUrl = temrminalDto.VisitUrl;
            temrminal.UpdatedTime = DateTimeOffset.UtcNow;

            var result = await _terminalInstanceRepository.UpdateAsync(temrminal);


            return result.Entity.Id;
        }

        /// <summary>
        /// 删除终端实例
        /// </summary>
        /// <param name="terminalInstanceId">终端实例ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteTerminalInstance(string terminalInstanceId)
        {
            var temrminal = await _terminalInstanceRepository.FirstOrDefaultAsync(p => p.Id.ToString() == terminalInstanceId && !p.DeleteFlag);


            temrminal.DeleteFlag = true;
            temrminal.UpdatedTime = DateTimeOffset.UtcNow;

            var result = await _terminalInstanceRepository.UpdateAsync(temrminal);

            return result.State == EntityState.Modified;
        }

        /// <summary>
        /// 获取下拉框字典
        /// </summary>
        /// <returns></returns>
        public DictionaryViewModel GetDictionary()
        {
            var result = new DictionaryViewModel
            {
                TerminalStatus = EnumTools.EnumToList<TerminalStatusEnum>(),
                AppTerminalType = EnumTools.EnumToList<AppTerminalTypeEnum>(),
                TerminalIcon = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="移动端圆角",Value="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg" },
                    new SysDictModel<string>{Key="PC端圆角",Value="/uploads/cqu/20211025/f8048d98464d41ed8bc4e8a6e0954f03.jpg" },
                    new SysDictModel<string>{Key="显示屏Metro",Value="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg" },
                    new SysDictModel<string>{Key="显示屏圆角",Value="/uploads/cqu/20211025/f8048d98464d41ed8bc4e8a6e0954f03.jpg" }
                    }
            };
            return result;
        }


    }
}
