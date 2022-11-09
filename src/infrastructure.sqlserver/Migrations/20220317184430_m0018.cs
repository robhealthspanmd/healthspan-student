using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "ActiveMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "ActiveMetrics");
        }
    }
}
