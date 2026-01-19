using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.ProductionControl
{
    /// <inheritdoc />
    public partial class createTable_ComponentAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentAlert",
                schema: "upm_productionControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriticalBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriticalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumberLogisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentAlert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentAlert_PartNumberLogistics_PartNumberLogisticsId",
                        column: x => x.PartNumberLogisticsId,
                        principalSchema: "upm_productionControl",
                        principalTable: "PartNumberLogistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentAlert_PartNumberLogisticsId",
                schema: "upm_productionControl",
                table: "ComponentAlert",
                column: "PartNumberLogisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentAlert_StatusId",
                schema: "upm_productionControl",
                table: "ComponentAlert",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentAlert",
                schema: "upm_productionControl");
        }
    }
}
