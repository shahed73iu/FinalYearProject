using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevSkill.TenantPro.Web.Migrations
{
    public partial class addedfinalbilldateproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinalBillDate",
                table: "Payment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalBillDate",
                table: "Payment");
        }
    }
}
