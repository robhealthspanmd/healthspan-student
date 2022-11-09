using Microsoft.EntityFrameworkCore.Migrations;

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRoles");

            migrationBuilder.CreateTable(
                name: "AuthorizedRole",
                columns: table => new
                {
                    AuthorizedRoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedRole", x => x.AuthorizedRoleId);
                    table.ForeignKey(
                        name: "FK_AuthorizedRole_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "UserRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorizedRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedRole_UserId",
                table: "AuthorizedRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedRole_UserRoleId",
                table: "AuthorizedRole",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizedRole");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
