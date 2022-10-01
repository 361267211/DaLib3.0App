/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Mapster;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibraryNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application
{
    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<HeadTemplateSetting, HeadTemplateSettingDto>()
                .Map(dest => dest.DisplayNavColumn, src => src.DisplayNavColumn.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .Map(dest => dest.Id, src => src.Id.ToString())
                ;
            config.ForType<HeadTemplateSettingDto, HeadTemplateSetting>()
                .Map(dest => dest.DisplayNavColumn, src => string.Join(';', src.DisplayNavColumn ?? new List<string>()))
                .Map(dest => dest.Id, src => src.Id??Guid.NewGuid().ToString())
                ;

            config.ForType<FootTemplateSetting, FootTemplateSettingDto>()
                .Map(dest => dest.JsPath, src => src.JsPath.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .Map(dest => dest.Id, src => src.Id.ToString())
                ;
            config.ForType<FootTemplateSettingDto, FootTemplateSetting>()
                .Map(dest => dest.JsPath, src => string.Join(';', src.JsPath ?? new List<string>()))
                .Map(dest => dest.Id, src => src.Id ?? Guid.NewGuid().ToString())
                ;

            config.ForType<NavCataListSingleItem, HeaderNavigationViewModel>()
 
                .Map(dest => dest.NavigationName, src => src.Name)
                .Map(dest => dest.NavigationIcon, src => src.Icon)
                .Map(dest => dest.NavigationUrl, src => src.Url)
                .Map(dest => dest.IsOpenNewWindow, src => src.IsOpenNewWindow)

                ;
             
        }
    }
}
