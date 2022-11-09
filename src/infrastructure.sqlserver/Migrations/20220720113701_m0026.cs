using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0026 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContentCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageFileId",
                table: "ContentCards",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContentCards");

            migrationBuilder.DropColumn(
                name: "ImageFileId",
                table: "ContentCards");
        }
    }
}
