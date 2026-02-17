using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_PartNumberStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialSuplier",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialSupplierDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSuplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartNumberStructure",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PartNumberDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialSuplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartNumberStructure_MaterialSuplier_MaterialSuplierId",
                        column: x => x.MaterialSuplierId,
                        principalSchema: "upm_productionControl",
                        principalTable: "MaterialSuplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberStructure_PartNumberLogistics_PartNumberLogisticsId",
                        column: x => x.PartNumberLogisticsId,
                        principalSchema: "upm_productionControl",
                        principalTable: "PartNumberLogistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_MaterialSuplierId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "MaterialSuplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_PartNumberLogisticsId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "PartNumberLogisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_ProductionStationId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "ProductionStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberStructure",
                schema: "upm_productionControl");

            migrationBuilder.DropTable(
                name: "MaterialSuplier",
                schema: "upm_productionControl");
        }
    }
}
