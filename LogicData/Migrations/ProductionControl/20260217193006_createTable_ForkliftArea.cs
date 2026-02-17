using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_ForkliftArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForkliftArea",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataProductionAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForkliftArea", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForkliftArea_DataProductionAreaId",
                schema: "upm_productionControl",
                table: "ForkliftArea",
                column: "DataProductionAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ForkliftArea_UserId",
                schema: "upm_productionControl",
                table: "ForkliftArea",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForkliftArea",
                schema: "upm_productionControl");
        }
    }
}
