using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Temporal
{
    /// <inheritdoc />
    public partial class createTable_OperationalEfficiency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationalEfficiency",
                schema: "upm_temporal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Supervisor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Leader = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartNumberName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HP = table.Column<float>(type: "real", nullable: false),
                    Neck = table.Column<float>(type: "real", nullable: false),
                    PartNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RealTime = table.Column<float>(type: "real", nullable: false),
                    OperativityPercent = table.Column<float>(type: "real", nullable: false),
                    PriductionReal = table.Column<float>(type: "real", nullable: false),
                    TotalTime = table.Column<float>(type: "real", nullable: false),
                    ProgramabeDowntimeTime = table.Column<float>(type: "real", nullable: false),
                    RealWorkingTime = table.Column<float>(type: "real", nullable: false),
                    NetoWorkingTime = table.Column<float>(type: "real", nullable: false),
                    NetoProduictiveTime = table.Column<float>(type: "real", nullable: false),
                    TotalDowntime = table.Column<float>(type: "real", nullable: false),
                    NoProgramabeDowntimeTime = table.Column<float>(type: "real", nullable: false),
                    NoReportedTime = table.Column<float>(type: "real", nullable: false),
                    DowntimePercent = table.Column<float>(type: "real", nullable: false),
                    NoProgramableDowntimePercent = table.Column<float>(type: "real", nullable: false),
                    ProgramableDowntimePercent = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalEfficiency", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationalEfficiency",
                schema: "upm_temporal");
        }
    }
}
