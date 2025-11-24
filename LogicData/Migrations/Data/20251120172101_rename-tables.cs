using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class renametables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "WorkShift",
                schema: "upm");

            migrationBuilder.CreateTable(
                name: "DataStatus",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataWorkShift",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShiftDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SecondsQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataWorkShift", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataStatus_StatusDescription",
                schema: "upm",
                table: "DataStatus",
                column: "StatusDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataWorkShift_ShiftDescription",
                schema: "upm",
                table: "DataWorkShift",
                column: "ShiftDescription",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataStatus",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "DataWorkShift",
                schema: "upm");

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkShift",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SecondsQuantity = table.Column<int>(type: "int", nullable: false),
                    ShiftDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShift", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Status_StatusDescription",
                schema: "upm",
                table: "Status",
                column: "StatusDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkShift_ShiftDescription",
                schema: "upm",
                table: "WorkShift",
                column: "ShiftDescription",
                unique: true);
        }
    }
}
