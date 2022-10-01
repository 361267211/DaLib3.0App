﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLibrary.ScoreCenter.EntityFramework.Core.DbContexts;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    [DbContext(typeof(ScoreCenterDbContext))]
    [Migration("20211230065900_init_schema")]
    partial class init_schema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.BasicConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("RuleContent")
                        .HasMaxLength(8000)
                        .HasColumnType("character varying(8000)");

                    b.Property<bool>("ShowLevel")
                        .HasColumnType("boolean");

                    b.Property<bool>("ShowRule")
                        .HasColumnType("boolean");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("BasicConfig", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.GoodsRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CurrentCount")
                        .HasColumnType("integer");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("DetailInfo")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("FreezeCount")
                        .HasColumnType("integer");

                    b.Property<string>("IntroPicUrl")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("ObtainAddress")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("ObtainContact")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ObtainTime")
                        .HasColumnType("text");

                    b.Property<int>("ObtainWay")
                        .HasColumnType("integer");

                    b.Property<int>("SaleOutCount")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("TotalCount")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("GoodsRecord", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.MedalObtainTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("AppCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("AppLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Desc")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EventCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("ForApp")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForH5")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForMicroApp")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForPc")
                        .HasColumnType("boolean");

                    b.Property<string>("H5Link")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("IntroPicUrl")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MicroAppLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("MustContinue")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PcLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("TotalTime")
                        .HasColumnType("integer");

                    b.Property<int>("TriggerWay")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("MedalObtainTask", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.OperationRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("OperateKey")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("OperateKey")
                        .IsUnique();

                    b.ToTable("OperationRecord", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.OrderRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("ExchangeCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ExchangeCount")
                        .HasColumnType("integer");

                    b.Property<string>("ExchangeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("ExchangeTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ExchangeUserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Express")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ExpressNo")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("GoodsID")
                        .HasColumnType("uuid");

                    b.Property<string>("ObtainTime")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("ObtainWay")
                        .HasColumnType("integer");

                    b.Property<string>("RecieveAddrss")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("OrderRecord", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.OverdueScoreClear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ExpireTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OverScore")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("OverdueScoreClear", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreConsumeTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("AppCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("ConsumeScore")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Desc")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("EventCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreConsumeTask", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<int>("ArchiveScore")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreLevel", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreManualProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("OperatorName")
                        .HasColumnType("text");

                    b.Property<DateTime>("OperatorTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("OperatorUserKey")
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("SourceFrom")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ValidTerm")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreManualProcess", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreObtainTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("AppCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("AppLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Desc")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EventCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("ForApp")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForH5")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForMicroApp")
                        .HasColumnType("boolean");

                    b.Property<bool>("ForPc")
                        .HasColumnType("boolean");

                    b.Property<string>("FullEventCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("H5Link")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("IntroPicUrl")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MicroAppLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("ObtainScore")
                        .HasColumnType("integer");

                    b.Property<string>("PcLink")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("TriggerTerm")
                        .HasColumnType("integer");

                    b.Property<int>("TriggerTime")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ValidTerm")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreObtainTask", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreRecieveRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("OperateType")
                        .HasColumnType("integer");

                    b.Property<Guid>("ProcessID")
                        .HasColumnType("uuid");

                    b.Property<string>("PropertyCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PropertyValue")
                        .HasColumnType("text");

                    b.Property<int>("RuleType")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreRecieveRule", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.ScoreRecieveUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ProcessID")
                        .HasColumnType("uuid");

                    b.Property<int>("SourceFrom")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ScoreRecieveUser", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.SysMenuPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("Component")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreateTime")
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

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("SysMenuPermission", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.UserEventScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("AppCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("EventCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("EventScore")
                        .HasColumnType("integer");

                    b.Property<string>("FullEventCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Remark")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("TriggerTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("UserEventScore", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.UserMedal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MedalObtainTaskId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("TotalTime")
                        .HasColumnType("integer");

                    b.Property<int>("TriggerTime")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("UserMedal", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.UserMedalEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MedalObtainTaskId")
                        .HasColumnType("uuid");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("TriggerReset")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("TriggerTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("UserMedalEvent", "dbo");
                });

            modelBuilder.Entity("SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys.UserScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<int>("AvailableScore")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DeleteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("FreezeScore")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreateTime")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("UserScore", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
