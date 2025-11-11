using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class editTableDataProductionPartNumber_addColumn_ProductionAreaID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartNumber_ProductionArea_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionAreaId",
                principalSchema: "upm",
                principalTable: "ProductionArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartNumber_ProductionArea_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartNumber_ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");

            migrationBuilder.DropColumn(
                name: "ProductionAreaId",
                schema: "upm",
                table: "ProductionPartNumber");
        }
    }
}
