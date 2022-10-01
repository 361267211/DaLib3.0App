﻿/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Grpc.Core;
using Mapster;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibraryNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    public class NavigationGrpc : NavigationGrpcService.NavigationGrpcServiceBase
    {
        private readonly IContentService _contentService;
        private readonly INavigationCatalogueService _catalogueService;

        public NavigationGrpc(
             IContentService contentService
            , INavigationCatalogueService catalogueService
            )
        {
            _contentService = contentService;
            _catalogueService = catalogueService;
        }

        public async override Task<NavigationListReply> GetNavigationList(NavigationListRequest request, ServerCallContext context)
        {
            NavigationListReply reply = new NavigationListReply();
            List<NavigationCatalogueDto> list = _catalogueService.GetCataListByColId(colId: request.Id);
            reply.NavigationList.AddRange(list.Adapt<List<NavCataListSingleItem>>());
            return reply;
        }
    }
}
