using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Metrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlphaSelectNumeric",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPositivePolarity",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Threshold",
                table: "Metrics",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "IsAlphaSelectNumeric",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "IsPositivePolarity",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Threshold",
                table: "Metrics");
        }
    }
}
