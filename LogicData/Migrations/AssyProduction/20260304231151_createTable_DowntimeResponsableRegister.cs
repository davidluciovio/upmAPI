using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class createTable_DowntimeResponsableRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DowntimeResponsableRegister",
                schema: "upm_assyProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DowntimeRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeResponsableRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DowntimeResponsableRegister_DowntimeRegister_DowntimeRegisterId",
                        column: x => x.DowntimeRegisterId,
                        principalSchema: "upm_assyProduction",
                        principalTable: "DowntimeRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeResponsableRegister_DowntimeRegisterId",
                schema: "upm_assyProduction",
                table: "DowntimeResponsableRegister",
                column: "DowntimeRegisterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DowntimeResponsableRegister",
                schema: "upm_assyProduction");
        }
    }
}
