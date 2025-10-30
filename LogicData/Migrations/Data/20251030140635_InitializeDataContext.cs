using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class InitializeDataContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "upm");

            migrationBuilder.CreateTable(
                name: "ProductionArea",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionLocation",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionModel",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkShift",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShiftDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SecondsQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPartNumber",
                schema: "upm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumberName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PartNumberDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SNP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPartNumber_ProductionLocation_ProductionLocationId",
                        column: x => x.ProductionLocationId,
                        principalSchema: "upm",
                        principalTable: "ProductionLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPartNumber_ProductionModel_ProductionModelId",
                        column: x => x.ProductionModelId,
                        principalSchema: "upm",
                        principalTable: "ProductionModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionArea_AreaDescription",
                schema: "upm",
                table: "ProductionArea",
                column: "AreaDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLocation_LocationDescription",
                schema: "upm",
                table: "ProductionLocation",
                column: "LocationDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionModel_ModelDescription",
                schema: "upm",
                table: "ProductionModel",
                column: "ModelDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_PartNumberName",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "PartNumberName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionLocationId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartNumber_ProductionModelId",
                schema: "upm",
                table: "ProductionPartNumber",
                column: "ProductionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_StatusDescription",
                schema: "upm",
                table: "Status",
                column: "StatusDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkShift_ShiftDescription",
                schema: "upm",
                table: "WorkShift",
                column: "ShiftDescription",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionArea",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "ProductionPartNumber",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "WorkShift",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "ProductionLocation",
                schema: "upm");

            migrationBuilder.DropTable(
                name: "ProductionModel",
                schema: "upm");
        }
    }
}
