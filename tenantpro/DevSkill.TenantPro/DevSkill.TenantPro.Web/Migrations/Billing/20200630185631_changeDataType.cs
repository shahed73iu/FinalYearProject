using Microsoft.EntityFrameworkCore.Migrations;

namespace DevSkill.TenantPro.Web.Migrations.Billing
{
    public partial class changeDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PreviousReading",
                table: "Readings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PresentReading",
                table: "Readings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PreviousReading",
                table: "Readings",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "PresentReading",
                table: "Readings",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
