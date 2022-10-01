using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class init_data : Migration
    {
        private readonly string schema;
        public init_data(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "BasicConfigSet",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    SensitiveFilter = table.Column<bool>(type: "boolean", nullable: false),
                    UserInfoConfirm = table.Column<bool>(type: "boolean", nullable: false),
                    PropertyConfirm = table.Column<bool>(type: "boolean", nullable: false),
                    CardClaim = table.Column<bool>(type: "boolean", nullable: false),
                    UserInfoSupply = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicConfigSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    No = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BarCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhysicNo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IdentityNo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Deposit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Secret = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Usage = table.Column<int>(type: "integer", nullable: false),
                    SysBuildIn = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CardProperty",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CardID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NumValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TimeValue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BoolValue = table.Column<bool>(type: "boolean", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardProperty", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Desc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SourceFrom = table.Column<int>(type: "integer", nullable: false),
                    CreateUserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUserName = table.Column<string>(type: "text", nullable: true),
                    LastSyncTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RefTaskKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InfoPermitReader",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfigType = table.Column<int>(type: "integer", nullable: false),
                    ReaderType = table.Column<int>(type: "integer", nullable: false),
                    RefID = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoPermitReader", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Intro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    Unique = table.Column<bool>(type: "boolean", nullable: false),
                    ShowOnTable = table.Column<bool>(type: "boolean", nullable: false),
                    CanSearch = table.Column<bool>(type: "boolean", nullable: false),
                    ForReader = table.Column<bool>(type: "boolean", nullable: false),
                    ForCard = table.Column<bool>(type: "boolean", nullable: false),
                    SysBuildIn = table.Column<bool>(type: "boolean", nullable: false),
                    PropertyGroupID = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApproveStatus = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyChangeItem",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    LogID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyID = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    FieldCode = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChangeItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyChangeLog",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyType = table.Column<int>(type: "integer", nullable: false),
                    ChangeType = table.Column<int>(type: "integer", nullable: false),
                    ChangeTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ChangeUserID = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeUserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ChangeUserPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChangeLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyGroup",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SysBuildIn = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredCode = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyGroupItem",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApproveStatus = table.Column<int>(type: "integer", nullable: false),
                    SysBuildIn = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyGroupItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyGroupRule",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupID = table.Column<Guid>(type: "uuid", nullable: false),
                    CompareType = table.Column<int>(type: "integer", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyValue = table.Column<string>(type: "text", nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    UnionWay = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyGroupRule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReaderEditProperty",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "text", nullable: true),
                    IsEnable = table.Column<bool>(type: "boolean", nullable: false),
                    IsCheck = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderEditProperty", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                schema: "public",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PId = table.Column<int>(type: "integer", nullable: false),
                    SName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CityCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    YzCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Lng = table.Column<float>(type: "real", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    PinYin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SchedulerEntity",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cron = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AssemblyFullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClassFullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TaskParam = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AdapterAssemblyFullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AdapterClassFullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AdapterParm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NextTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BeginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IntervalSecond = table.Column<int>(type: "integer", nullable: false),
                    IsRepeat = table.Column<bool>(type: "boolean", nullable: false),
                    RunTimes = table.Column<int>(type: "integer", nullable: false),
                    RetryTimes = table.Column<int>(type: "integer", nullable: false),
                    IsDelete = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CronRemark = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulerEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuPermission",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Pid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "父级id"),
                    Path = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FullPath = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Icon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Router = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Component = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Permission = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OpenWay = table.Column<int>(type: "integer", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: false),
                    Remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsSysMenu = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuPermission", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysOrg",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Pid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Path = table.Column<int>(type: "integer", nullable: false),
                    FullPath = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysOrg", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SysBuildIn = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleMenu",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuPermissionID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleMenu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysUserRole",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NickName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StudentNo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Unit = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Edu = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Depart = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DepartName = table.Column<string>(type: "text", nullable: true),
                    College = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CollegeName = table.Column<string>(type: "text", nullable: true),
                    CollegeDepart = table.Column<string>(type: "text", nullable: true),
                    CollegeDepartName = table.Column<string>(type: "text", nullable: true),
                    Major = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Grade = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Class = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IdCard = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Addr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddrDetail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Photo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LeaveTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsStaff = table.Column<bool>(type: "boolean", nullable: false),
                    StaffStatus = table.Column<int>(type: "integer", nullable: true),
                    StaffBeginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdCardIdentity = table.Column<bool>(type: "boolean", nullable: false),
                    MobileIdentity = table.Column<bool>(type: "boolean", nullable: false),
                    EmailIdentity = table.Column<bool>(type: "boolean", nullable: false),
                    SourceFrom = table.Column<int>(type: "integer", nullable: false),
                    FirstLoginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastLoginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AsyncReaderId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserCardClaim",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CardID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplyTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCardClaim", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LaterDel = table.Column<bool>(type: "boolean", nullable: false),
                    LaterInsert = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserImportTempData",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserGender = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserPhone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StudentNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Edu = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    College = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CollegeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CollegeDepart = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CollegeDepartName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Major = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Class = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IdCard = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Addr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddrDetail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CardNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CardType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CardTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Error = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMsg = table.Column<string>(type: "text", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImportTempData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserProperty",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NumValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TimeValue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BoolValue = table.Column<bool>(type: "boolean", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProperty", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserPropertyChangeItem",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    LogID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    IsField = table.Column<bool>(type: "boolean", nullable: false),
                    PropertyType = table.Column<int>(type: "integer", nullable: false),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    PropertyCode = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPropertyChangeItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserPropertyChangeLog",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeType = table.Column<int>(type: "integer", nullable: false),
                    ChangeTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ChangeUserID = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeUserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ChangeUserPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPropertyChangeLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserRegister",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegister", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserThirdAuth",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginType = table.Column<string>(type: "text", nullable: false),
                    OpenId = table.Column<string>(type: "text", nullable: true),
                    UnionId = table.Column<string>(type: "text", nullable: true),
                    BindTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserThirdAuth", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicConfigSet_CreateTime",
                schema: $"{schema}",
                table: "BasicConfigSet",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Card_CreateTime",
                schema: $"{schema}",
                table: "Card",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_CardProperty_CreateTime",
                schema: $"{schema}",
                table: "CardProperty",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Group_CreateTime",
                schema: $"{schema}",
                table: "Group",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_InfoPermitReader_CreateTime",
                schema: $"{schema}",
                table: "InfoPermitReader",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CreateTime",
                schema: $"{schema}",
                table: "Property",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeItem_CreateTime",
                schema: $"{schema}",
                table: "PropertyChangeItem",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeLog_CreateTime",
                schema: $"{schema}",
                table: "PropertyChangeLog",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyGroup_CreateTime",
                schema: $"{schema}",
                table: "PropertyGroup",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyGroupItem_CreateTime",
                schema: $"{schema}",
                table: "PropertyGroupItem",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyGroupRule_CreateTime",
                schema: $"{schema}",
                table: "PropertyGroupRule",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ReaderEditProperty_CreateTime",
                schema: $"{schema}",
                table: "ReaderEditProperty",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuPermission_CreateTime",
                schema: $"{schema}",
                table: "SysMenuPermission",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysOrg_CreateTime",
                schema: $"{schema}",
                table: "SysOrg",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysRole_CreateTime",
                schema: $"{schema}",
                table: "SysRole",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysRoleMenu_CreateTime",
                schema: $"{schema}",
                table: "SysRoleMenu",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysUserRole_CreateTime",
                schema: $"{schema}",
                table: "SysUserRole",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreateTime",
                schema: $"{schema}",
                table: "User",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserCardClaim_CreateTime",
                schema: $"{schema}",
                table: "UserCardClaim",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_CreateTime",
                schema: $"{schema}",
                table: "UserGroup",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserImportTempData_CreateTime",
                schema: $"{schema}",
                table: "UserImportTempData",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserProperty_CreateTime",
                schema: $"{schema}",
                table: "UserProperty",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyChangeItem_CreateTime",
                schema: $"{schema}",
                table: "UserPropertyChangeItem",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyChangeLog_CreateTime",
                schema: $"{schema}",
                table: "UserPropertyChangeLog",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegister_CreateTime",
                schema: $"{schema}",
                table: "UserRegister",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserThirdAuth_CreateTime",
                schema: $"{schema}",
                table: "UserThirdAuth",
                column: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicConfigSet",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "Card",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "CardProperty",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "Group",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "InfoPermitReader",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "Property",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "PropertyChangeItem",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "PropertyChangeLog",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "PropertyGroup",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "PropertyGroupItem",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "PropertyGroupRule",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ReaderEditProperty",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "Region",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SchedulerEntity",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SysMenuPermission",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysOrg",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysRole",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysRoleMenu",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysUserRole",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "User",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserCardClaim",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserGroup",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserImportTempData",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserProperty",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserPropertyChangeItem",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserPropertyChangeLog",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserRegister",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserThirdAuth",
                schema: $"{schema}");
        }
    }
}
