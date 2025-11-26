using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class changeSchema_upm_upm_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "upm_data");

            migrationBuilder.RenameTable(
                name: "ProductionPartNumber",
                schema: "upm",
                newName: "ProductionPartNumber",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "ProductionModel",
                schema: "upm",
                newName: "ProductionModel",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "ProductionLocation",
                schema: "upm",
                newName: "ProductionLocation",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "ProductionLine",
                schema: "upm",
                newName: "ProductionLine",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "ProductionArea",
                schema: "upm",
                newName: "ProductionArea",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "DataWorkShift",
                schema: "upm",
                newName: "DataWorkShift",
                newSchema: "upm_data");

            migrationBuilder.RenameTable(
                name: "DataStatus",
                schema: "upm",
                newName: "DataStatus",
                newSchema: "upm_data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "upm");

            migrationBuilder.RenameTable(
                name: "ProductionPartNumber",
                schema: "upm_data",
                newName: "ProductionPartNumber",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "ProductionModel",
                schema: "upm_data",
                newName: "ProductionModel",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "ProductionLocation",
                schema: "upm_data",
                newName: "ProductionLocation",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "ProductionLine",
                schema: "upm_data",
                newName: "ProductionLine",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "ProductionArea",
                schema: "upm_data",
                newName: "ProductionArea",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "DataWorkShift",
                schema: "upm_data",
                newName: "DataWorkShift",
                newSchema: "upm");

            migrationBuilder.RenameTable(
                name: "DataStatus",
                schema: "upm_data",
                newName: "DataStatus",
                newSchema: "upm");
        }
    }
}
