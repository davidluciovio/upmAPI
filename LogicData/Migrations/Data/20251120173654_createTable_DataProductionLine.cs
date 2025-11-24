using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class createTable_DataProductionLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartNumber_ProductionArea_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartNumber_ProductionLocation_ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartNumber_ProductionModel_ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartNumber_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartNumber_ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartNumber_ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropColumn(
                name: "ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropColumn(
                name: "ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropColumn(
                name: "ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropColumn(
                name: "SNP",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.CreateTable(
                name: "ProductionLine",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLine_LineDescription",
                schema: "upm",
                table: "ProductionLine",
                column: "LineDescription",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionLine",
                schema: "upm");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SNP",
                schema: "upm",
                table: "ProductionPartNumber",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartNumber_ProductionArea_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionAreaId",
                principalSchema: "upm",
                principalTable: "ProductionArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartNumber_ProductionLocation_ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionLocationId",
                principalSchema: "upm",
                principalTable: "ProductionLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartNumber_ProductionModel_ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionModelId",
                principalSchema: "upm",
                principalTable: "ProductionModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
