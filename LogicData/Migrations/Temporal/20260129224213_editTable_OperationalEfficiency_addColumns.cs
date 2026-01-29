using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Temporal
{
    /// <inheritdoc />
    public partial class editTable_OperationalEfficiency_addColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Jefe",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Managment",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Jefe",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "Managment",
                schema: "upm_temporal",
                table: "OperationalEfficiency");
        }
    }
}
