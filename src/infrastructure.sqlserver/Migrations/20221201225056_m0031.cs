using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0031 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    ContentTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.ContentTagId);
                });

            migrationBuilder.CreateTable(
                name: "ContentTagAssignments",
                columns: table => new
                {
                    ContentTagAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentTagId = table.Column<int>(type: "int", nullable: false),
                    ContentCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTagAssignments", x => x.ContentTagAssignmentId);
                    table.ForeignKey(
                        name: "FK_ContentTagAssignments_ContentCards_ContentCardId",
                        column: x => x.ContentCardId,
                        principalTable: "ContentCards",
                        principalColumn: "ContentCardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTagAssignments_ContentTags_ContentTagId",
                        column: x => x.ContentTagId,
                        principalTable: "ContentTags",
                        principalColumn: "ContentTagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTagAssignments_ContentCardId",
                table: "ContentTagAssignments",
                column: "ContentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTagAssignments_ContentTagId",
                table: "ContentTagAssignments",
                column: "ContentTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTagAssignments");

            migrationBuilder.DropTable(
                name: "ContentTags");
        }
    }
}
