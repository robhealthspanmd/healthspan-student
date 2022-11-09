using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueAsBoolean",
                table: "MetricTrackingEntries");

            migrationBuilder.DropColumn(
                name: "ValueAsDate",
                table: "MetricTrackingEntries");

            migrationBuilder.DropColumn(
                name: "ValueAsNumber",
                table: "MetricTrackingEntries");

            migrationBuilder.DropColumn(
                name: "ValueAsString",
                table: "MetricTrackingEntries");

            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleValues",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MetricTrackingEntriesValues",
                columns: table => new
                {
                    MetricTrackingEntryValueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricTrackingEntryId = table.Column<long>(type: "bigint", nullable: false),
                    ValueAsNumber = table.Column<double>(type: "float", nullable: true),
                    ValueAsBoolean = table.Column<bool>(type: "bit", nullable: true),
                    ValueAsString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueAsDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricTrackingEntriesValues", x => x.MetricTrackingEntryValueId);
                    table.ForeignKey(
                        name: "FK_MetricTrackingEntriesValues_MetricTrackingEntries_MetricTrackingEntryId",
                        column: x => x.MetricTrackingEntryId,
                        principalTable: "MetricTrackingEntries",
                        principalColumn: "MetricTrackingEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetricTrackingEntriesValues_MetricTrackingEntryId",
                table: "MetricTrackingEntriesValues",
                column: "MetricTrackingEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricTrackingEntriesValues");

            migrationBuilder.DropColumn(
                name: "AllowMultipleValues",
                table: "Metrics");

            migrationBuilder.AddColumn<bool>(
                name: "ValueAsBoolean",
                table: "MetricTrackingEntries",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValueAsDate",
                table: "MetricTrackingEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ValueAsNumber",
                table: "MetricTrackingEntries",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueAsString",
                table: "MetricTrackingEntries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
