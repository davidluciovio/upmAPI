using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.AssyProduction
{
    /// <inheritdoc />
    public partial class createTable_ProductionRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionRegister",
                schema: "upm_assyProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    ProductionStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionRegister_ProductionStation_ProductionStationId",
                        column: x => x.ProductionStationId,
                        principalSchema: "upm_assyProduction",
                        principalTable: "ProductionStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRegister_ProductionStationId",
                schema: "upm_assyProduction",
                table: "ProductionRegister",
                column: "ProductionStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionRegister",
                schema: "upm_assyProduction");
        }
    }
}
