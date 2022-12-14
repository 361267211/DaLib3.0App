// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLibrary.SceneManage.EntityFramework.Core.DbContexts;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    [DbContext(typeof(SceneManageDbContext))]
    [Migration("20211111123223_update-sorttype")]
    partial class updatesorttype
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.Layout", b =>
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

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("TerminalType")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Layout", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.Scene", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Cover")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("FooterTemplateId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("HeaderTemplateId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<bool>("IsSystemScene")
                        .HasColumnType("boolean");

                    b.Property<string>("LayoutId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TemplateId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("TerminalInstanceId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("ThemeColor")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VisitUrl")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("VisitorLimitType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Scene", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SceneApp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("AppWidgetId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("SceneId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("ScreenId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.Property<int>("XIndex")
                        .HasColumnType("integer");

                    b.Property<int>("YIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SceneApp", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SceneAppPlate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppPlateId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("SceneAppId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("SceneId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("SortType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("TopCount")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("SceneAppPlate", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SceneScreen", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer");

                    b.Property<string>("SceneId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("ScreenName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("SceneScreen", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SceneUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("SceneId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserSetId")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<int>("UserSetType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SceneUser", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SysMenuCategory", b =>
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

                    b.ToTable("SysMenuCategory", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SysMenuPermission", b =>
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

                    b.ToTable("SysMenuPermission", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SysRole", b =>
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

                    b.ToTable("SysRole", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SysRoleMenu", b =>
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

                    b.ToTable("SysRoleMenu", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.SysUserRole", b =>
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

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("SysUserRole", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AspectRatio")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<int>("ColumnCount")
                        .HasColumnType("integer");

                    b.Property<string>("Cover")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DefaultFooterTemplateId")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("DefaultHeaderTemplateId")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsLock")
                        .HasColumnType("boolean");

                    b.Property<string>("LayoutId")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Router")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ScreenCount")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasMaxLength(48)
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Template", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.TerminalInstance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsSystemInstance")
                        .HasColumnType("boolean");

                    b.Property<string>("KeyWords")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Remark")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("TerminalType")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VisitUrl")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("TerminalInstance", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.SceneManage.EntityFramework.Core.Entitys.ThemeColor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ThemeColor", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
