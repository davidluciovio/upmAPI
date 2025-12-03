using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class editTable_ProductionStation_addColumn_ModelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStation_ModelId",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                column: "ModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductionStation_ModelId",
                schema: "upm_assyProduction",
                table: "ProductionStation");

            migrationBuilder.DropColumn(
                name: "ModelId",
                schema: "upm_assyProduction",
                table: "ProductionStation");
        }
    }
}
