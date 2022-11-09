using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0030 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                table: "ContentCardsAssignments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationMessage",
                table: "ContentCardsAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationMessage",
                table: "ContentCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "ContentCardsAssignments");

            migrationBuilder.DropColumn(
                name: "NotificationMessage",
                table: "ContentCardsAssignments");

            migrationBuilder.DropColumn(
                name: "NotificationMessage",
                table: "ContentCards");
        }
    }
}
