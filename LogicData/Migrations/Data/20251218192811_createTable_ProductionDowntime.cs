using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class createTable_ProductionDowntime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionDowntime",
                schema: "upm_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DowntimeDescription = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InforCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDirectDowntime = table.Column<bool>(type: "bit", nullable: false),
                    Programable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionDowntime", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDowntime_DowntimeDescription",
                schema: "upm_data",
                table: "ProductionDowntime",
                column: "DowntimeDescription",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionDowntime",
                schema: "upm_data");
        }
    }
}
