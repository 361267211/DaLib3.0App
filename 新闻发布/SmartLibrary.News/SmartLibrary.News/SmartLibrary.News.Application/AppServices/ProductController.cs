
using DotNetCore.CAP.Internal;
using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.News.EntityFramework.Core.DbContexts;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly NewsDbContext _storeDbContext;
        private readonly IRepository<NewsColumn> _newsColumnRepository;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        public ProductController(NewsDbContext storeDbContext, IRepository<NewsColumn> newsColumnRepository,
             IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository
            )
        {
            _storeDbContext = storeDbContext;
            // this.storeDbContext.Database.EnsureCreated();
            _storeDbContext.Database.Migrate();
            _newsColumnRepository = newsColumnRepository;
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
        }

        /// <summary>
        /// 初始话数据，本接口示范  向基础表插入基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async Task InitializeBasicData(string name)
        {
            #region 插入基础数据
            var gu = Guid.NewGuid();
            if (!_sysMenuPermissionRepository.Any())
                //系统菜单及权限
                _sysMenuPermissionRepository.Context.BulkInsert(
                      new[]
                      {
                          new SysMenuPermission{Id=Guid.NewGuid(),Pid="",Path="1",FullPath="1", Name="虚拟头部",Type=1,Router="路由地址",Component="组件地址",Permission="",Remark="虚拟头部",IsSysMenu = false,Visible=true },
                          //栏目管理
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path="1",FullPath="1-1", Name="栏目管理",Type=1,Router="路由地址",Component="/newsProgram",Permission="",Remark="栏目管理",IsSysMenu = false,Visible=true,Sort=1 },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="1",FullPath="1-1-1", Name="修改",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="修改",IsSysMenu = false,Visible=true },
                        //new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-1",Path="1",FullPath="1-1-1-1", Name="更新新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-update",Remark="更新新闻栏目API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-1",Path="1",FullPath="1-1-1-1", Name="获取当前栏目之外的其他栏目键值对API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:delivery-column-list-get",Remark="获取当前栏目之外的其他栏目键值对API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-1",Path="2",FullPath="1-1-1-2", Name="获取栏目指定不同源类型的用户API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:user-perimission-list-get",Remark="获取栏目指定不同源类型的用户API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="2",FullPath="1-1-2", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                        //new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-2",Path="1",FullPath="1-1-2-1", Name="删除新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-delete",Remark="删除新闻栏目API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="3",FullPath="1-1-3", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-3",Path="1",FullPath="1-1-3-1", Name="新增新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-add",Remark="新增新闻栏目API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="4",FullPath="1-1-4", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path="1",FullPath="1-1-4-1", Name="获取标签分组及新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-get-by-manager-id",Remark="获取标签分组及新闻栏目API",IsSysMenu = false,Visible=true },
                        //new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path="2",FullPath="1-1-4-2", Name="获取新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-get",Remark="获取新闻栏目API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path="2",FullPath="1-1-4-2", Name="获取后台检索新闻API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-content-get-by-search",Remark="获取后台检索新闻API",IsSysMenu = false,Visible=true },
                        //标签 目标归属于栏目的api
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="5",FullPath="1-1-5", Name="获取标签API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:/api/news/lable-info-get-by-type",Remark="获取标签API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="6",FullPath="1-1-6", Name="保存标签API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:lable-list-update",Remark="保存标签API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="7",FullPath="1-1-7", Name="添加新闻模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-template-add",Remark="添加新闻模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="8",FullPath="1-1-8", Name="获取新闻模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-template-get",Remark="获取新闻模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="9",FullPath="1-1-9", Name="获取单个新闻模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:single-news-template-get",Remark="获取单个新闻模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="10",FullPath="1-1-10", Name="更新新闻模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-template-update",Remark="更新新闻模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="11",FullPath="1-1-11", Name="删除新闻模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-template-delete",Remark="删除新闻模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="12",FullPath="1-1-12", Name="添加新闻头尾模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-body-template-add",Remark="添加新闻头尾模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="13",FullPath="1-1-13", Name="获取新闻头尾模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-body-template-get-by-type",Remark="获取新闻头尾模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="14",FullPath="1-1-14", Name="获取新闻头尾模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-body-template-get",Remark="获取新闻头尾模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="15",FullPath="1-1-15", Name="更新新闻头尾模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-body-template-update",Remark="更新新闻头尾模板API",IsSysMenu = false,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1",Path="16",FullPath="1-1-16", Name="删除新闻头尾模板API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-body-template-delete",Remark="删除新闻头尾模板API",IsSysMenu = false,Visible=true },
                        //应用设置
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path="2",FullPath="1-2", Name="应用设置",Type=1,Router="路由地址",Component="/newsSet",Permission="",Remark="应用设置",IsSysMenu = true,Visible=true,Sort=9999 },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2",Path="1",FullPath="1-2-1", Name="设置保存",Type=4,Router="路由地址",Component="组件地址",Permission="save",Remark="设置保存",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-1",Path="1",FullPath="1-2-1-1", Name="保存应用设置API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-settings-save",Remark="保存应用设置API",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2",Path="2",FullPath="1-2-2", Name="栏目权限修改",Type=4,Router="路由地址",Component="组件地址",Permission="permissionedit",Remark="栏目权限修改",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-2",Path="1",FullPath="1-2-2-1", Name="栏目权限保存API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:save-news-column-permissions",Remark="栏目权限保存API",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-2",Path="2",FullPath="1-2-2-2", Name="获取全部栏目及其管理者API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-permissions-list-get",Remark="获取全部栏目及其管理者API",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-2",Path="3",FullPath="1-2-2-3", Name="检索获取权限管理员API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:search-permission-manager",Remark="检索获取权限管理员API",IsSysMenu = true,Visible=true },
                         new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-2",Path="4",FullPath="1-2-2-4", Name="获取栏目某一权限管理员列表API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-permissions-get",Remark="获取栏目某一权限管理员列表API",IsSysMenu = true,Visible=true },
                         new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-2",Path="5",FullPath="1-2-2-5", Name="获取栏目对应的权限设置API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-column-permissions-by-columnid-get",Remark="获取栏目对应的权限设置API",IsSysMenu = true,Visible=true },

                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2",Path="3",FullPath="1-2-3", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = true,Visible=true },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-2-3",Path="1",FullPath="1-2-3-1", Name="获取应用设置API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:news-settings-get",Remark="获取应用设置API",IsSysMenu = true,Visible=true },
                        //前台接口
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path="3",FullPath="1-3", Name="前台接口",Type=1,Router="路由地址",Component="组件地址",Permission="",Remark="前台接口",IsSysMenu = false,Visible=false },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-3",Path="1",FullPath="1-3-1", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="",Remark="查询",IsSysMenu = false,Visible=false },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-3-1",Path="1",FullPath="1-3-1-1", Name="获取前台新闻栏目数据API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:pront-news-column-list-get",Remark="获取前台新闻栏目数据API",IsSysMenu = false,Visible=false },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-3-1",Path="2",FullPath="1-3-1-2", Name="获取前台新闻列表内容数据API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:pront-news-list-data-get",Remark="获取前台新闻列表内容数据API",IsSysMenu = false,Visible=false },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-3-1",Path="3",FullPath="1-3-1-3", Name="获取前台内容数据API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:pront-news-content-get",Remark="获取前台内容数据API",IsSysMenu = false,Visible=false },
                        new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-3-1",Path="4",FullPath="1-3-1-4", Name="获取场景新闻信息API",Type=5,Router="路由地址",Component="组件地址",Permission="api:news:pront-scenes-news",Remark="获取场景新闻信息API",IsSysMenu = false,Visible=false },

                       });
            //系统角色
            var roleId = Guid.NewGuid();
            if (!_sysRoleRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{
                       new SysRole{ Name="测试员",Remark="测试员",Id=roleId,Code="sys_tester_role" },

                     });
            //角色-用户关系
            if (!_sysUserRoleRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{
                       new SysUserRole{ Id=Guid.NewGuid(),RoleID=roleId,UserID="cqu_vipsmart00001" },

                     });
            //角色-菜单权限 关系
            if (!_sysRoleMenuRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{

                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("784FBA89-42CB-0D4C-2601-D7BE7DC6A4BA") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("34122400-7AEB-4BFD-4EE8-008FA6683450") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("C5E150FF-4490-357E-7CD1-3E1ED43D0E58") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("41AC3AA0-2A1E-58C5-0511-CD91A8744880") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("509FF994-4601-539B-3B89-F2ABDA27B4BC") },
                     });
            #endregion

        }
    }
}
