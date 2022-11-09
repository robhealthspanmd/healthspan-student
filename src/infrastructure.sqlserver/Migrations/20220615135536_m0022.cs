using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentCards",
                columns: table => new
                {
                    ContentCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCards", x => x.ContentCardId);
                });

            migrationBuilder.CreateTable(
                name: "ContentDocuments",
                columns: table => new
                {
                    ContentFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDocuments", x => x.ContentFileId);
                });

            migrationBuilder.CreateTable(
                name: "ContentCardsAssignments",
                columns: table => new
                {
                    ContentCardAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentCardId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCardsAssignments", x => x.ContentCardAssignmentId);
                    table.ForeignKey(
                        name: "FK_ContentCardsAssignments_ContentCards_ContentCardId",
                        column: x => x.ContentCardId,
                        principalTable: "ContentCards",
                        principalColumn: "ContentCardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentCardsAssignments_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ContentItems",
                columns: table => new
                {
                    ContentItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentFileId = table.Column<int>(type: "int", nullable: true),
                    IsCaption = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentItems", x => x.ContentItemId);
                    table.ForeignKey(
                        name: "FK_ContentItems_ContentDocuments_ContentFileId",
                        column: x => x.ContentFileId,
                        principalTable: "ContentDocuments",
                        principalColumn: "ContentFileId");
                });

            migrationBuilder.CreateTable(
                name: "ContentCardsMembers",
                columns: table => new
                {
                    ContentCardMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentCardId = table.Column<int>(type: "int", nullable: false),
                    ContentItemId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCardsMembers", x => x.ContentCardMemberId);
                    table.ForeignKey(
                        name: "FK_ContentCardsMembers_ContentCards_ContentCardId",
                        column: x => x.ContentCardId,
                        principalTable: "ContentCards",
                        principalColumn: "ContentCardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentCardsMembers_ContentItems_ContentItemId",
                        column: x => x.ContentItemId,
                        principalTable: "ContentItems",
                        principalColumn: "ContentItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsAssignments_ContentCardId",
                table: "ContentCardsAssignments",
                column: "ContentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsAssignments_UserId1",
                table: "ContentCardsAssignments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsMembers_ContentCardId",
                table: "ContentCardsMembers",
                column: "ContentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCardsMembers_ContentItemId",
                table: "ContentCardsMembers",
                column: "ContentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentItems_ContentFileId",
                table: "ContentItems",
                column: "ContentFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentCardsAssignments");

            migrationBuilder.DropTable(
                name: "ContentCardsMembers");

            migrationBuilder.DropTable(
                name: "ContentCards");

            migrationBuilder.DropTable(
                name: "ContentItems");

            migrationBuilder.DropTable(
                name: "ContentDocuments");
        }
    }
}
