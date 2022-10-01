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
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
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
            return str.Split(';',StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 多值字段判空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringFromList(List<string> list)
        {
            if (list==null)
                return "";
            return string.Join(';',list);
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
                 //  .Map(dest => dest.Introduction, src => src.Introduction.Trim(';') ))
                 ;
            //数据库  Dto->Entity
            config.ForType<DatabaseTerraceDto, Entitys.DatabaseTerrace>()
                 .Map(dest => dest.ArticleTypes, src => GetStringFromList(src.DomainEscs))
                 .Map(dest => dest.DomainEscs, src => GetStringFromList( src.DomainEscs))
                 .Map(dest => dest.Label, src => GetStringFromList( src.Label))
                 .Map(dest => dest.UserTypes, src => GetStringFromList( src.UserTypes))
                 .Map(dest => dest.UserGroups, src => GetStringFromList( src.UserGroups))
                 .Map(dest => dest.Status, src => int.Parse(src.Status))
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
            .Map(dest => dest.Introduce, src => GetStringFromList( src.Introduce));
            ;

            //应用设置  Entity->Dto
            config.ForType<DatabaseTerraceSettings, DatabaseTerraceSettingsDto>()
                 .Map(dest => dest.Introduce, src => GetListFromString(src.Introduce.Trim(';')))
            ;

            //数据库栏目  Dto->Entity
            config.ForType<DatabaseColumnDto, DatabaseColumn>()
                 .Map(dest => dest.OrderRule, src => int.Parse(src.OrderRule))
            ;

            //数据库栏目  Entity->Dto
            config.ForType<DatabaseColumn, DatabaseColumnDto>()
                 .Map(dest => dest.OrderRule, src => src.OrderRule.ToString())
            ;

            config.ForType< DomainTreeItem,OptionDto>


        }
    }
}
