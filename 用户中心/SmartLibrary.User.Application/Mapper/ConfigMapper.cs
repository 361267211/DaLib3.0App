using Mapster;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.DataApprove;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System.Collections.Generic;
using System.Linq;

namespace SmartLibrary.User.Application.Mapper
{
    public class ConfigMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            var baseEncrypt = new Base64Crypt(codeTable);
            var notEncodeProperty = new List<string> { "User_SourceFrom", "User_Status", "Card_Status" };

            config.ForType<UserDto, EntityFramework.Core.Entitys.User>()
                    .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                    .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Encode(src.NickName))
                    .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo))
                    .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Encode(src.Unit))
                    .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Encode(src.Edu))
                    .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Encode(src.Title))
                    .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Encode(src.Depart))
                    .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Encode(src.DepartName))
                    .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Encode(src.College))
                    .Map(dest => dest.CollegeName, src => src.College == null ? null : baseEncrypt.Encode(src.CollegeName))
                    .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Encode(src.CollegeDepart))
                    .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Encode(src.CollegeDepartName))
                    .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Encode(src.Major))
                    .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Encode(src.Grade))
                    .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Encode(src.Class))
                    .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Encode(src.Type))
                    .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Encode(src.TypeName))
                    .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard))
                    .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone))
                    .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Encode(src.Email))
                    .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Encode(src.Gender))
                    .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Encode(src.Addr))
                    .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Encode(src.AddrDetail))
                    .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Encode(src.Photo));

            config.ForType<EntityFramework.Core.Entitys.User, UserDto>()
               .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
               .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
               .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
               .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
               .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
               .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
               .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
               .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
               .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
               .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
               .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
               .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
               .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
               .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
               .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
               .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
               .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
               .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
               .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
               .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
               .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
               .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
               .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail))
               .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Decode(src.Photo));

            config.ForType<UserDetailOutput, SensitiveUserDetailOutput>()
               .Map(dest => dest.Name, src => src.Name == null ? null : SensitiveCrypt.EncodeName(src.Name))
               .Map(dest => dest.NickName, src => src.NickName == null ? null : SensitiveCrypt.EncodeName(src.NickName))
               .Map(dest => dest.IdCard, src => src.IdCard == null ? null : SensitiveCrypt.EncodeIdCard(src.IdCard))
               .Map(dest => dest.Phone, src => src.Phone == null ? null : SensitiveCrypt.EncodePhone(src.Phone))
               .Map(dest => dest.Email, src => src.Email == null ? null : SensitiveCrypt.EncodeEmail(src.Email))
               .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : SensitiveCrypt.EncodeAddr(src.AddrDetail));

            config.ForType<EntityFramework.Core.Entitys.User, UserDetailOutput>()
               .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
               .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
               .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
               .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
               .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
               .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
               .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
               .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
               .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
               .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
               .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
               .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
               .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
               .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
               .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
               .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
               .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
               .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
               .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
               .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
               .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
               .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
               .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail))
               .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Decode(src.Photo));

            config.ForType<UserListItemDto, UserListItemOutput>()
              .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
              .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
              .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
              .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
              .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
              .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
              .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
              .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
              .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
              .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
              .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
              .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
              .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
              .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
              .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
              .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
              .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
              .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
              .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
              .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
              .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
              .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
              .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail))
              .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Decode(src.Photo))
              ;

            config.ForType<UserListItemOutput, SensitiveUserListItemOutput>()
                .Map(dest => dest.Name, src => src.Name == null ? null : SensitiveCrypt.EncodeName(src.Name))
                .Map(dest => dest.NickName, src => src.NickName == null ? null : SensitiveCrypt.EncodeName(src.NickName))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : SensitiveCrypt.EncodeIdCard(src.IdCard))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : SensitiveCrypt.EncodePhone(src.Phone))
                .Map(dest => dest.Email, src => src.Email == null ? null : SensitiveCrypt.EncodeEmail(src.Email))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : SensitiveCrypt.EncodeAddr(src.AddrDetail));

            config.ForType<ExportUserListItemDto, ExportUserListItemOutput>()
             .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
             .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
             .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
             .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
             .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
             .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
             .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
             .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
             .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
             .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
             .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
             .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
             .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
             .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
             .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
             .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
             .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
             .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
             .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
             .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
             .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
             .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
             .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail))
             .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Decode(src.Photo))
             ;

            config.ForType<SimpleUserListItemDto, SimpleUserListItemOutput>()
             .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
             .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
             .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
             .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
             .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
             .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
             .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
             .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
             .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
             .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
             .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
             .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
             .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
             .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
             .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
             .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
             .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
             .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
             .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
             .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
             .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
             .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
             .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail))
             .Map(dest => dest.Photo, src => src.Photo == null ? null : baseEncrypt.Decode(src.Photo))
             ;

            config.ForType<SimpleUserListItemOutput, SensitiveSimpleUserListItemOutput>()
                .Map(dest => dest.Name, src => src.Name == null ? null : SensitiveCrypt.EncodeName(src.Name))
                .Map(dest => dest.NickName, src => src.NickName == null ? null : SensitiveCrypt.EncodeName(src.NickName))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : SensitiveCrypt.EncodeIdCard(src.IdCard))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : SensitiveCrypt.EncodePhone(src.Phone))
                .Map(dest => dest.Email, src => src.Email == null ? null : SensitiveCrypt.EncodeEmail(src.Email))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : SensitiveCrypt.EncodeAddr(src.AddrDetail));

            config.ForType<UserDetailOutput, RpcService.UserData>()
            .Map(dest => dest.Key, src => src.UserKey.ToString())
            .Map(dest => dest.Name, src => src.Name ?? "")
            .Map(dest => dest.NickName, src => src.NickName ?? "")
            .Map(dest => dest.StudentNo, src => src.StudentNo ?? "")
            .Map(dest => dest.Gender, src => src.Gender ?? "")
            .Map(dest => dest.Photo, src => src.Photo ?? "")
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
            .Map(dest => dest.IdCard, src => src.IdCard ?? "")
            .Map(dest => dest.Phone, src => src.Phone ?? "")
            .Map(dest => dest.Email, src => src.Email ?? "")
            .Map(dest => dest.Addr, src => src.Addr ?? "")
            .Map(dest => dest.AddrDetail, src => src.AddrDetail ?? "")
            .Map(dest => dest.CardNo, src => src.CardNo ?? "")
            .Map(dest => dest.AsyncReaderId, src => src.AsyncReaderId ?? "")
            .Ignore(dest => dest.GroupIds)
            .Ignore(dest => dest.ShowSourceFrom)
            .Ignore(dest => dest.ShowCardStatus)
            .Ignore(dest => dest.ShowStatus)
            ;

            config.ForType<SimpleUserListItemDto, RpcService.UserData>()
            .Map(dest => dest.Key, src => src.UserKey.ToString())
            .Map(dest => dest.Name, src => src.Name == null ? "" : baseEncrypt.Decode(src.Name))
            .Map(dest => dest.NickName, src => src.NickName == null ? "" : baseEncrypt.Decode(src.NickName))
            .Map(dest => dest.StudentNo, src => src.StudentNo == null ? "" : baseEncrypt.Decode(src.StudentNo))
            .Map(dest => dest.Gender, src => src.Gender == null ? "" : baseEncrypt.Decode(src.Gender))
            .Map(dest => dest.Photo, src => src.Photo == null ? "" : baseEncrypt.Decode(src.Photo))
            .Map(dest => dest.Unit, src => src.Unit == null ? "" : baseEncrypt.Decode(src.Unit))
            .Map(dest => dest.Edu, src => src.Edu == null ? "" : baseEncrypt.Decode(src.Edu))
            .Map(dest => dest.Title, src => src.Title == null ? "" : baseEncrypt.Decode(src.Title))
            .Map(dest => dest.Depart, src => src.Depart == null ? "" : baseEncrypt.Decode(src.Depart))
            .Map(dest => dest.DepartName, src => src.DepartName == null ? "" : baseEncrypt.Decode(src.DepartName))
            .Map(dest => dest.College, src => src.College == null ? "" : baseEncrypt.Decode(src.College))
            .Map(dest => dest.CollegeName, src => src.CollegeName == null ? "" : baseEncrypt.Decode(src.CollegeName))
            .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? "" : baseEncrypt.Decode(src.CollegeDepart))
            .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? "" : baseEncrypt.Decode(src.CollegeDepartName))
            .Map(dest => dest.Major, src => src.Major == null ? "" : baseEncrypt.Decode(src.Major))
            .Map(dest => dest.Grade, src => src.Grade == null ? "" : baseEncrypt.Decode(src.Grade))
            .Map(dest => dest.Class, src => src.Class == null ? "" : baseEncrypt.Decode(src.Class))
            .Map(dest => dest.Type, src => src.Type == null ? "" : baseEncrypt.Decode(src.Type))
            .Map(dest => dest.TypeName, src => src.TypeName == null ? "" : baseEncrypt.Decode(src.TypeName))
            .Map(dest => dest.IdCard, src => src.IdCard == null ? "" : baseEncrypt.Decode(src.IdCard))
            .Map(dest => dest.Phone, src => src.Phone == null ? "" : baseEncrypt.Decode(src.Phone))
            .Map(dest => dest.Email, src => src.Email == null ? "" : baseEncrypt.Decode(src.Email))
            .Map(dest => dest.Addr, src => src.Addr == null ? "" : baseEncrypt.Decode(src.Addr))
            .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? "" : baseEncrypt.Decode(src.AddrDetail))
            ;

            config.ForType<SimpleUserListItemDto, RpcService.UserPageData.Types.UserListItem>()
            .Map(dest => dest.Key, src => src.UserKey.ToString())
            .Map(dest => dest.Name, src => src.Name == null ? "" : baseEncrypt.Decode(src.Name))
            .Map(dest => dest.NickName, src => src.NickName == null ? "" : baseEncrypt.Decode(src.NickName))
            .Map(dest => dest.StudentNo, src => src.StudentNo == null ? "" : baseEncrypt.Decode(src.StudentNo))
            .Map(dest => dest.Gender, src => src.Gender == null ? "" : baseEncrypt.Decode(src.Gender))
            .Map(dest => dest.Photo, src => src.Photo == null ? "" : baseEncrypt.Decode(src.Photo))
            .Map(dest => dest.Unit, src => src.Unit == null ? "" : baseEncrypt.Decode(src.Unit))
            .Map(dest => dest.Edu, src => src.Edu == null ? "" : baseEncrypt.Decode(src.Edu))
            .Map(dest => dest.Title, src => src.Title == null ? "" : baseEncrypt.Decode(src.Title))
            .Map(dest => dest.Depart, src => src.Depart == null ? "" : baseEncrypt.Decode(src.Depart))
            .Map(dest => dest.DepartName, src => src.DepartName == null ? "" : baseEncrypt.Decode(src.DepartName))
            .Map(dest => dest.College, src => src.College == null ? "" : baseEncrypt.Decode(src.College))
            .Map(dest => dest.CollegeName, src => src.CollegeName == null ? "" : baseEncrypt.Decode(src.CollegeName))
            .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? "" : baseEncrypt.Decode(src.CollegeDepart))
            .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? "" : baseEncrypt.Decode(src.CollegeDepartName))
            .Map(dest => dest.Major, src => src.Major == null ? "" : baseEncrypt.Decode(src.Major))
            .Map(dest => dest.Grade, src => src.Grade == null ? "" : baseEncrypt.Decode(src.Grade))
            .Map(dest => dest.Class, src => src.Class == null ? "" : baseEncrypt.Decode(src.Class))
            .Map(dest => dest.Type, src => src.Type == null ? "" : baseEncrypt.Decode(src.Type))
            .Map(dest => dest.TypeName, src => src.TypeName == null ? "" : baseEncrypt.Decode(src.TypeName))
            .Map(dest => dest.IdCard, src => src.IdCard == null ? "" : baseEncrypt.Decode(src.IdCard))
            .Map(dest => dest.Phone, src => src.Phone == null ? "" : baseEncrypt.Decode(src.Phone))
            .Map(dest => dest.Email, src => src.Email == null ? "" : baseEncrypt.Decode(src.Email))
            .Map(dest => dest.ShowStatus, src => "")
            .Map(dest => dest.ShowSourceFrom, src => "")
            .Map(dest => dest.ShowCardStatus, src => "")
            .Map(dest => dest.CardNo, src => src.CardNo ?? "")
            .Map(dest => dest.Birthday, src => src.Birthday != null ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.Birthday.Value) : null)
            .Map(dest => dest.LeaveTime, src => src.LeaveTime != null ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.LeaveTime.Value) : null)
            .Map(dest => dest.FirstLoginTime, src => src.FirstLoginTime != null ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.FirstLoginTime.Value) : null)
            .Map(dest => dest.LastLoginTime, src => src.LastLoginTime != null ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.LastLoginTime.Value) : null)
            .Map(dest => dest.CreateTime, src => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.CreateTime))
            .Map(dest => dest.CardExpireDate, src => src.CardExpireDate != null ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(src.CardExpireDate.Value) : null)
            ;


            config.ForType<UserBatchEditInput, UserBatchEditDto>()
             .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Encode(src.Edu))
             .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Encode(src.College))
             .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Encode(src.CollegeName))
             .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Encode(src.CollegeDepart))
             .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Encode(src.CollegeDepartName))
             .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Encode(src.Major))
             .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Encode(src.Grade))
             .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Encode(src.Class))
             .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Encode(src.Type))
             .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Encode(src.TypeName))
             .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Encode(src.Gender));

            //读者修改日志
            config.ForType<UserChangeLogDetailUserDto, UserChangeLogDetailUser>()
                  .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
                  .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
                  .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
                  .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName));

            config.ForType<CardListItemDto, CardListItemOutput>()
                 .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName))
                 .Map(dest => dest.UserType, src => src.UserType == null ? null : baseEncrypt.Decode(src.UserType))
                 .Map(dest => dest.UserTypeName, src => src.UserTypeName == null ? null : baseEncrypt.Decode(src.UserTypeName))
                 .Map(dest => dest.UserStudentNo, src => src.UserStudentNo == null ? null : baseEncrypt.Decode(src.UserStudentNo));

            config.ForType<CardListItemOutput, SensitiveCardListItemOutput>()
                .Map(dest => dest.UserName, src => src.UserName == null ? null : SensitiveCrypt.EncodeName(src.UserName));
            //.Map(dest => dest.NickName, src => src.NickName == null ? null : SensitiveCrypt.EncodeName(src.NickName));

            //管理设置-权限管理
            config.ForType<StaffListItemDto, StaffListItemOutput>()
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
                .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
                .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
                .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo));

            config.ForType<StaffDepartEditInput, StaffDepartSetDto>()
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Encode(src.Depart))
                .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Encode(src.DepartName));

            config.ForType<StaffRoleTableQuery, StaffRoleEncodeTableQuery>()
                .Map(dest => dest.Keyword, src => src.Keyword == null ? null : baseEncrypt.Encode(src.Keyword))
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone))
                .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo))
                ;

            config.ForType<PropertyChangeLogTableQuery, EncodePropertyChangeLogTableQuery>()
                .Map(dest => dest.ChangeUserName, src => src.ChangeUserName == null ? null : baseEncrypt.Encode(src.ChangeUserName))
                .Map(dest => dest.ChangeUserPhone, src => src.ChangeUserPhone == null ? null : baseEncrypt.Encode(src.ChangeUserPhone));

            config.ForType<UserChangeLogTableQuery, EncodeUserChangeLogTableQuery>()
                .Map(dest => dest.ChangeUserName, src => src.ChangeUserName == null ? null : baseEncrypt.Encode(src.ChangeUserName))
                .Map(dest => dest.ChangeUserPhone, src => src.ChangeUserPhone == null ? null : baseEncrypt.Encode(src.ChangeUserPhone));


            config.ForType<StaffTableQuery, StaffEncodeTableQuery>()
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Encode(src.Depart))
                .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard));


            config.ForType<UserRegisterListItemDto, UserRegisterListItemOutput>()
               .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Encode(src.UserName))
               .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Encode(src.UserPhone));

            config.ForType<CardClaimListItemDto, CardClaimListItemOutput>()
               .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName))
               .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Decode(src.UserPhone))
               .Map(dest => dest.UserCollege, src => src.UserCollege == null ? null : baseEncrypt.Decode(src.UserCollege));


            config.ForType<CardClaimTableQuery, CardClaimEncodeTableQuery>()
              .Map(dest => dest.Keyword, src => src.Keyword == null ? null : baseEncrypt.Encode(src.Keyword))
              .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Encode(src.UserName))
              .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Encode(src.UserPhone));

            config.ForType<UserRegisterTableQuery, UserRegisterEncodeTableQuery>()
              .Map(dest => dest.Keyword, src => src.Keyword == null ? null : baseEncrypt.Encode(src.Keyword))
              .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Encode(src.UserName))
              .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Encode(src.UserPhone));
            config.ForType<CardTableQuery, CardEncodeTableQuery>()
               .Map(dest => dest.Keyword, src => src.Keyword == null ? null : baseEncrypt.Encode(src.Keyword))
               .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
               .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo));
            config.ForType<UserTableQuery, UserEncodeTableQuery>()
               .Map(dest => dest.Keyword, src => src.Keyword == null ? null : baseEncrypt.Encode(src.Keyword))
               .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
               .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Encode(src.NickName))
               .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo))
               .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Encode(src.Unit))
               .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Encode(src.Edu))
               .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Encode(src.Title))
               .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Encode(src.Depart))
               .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Encode(src.DepartName))
               .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Encode(src.College))
               .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Encode(src.CollegeName))
               .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Encode(src.CollegeDepart))
               .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Encode(src.CollegeDepartName))
               .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Encode(src.Major))
               .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Encode(src.Grade))
               .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Encode(src.Class))
               .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Encode(src.Type))
               .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Encode(src.TypeName))
               .Map(dest => dest.IDCard, src => src.IDCard == null ? null : baseEncrypt.Encode(src.IDCard))
               .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone))
               .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Encode(src.Email))
               .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Encode(src.Gender))
               .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Encode(src.Addr))
               .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Encode(src.AddrDetail))
               ;

            config.ForType<SimpleUserTableQuery, SimpleUserEncodeTableQuery>()
                .Map(dest => dest.UserTypeCodes, src => src.UserTypeCodes.Select(x => baseEncrypt.Encode(x)));

            config.ForType<GroupUserInfoDto, GroupUserInfoOutput>()
              .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName))
              .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone));

            config.ForType<UserPropertyItemDto, UserPropertyItemOutput>()
                .Map(dest => dest.PropertyValue, src => src.PropertyValue == null ? null : baseEncrypt.Decode(src.PropertyValue));

            config.ForType<UserImportTempDataDto, UserImportTempData>()
                .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Encode(src.UserName))
                .Map(dest => dest.UserGender, src => src.UserGender == null ? null : baseEncrypt.Encode(src.UserGender))
                .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Encode(src.UserPhone))
                .Map(dest => dest.UserType, src => src.UserType == null ? null : baseEncrypt.Encode(src.UserType))
                .Map(dest => dest.UserTypeName, src => src.UserTypeName == null ? null : baseEncrypt.Encode(src.UserTypeName))
                .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Encode(src.StudentNo))
                .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Encode(src.Unit))
                .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Encode(src.Edu))
                .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Encode(src.College))
                .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Encode(src.CollegeName))
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Encode(src.CollegeDepart))
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Encode(src.CollegeDepartName))
                .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Encode(src.Major))
                .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Encode(src.Grade))
                .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Encode(src.Class))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard))
                .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Encode(src.Email))
                .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Encode(src.Addr))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Encode(src.AddrDetail));

            config.ForType<UserImportTempData, UserTempDataListItemDto>()
                .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName))
                .Map(dest => dest.UserGender, src => src.UserGender == null ? null : baseEncrypt.Decode(src.UserGender))
                .Map(dest => dest.UserPhone, src => src.UserPhone == null ? null : baseEncrypt.Decode(src.UserPhone))
                .Map(dest => dest.UserType, src => src.UserType == null ? null : baseEncrypt.Decode(src.UserType))
                .Map(dest => dest.UserTypeName, src => src.UserTypeName == null ? null : baseEncrypt.Decode(src.UserTypeName))
                .Map(dest => dest.StudentNo, src => src.StudentNo == null ? null : baseEncrypt.Decode(src.StudentNo))
                .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
                .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
                .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
                .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
                .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
                .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
                .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
                .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
                .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail));

            config.ForType<UserRegisterDetailDto, UserRegisterDetailOutput>()
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
                .Map(dest => dest.NickName, src => src.NickName == null ? null : baseEncrypt.Decode(src.NickName))
                .Map(dest => dest.Unit, src => src.Unit == null ? null : baseEncrypt.Decode(src.Unit))
                .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
                .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
                .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
                .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
                .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
                .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
                .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
                .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
                .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
                .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
                .Map(dest => dest.Email, src => src.Email == null ? null : baseEncrypt.Decode(src.Email))
                .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
                .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail));

            config.ForType<CardClaimDetailDto, CardClaimDetailOutput>()
                .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName));

            config.ForType<ExportCardListItemDto, ExportCardListItemOutput>()
                .Map(dest => dest.UserName, src => src.UserName == null ? null : baseEncrypt.Decode(src.UserName))
                .Map(dest => dest.UserType, src => src.UserType == null ? null : baseEncrypt.Decode(src.UserType))
                .Map(dest => dest.UserTypeName, src => src.UserTypeName == null ? null : baseEncrypt.Decode(src.UserTypeName))
                .Map(dest => dest.UserStudentNo, src => src.UserStudentNo == null ? null : baseEncrypt.Decode(src.UserStudentNo));

            config.ForType<GroupUserImportSearchInput, UserImportSearchDto>()
                 .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard))
                 .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                 .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone));

            config.ForType<RpcService.UserListImportSearchItem, UserImportSearchDto>()
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard))
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone));

            config.ForType<RpcService.IdCardInfo, IdCardInfoDto>()
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard));

            config.ForType<RpcService.PhoneInfo, PhoneInfoDto>()
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone));

            config.ForType<EntityFramework.Core.Entitys.User, AppUserInfo>()
                .Map(dest => dest.UserKey, src => src.UserKey.ToString())
                .Map(dest => dest.UserID, src => src.Id)
                .Map(dest => dest.UserName, src => baseEncrypt.Decode(src.Name))
                .Map(dest => dest.UserPhone, src => baseEncrypt.Decode(src.Phone))
                .Map(dest => dest.MobileIdentity, src => src.MobileIdentity)
                .Map(dest => dest.UserEmail, src => baseEncrypt.Decode(src.Email))
                .Map(dest => dest.EmailIdentity, src => src.EmailIdentity)
                .Map(dest => dest.UserIdCard, src => baseEncrypt.Decode(src.IdCard))
                .Map(dest => dest.IdCardIdentity, src => src.IdCardIdentity)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.IsStaff, src => src.IsStaff);

            config.ForType<CardSearchResultDto, RpcService.CardSearchResult>()
                .Map(dest => dest.Phone, src => baseEncrypt.Decode(src.Phone));

            config.ForType<PropertyGroupRuleDto, PropertyGroupRule>()
                .Map(dest => dest.PropertyValue, src => notEncodeProperty.Contains(src.PropertyCode) ? src.PropertyValue : baseEncrypt.Encode(src.PropertyValue));

            config.ForType<PropertyGroupRule, PropertyGroupRuleDto>()
                .Map(dest => dest.PropertyValue, src => notEncodeProperty.Contains(src.PropertyCode) ? src.PropertyValue : baseEncrypt.Decode(src.PropertyValue));

            config.ForType<PropertyChangeListDto, ApprovePropertyChangeListOutput>()
                .Map(dest => dest.ChangeUserName, src => baseEncrypt.Decode(src.ChangeUserName));

            config.ForType<PropertyChangeListDto, ChangeLogMain>()
                .Map(dest => dest.ChangeUserName, src => baseEncrypt.Decode(src.ChangeUserName));

            config.ForType<UserChangeListDto, ApproveUserChangeListOutput>()
                .Map(dest => dest.ChangeUserName, src => baseEncrypt.Decode(src.ChangeUserName))
                .Map(dest => dest.ChangeUserPhone, src => baseEncrypt.Decode(src.ChangeUserPhone));

            config.ForType<RpcService.UserSearchTableQuery, UserTableQuery>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.NickName, src => src.NickName)
                .Map(dest => dest.StudentNo, src => src.StudentNo)
                .Map(dest => dest.Unit, src => src.Unit)
                .Map(dest => dest.Edu, src => src.Edu)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Depart, src => src.Depart)
                .Map(dest => dest.DepartName, src => src.DepartName)
                .Map(dest => dest.College, src => src.College)
                .Map(dest => dest.CollegeName, src => src.CollegeName)
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart)
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName)
                .Map(dest => dest.Major, src => src.Major)
                .Map(dest => dest.Grade, src => src.Grade)
                .Map(dest => dest.Class, src => src.Class)
                .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.TypeName, src => src.TypeName)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.IDCard, src => src.IDCard)
                .Map(dest => dest.Phone, src => src.Phone)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.BirthdayStartTime, src => DataConverter.ToNumableDateTime(src.BirthdayStartTime))
                .Map(dest => dest.BirthdayEndTime, src => DataConverter.ToNumableDateTime(src.BirthdayEndTime))
                .Map(dest => dest.Gender, src => src.Gender)
                .Map(dest => dest.Addr, src => src.Addr)
                .Map(dest => dest.AddrDetail, src => src.AddrDetail)
                .Map(dest => dest.SourceFrom, src => src.SourceFrom)
                .Map(dest => dest.LastLoginStartTime, src => DataConverter.ToNumableDateTime(src.LastLoginStartTime))
                .Map(dest => dest.LastLoginEndTime, src => DataConverter.ToNumableDateTime(src.LastLoginEndTime))
                .Map(dest => dest.LeaveStartTime, src => DataConverter.ToNumableDateTime(src.LeaveStartTime))
                .Map(dest => dest.LeaveEndTime, src => DataConverter.ToNumableDateTime(src.LeaveEndTime))
                .Map(dest => dest.CardNo, src => src.CardNo)
                .Map(dest => dest.CardBarCode, src => src.CardBarCode)
                .Map(dest => dest.CardPhysicNo, src => src.CardPhysicNo)
                .Map(dest => dest.CardIdentityNo, src => src.CardIdentityNo)
                .Map(dest => dest.CardIsPrincipal, src => src.CardIsPrincipal)
                .Map(dest => dest.CardType, src => src.CardType)
                .Map(dest => dest.CardTypeName, src => src.CardTypeName)
                .Map(dest => dest.CardStatus, src => src.CardStatus)
                .Map(dest => dest.CardIssueStartTime, src => DataConverter.ToNumableDateTime(src.CardIssueStartTime))
                .Map(dest => dest.CardIssueEndTime, src => DataConverter.ToNumableDateTime(src.CardIssueEndTime))
                .Map(dest => dest.CardExpireStartTime, src => DataConverter.ToNumableDateTime(src.CardExpireStartTime))
                .Map(dest => dest.CardExpireEndTime, src => DataConverter.ToNumableDateTime(src.CardExpireEndTime))
                .Map(dest => dest.IsStaff, src => src.IsStaff);

            config.ForType<MergeUserDto, MergeUserOutput>()
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Decode(src.Name))
                .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Decode(src.Type))
                .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Decode(src.TypeName))
                .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Decode(src.Title))
                .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Decode(src.College))
                .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Decode(src.CollegeName))
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Decode(src.CollegeDepart))
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Decode(src.CollegeDepartName))
                .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Decode(src.Major))
                .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Decode(src.Grade))
                .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Decode(src.Class))
                .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Decode(src.Gender))
                .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Decode(src.Edu))
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Decode(src.Depart))
                .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Decode(src.DepartName))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Decode(src.Phone))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Decode(src.IdCard))
                .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Decode(src.Addr))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Decode(src.AddrDetail));

            config.ForType<MergeUserInput, MergeUserInfo>()
                .Map(dest => dest.Name, src => src.Name == null ? null : baseEncrypt.Encode(src.Name))
                .Map(dest => dest.Type, src => src.Type == null ? null : baseEncrypt.Encode(src.Type))
                .Map(dest => dest.TypeName, src => src.TypeName == null ? null : baseEncrypt.Encode(src.TypeName))
                .Map(dest => dest.Title, src => src.Title == null ? null : baseEncrypt.Encode(src.Title))
                .Map(dest => dest.College, src => src.College == null ? null : baseEncrypt.Encode(src.College))
                .Map(dest => dest.CollegeName, src => src.CollegeName == null ? null : baseEncrypt.Encode(src.CollegeName))
                .Map(dest => dest.CollegeDepart, src => src.CollegeDepart == null ? null : baseEncrypt.Encode(src.CollegeDepart))
                .Map(dest => dest.CollegeDepartName, src => src.CollegeDepartName == null ? null : baseEncrypt.Encode(src.CollegeDepartName))
                .Map(dest => dest.Major, src => src.Major == null ? null : baseEncrypt.Encode(src.Major))
                .Map(dest => dest.Grade, src => src.Grade == null ? null : baseEncrypt.Encode(src.Grade))
                .Map(dest => dest.Class, src => src.Class == null ? null : baseEncrypt.Encode(src.Class))
                .Map(dest => dest.Gender, src => src.Gender == null ? null : baseEncrypt.Encode(src.Gender))
                .Map(dest => dest.Edu, src => src.Edu == null ? null : baseEncrypt.Encode(src.Edu))
                .Map(dest => dest.Depart, src => src.Depart == null ? null : baseEncrypt.Encode(src.Depart))
                .Map(dest => dest.DepartName, src => src.DepartName == null ? null : baseEncrypt.Encode(src.DepartName))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : baseEncrypt.Encode(src.Phone))
                .Map(dest => dest.IdCard, src => src.IdCard == null ? null : baseEncrypt.Encode(src.IdCard))
                .Map(dest => dest.Addr, src => src.Addr == null ? null : baseEncrypt.Encode(src.Addr))
                .Map(dest => dest.AddrDetail, src => src.AddrDetail == null ? null : baseEncrypt.Encode(src.AddrDetail));
        }
    }
}
