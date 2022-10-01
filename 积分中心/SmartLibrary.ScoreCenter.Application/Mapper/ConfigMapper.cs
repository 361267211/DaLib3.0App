using Mapster;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Common.Utils;
/*********************************************************
* 名    称：ConfigMapper.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：数据结构映射
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.RpcService;
using System;

namespace SmartLibrary.ScoreCenter.Application.Mapper
{
    /// <summary>
    /// 数据结构映射
    /// </summary>
    public class ConfigMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UserData, AppUserInfo>()
                .Map(dest => dest.UserKey, src => src.Key)
                .Map(dest => dest.UserName, src => src.Name)
                .Map(dest => dest.UserPhoto, src => src.Photo)
                .Map(dest => dest.UserPhone, src => src.Phone)
                .Map(dest => dest.UserEmail, src => src.Email)
                .Map(dest => dest.UserIdCard, src => src.IdCard)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.IsStaff, src => src.IsStaff);

            config.ForType<UserPageData.Types.UserListItem, SimpleUserListItemDto>()
                .Map(dest => dest.Birthday, src => src.Birthday != null ? src.Birthday.ToDateTime() : (DateTime?)null)
                .Map(dest => dest.LeaveTime, src => src.LeaveTime != null ? src.LeaveTime.ToDateTime() : (DateTime?)null)
                .Map(dest => dest.CreateTime, src => src.CreateTime != null ? src.CreateTime.ToDateTime() : (DateTime?)null);

            config.ForType<ViewModels.UserTableQuery, User.RpcService.UserSearchTableQuery>()
                .Map(dest => dest.Name, src => src.Name ?? "")
                .Map(dest => dest.NickName, src => src.NickName ?? "")
                .Map(dest => dest.StudentNo, src => src.StudentNo ?? "")
                .Map(dest => dest.Unit, src => src.Unit ?? "")
                .Map(dest => dest.Edu, src => src.Edu ?? "")
                .Map(dest => dest.Title, src => src.Title ?? "")
                .Map(dest => dest.Depart, src => src.Depart ?? "")
                .Map(dest => dest.DepartName, src => src.DepartName ?? "")
                .Map(dest => dest.College, src => src.College ?? "")
                .Map(dest => dest.CollegeName, src => src.CollegeName ?? "")
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart ?? "")
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName ?? "")
                .Map(dest => dest.Major, src => src.Major ?? "")
                .Map(dest => dest.Grade, src => src.Grade ?? "")
                .Map(dest => dest.Class, src => src.Class ?? "")
                .Map(dest => dest.Type, src => src.Type ?? "")
                .Map(dest => dest.TypeName, src => src.TypeName ?? "")
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.IDCard, src => src.IDCard ?? "")
                .Map(dest => dest.Phone, src => src.Phone ?? "")
                .Map(dest => dest.Email, src => src.Email ?? "")
                .Map(dest => dest.BirthdayStartTime, src => DataConverter.ObjectToString(src.BirthdayStartTime, typeof(DateTime?)))
                .Map(dest => dest.BirthdayEndTime, src => DataConverter.ObjectToString(src.BirthdayEndTime, typeof(DateTime?)))
                .Map(dest => dest.Gender, src => src.Gender ?? "")
                .Map(dest => dest.Addr, src => src.Addr ?? "")
                .Map(dest => dest.AddrDetail, src => src.AddrDetail ?? "")
                .Map(dest => dest.SourceFrom, src => src.SourceFrom)
                .Map(dest => dest.LastLoginStartTime, src => DataConverter.ObjectToString(src.LastLoginStartTime, typeof(DateTime?)))
                .Map(dest => dest.LastLoginEndTime, src => DataConverter.ObjectToString(src.LastLoginEndTime, typeof(DateTime?)))
                .Map(dest => dest.LeaveStartTime, src => DataConverter.ObjectToString(src.LeaveStartTime, typeof(DateTime?)))
                .Map(dest => dest.LeaveEndTime, src => DataConverter.ObjectToString(src.LeaveEndTime, typeof(DateTime?)))
                .Map(dest => dest.CardNo, src => src.CardNo ?? "")
                .Map(dest => dest.CardBarCode, src => src.CardBarCode ?? "")
                .Map(dest => dest.CardPhysicNo, src => src.CardPhysicNo ?? "")
                .Map(dest => dest.CardIdentityNo, src => src.CardIdentityNo ?? "")
                .Map(dest => dest.CardIsPrincipal, src => src.CardIsPrincipal)
                .Map(dest => dest.CardType, src => src.CardType ?? "")
                .Map(dest => dest.CardTypeName, src => src.CardTypeName ?? "")
                .Map(dest => dest.CardStatus, src => src.CardStatus)
                .Map(dest => dest.CardIssueStartTime, src => DataConverter.ObjectToString(src.CardIssueStartTime, typeof(DateTime?)))
                .Map(dest => dest.CardIssueEndTime, src => DataConverter.ObjectToString(src.CardIssueEndTime, typeof(DateTime?)))
                .Map(dest => dest.CardExpireStartTime, src => DataConverter.ObjectToString(src.CardExpireStartTime, typeof(DateTime?)))
                .Map(dest => dest.CardExpireEndTime, src => DataConverter.ObjectToString(src.CardExpireEndTime, typeof(DateTime?)))
                .Map(dest => dest.IsStaff, src => src.IsStaff);
        }
    }
}
