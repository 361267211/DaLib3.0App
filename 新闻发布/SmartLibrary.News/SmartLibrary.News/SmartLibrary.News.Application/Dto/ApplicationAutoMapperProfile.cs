using AutoMapper;
using SmartLibrary.News.Common.Const;
using SmartLibrary.News.Common.Dtos;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：ApplicationAutoMapperProfile
    /// 作    者：张泽军
    /// 创建时间：2021/9/8 16:27:53
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<NewsColumn, NewsColumnDto>();
            CreateMap<NewsColumnDto, NewsColumn>();
            CreateMap<NewsContent, NewsContentDto>()
                .ForMember(dest => dest.Content, src => src.MapFrom(e => e.Content.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace)));


            CreateMap<NewsContentDto, NewsContent>()
                .ForMember(dest => dest.Content, src => src.MapFrom(e => e.Content.AsSpanReplace(SiteGlobalConfig.FileServerReplace, PlaceholderKeys.FileServer)));
            CreateMap<NewsColumnPermissions, NewsColumnPermissionsDto>();
            CreateMap<NewsColumnPermissionsDto, NewsColumnPermissions>();
            CreateMap<NewsSettings, NewsSettingsDto>();
            CreateMap<NewsSettingsDto, NewsSettings>();
            CreateMap<LableInfo, LableInfoDto>();
            CreateMap<LableInfoDto, LableInfo>();
            CreateMap<NewsContentExpend, NewsContentExpendDto>();
            CreateMap<NewsContentExpendDto, NewsContentExpend>();
            CreateMap<NewsBodyTemplate, NewsBodyTemplateDto>();
            CreateMap<NewsBodyTemplateDto, NewsBodyTemplate>();
            CreateMap<NewsTemplate, NewsTemplateDto>();
            CreateMap<NewsTemplateDto, NewsTemplate>();
        }

    }
}
