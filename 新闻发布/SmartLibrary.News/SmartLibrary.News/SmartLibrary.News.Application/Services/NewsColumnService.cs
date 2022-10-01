using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartLibrary.AppCenter;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Dtos.Cap;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.User.RpcService;
using SmartLibraryNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartLibrary.News.Application.Services
{
    /// <summary>
    /// 名    称：NewsService
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 19:29:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnService : INewsColumnService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<LableInfo> _lableRepository;
        private IRepository<NewsColumn> _newsColumnRepository;
        private IRepository<NewsContent> _newsContentRepository;
        private IRepository<NewsColumnPermissions> _permissionRepository;
        private INewsSettingsService _newsSettingsService;
        private ILableInfoService _lableService;
        private INewsContentService _newsContentService;
        private readonly IRepository<SysMenuPermission> _sysMenuPermRepository;
        private TenantInfo _tenantInfo;
        private readonly int UserRole = 3;//1 管理者，2 操作者，3 浏览者

        public NewsColumnService(ICapPublisher capPublisher,
            IRepository<LableInfo> lableRepository,
            IRepository<NewsColumn> newsColumnRepository,
            IRepository<NewsContent> newsContentRepository,
            IRepository<NewsContentExpend> newsConExpRepository,
            INewsSettingsService newsSettingsService,
            ILableInfoService lableService,
            INewsContentService newsContentService,
            IRepository<NewsColumnPermissions> permissionRepository,
            IRepository<SysMenuPermission> sysMenuPermRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _lableRepository = lableRepository;
            _newsColumnRepository = newsColumnRepository;
            _newsContentRepository = newsContentRepository;
            _newsSettingsService = newsSettingsService;
            _lableService = lableService;
            _newsContentService = newsContentService;
            _permissionRepository = permissionRepository;
            _sysMenuPermRepository = sysMenuPermRepository;
            _tenantInfo = tenantInfo;
            //UserRoleReply userRole = new UserRoleReply();
            ////调用grpc，获取角色信息，
            //userRole = App.GetService<IGrpcClientResolver>().EnsureClient<UserRoleGrpcService.UserRoleGrpcServiceClient>().GetUserRole(new UserRoleRequest {  UserKey = _tenantInfo.UserKey.ToString() });
            //UserRole = userRole.UserRole;

            //模拟假数据TODO：GRPC调用正常后删除
            UserRole = 2;
        }

        #region NewsColumn 栏目管理
        ///// <summary>
        ///// 获取新闻栏目
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="callContext"></param>
        ///// <returns></returns>
        //public async override Task<NewsColumnReply> GetNewsColumn(NewsColumnRequest request, ServerCallContext callContext = null)
        //{
        //    var result = _newsColumnRepository.Entities.First(d => !d.DeleteFlag && d.Id.ToString() == request.PlateId &&
        //    string.IsNullOrEmpty(request.SearchKey) ? true : (d.Title.Contains(request.SearchKey) || d.Label.Contains(request.SearchKey))
        //    );
        //    var replay = new NewsColumnReply
        //    {
        //        Id = result.Id.ToString(),
        //        Title = result.Title,
        //        Label = result.Label
        //    };

        //    return await Task.FromResult(replay);
        //}
        /// <summary>
        /// 获取标签分组及新闻栏目
        /// </summary>
        /// <returns></returns>
        public async Task<List<LableNewsColumnView>> GetLableNewsColumnList()
        {
            var managerID = _tenantInfo.UserKey;
            List<LableNewsColumnView> list = new List<LableNewsColumnView>();
            var result = _lableRepository.Where(d => !d.DeleteFlag).ToList();
            LableNewsColumnView lableNewsDefault = new LableNewsColumnView();
            lableNewsDefault.LableID = "";
            lableNewsDefault.LableName = "默认标签";
            var columnListDefault = from col in _newsColumnRepository.Where(d => !d.DeleteFlag && string.IsNullOrEmpty(d.Label)).AsQueryable()
                                    join perm in _permissionRepository.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(managerID) ? true : d.ManagerID == managerID)) on col.Id equals perm.ColumnID into res
                                    from labNC in res.DefaultIfEmpty()
                                    select new LableNewsColumn { ColumnID = col.Id, Title = col.Title, Enable = (string.IsNullOrEmpty(managerID) ? true : labNC.ManagerID == managerID) };
            lableNewsDefault.ColumnList = columnListDefault.Distinct().ToList();
            if (columnListDefault.Count() > 0)
                list.Add(lableNewsDefault);
            foreach (var item in result)
            {
                LableNewsColumnView lableNews = new LableNewsColumnView();
                lableNews.LableID = item.Id;
                lableNews.LableName = item.Name;
                var columnList = from col in _newsColumnRepository.Where(d => !d.DeleteFlag && d.Label.Contains(item.Id)).AsQueryable()
                                 join perm in _permissionRepository.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(managerID) ? true : d.ManagerID == managerID)) on col.Id equals perm.ColumnID into res
                                 from labNC in res.DefaultIfEmpty()
                                 select new LableNewsColumn { ColumnID = col.Id, Title = col.Title, Enable = (string.IsNullOrEmpty(managerID) ? true : labNC.ManagerID == managerID) };
                lableNews.ColumnList = columnList.Distinct().ToList();
                if (columnList.Count() > 0)
                    list.Add(lableNews);
            }
            return await Task.FromResult(list);
        }


        /// <summary>
        /// 获取新闻栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        public async Task<NewsColumnDto> GetNewsColumn(string columnID)
        {
            var result = _newsColumnRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            if (result == null)
                return null;
            var item = result.ToModel<NewsColumnDto>();


            var itemLables = item.Label.Split(';');
            var lables = from lab in _lableRepository.Where(d => itemLables.Contains(d.Id))
                         select new KeyValuePair<string, string>(lab.Id, lab.Name);
            item.LabelKV = lables.ToList();
            //特殊处理 ID串转标签名串
            item.Label = string.Join(";", lables.Select(d => d.Value));
            List<KeyValuePair<int, string>> contentDefaultAuditStatusKV = new List<KeyValuePair<int, string>>();
            if (item.IsOpenAudit == 1)//开启审核流程
            {
                //开启审核流程下，新增新闻只能是待提交或勾选审核流程第一步审核状态
                //todo
                contentDefaultAuditStatusKV.Add(new KeyValuePair<int, string>(((int)AuditStatusEnum.UnSubmit), "保存"));
                //保存的下一步是提交！！
                var auditStatusInOrder = item.AuditFlow.Split(';').Select(d => Converter.ObjectToInt(d)).OrderBy(d => d).ToList();

                var curAuditStatus = auditStatusInOrder.FirstOrDefault();
                var nextAuditStatus = auditStatusInOrder.Skip(1).FirstOrDefault();
                // var nextAuditStatus = item.AuditFlow.Split(';').Select(d => Converter.ObjectToInt(d)).OrderBy(d => d).FirstOrDefault(d => d == ((int)AuditStatusEnum.UnSubmit));



                nextAuditStatus = nextAuditStatus > 1 ? nextAuditStatus - 1 : nextAuditStatus;//审核状态和审核流程差值1
                contentDefaultAuditStatusKV.Add(new KeyValuePair<int, string>(nextAuditStatus, "保存并" + EnumUtils.GetDescription(Converter.ToType<AuditStatusEnum>(curAuditStatus))));
            }
            else
            {
                //关闭审核流程下，新增新闻只能是待发布或已发布
                contentDefaultAuditStatusKV.Add(new KeyValuePair<int, string>(((int)AuditStatusEnum.UnPublish), "保存"));
                contentDefaultAuditStatusKV.Add(new KeyValuePair<int, string>(((int)AuditStatusEnum.Published), "保存并" + EnumUtils.GetDescription(AuditStatusEnum.Published)));
            }
            item.ContentDefaultAuditStatusKV = contentDefaultAuditStatusKV;
            //item.SideListKV = EnumUtils.GetValueName(typeof(SideListEnum), item.SideList);
            //item.SysMesListKV = EnumUtils.GetValueName(typeof(SysMesListEnum), item.SysMesList);
            //item.AuditFlowKV = EnumUtils.GetValueName(typeof(AuditProcessEnum), item.AuditFlow);
            return await Task.FromResult(item);
        }

        /// <summary>
        /// 获取当前栏目之外的其他栏目键值对
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetDeliveryColumnList(string columnID)
        {
            var list = from col in _newsColumnRepository.Where(d => !d.DeleteFlag && d.Id != columnID)
                       select new KeyValuePair<string, string>
                       (col.Id, col.Title);
            return await Task.FromResult(list.ToList());
        }

        /// <summary>
        /// 添加新闻栏目
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNewsColumn(NewsColumnDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            if (model.Status != 1 && model.Status != 2)
            {
                result.Succeeded = false;
                result.Message = "栏目状态不存在";
                return result;
            }
            if (!string.IsNullOrEmpty(model.SideList))
            {
                int[] sideList = model.SideList.Split(';').Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sideList)
                {
                    SideListEnum? temp = Converter.ToType<SideListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "侧边列表选择项不存在";
                        return result;
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.SysMesList))
            {
                int[] sysMesList = model.SysMesList.Split(';').Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sysMesList)
                {
                    SysMesListEnum? temp = Converter.ToType<SysMesListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "显示系统信息选择项不存在";
                        return result;
                    }
                }
            }
            if (model.IsOpenAudit == 1)
            {
                int[] auditFlow = model.AuditFlow.Split(';').Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                if (!auditFlow.Contains(((int)AuditProcessEnum.Edit)) || !auditFlow.Contains(((int)AuditProcessEnum.Publish)))
                {
                    result.Succeeded = false;
                    result.Message = "编辑和发布审核流程选择项必须存在";
                    return result;
                }
                foreach (var item in auditFlow)
                {
                    AuditProcessEnum? temp = Converter.ToType<AuditProcessEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "审核流程选择项不存在";
                        return result;
                    }
                }
            }

            var settings = _newsSettingsService.GetNewsSettings();
            if (settings.Result.Comments == 0)
            {
                model.IsOpenComment = 0;
            }

            var newsCL = model.ToModel<NewsColumn>();
            newsCL.CreatedTime = DateTime.Now;

            newsCL.Label = await _lableService.ProcessLablesFromLableStr(1, newsCL.Label);
            var newsColumn = await _newsColumnRepository.InsertNowAsync(newsCL);
            _newsContentService.ProcessNewsContentExpend(newsColumn.Entity.Id, newsColumn.Entity.Extension, "");

            #region 新增栏目菜单权限
            var columnID = newsCL.Id;
            var columnName = newsCL.Title;

            var maxPath = _sysMenuPermRepository.Where(d => d.Pid == "1").Max(d => d.Path);
            var maxPathID = Converter.ObjectToInt(maxPath) + 1;
            var parentFullPath = $"1-{maxPathID}";
            //栏目管理
            _sysMenuPermRepository.Context.BulkInsert(
               new[]
               {
                    new SysMenuPermission { Id = Guid.NewGuid(), Pid = "1-1-1", Path = maxPathID.ToString(), FullPath = $"1-1-1-{maxPathID}", Name = "更新新闻栏目API", Type = 5, Router = "路由地址", Component = "组件地址", Permission = $"api:news:news-column-update:{columnID}", Remark = "更新新闻栏目API", IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-2",Path = maxPathID.ToString(), FullPath = $"1-1-2-{maxPathID}", Name="删除新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-delete:{columnID}",Remark="删除新闻栏目API",IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path=maxPathID.ToString(),FullPath=$"1-1-4-{maxPathID}", Name="获取新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-get:{columnID}",Remark="获取新闻栏目API",IsSysMenu = false,Visible=true },
               }
           );

            //当前栏目
            _sysMenuPermRepository.Context.BulkInsert(
                new[]
                {
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path=maxPathID.ToString(),FullPath=parentFullPath, Name=columnName,Type=1,Router="路由地址",Component=$"/admin_programInfo?id={columnID}",Permission=$"column:{columnID}",Remark=columnName,IsSysMenu = false,Visible=true,Sort=maxPathID },
                     //*-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="1",FullPath=$"{parentFullPath}-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="1",FullPath=$"{parentFullPath}-1-1", Name="添加新闻内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-add:{columnID}",Remark="添加新闻内容API",IsSysMenu = false,Visible=true },
                      //*-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="2",FullPath=$"{parentFullPath}-2", Name="下架",Type=4,Router="路由地址",Component="组件地址",Permission="offshelf",Remark="下架",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="1",FullPath=$"{parentFullPath}-2-1", Name="批量下架内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-off-shelf:{columnID}",Remark="批量下架内容API",IsSysMenu = false,Visible=true },
                      //*-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="3",FullPath=$"{parentFullPath}-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-3",Path="1",FullPath=$"{parentFullPath}-3-1", Name="批量删除内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-delete:{columnID}",Remark="批量删除内容API",IsSysMenu = false,Visible=true },
                      //*-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="4",FullPath=$"{parentFullPath}-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-4",Path="1",FullPath=$"{parentFullPath}-4-1", Name="修改新闻内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-update:{columnID}",Remark="修改新闻内容API",IsSysMenu = false,Visible=true },
                      //*-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="5",FullPath=$"{parentFullPath}-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-5",Path="1",FullPath=$"{parentFullPath}-5-1", Name="内容弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:sort-content-by-index:{columnID}",Remark="内容弹窗输入序号排序API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-5",Path="2",FullPath=$"{parentFullPath}-5-2", Name="内容拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:sort-content-by-target:{columnID}",Remark="内容拖动排序API",IsSysMenu = false,Visible=true },
                      //*-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="6",FullPath=$"{parentFullPath}-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="1",FullPath=$"{parentFullPath}-6-1", Name="获取新闻内容列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-get-by-column:{columnID}",Remark="获取新闻内容列表API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="2",FullPath=$"{parentFullPath}-6-2", Name="获取后台新闻详情API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-manage-get:{columnID}",Remark="获取后台新闻详情API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="3",FullPath=$"{parentFullPath}-6-3", Name="获取内容操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:content-process-log-get:{columnID}",Remark="获取内容操作日志API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="4",FullPath=$"{parentFullPath}-6-4", Name="获取后台新闻栏目状态集合以及新闻内容标签集合API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-content-manage-get:{columnID}",Remark="获取后台新闻栏目状态集合以及新闻内容标签集合API",IsSysMenu = false,Visible=true },
                      //*-7 审核
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="7",FullPath=$"{parentFullPath}-7", Name="审核",Type=3,Router="路由地址",Component="组件地址",Permission="audit",Remark="审核",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-7",Path="1",FullPath=$"{parentFullPath}-7-1", Name="更新新闻内容审核状态API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-update-audit-status:{columnID}",Remark="更新新闻内容审核状态API",IsSysMenu = false,Visible=true },
                }
            );
            #endregion

            #region 同步到应用中心
            await AppColumnOperation(newsCL.Id.ToString(), newsCL.Title, newsCL.CreatedTime.ToString(), 1, "/admin_newsProgram", "news");

            #endregion

            return result;
        }

        /// <summary>
        /// 同步栏目信息到应用中心
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="columnName"></param>
        /// <param name="createTime"></param>
        /// <param name="operationType"></param>
        /// <param name="visitUrl"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        public async Task AppColumnOperation(string columnId, string columnName, string createTime, int operationType, string visitUrl, string appRouteCode)
        {
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            AppColumnOperationRequest request1 = new AppColumnOperationRequest { AppRouteCode = appRouteCode, ColumnId = columnId, ColumnName = columnName, CreateTime = createTime, OperationType = operationType, VisitUrl = visitUrl, };
            AppColumnOperationReply reply1 = new AppColumnOperationReply();
            try
            {
                reply1 = await grpcClient1.AppColumnOperationAsync(request1);
                return;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc调用异常");
            }
        }


        /// <summary>
        /// 更新新闻栏目
        /// </summary>
        /// <param name="model"></param>
        public async Task<ApiResultInfoModel> UpdateNewsColumn(NewsColumnDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            if (model.Status != 1 && model.Status != 2)
            {
                result.Succeeded = false;
                result.Message = "栏目状态不存在";
                return result;
            }
            if (!string.IsNullOrEmpty(model.SideList))
            {
                int[] sideList = model.SideList.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sideList)
                {
                    SideListEnum? temp = Converter.ToType<SideListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "侧边列表选择项不存在";
                        return result;
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.SideList))
            {
                int[] sysMesList = model.SysMesList.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sysMesList)
                {
                    SysMesListEnum? temp = Converter.ToType<SysMesListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "显示系统信息选择项不存在";
                        return result;
                    }
                }
            }
            if (model.IsOpenAudit == 1)
            {
                int[] auditFlow = model.AuditFlow.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                if (!auditFlow.Contains(((int)AuditProcessEnum.Edit)) || !auditFlow.Contains(((int)AuditProcessEnum.Publish)))
                {
                    result.Succeeded = false;
                    result.Message = "编辑和发布审核流程选择项必须存在";
                    return result;
                }
                foreach (var item in auditFlow)
                {
                    AuditProcessEnum? temp = Converter.ToType<AuditProcessEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "审核流程选择项不存在";
                        return result;
                    }
                }
            }
            var newsClOld = _newsColumnRepository.Entities.AsNoTracking().FirstOrDefault(d => d.Id == model.Id);
            if (newsClOld == null)
            {
                result.Succeeded = false;
                result.Message = "栏目不存在！";
                return result;
            }

            var newsCL = model.ToModel<NewsColumn>();
            newsCL.UpdatedTime = DateTime.Now;
            newsCL.Label = _lableService.ProcessLablesFromLableStr(1, newsCL.Label).Result;
            var newsColumn = await _newsColumnRepository.UpdateExcludeAsync(newsCL, new[] { nameof(newsCL.CreatedTime), nameof(newsCL.TenantId) });
            _newsContentService.ProcessNewsContentExpend(newsCL.Id, newsCL.Extension, newsClOld.Extension);
            await _newsContentService.ResetAuditStatusNewsContent(newsCL.Id);
            //从开启更新为关闭 1. 删除栏目全部非管理的权限管理 2.变更所有未发布的新闻内容为已发布且已下架
            if (newsClOld.IsOpenAudit == 1 && newsCL.IsOpenAudit == 0)
            {
                await _newsSettingsService.DeleteNewsColumnPermissionsByColumnID(newsCL.Id);
                await _newsContentService.PublishOffShelfNewsContent(newsCL.Id);
            }

            #region 同步到应用中心
            await AppColumnOperation(newsCL.Id.ToString(), newsCL.Title, newsCL.CreatedTime.ToString(), 2, "/admin_newsProgram", "news");

            #endregion

            return result;
        }



        /// <summary>
        /// 删除新闻栏目
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsColumn(string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _newsColumnRepository.FindOrDefault(columnID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "栏目不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _newsColumnRepository.UpdateAsync(model);

            //删除菜单页            
            await _sysMenuPermRepository.Context.BatchUpdate<SysMenuPermission>()
                 .Set(b => b.DeleteFlag, b => true)
                 .Where(e => e.Permission.EndsWith(columnID))
                 .ExecuteAsync();

            #region 同步到应用中心
            await AppColumnOperation(columnID, "", "", 3, "/admin_newsProgram", "news");

            #endregion

            return result;
        }

        /// <summary>
        /// 删除新闻栏目
        /// </summary>
        /// <param name="columnIDs"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsColumn(string[] columnIDs)
        {
            var result = new ApiResultInfoModel { Succeeded = true };

            foreach (var columnID in columnIDs)
            {
                var model = _newsColumnRepository.FindOrDefault(columnID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = "栏目不存在！";
                    return result;
                }
                model.DeleteFlag = true;
                model.UpdatedTime = DateTime.Now;
                //此处事务必须用带Now的执行方法
                await _newsColumnRepository.UpdateNowAsync(model);
            }
            return result;
        }

        /// <summary>
        /// 获取前台新闻栏目数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<ProntNewsColumnListView>> GetProntNewsColumnList(string columnID)
        {
            var thisColumn = _newsColumnRepository.FindOrDefault(columnID);
            //左侧显示同标签栏目
            bool isSameLableColumn = thisColumn.SideList.Contains(((int)SideListEnum.SameLableColumn).ToString());
            //新闻栏目
            List<ProntNewsColumnListView> columnList = new List<ProntNewsColumnListView>();
            List<NewsColumn> newsClList = new List<NewsColumn>();
            //同标签的栏目
            if (isSameLableColumn)
            {
                var columnLables = thisColumn.Label.Split(';');
                foreach (var item in columnLables)
                {
                    var sameLableColumn = _newsColumnRepository.Where(d => !d.DeleteFlag&&  d.Label.Contains(item));
                    newsClList.AddRange(sameLableColumn);
                }
            }
            else
                newsClList.Add(thisColumn);

            foreach (var column in newsClList.Distinct())
            {
                //左侧显示新闻标签
                bool isSameColumnNewsLable = column.SideList.Contains(((int)SideListEnum.SameColumnNewsLable).ToString());
                //显示新闻标签
                if (isSameColumnNewsLable)
                {
                    var query = from gas in _newsContentRepository.Entities.Where(d => !d.DeleteFlag && d.ColumnIDs.Contains(column.Id) || d.ColumnID == column.Id).AsQueryable()
                                group gas by new { gas.ParentCatalogue } into dateGroup
                                select new { Key = dateGroup.Key.ParentCatalogue, Counts = dateGroup.Count() };
                    List<string> lableIDs = new List<string>();
                    foreach (var item in query)
                    {
                        lableIDs.AddRange(item.Key.Split(';').Select(d => d).ToList());
                    }
                    var contentLables = _lableRepository.Where(d => !d.DeleteFlag && lableIDs.Contains(d.Id)).Select(d => new { d.Id, d.Name });
                    List<KeyValuePair<string, string>> lableList = new List<KeyValuePair<string, string>>();
                    foreach (var item in contentLables)
                    {
                        lableList.Add(new KeyValuePair<string, string>(item.Id, item.Name));
                    }
                    columnList.Add(new ProntNewsColumnListView
                    {
                        ColumnID = column.Id,
                        Name = column.Title,
                        LableList = lableList,
                        DefaultTemplate = column.DefaultTemplate,
                    });
                }
                else
                {
                    columnList.Add(new ProntNewsColumnListView
                    {
                        ColumnID = column.Id,
                        Name = column.Title,
                        DefaultTemplate = column.DefaultTemplate,
                    });
                }
            }
            columnList= columnList.OrderBy(e => e.Name).ToList();
            return await Task.FromResult(columnList);
        }

        /// <summary>
        /// 获取管理员具有发布权限的栏目id-Name键值对
        /// </summary>
        /// <param name="managerID">管理员ID 可空 空则查询全部</param>
        /// <param name="counts">返回个数</param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetPublishColumnList(string managerID, int counts)
        {
            var result = from col in _newsColumnRepository.Where(d => !d.DeleteFlag)
                         join perm in _permissionRepository.Where(d => !d.DeleteFlag) on col.Id equals perm.ColumnID
                         where (string.IsNullOrEmpty(managerID) ? true : perm.ManagerID == managerID) && (perm.Permission == ((int)AuditPowerEunm.Publish) || perm.Permission == ((int)AuditPowerEunm.Manage))
                         select new KeyValuePair<string, string>(col.Id, col.Title);

            return await Task.FromResult(result.Take(counts).ToList());
        }

        /// <summary>
        /// 获取新闻栏目总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAllNewsColumnCount()
        {
            var result = await _newsColumnRepository.CountAsync(d => !d.DeleteFlag);

            return await Task.FromResult(result);
        }







        /// <summary>
        /// 获取栏目指定不同源类型的用户
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetUserPermissionList(int type)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();








            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            //TODO 调用用户接口获取对应不同用户
            //1 按类型，2 按分组，3 按标签
            if (type == 1)
            {
                try
                {
                    //调用grpc，获取用户类型，
                    var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
                    SimpleTableQuery query = new SimpleTableQuery { KeyWord = "", PageIndex = 1, PageSize = 100 };
                    var grpcUserTypes = await grpcClient.GetUserTypeListAsync(query);

                    foreach (var item in grpcUserTypes.Items)
                    {
                        result.Add(new KeyValuePair<string, string>(item.Value, item.Key));

                    }
                }
                catch (Exception ex)
                {
                    throw Oops.Oh(ex.Message);

                }
            }
            else if (type == 2)
            {
                try
                {
                    //调用grpc，获取用户分组
                    var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
                    SimpleTableQuery query = new SimpleTableQuery { KeyWord = "", PageIndex = 1, PageSize = 100 };
                    var grpcUserGroups = await grpcClient.GetUserGroupListAsync(query);

                    foreach (var item in grpcUserGroups.Items)
                    {
                        result.Add(new KeyValuePair<string, string>(item.Value, item.Key));

                    }

                }
                catch (Exception ex)
                {

                    throw Oops.Oh(ex.Message);

                }
            }
            //else if (type == 3)
            //{
            //    result.Add(new KeyValuePair<string, string>("lable1", "标签1"));
            //    result.Add(new KeyValuePair<string, string>("lable2", "标签2"));
            //    result.Add(new KeyValuePair<string, string>("lable3", "标签3"));
            //    result.Add(new KeyValuePair<string, string>("lable4", "标签4"));
            //}
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取专题栏目信息 根据id
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public async Task<GetColumnLinkInfoReply> GetColumnLinkInfo(string columnId)
        {
            var column = _newsColumnRepository.FirstOrDefault(e => e.Id == columnId && !e.DeleteFlag);

            return column.Adapt<GetColumnLinkInfoReply>();

        }
        #endregion
    }
}
