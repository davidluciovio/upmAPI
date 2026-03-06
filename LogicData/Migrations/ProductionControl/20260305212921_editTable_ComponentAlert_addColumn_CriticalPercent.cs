using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class editTable_ComponentAlert_addColumn_CriticalPercent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CriticalPercent",
                schema: "upm_productionControl",
                table: "ComponentAlert",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalPercent",
                schema: "upm_productionControl",
                table: "ComponentAlert");
        }
    }
}
