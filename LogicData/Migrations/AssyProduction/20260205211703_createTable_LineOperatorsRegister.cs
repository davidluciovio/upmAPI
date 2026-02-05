using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class createTable_LineOperatorsRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineOperatorsRegister",
                schema: "upm_assyProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineOperatorsRegister", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineOperatorsRegister_LineId",
                schema: "upm_assyProduction",
                table: "LineOperatorsRegister",
                column: "LineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineOperatorsRegister",
                schema: "upm_assyProduction");
        }
    }
}
