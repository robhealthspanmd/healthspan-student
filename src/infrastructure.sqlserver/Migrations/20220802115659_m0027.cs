using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0027 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Value2AsNumber",
                table: "MetricTrackingEntriesValues",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value2AsNumber",
                table: "MetricTrackingEntriesValues");
        }
    }
}
