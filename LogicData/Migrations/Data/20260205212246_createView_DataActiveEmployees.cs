using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicData.Migrations.Data
{
    /// <inheritdoc />
    public partial class createView_DataActiveEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW upm_data.vw_active_employees AS
                SELECT 
                    CB_CODIGO, 
                    CB_NOMBRES, 
                    CB_APE_MAT, 
                    CB_APE_PAT, 
                    PRETTYNAME
                FROM [UPMRHUMANOS\UPMTRESS].[Unipres].[dbo].[COLABORA]
                WHERE CB_ACTIVO = 'S';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS upm_data.vw_active_employees");
        }
    }
}
