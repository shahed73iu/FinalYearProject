using Microsoft.EntityFrameworkCore.Migrations;

namespace DevSkill.TenantPro.Web.Migrations.Billing
{
    public partial class ChangeNetUnitDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NetUnit",
                table: "Readings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalUnitLocal",
                table: "Bills",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NetUnit",
                table: "Readings",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "TotalUnitLocal",
                table: "Bills",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
