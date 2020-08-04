using Microsoft.EntityFrameworkCore.Migrations;

namespace DevSkill.TenantPro.Web.Migrations.Billing
{
    public partial class RemoveMonthPropertyFromReadin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Readings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Readings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
