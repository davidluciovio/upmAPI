using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations
{
    /// <inheritdoc />
    public partial class editTable_AuthUser_addColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "upm_auth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "upm_auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                schema: "upm_auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "upm_auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                schema: "upm_auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "upm_auth",
                table: "AspNetUsers");
        }
    }
}
