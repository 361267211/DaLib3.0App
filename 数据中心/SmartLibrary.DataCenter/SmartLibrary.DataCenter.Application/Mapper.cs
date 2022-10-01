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
using SmartLibrary.DataCenter;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
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
            return string.Join(';', list);
        }

        public void Register(TypeAdapterConfig config)
        {


            //数据库栏目  Entity->Dto
            config.ForType<ProviderResource, ProviderResourceItem>()
                 .Map(dest => dest.LinkUrl, src => src.LinkUrl ?? "")
                 .Map(dest => dest.TerraceShortName, src => src.TerraceShortName ?? "")
                 .Map(dest => dest.TerraceFullName, src => src.TerraceFullName ?? "")
                 .Map(dest => dest.Introduction, src => src.Introduction ?? "")
                 .Map(dest => dest.ChildDatabaseName, src => src.ChildDatabaseName ?? "")
            ;
            //数据库栏目  Entity->Dto
            config.ForType<DatabaseProvider, DatabaseProviderItem>()
                 .Map(dest => dest.Id, src => src.Id.ToString())
                 .Map(dest => dest.Introduction, src => src.Introduction ?? "")
                 .Map(dest => dest.ProviderName, src => src.ProviderName ?? "")
                 .Map(dest => dest.Remark, src => src.Remark ?? "")
                 .Map(dest => dest.Address, src => src.Address ?? "")
                 .Map(dest => dest.Gathering, src => src.Gathering ?? "")
                 .Map(dest => dest.Contacts, src => src.Contacts ?? "")
                 .Map(dest => dest.District, src => src.District ?? "")
                 .Map(dest => dest.Tel, src => src.Tel ?? "")
                 .Map(dest => dest.ContractsTel, src => src.ContractsTel ?? "")
            ;


/*            //数据库栏目  Entity->Dto
            config.ForType<ResourceAlbumDto, ResourceAlbumItem>()

                 .Map(dest => dest.NextNodes, src => src.ChildAlbum.Adapt<List<ResourceAlbumItem>>())
            ;*/

            
        }
    }
}
