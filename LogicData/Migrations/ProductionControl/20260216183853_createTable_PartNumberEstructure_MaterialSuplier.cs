using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_PartNumberEstructure_MaterialSuplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialSupplier",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialSupplierDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSupplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartNumberEstructure",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberLogisticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletePartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletePartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MaterialSuplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialSupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberEstructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartNumberEstructure_MaterialSupplier_MaterialSupplierId",
                        column: x => x.MaterialSupplierId,
                        principalSchema: "upm_productionControl",
                        principalTable: "MaterialSupplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberEstructure_PartNumberLogistics_PartNumberLogisticsId",
                        column: x => x.PartNumberLogisticsId,
                        principalSchema: "upm_productionControl",
                        principalTable: "PartNumberLogistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberEstructure_MaterialSupplierId",
                schema: "upm_productionControl",
                table: "PartNumberEstructure",
                column: "MaterialSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberEstructure_PartNumberLogisticsId",
                schema: "upm_productionControl",
                table: "PartNumberEstructure",
                column: "PartNumberLogisticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberEstructure",
                schema: "upm_productionControl");

            migrationBuilder.DropTable(
                name: "MaterialSupplier",
                schema: "upm_productionControl");
        }
    }
}
