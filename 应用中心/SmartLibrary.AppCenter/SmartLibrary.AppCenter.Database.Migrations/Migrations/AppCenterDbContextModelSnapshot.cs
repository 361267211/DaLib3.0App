﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLibrary.AppCenter.EntityFramework.Core.DbContexts;

namespace SmartLibrary.AppCenter.Database.Migrations.Migrations
{
    [DbContext(typeof(AppCenterDbContext))]
    partial class AppCenterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppCenterSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("ItemKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ItemValue")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AppCenterSettings", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppCollection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("AppCollection", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppColumnInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("AppRouteCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("ColumnCreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ColumnId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ColumnName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VisitUrl")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("AppColumnInfo", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppManager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("ManageRoleId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ManagerType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AppManager", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppNavigation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("NavigationId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AppNavigation", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppReName", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("AppNewName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("AppReName", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserSetId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("UserSetType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AppUser", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.ManagerRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Remark")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ManagerRole", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Navigation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("PrivateFirst")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Navigation", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission.SysMenuCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Sort")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("SysMenuCategory", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission.SysMenuPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("Component")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("FullPath")
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<bool>("IsSysMenu")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("OpenWay")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<string>("Permission")
                        .HasColumnType("text");

                    b.Property<string>("Pid")
                        .HasColumnType("text")
                        .HasComment("父级id");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.Property<string>("Router")
                        .HasColumnType("text");

                    b.Property<int>("Sort")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("SysMenuPermission", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission.SysRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("SysRole", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission.SysRoleMenu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MenuPermissionID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("SysRoleMenu", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission.SysUserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserID")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SysUserRole", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.ThirdApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppExplain")
                        .HasColumnType("text");

                    b.Property<string>("AppIcon")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("AppIntroduction")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("AppType")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Contacts")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Developer")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FrontUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Terminal")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ThirdApplication", "public");
                });

            modelBuilder.Entity("SmartLibrary.AppCenter.EntityFramework.Core.Entitys.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSuper")
                        .HasColumnType("boolean");

                    b.Property<string>("ManagerRoleIds")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("UserRole", "public");
                });
#pragma warning restore 612, 618
        }
    }
}
