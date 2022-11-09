using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0028 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Threshold2",
                table: "Metrics",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Threshold2",
                table: "Metrics");
        }
    }
}
