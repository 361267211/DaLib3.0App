/*********************************************************
* 名    称：RoleService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户角色服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Permission;
using SmartLibrary.User.Application.Dtos.Role;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户角色服务
    /// </summary>
    public class RoleService : IRoleService, IScoped
    {
        private readonly Base64Crypt _baseEncrypt;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<Card> _cardRepository;

        public RoleService(IDistributedIDGenerator idGenerator
            , IRepository<SysRole> sysRoleRepository
            , IRepository<SysUserRole> sysUserRoleRepository
            , IRepository<SysRoleMenu> sysRoleMenuRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<Card> cardRepository)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            _baseEncrypt = new Base64Crypt(codeTable);
            _idGenerator = idGenerator;
            _sysRoleRepository = sysRoleRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _userRepository = userRepository;
            _cardRepository = cardRepository;
        }

        /// <summary>
        /// 查询用户服务
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<RoleListItemDto>> QueryRoleTableData(TableQueryBase queryFilter)
        {
            var userRoleQuery = from userRole in _sysUserRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userRole.UserID equals user.Id into users
                                from user in users
                                select new { userRole.RoleID, userRole.UserID };
            var roleQuery = from role in _sysRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                            .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.Name != null && x.Name != "" && x.Name.Contains(queryFilter.Keyword))
                            select new RoleListItemDto
                            {
                                ID = role.Id,
                                Name = role.Name,
                                Code = role.Code,
                                Remark = role.Remark,
                                SysBuildIn = role.SysBuildIn,
                                StaffCount = userRoleQuery.Where(ur => ur.RoleID == role.Id).Select(ur => ur.UserID).Distinct().Count(),
                                CreateTime = role.CreateTime
                            };
            var rolePageList = await roleQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return rolePageList;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        public async Task<Guid> CreateRole(RoleEditDto roleData)
        {
            roleData.ID = _idGenerator.CreateGuid(roleData.ID);
            var roleEntity = roleData.Adapt<SysRole>();
            var roleEntry = await _sysRoleRepository.InsertAsync(roleEntity);
            var roleMenu = roleData.MenuIds.Select(x => new SysRoleMenu
            {
                RoleID = roleData.ID,
                MenuPermissionID = x
            }).ToList();
            await _sysRoleMenuRepository.InsertAsync(roleMenu);
            return roleEntry.Entity.Id;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateRole(RoleEditDto roleData)
        {
            var roleEntity = await _sysRoleRepository.FirstOrDefaultAsync(x => x.Id == roleData.ID);
            if (roleEntity == null)
            {
                throw Oops.Oh("未找到角色信息");
            }
            if (roleEntity.DeleteFlag)
            {
                throw Oops.Oh("角色已删除");
            }
            roleEntity = roleData.Adapt(roleEntity);
            await _sysRoleRepository.UpdateAsync(roleEntity);
            var existMenus = await _sysRoleMenuRepository.Where(x => !x.DeleteFlag && x.RoleID == roleData.ID).ToListAsync();
            existMenus.ForEach(x => x.DeleteFlag = true);
            var roleMenu = roleData.MenuIds.Select(x => new SysRoleMenu
            {
                RoleID = roleData.ID,
                MenuPermissionID = x
            }).ToList();
            await _sysRoleMenuRepository.UpdateAsync(existMenus);
            await _sysRoleMenuRepository.InsertAsync(roleMenu);
            return roleEntity.Id;
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<RoleEditDto> GetRoleData(Guid roleId)
        {
            var roleEntity = await _sysRoleRepository.FirstOrDefaultAsync(x => x.Id == roleId);
            if (roleEntity == null)
            {
                throw Oops.Oh("未找到角色信息");
            }
            if (roleEntity.DeleteFlag)
            {
                throw Oops.Oh("角色已删除");
            }
            var menuIds = await _sysRoleMenuRepository.Where(x => !x.DeleteFlag && x.RoleID == roleId).Select(x => x.MenuPermissionID).ToListAsync();
            var roleData = roleEntity.Adapt<RoleEditDto>();
            roleData.MenuIds = menuIds;
            return roleData;
        }

        /// <summary>
        /// 通过编码获取角色
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public async Task<RoleEditDto> GetRoleDataByCode(string roleCode)
        {
            var roleEntity = await _sysRoleRepository.FirstOrDefaultAsync(x => x.Code == roleCode);
            if (roleEntity == null)
            {
                throw Oops.Oh("未找到角色信息");
            }
            if (roleEntity.DeleteFlag)
            {
                throw Oops.Oh("角色已删除");
            }
            var menuIds = await _sysRoleMenuRepository.Where(x => !x.DeleteFlag && x.RoleID == roleEntity.Id).Select(x => x.MenuPermissionID).ToListAsync();
            var roleData = roleEntity.Adapt<RoleEditDto>();
            roleData.MenuIds = menuIds;
            return roleData;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRole(Guid roleId)
        {
            var roleEntity = await _sysRoleRepository.FirstOrDefaultAsync(x => x.Id == roleId);
            if (roleEntity == null)
            {
                throw Oops.Oh("未找到角色信息");
            }
            if (roleEntity.DeleteFlag)
            {
                return true;
            }
            roleEntity.DeleteFlag = true;
            await _sysRoleRepository.UpdateAsync(roleEntity);
            return true;
        }

        /// <summary>
        /// 设置角色用户
        /// </summary>
        /// <param name="roleUserSet"></param>
        /// <returns></returns>
        public async Task<bool> AddRoleUsers(RoleUserSetDto roleUserSet)
        {
            var existRoleUsers = await _sysUserRoleRepository.Where(x => !x.DeleteFlag && x.RoleID == roleUserSet.RoleID && roleUserSet.UserIds.Contains(x.UserID)).ToListAsync();
            var existUserIds = existRoleUsers.Select(x => x.UserID).ToList();
            existUserIds.ForEach(x =>
            {
                if (roleUserSet.UserIds.Contains(x))
                    roleUserSet.UserIds.Remove(x);
            });
            var insertRoleUsers = roleUserSet.UserIds.Select(x => new SysUserRole
            {
                UserID = x,
                RoleID = roleUserSet.RoleID,
            }).ToList();
            await _sysUserRoleRepository.InsertAsync(insertRoleUsers);
            return true;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userRoleSet"></param>
        /// <returns></returns>
        public async Task<bool> SetUserRoles(UserRoleSetDto userRoleSet)
        {
            var existUserRoles = await _sysUserRoleRepository.Where(x => !x.DeleteFlag && x.UserID == userRoleSet.UserId).ToListAsync();
            existUserRoles.ForEach(x =>
            {
                x.DeleteFlag = true;
                x.UpdateTime = DateTime.Now;
            });
            await _sysUserRoleRepository.UpdateAsync(existUserRoles);
            var insertUserRoles = userRoleSet.RoleIds.Select(x => new SysUserRole
            {
                UserID = userRoleSet.UserId,
                RoleID = x
            }).ToList();
            await _sysUserRoleRepository.InsertAsync(insertUserRoles);
            return true;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<RoleListItemDto>> GetUserRoles(Guid userId)
        {
            var roleQuery = from userRole in _sysUserRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userId)
                            join role in _sysRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userRole.RoleID equals role.Id into roles
                            from role in roles
                            select new RoleListItemDto
                            {
                                ID = role.Id,
                                Name = role.Name,
                                Code = role.Code,
                                Remark = role.Remark,
                                SysBuildIn = role.SysBuildIn,
                                CreateTime = DateTime.Now
                            };
            var rolePageList = await roleQuery.OrderBy(x => x.CreateTime).ToListAsync();
            return rolePageList;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userRoleInfo"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserRole(UserRoleDeleteDto userRoleInfo)
        {
            Guid? roleId = null;
            try
            {
                roleId = new Guid(userRoleInfo.RoleID);
            }
            catch
            {
                roleId = null;
            }
            var roleInfo = await _sysRoleRepository
                .Where(x => !x.DeleteFlag)
                .Where(string.IsNullOrWhiteSpace(userRoleInfo.RoleCode), x => x.Code == userRoleInfo.RoleCode)
                .FirstOrDefaultAsync();
            var finalRoleId = roleId.HasValue ? roleId.Value : roleInfo?.Id;
            var userRole = await _sysUserRoleRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserID == userRoleInfo.UserID && x.RoleID == finalRoleId);
            if (userRole == null)
            {
                return true;
            }
            userRole.DeleteFlag = true;
            await _sysUserRoleRepository.UpdateAsync(userRole);
            return true;
        }

        /// <summary>
        /// 查询角色馆员信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<StaffListItemDto>> QueryStaffTableData(StaffRoleTableQuery queryFilter)
        {
            var roleCode = queryFilter.RoleCode;
            Guid? roleId = null;
            if (!string.IsNullOrWhiteSpace(roleCode))
            {
                var roleEntity = await _sysRoleRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Code == roleCode);
                roleId = roleEntity != null ? roleEntity.Id : _idGenerator.CreateGuid();
            }
            queryFilter = AdaptEncoder.EncodedFilter<StaffRoleTableQuery, StaffRoleEncodeTableQuery>(queryFilter);
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsStaff)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name != null && x.Name != "" && x.Name.Contains(queryFilter.Name))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Phone), x => x.Phone != null && x.Phone != "" && x.Phone.Contains(queryFilter.Phone))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo != "" && x.StudentNo.Contains(queryFilter.StudentNo))
                               .Where(roleId.HasValue, x => _sysUserRoleRepository.DetachedEntities.Any(ur => ur.RoleID == roleId && !ur.DeleteFlag && ur.UserID == x.Id))
                               .Where(queryFilter.RoleId.HasValue, x => _sysUserRoleRepository.DetachedEntities.Any(ur => ur.RoleID == queryFilter.RoleId && !ur.DeleteFlag && ur.UserID == x.Id))
                               .Where(queryFilter.ExcludeRoleId.HasValue, x => !_sysUserRoleRepository.DetachedEntities.Any(ur => ur.RoleID == queryFilter.ExcludeRoleId && !ur.DeleteFlag && ur.UserID == x.Id))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new StaffListItemDto
                                {
                                    ID = user.Id,
                                    Name = user.Name,
                                    StudentNo = user.StudentNo,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    Phone = user.Phone,
                                    CardExpireDate = card.ExpireDate,
                                    CardNo = card.No,
                                    CardStatus = card.Status,
                                    StaffBeginTime = user.StaffBeginTime,
                                };
            var pageList = await userCardQuery.OrderByDescending(x => x.StaffBeginTime)
                                              .ToPagedListAsync<StaffListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            //获取角色
            var userIds = pageList.Items.Select(x => x.ID).ToList();
            var userRolesQuery = from userRole in _sysUserRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID))
                                 join role in _sysRoleRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                 on userRole.RoleID equals role.Id into roles
                                 from role in roles
                                 select new
                                 {
                                     RoleId = role.Id,
                                     Name = role.Name,
                                     Code = role.Code,
                                     Remark = role.Remark,
                                     UserId = userRole.UserID
                                 };
            var userRoles = await userRolesQuery.Where(x => userIds.Contains(x.UserId)).ToListAsync();
            foreach (var item in pageList.Items)
            {
                item.Roles = userRoles.Where(x => x.UserId == item.ID).Select(x => new SysRoleDto
                {
                    Id = x.RoleId,
                    Name = x.Name,
                    Code = x.Code,
                    Remark = x.Remark
                }).ToList();
            }

            return pageList;
        }

    }
}
