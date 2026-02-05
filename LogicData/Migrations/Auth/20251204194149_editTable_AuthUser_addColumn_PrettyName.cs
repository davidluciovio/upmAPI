using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations
{
    /// <inheritdoc />
    public partial class editTable_AuthUser_addColumn_PrettyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrettyName",
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
                name: "PrettyName",
                schema: "upm_auth",
                table: "AspNetUsers");
        }
    }
}
