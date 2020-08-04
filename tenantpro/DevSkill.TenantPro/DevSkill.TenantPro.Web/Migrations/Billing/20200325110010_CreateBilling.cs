using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevSkill.TenantPro.Web.Migrations.Billing
{
    public partial class CreateBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    DescoBillOfThisMonth = table.Column<decimal>(nullable: false),
                    TotalUnitLocal = table.Column<int>(nullable: false),
                    UnitPriceForNextMonth = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    PreviousReading = table.Column<int>(nullable: false),
                    PresentReading = table.Column<int>(nullable: false),
                    ReadingTakenDate = table.Column<DateTime>(nullable: false),
                    DayOffset = table.Column<int>(nullable: false),
                    NetUnit = table.Column<int>(nullable: false),
                    PerUnitPrice = table.Column<decimal>(nullable: false),
                    TotalBillofThisMonth = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readings_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
                migrationBuilder.CreateIndex(
                name: "IX_Readings_TenantId",
                table: "Readings",
                column: "TenantId");
                
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Readings");
        }
    }
}
