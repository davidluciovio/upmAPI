using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Temporal
{
    /// <inheritdoc />
    public partial class editTable_OperationalEfficiency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Aprov",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Junta",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Pilotaje",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SPM_Real",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SPM_Set",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ST_SPM_Set",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Stroke",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TT",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TTT",
                schema: "upm_temporal",
                table: "OperationalEfficiency",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprov",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "Junta",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "Pilotaje",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "SPM_Real",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "SPM_Set",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "ST_SPM_Set",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "Stroke",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "TT",
                schema: "upm_temporal",
                table: "OperationalEfficiency");

            migrationBuilder.DropColumn(
                name: "TTT",
                schema: "upm_temporal",
                table: "OperationalEfficiency");
        }
    }
}
