using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class createTable_DowntimeRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DowntimeRegister",
                schema: "upm_assyProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDowntimeDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDowntimeDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataProductionDowntimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DowntimeRegister_ProductionStation_ProductionStationId",
                        column: x => x.ProductionStationId,
                        principalSchema: "upm_assyProduction",
                        principalTable: "ProductionStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DowntimeRegister_ProductionStationId",
                schema: "upm_assyProduction",
                table: "DowntimeRegister",
                column: "ProductionStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DowntimeRegister",
                schema: "upm_assyProduction");
        }
    }
}
