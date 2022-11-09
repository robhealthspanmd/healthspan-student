using Microsoft.EntityFrameworkCore.Migrations;

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedRole_UserRoles_UserRoleId",
                table: "AuthorizedRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedRole_Users_UserId",
                table: "AuthorizedRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizedRole",
                table: "AuthorizedRole");

            migrationBuilder.RenameTable(
                name: "AuthorizedRole",
                newName: "AuthorizedRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizedRole_UserRoleId",
                table: "AuthorizedRoles",
                newName: "IX_AuthorizedRoles_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizedRole_UserId",
                table: "AuthorizedRoles",
                newName: "IX_AuthorizedRoles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizedRoles",
                table: "AuthorizedRoles",
                column: "AuthorizedRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedRoles_UserRoles_UserRoleId",
                table: "AuthorizedRoles",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedRoles_Users_UserId",
                table: "AuthorizedRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedRoles_UserRoles_UserRoleId",
                table: "AuthorizedRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedRoles_Users_UserId",
                table: "AuthorizedRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizedRoles",
                table: "AuthorizedRoles");

            migrationBuilder.RenameTable(
                name: "AuthorizedRoles",
                newName: "AuthorizedRole");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizedRoles_UserRoleId",
                table: "AuthorizedRole",
                newName: "IX_AuthorizedRole_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizedRoles_UserId",
                table: "AuthorizedRole",
                newName: "IX_AuthorizedRole_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizedRole",
                table: "AuthorizedRole",
                column: "AuthorizedRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedRole_UserRoles_UserRoleId",
                table: "AuthorizedRole",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedRole_Users_UserId",
                table: "AuthorizedRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
