using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class editTable_PartNumberLogistics_addColumn_LocationId_SNP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberArea",
                schema: "upm_productionControl");

            migrationBuilder.CreateTable(
                name: "PartNumberLogistics",
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
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SNP = table.Column<float>(type: "real", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberLogistics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberLogistics_AreaId",
                schema: "upm_productionControl",
                table: "PartNumberLogistics",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberLogistics_LocationId",
                schema: "upm_productionControl",
                table: "PartNumberLogistics",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberLogistics_PartNumberId",
                schema: "upm_productionControl",
                table: "PartNumberLogistics",
                column: "PartNumberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberLogistics",
                schema: "upm_productionControl");

            migrationBuilder.CreateTable(
                name: "PartNumberArea",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberArea", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberArea_AreaId",
                schema: "upm_productionControl",
                table: "PartNumberArea",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberArea_PartNumberId",
                schema: "upm_productionControl",
                table: "PartNumberArea",
                column: "PartNumberId");
        }
    }
}
