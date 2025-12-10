using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations
{
    /// <inheritdoc />
    public partial class editTable_Module_addColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "upm_auth",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Route",
                schema: "upm_auth",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "upm_auth",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Route",
                schema: "upm_auth",
                table: "Modules");
        }
    }
}
