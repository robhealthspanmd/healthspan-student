using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentItems_ContentDocuments_ContentFileId",
                table: "ContentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentDocuments",
                table: "ContentDocuments");

            migrationBuilder.RenameTable(
                name: "ContentDocuments",
                newName: "ContentFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentFiles",
                table: "ContentFiles",
                column: "ContentFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentItems_ContentFiles_ContentFileId",
                table: "ContentItems",
                column: "ContentFileId",
                principalTable: "ContentFiles",
                principalColumn: "ContentFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentItems_ContentFiles_ContentFileId",
                table: "ContentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentFiles",
                table: "ContentFiles");

            migrationBuilder.RenameTable(
                name: "ContentFiles",
                newName: "ContentDocuments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentDocuments",
                table: "ContentDocuments",
                column: "ContentFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentItems_ContentDocuments_ContentFileId",
                table: "ContentItems",
                column: "ContentFileId",
                principalTable: "ContentDocuments",
                principalColumn: "ContentFileId");
        }
    }
}
