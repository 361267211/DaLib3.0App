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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using SmartLibrary.Assembly.Application.Protos;

namespace SmartLibrary.Open.Services.Mapper
{
    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            //grpc类的映射关系
            config.ForType<AssemblyBaseInfo, AssemblyBaseInfoTab>()
                .Map(dest => dest.Id, src => src.Id==Guid.Empty? Guid.NewGuid():src.Id)
                .Map(dest => dest.OrgCode, src => src.OrgCode ??""  )
                .Map(dest => dest.RecommendType, src => src.RecommendType ?? "" )
                .Map(dest => dest.RejectionReason, src => src.RejectionReason ?? ""  )
               // .Map(dest => dest.CreaterId, src => Guid.NewGuid()  )
              //  .Map(dest => dest.MaintainerId, src =>Guid.NewGuid())

                ;

            //grpc类的映射关系
            config.ForType<AssemblyBaseInfoTab, AssemblyBaseInfo>()
                .Map(dest => dest.MaintainerUserKeys, src => "")

                ;


            config.ForType<ArtByImported, ArtByImportedTab>()
                .Map(dest => dest.Id, src => Guid.NewGuid())
                .Map(dest => dest.Comment, src => src.Comment == null ? "" : src.Comment)
                .Map(dest => dest.DeleteFlag, src => src.DeleteFlag)
                ;

            config.ForType<ArtRetrievalExp, ArtRetrievalExpTab>()
                .Map(dest => dest.Id, src => Guid.NewGuid())
                .Map(dest => dest.CoreCollection, src => src.CoreCollection == null ? "" : src.CoreCollection)
                .Map(dest => dest.DeleteFlag, src => src.DeleteFlag)
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
                return null;
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
