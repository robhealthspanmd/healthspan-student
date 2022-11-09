using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MetricsSelectItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Metrics");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MetricsSelectItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
