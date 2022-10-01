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

using Furion.JsonSerialization;
using Mapster;
using SmartLibrary.AppCenter;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.Common.Const;
using SmartLibrary.News.Common.Dtos;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.SceneManage;
using SmartLibrary.Search.EsSearchProxy.Core.Dto;
using SmartLibrary.Search.EsSearchProxy.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.User.RpcService.UserPageData.Types;

namespace SmartLibrary.Assembly.EntityFramework.Core
{
    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            //自定义标签->标准键值对
            config.ForType<HeaderFooterListSingle, NewsBodyTemplateDto>()
                 .Map(dest => dest.Id, src => new Guid(src.Id))
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.PreviewPic, src => src.Cover)
                 .Map(dest => dest.Note, src =>"备注")
                 ;

            //自定义标签->标准键值对
            config.ForType<UserListItem, PermissionManagerInfo>()
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.CardNum, src => src.CardNo)
                 .Map(dest => dest.Depart, src => src.Depart)
                 .Map(dest => dest.UserKey, src => src.Key)
                 ;

            //自定义标签->标准键值对
            config.ForType<NewsColumn, GetColumnLinkInfoReply>()
                 .Map(dest => dest.ColumnId, src => src.Id)
                 .Map(dest => dest.ColumnName, src => src.Title)
                 .Map(dest => dest.Template, src => int.Parse(src.DefaultTemplate))
                 ;

            //自定义标签->标准键值对
            config.ForType<NewsContent, NewsContentDto>()
                 .Map(dest => dest.Content, src =>   src.Content.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace))
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
