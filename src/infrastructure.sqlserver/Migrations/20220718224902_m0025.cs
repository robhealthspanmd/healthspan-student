using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentCardsAssignments_Users_UserId1",
                table: "ContentCardsAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ContentCardsAssignments_UserId1",
                table: "ContentCardsAssignments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ContentCardsAssignments");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "ContentCardsAssignments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "AssignedBy",
                table: "ContentCardsAssignments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsAssignments_UserId",
                table: "ContentCardsAssignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCardsAssignments_Users_UserId",
                table: "ContentCardsAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentCardsAssignments_Users_UserId",
                table: "ContentCardsAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ContentCardsAssignments_UserId",
                table: "ContentCardsAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ContentCardsAssignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedBy",
                table: "ContentCardsAssignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "ContentCardsAssignments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsAssignments_UserId1",
                table: "ContentCardsAssignments",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCardsAssignments_Users_UserId1",
                table: "ContentCardsAssignments",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
