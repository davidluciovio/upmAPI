using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_Area : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Area",
                schema: "upm_pc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProductionLocation",
                schema: "upm_pc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionLocationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProductionLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProductionModel",
                schema: "upm_pc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionModelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProductionModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentAlert_ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert",
                column: "ProductionPartNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_AreaName",
                schema: "upm_pc",
                table: "Area",
                column: "AreaName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentAlert_ProductionPartNumber_ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert",
                column: "ProductionPartNumberId",
                principalSchema: "upm_pc",
                principalTable: "ProductionPartNumber",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentAlert_ProductionPartNumber_ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert");

            migrationBuilder.DropTable(
                name: "Area",
                schema: "upm_pc");

            migrationBuilder.DropTable(
                name: "DataProductionLocation",
                schema: "upm_pc");

            migrationBuilder.DropTable(
                name: "DataProductionModel",
                schema: "upm_pc");

            migrationBuilder.DropIndex(
                name: "IX_ComponentAlert_ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert");

            migrationBuilder.DropColumn(
                name: "ProductionPartNumberId",
                schema: "upm_pc",
                table: "ComponentAlert");
        }
    }
}
