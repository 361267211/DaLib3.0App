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
using SmartLibrary.DatabaseTerrace.Application.Dto;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.Common.Utilitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.Utility;
using SmartLibrary.DataCenter;
using SmartLibrary.SceneManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core
{
    public class Mapper : IRegister
    {
        /// <summary>
        /// 多值字段判空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetListFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();
            return str.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 多值字段判空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringFromList(List<string> list)
        {
            if (list == null)
                return "";
            return $";{string.Join(';', list)};";
        }

        public void Register(TypeAdapterConfig config)
        {
            //数据库  Entity->Dto
            config.ForType<Entitys.DatabaseTerrace, DatabaseTerraceDto>()
                 .Map(dest => dest.ArticleTypes, src => GetListFromString(src.ArticleTypes.Trim(';')))
                 .Map(dest => dest.DomainEscs, src => GetListFromString(src.DomainEscs.Trim(';')))
                 .Map(dest => dest.Label, src => GetListFromString(src.Label.Trim(';')))
                 .Map(dest => dest.UserTypes, src => GetListFromString(src.UserTypes.Trim(';')))
                 .Map(dest => dest.UserGroups, src => GetListFromString(src.Label.Trim(';')))
                 .Map(dest => dest.Status, src => src.Status.ToString())                 
                 .Map(dest => dest.Information, src => src.Information.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace))
                 .Map(dest => dest.UseGuide, src => src.UseGuide.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace))
                 .Map(dest => dest.ResourceStatistics, src => src.ResourceStatistics.AsSpanReplace(PlaceholderKeys.FileServer, SiteGlobalConfig.FileServerReplace))
                 //  .Map(dest => dest.Introduction, src => src.Introduction.Trim(';') ))
                 ;
            //数据库  Dto->Entity
            config.ForType<DatabaseTerraceDto, Entitys.DatabaseTerrace>()
                 .Map(dest => dest.ArticleTypes, src => GetStringFromList(src.ArticleTypes))
                 .Map(dest => dest.DomainEscs, src => GetStringFromList(src.DomainEscs))
                 .Map(dest => dest.Label, src => GetStringFromList(src.Label))
                 .Map(dest => dest.UserTypes, src => GetStringFromList(src.UserTypes))
                 .Map(dest => dest.UserGroups, src => GetStringFromList(src.UserGroups))
                 .Map(dest => dest.Status, src => int.Parse(src.Status))
                 .Map(dest => dest.UseGuideText, src => StringUtils.HtmlDecode(src.UseGuide))
                 .Map(dest => dest.InformationText, src => StringUtils.HtmlDecode(src.Information))
                 .Map(dest => dest.ResourceStatisticsText, src => StringUtils.HtmlDecode(src.ResourceStatistics))
                 .Map(dest => dest.Information, src => src.Information.AsSpanReplace( SiteGlobalConfig.FileServerReplace,PlaceholderKeys.FileServer ))
                 .Map(dest => dest.UseGuide, src => src.UseGuide.AsSpanReplace( SiteGlobalConfig.FileServerReplace,PlaceholderKeys.FileServer ))
                 .Map(dest => dest.ResourceStatistics, src => src.ResourceStatistics.AsSpanReplace( SiteGlobalConfig.FileServerReplace,PlaceholderKeys.FileServer ))

            ;

            //学科分类  Dto->Entity
            config.ForType<OptionDto, DomainEscsAttr>()
                 .Map(dest => dest.UpdatedTime, src => DateTime.Now)
                 .Map(dest => dest.Id, src => src.Value)
            ;

            //学科分类  Entity->Dto
            config.ForType<DomainEscsAttr, OptionDto>()
                 .Map(dest => dest.Value, src => src.Id)
            ;

            //自定义标签  Entity->Dto
            config.ForType<CustomLabel, OptionDto>()
                 .Map(dest => dest.Key, src => src.LabelName)
                 .Map(dest => dest.Value, src => src.Id.ToString())
            ;

            //自定义标签  Dto->Entity
            config.ForType<OptionDto, CustomLabel>()
                 .Map(dest => dest.LabelName, src => src.Key)
                 .Map(dest => dest.Id, src => new Guid(src.Value))
                 .Map(dest => dest.CreatedTime, src => DateTime.Now)
                 .Map(dest => dest.UpdatedTime, src => DateTime.Now)
            ;

            //自定义标签  Dto->Entity
            config.ForType<OptionDto, DatabaseUrlName>()
                 .Map(dest => dest.Name, src => src.Key)
                 .Map(dest => dest.Id, src => src.Value)
            ;

            //自定义标签  Entity->Dto
            config.ForType<DatabaseUrlName, OptionDto>()
                 .Map(dest => dest.Key, src => src.Name)
                 .Map(dest => dest.Value, src => src.Id)
            ;

            //应用设置  Dto->Entity
            config.ForType<DatabaseTerraceSettingsDto, DatabaseTerraceSettings>()
            .Map(dest => dest.Introduce, src => GetStringFromList(src.Introduce));
            ;

            //应用设置  Entity->Dto
            config.ForType<DatabaseTerraceSettings, DatabaseTerraceSettingsDto>()
                 .Map(dest => dest.Introduce, src => GetListFromString(src.Introduce.Trim(';')))
            ;

            //数据库栏目  Dto->Entity
            config.ForType<DatabaseColumnDto, DatabaseColumn>()
                 .Map(dest => dest.OrderRule, src => int.Parse(src.OrderRule))
                 .Map(dest => dest.MatchCount, src => src.MatchCount == 0 ? 10 : src.MatchCount)
            ;

            //数据库栏目  Entity->Dto
            config.ForType<DatabaseColumn, DatabaseColumnDto>()
                 .Map(dest => dest.OrderRule, src => src.OrderRule.ToString())
            ;

            //数据库栏目  Entity->Dto
            config.ForType<SourceTypeItem, OptionDto>()
                 .Map(dest => dest.Key, src => src.Name)
                 .Map(dest => dest.Value, src => src.Code)

            ;


            //数据库栏目  Entity->Dto
            config.ForType<DomainTreeItem, OptionDto>()
                 .Map(dest => dest.Key, src => src.DomainName)
                 .Map(dest => dest.Value, src => src.DomainID)
                 .Map(dest => dest.Child, src => src.NextNodes)
            ;

            //数据库栏目  Entity->Dto
            config.ForType<ProviderResourceItem, OptionDto>()
                 .Map(dest => dest.Key, src => src.ChildDatabaseName)
                 .Map(dest => dest.Value, src => src.Provider)
            ;
            //数据库栏目  Entity->Dto
            config.ForType<ProviderResource, OptionDto>()
                 .Map(dest => dest.Key, src => src.TerraceFullName)
                 .Map(dest => dest.Value, src => src.Provider)
            ;
            //数据库栏目  Entity->Dto
            config.ForType<DatabaseProviderItem, OptionDto>()
                 .Map(dest => dest.Key, src => src.ProviderName)
                 .Map(dest => dest.Value, src => src.DatabaseCode)
            ;

            //数据库栏目  Entity->Dto
            config.ForType<ResourceAlbumItem, OptionDto>()
                 .Map(dest => dest.Key, src => src.AlbumName)
                 .Map(dest => dest.Value, src => src.AlbumCode)
                 .Map(dest => dest.Child, src => src.ChildAlbum)
            ;

            //数据库栏目  Entity->Dto
            config.ForType<ProviderResourceItem, ProviderResource>()
                 .Map(dest => dest.Id, src => Guid.NewGuid())
            ;

            //数据库栏目  Entity->Dto
            config.ForType<ProviderResourceItem, DatabaseTerraceDto>()
                 .Map(dest => dest.Id, src => Guid.Empty)
                 .Map(dest => dest.Language, src => src.LanguageKind.ToString())
                 .Map(dest => dest.Title, src => src.TerraceFullName)
                 .Map(dest => dest.Abbreviation, src => src.TerraceShortName)
                 .Map(dest => dest.Title, src => src.TerraceShortName)
                 .Map(dest => dest.Label, src => GetListFromString(""))
                 .Map(dest => dest.ArticleTypes, src => GetListFromString(""))
                 .Map(dest => dest.DomainEscs, src => GetListFromString(""))
                 .Map(dest => dest.DatabaseProviderID, src => src.DatabaseCode)
            ;

            //grpc头尾模板->头尾模板dto
            config.ForType<HeaderFooterListSingle, DatabaseTemplateDto>()
                 .Map(dest => dest.Id, src => new Guid(src.Id))
                 .Map(dest => dest.Name, src => src.Name)
                 .Map(dest => dest.PreviewPic, src => src.Cover)
                 .Map(dest => dest.Note, src => "备注")
                 ;
        }
    }
}
