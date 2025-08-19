using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace upmData.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "upmdat");

            migrationBuilder.EnsureSchema(
                name: "upmap");

            migrationBuilder.EnsureSchema(
                name: "upmconf");

            migrationBuilder.EnsureSchema(
                name: "upmsh");

            migrationBuilder.EnsureSchema(
                name: "upmpc");

            migrationBuilder.CreateTable(
                name: "Downtime",
                schema: "upmdat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InforCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDirectDowntime = table.Column<bool>(type: "bit", nullable: false),
                    Programable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downtime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                schema: "upmdat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Line",
                schema: "upmdat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkCenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeLine = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    PLC_IP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                schema: "upmdat",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PartNumber",
                schema: "upmdat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectiveTime = table.Column<float>(type: "real", nullable: false),
                    NetoTime = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "upmdat",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SupplyArea",
                schema: "upmdat",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyArea", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "upmdat",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkShift",
                schema: "upmdat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    SecondsQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartNumberConfiguration",
                schema: "upmconf",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    SupplyAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("01d1daf8-ab61-42f6-8520-cb1afe9807cd")),
                    PartNumberCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberConfiguration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PartNumberConfiguration_Line_LineId",
                        column: x => x.LineId,
                        principalSchema: "upmdat",
                        principalTable: "Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberConfiguration_Model_ModelId",
                        column: x => x.ModelId,
                        principalSchema: "upmdat",
                        principalTable: "Model",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberConfiguration_PartNumber_PartNumberId",
                        column: x => x.PartNumberId,
                        principalSchema: "upmdat",
                        principalTable: "PartNumber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberConfiguration_SupplyArea_SupplyAreaId",
                        column: x => x.SupplyAreaId,
                        principalSchema: "upmdat",
                        principalTable: "SupplyArea",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberConfiguration_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "upmdat",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConfiguration",
                schema: "upmconf",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfiguration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserConfiguration_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "upmdat",
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConfiguration_User_UserID",
                        column: x => x.UserID,
                        principalSchema: "upmdat",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeReport",
                schema: "upmap",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeUser = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReport", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmployeeReport_Line_LineId",
                        column: x => x.LineId,
                        principalSchema: "upmdat",
                        principalTable: "Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeReport_WorkShift_WorkShiftId",
                        column: x => x.WorkShiftId,
                        principalSchema: "upmdat",
                        principalTable: "WorkShift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DowntimeRegister",
                schema: "upmap",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DowntimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeRegister", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DowntimeRegister_Downtime_DowntimeId",
                        column: x => x.DowntimeId,
                        principalSchema: "upmdat",
                        principalTable: "Downtime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DowntimeRegister_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiderConfiguration",
                schema: "upmconf",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiderConfiguration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LiderConfiguration_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiderConfiguration_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "upmdat",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialAlerts",
                schema: "upmsh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false),
                    CompleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialAlerts_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionRegister",
                schema: "upmap",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRegister", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductionRegister_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RackRegister",
                schema: "upmap",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoRACK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackRegister", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RackRegister_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequerimentsHistory",
                schema: "upmpc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plant01 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plant02 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RanQuantity = table.Column<float>(type: "real", nullable: false),
                    Production = table.Column<int>(type: "int", nullable: false),
                    PartNumberConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequerimentsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequerimentsHistory_PartNumberConfiguration_PartNumberConfigurationId",
                        column: x => x.PartNumberConfigurationId,
                        principalSchema: "upmconf",
                        principalTable: "PartNumberConfiguration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DowntimeRegister_Responsables",
                schema: "upmap",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeUser = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DowntimeRegisterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeRegister_Responsables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DowntimeRegister_Responsables_DowntimeRegister_DowntimeRegisterId",
                        column: x => x.DowntimeRegisterId,
                        principalSchema: "upmap",
                        principalTable: "DowntimeRegister",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeRegister_DowntimeId",
                schema: "upmap",
                table: "DowntimeRegister",
                column: "DowntimeId");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeRegister_PartNumberConfigurationId",
                schema: "upmap",
                table: "DowntimeRegister",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeRegister_Responsables_DowntimeRegisterId",
                schema: "upmap",
                table: "DowntimeRegister_Responsables",
                column: "DowntimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeReport_LineId",
                schema: "upmap",
                table: "EmployeeReport",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeReport_WorkShiftId",
                schema: "upmap",
                table: "EmployeeReport",
                column: "WorkShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_LiderConfiguration_PartNumberConfigurationId",
                schema: "upmconf",
                table: "LiderConfiguration",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_LiderConfiguration_UserId",
                schema: "upmconf",
                table: "LiderConfiguration",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAlerts_PartNumberConfigurationId",
                schema: "upmsh",
                table: "MaterialAlerts",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberConfiguration_LineId",
                schema: "upmconf",
                table: "PartNumberConfiguration",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberConfiguration_ModelId",
                schema: "upmconf",
                table: "PartNumberConfiguration",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberConfiguration_PartNumberId",
                schema: "upmconf",
                table: "PartNumberConfiguration",
                column: "PartNumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberConfiguration_SupplyAreaId",
                schema: "upmconf",
                table: "PartNumberConfiguration",
                column: "SupplyAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberConfiguration_UserId",
                schema: "upmconf",
                table: "PartNumberConfiguration",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRegister_PartNumberConfigurationId",
                schema: "upmap",
                table: "ProductionRegister",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_RackRegister_PartNumberConfigurationId",
                schema: "upmap",
                table: "RackRegister",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequerimentsHistory_PartNumberConfigurationId",
                schema: "upmpc",
                table: "RequerimentsHistory",
                column: "PartNumberConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Description",
                schema: "upmdat",
                table: "Role",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyArea_Description",
                schema: "upmdat",
                table: "SupplyArea",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CodeUser",
                schema: "upmdat",
                table: "User",
                column: "CodeUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguration_RoleID",
                schema: "upmconf",
                table: "UserConfiguration",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguration_UserID",
                schema: "upmconf",
                table: "UserConfiguration",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DowntimeRegister_Responsables",
                schema: "upmap");

            migrationBuilder.DropTable(
                name: "EmployeeReport",
                schema: "upmap");

            migrationBuilder.DropTable(
                name: "ErrorLog",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "LiderConfiguration",
                schema: "upmconf");

            migrationBuilder.DropTable(
                name: "MaterialAlerts",
                schema: "upmsh");

            migrationBuilder.DropTable(
                name: "ProductionRegister",
                schema: "upmap");

            migrationBuilder.DropTable(
                name: "RackRegister",
                schema: "upmap");

            migrationBuilder.DropTable(
                name: "RequerimentsHistory",
                schema: "upmpc");

            migrationBuilder.DropTable(
                name: "UserConfiguration",
                schema: "upmconf");

            migrationBuilder.DropTable(
                name: "DowntimeRegister",
                schema: "upmap");

            migrationBuilder.DropTable(
                name: "WorkShift",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "Downtime",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "PartNumberConfiguration",
                schema: "upmconf");

            migrationBuilder.DropTable(
                name: "Line",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "Model",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "PartNumber",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "SupplyArea",
                schema: "upmdat");

            migrationBuilder.DropTable(
                name: "User",
                schema: "upmdat");
        }
    }
}
