﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    [DbContext(typeof(DatabaseTerraceDbContext))]
    [Migration("20211009022636_v1.0.11")]
    partial class v1011
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SmartLibrary.Core.Entitys.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Person", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EnglihName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<double?>("Language")
                        .HasColumnType("double precision");

                    b.Property<string>("Plate")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<double?>("Type")
                        .HasColumnType("double precision");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Asset", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.CustomLabel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Createtime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("LabelName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("LabelName");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("CustomLabel", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseAcessUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DatabaseID")
                        .HasColumnType("text");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DatabaseAcessUrl", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseColumn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ColumnName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("ColumnName");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean")
                        .HasColumnName("DeleteFlag");

                    b.Property<int>("MatchCount")
                        .HasColumnType("integer")
                        .HasColumnName("MatchCount");

                    b.Property<int>("OrderRule")
                        .HasColumnType("integer")
                        .HasColumnName("OrderRule");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreatedTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("DatabaseColumn", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseColumnRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ColumnID")
                        .HasMaxLength(50)
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean")
                        .HasColumnName("DeleteFlag");

                    b.Property<string>("RuleKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RuleValue")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RuleValueName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DatabaseColumnRule", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseSubscriber", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DatabaseID")
                        .HasColumnType("uuid");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DatabaseSubscriber", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseTerrace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Abbreviation")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Abbreviation");

                    b.Property<string>("ArticleTypes")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("ArticleTypes");

                    b.Property<string>("Cover")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("Cover");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DatabaseProviderID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("DatabaseProviderID");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean")
                        .HasColumnName("DeleteFlag");

                    b.Property<string>("DomainClcs")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("DomainClcs");

                    b.Property<string>("DomainEscs")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("DomainEscs");

                    b.Property<DateTime>("ExpiryBeginTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("ExpiryBeginTime");

                    b.Property<DateTime>("ExpiryEndTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("ExpiryEndTime");

                    b.Property<string>("IndirectUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("IndirectUrl");

                    b.Property<string>("Information")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("Information");

                    b.Property<string>("Initials")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Initials");

                    b.Property<string>("Introduction")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Introduction");

                    b.Property<bool>("IsShow")
                        .HasColumnType("boolean")
                        .HasColumnName("IsShow");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("Label");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Language");

                    b.Property<long>("MonthClickNum")
                        .HasColumnType("bigint");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer")
                        .HasColumnName("OrderIndex");

                    b.Property<string>("PurchaseType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("PurchaseType");

                    b.Property<string>("Remark")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("Status");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Title");

                    b.Property<long>("TotalClickNum")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UseGuide")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("UseGuide");

                    b.Property<string>("WhiteList")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("WhiteList");

                    b.HasKey("Id");

                    b.ToTable("DatabaseTerrace", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseTerraceSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("CanFilterCustomLabel")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Introduce")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsLoginAcess")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpenComment")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpenExternalUrl")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpenFeedback")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpenVpn")
                        .HasColumnType("boolean");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DatabaseTerraceSettings", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseUrlName", b =>
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
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Name");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DatabaseUrlName", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DomainEscsAttr", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DomainEscsAttr", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.SysMenuCategory", b =>
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

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.SysMenuPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BtnType")
                        .HasColumnType("integer");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("Code")
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

                    b.Property<long>("Path")
                        .HasColumnType("bigint");

                    b.Property<string>("Permission")
                        .HasColumnType("text");

                    b.Property<int>("Pid")
                        .HasColumnType("integer")
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

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.SysRole", b =>
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

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.SysRoleMenu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("MenuPermissionID")
                        .HasColumnType("integer");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("SysRoleMenu", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.SysUserRole", b =>
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
#pragma warning restore 612, 618
        }
    }
}
