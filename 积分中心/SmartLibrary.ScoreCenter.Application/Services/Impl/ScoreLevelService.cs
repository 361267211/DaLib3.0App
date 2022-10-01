/*********************************************************
* 名    称：ScoreLevelService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分等级管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分等级管理
    /// </summary>
    public class ScoreLevelService : IScoreLevelService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<ScoreLevel> _scoreLevelRepository;
        private readonly IRepository<BasicConfig> _configRepository;

        public ScoreLevelService(IDistributedIDGenerator idGenerator
            , IRepository<ScoreLevel> scoreLevelRepository
            , IRepository<BasicConfig> configRepository)
        {
            _idGenerator = idGenerator;
            _scoreLevelRepository = scoreLevelRepository;
            _configRepository = configRepository;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            //获取所有应用
            var initData = new
            {
                levelData = new ScoreLevelDto
                {
                    ID = _idGenerator.CreateGuid(),
                },
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 获取等级列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreLevelListItemDto>> QueryListData()
        {
            var listQuery = _scoreLevelRepository.DetachedEntities.Where(x => !x.DeleteFlag).ProjectToType<ScoreLevelListItemDto>();
            var dataList = await listQuery.OrderBy(x => x.ArchiveScore).ThenBy(x => x.CreateTime).ToListAsync();
            return dataList;
        }

        /// <summary>
        /// 新增等级
        /// </summary>
        /// <param name="scoreLevel"></param>
        /// <returns></returns>
        public async Task<Guid> Create(ScoreLevelDto scoreLevel)
        {
            scoreLevel.ID = _idGenerator.CreateGuid();
            if (await _scoreLevelRepository.AnyAsync(x => !x.DeleteFlag && x.Id != scoreLevel.ID && x.ArchiveScore == scoreLevel.ArchiveScore))
            {
                throw Oops.Oh("该目标积分已设置等级").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (await _scoreLevelRepository.AnyAsync(x => !x.DeleteFlag && x.Id != scoreLevel.ID && x.Name == scoreLevel.Name))
            {
                throw Oops.Oh("积分等级名称重复").StatusCode(Consts.Consts.ExceptionStatus);
            }

            var levelEntity = scoreLevel.Adapt<ScoreLevel>();
            var entityEntry = await _scoreLevelRepository.InsertAsync(levelEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑等级
        /// </summary>
        /// <param name="scoreLevel"></param>
        /// <returns></returns>
        public async Task<Guid> Update(ScoreLevelDto scoreLevel)
        {
            var levelEntity = await _scoreLevelRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == scoreLevel.ID);
            if (levelEntity == null)
            {
                throw Oops.Oh("未找到积分等级数据").StatusCode(Consts.Consts.ExceptionStatus);
            }

            if (await _scoreLevelRepository.AnyAsync(x => !x.DeleteFlag && x.Id != scoreLevel.ID && x.ArchiveScore == scoreLevel.ArchiveScore))
            {
                throw Oops.Oh("该目标积分已设置等级").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (await _scoreLevelRepository.AnyAsync(x => !x.DeleteFlag && x.Id != scoreLevel.ID && x.Name == scoreLevel.Name))
            {
                throw Oops.Oh("积分等级名称重复").StatusCode(Consts.Consts.ExceptionStatus);
            }

            levelEntity = scoreLevel.Adapt(levelEntity);
            levelEntity.UpdateTime = DateTime.Now;
            var entityEntry = await _scoreLevelRepository.UpdateAsync(levelEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 删除等级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var levelEntity = await _scoreLevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (levelEntity == null)
            {
                throw Oops.Oh("未找到积分等级数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            levelEntity.DeleteFlag = true;
            levelEntity.UpdateTime = DateTime.Now;
            await _scoreLevelRepository.UpdateAsync(levelEntity);
            return true;
        }

        /// <summary>
        /// 设置是否显示积分等级
        /// </summary>
        /// <param name="showLevel"></param>
        /// <returns></returns>
        public async Task<bool> SetLevelShowStatus(ScoreLevelShowStatus showLevel)
        {
            var config = await _configRepository.FirstOrDefaultAsync(x => x!.DeleteFlag);
            if (config == null)
            {
                throw Oops.Oh("未找到配置数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            config.ShowLevel = showLevel.ShowLevel;
            config.UpdateTime = DateTime.Now;
            await _configRepository.UpdateAsync(config);
            return true;
        }

    }
}
