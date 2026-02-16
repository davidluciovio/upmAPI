using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Auth
{
    /// <inheritdoc />
    public partial class editTable_AuthUser_addColoumn_CodeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeUser",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeUser",
                schema: "upm_auth",
                table: "AspNetUsers");
        }
    }
}
