using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class editTable_ProductionStation_addColumns_parameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "NetoTime",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ObjetiveTime",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "OperatorQuantity",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PartNumberQuantity",
                schema: "upm_assyProduction",
                table: "ProductionStation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NetoTime",
                schema: "upm_assyProduction",
                table: "ProductionStation");

            migrationBuilder.DropColumn(
                name: "ObjetiveTime",
                schema: "upm_assyProduction",
                table: "ProductionStation");

            migrationBuilder.DropColumn(
                name: "OperatorQuantity",
                schema: "upm_assyProduction",
                table: "ProductionStation");

            migrationBuilder.DropColumn(
                name: "PartNumberQuantity",
                schema: "upm_assyProduction",
                table: "ProductionStation");
        }
    }
}
