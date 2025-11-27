using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_PartNumberLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartNumberLocation",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberLocation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberLocation_LocationId",
                schema: "upm_productionControl",
                table: "PartNumberLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberLocation_PartNumberId",
                schema: "upm_productionControl",
                table: "PartNumberLocation",
                column: "PartNumberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberLocation",
                schema: "upm_productionControl");
        }
    }
}
