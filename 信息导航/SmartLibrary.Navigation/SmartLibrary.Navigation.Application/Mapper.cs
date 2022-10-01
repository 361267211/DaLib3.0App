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
using SmartLibrary.AppCenter;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.Common.Const;
using SmartLibrary.Navigation.Common.Dtos;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.Utility;
using SmartLibrary.SceneManage;
using SmartLibraryNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.User.RpcService.UserPageData.Types;

namespace SmartLibrary.Navigation.Application
{
    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {


            //自定义标签->标准键值对
            config.ForType<HeaderFooterListSingle, NavigationTemplateDto>()
                 .Map(dest => dest.Id, src => src.Id)
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.PreviewPic, src => src.Cover)
                 ;

            //自定义标签->标准键值对
            config.ForType<NavigationColumn, NavigationColumnDto>()
                 .Map(dest => dest.UserGroups, src => GetListFromStr(src.UserGroups))
                 .Map(dest => dest.UserTypes, src => GetListFromStr(src.UserTypes))
                 .Map(dest => dest.LinkUrl, src => $"/#/web_list?c_id={src.Id}")
                 ;

            //自定义标签->标准键值对
            config.ForType<NavigationColumnDto, NavigationColumn>()
                 .Map(dest => dest.UserGroups, src => GetStrFromList(src.UserGroups))
                 .Map(dest => dest.UserTypes, src => GetStrFromList(src.UserTypes))
                 ;


            //自定义标签->标准键值对
            config.ForType<ContentDto, Content>()
                 .Map(dest => dest.Contents, src => src.Contents.AsSpanReplace(SiteGlobalConfig.FileServerReplace, PlaceholderKeys.FileServer))
                 .Map(dest => dest.ContentsText, src => StringUtils.FilterHTML(src.Contents))
                 ;

            //自定义标签->标准键值对
            config.ForType<Content, ContentDto>()
                 .Map(dest => dest.Contents, src => src.Contents.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace))
                 ;
            //自定义标签->标准键值对
            config.ForType<NavigationCatalogueDto, NavCataListSingleItem>()
                 .Map(dest => dest.Name, src => src.Title)
                 .Map(dest => dest.Url, src => src.ExternalLinks)
                 .Map(dest => dest.Icon, src => src.Cover)
                 .Map(dest => dest.IsOpenNewWindow, src => src.IsOpenNewWindow)
                 ;

            //自定义标签->标准键值对
            config.ForType<UserListItem, PermissionManagerInfo>()
                 .Map(dest => dest.CardNum, src => src.CardNo)
                 .Map(dest => dest.Depart, src => src.DepartName)
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.UserKey, src => src.Key)
                 ;

            //自定义标签->标准键值对
            config.ForType<NavigationColumn, GetColumnLinkInfoReply>()
                 .Map(dest => dest.ColumnId, src => src.Id)
                 .Map(dest => dest.ColumnName, src => src.Title)
                 .Map(dest => dest.Template, src => int.Parse(src.DefaultTemplate))
                 ;

            //自定义标签->标准键值对
            config.ForType<UserListItemSingle, PermissionManagerInfo>()
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.CardNum, src => src.CardNo)
                 .Map(dest => dest.Depart, src => src.DepartName)
                 .Map(dest => dest.UserKey, src => src.Key)
                 .Map(dest => dest.Phone, src => src.Phone)
                 ;
        }


        /// <summary>
        /// 多值字段判空+转数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetListFromStr(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();
            return str.Split(',').ToList();
        }

        /// <summary>
        ///  数组转多值字段，判空
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string GetStrFromList(List<string> strs)
        {
            if (strs == null)
            {
                return "";
            }

            return string.Join(',', strs.ToArray());
        }
    }
}
