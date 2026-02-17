using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class renameTable_PartNumberStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberEstructure",
                schema: "upm_productionControl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialSupplier",
                schema: "upm_productionControl",
                table: "MaterialSupplier");

            migrationBuilder.RenameTable(
                name: "MaterialSupplier",
                schema: "upm_productionControl",
                newName: "MaterialSuplier",
                newSchema: "upm_productionControl");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialSupplierDescription",
                schema: "upm_productionControl",
                table: "MaterialSuplier",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialSuplier",
                schema: "upm_productionControl",
                table: "MaterialSuplier",
                column: "Id");

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
                    PartNumberLogisticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletePartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletePartName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MaterialSuplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        name: "FK_PartNumberStructure_PartNumberLogistics_PartNumberLogisticId",
                        column: x => x.PartNumberLogisticId,
                        principalSchema: "upm_productionControl",
                        principalTable: "PartNumberLogistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartNumberStructure_PartNumberLogistics_PartNumberLogisticsId",
                        column: x => x.PartNumberLogisticsId,
                        principalSchema: "upm_productionControl",
                        principalTable: "PartNumberLogistics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_MaterialSuplierId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "MaterialSuplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_PartNumberLogisticId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "PartNumberLogisticId");

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberStructure_PartNumberLogisticsId",
                schema: "upm_productionControl",
                table: "PartNumberStructure",
                column: "PartNumberLogisticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartNumberStructure",
                schema: "upm_productionControl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialSuplier",
                schema: "upm_productionControl",
                table: "MaterialSuplier");

            migrationBuilder.RenameTable(
                name: "MaterialSuplier",
                schema: "upm_productionControl",
                newName: "MaterialSupplier",
                newSchema: "upm_productionControl");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialSupplierDescription",
                schema: "upm_productionControl",
                table: "MaterialSupplier",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialSupplier",
                schema: "upm_productionControl",
                table: "MaterialSupplier",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PartNumberEstructure",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialSupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CompletePartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletePartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaterialSuplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
    }
}
