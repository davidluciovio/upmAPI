using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Temporal
{
    /// <inheritdoc />
    public partial class inicialitze_context : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "upm_temporal");

            migrationBuilder.CreateTable(
                name: "ProductionAchievement",
                schema: "upm_temporal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Supervisor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Leader = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartNumberName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PartNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkingTime = table.Column<float>(type: "real", nullable: false),
                    ProductionObjetive = table.Column<float>(type: "real", nullable: false),
                    ProductionReal = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionAchievement", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionAchievement",
                schema: "upm_temporal");
        }
    }
}
