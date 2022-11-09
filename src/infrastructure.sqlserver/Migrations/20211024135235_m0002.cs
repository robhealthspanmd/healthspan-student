using Microsoft.EntityFrameworkCore.Migrations;

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MetricTrackingEntries_MetricId",
                table: "MetricTrackingEntries",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMetrics_MetricId",
                table: "ActiveMetrics",
                column: "MetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveMetrics_Metrics_MetricId",
                table: "ActiveMetrics",
                column: "MetricId",
                principalTable: "Metrics",
                principalColumn: "MetricId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MetricTrackingEntries_Metrics_MetricId",
                table: "MetricTrackingEntries",
                column: "MetricId",
                principalTable: "Metrics",
                principalColumn: "MetricId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveMetrics_Metrics_MetricId",
                table: "ActiveMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_MetricTrackingEntries_Metrics_MetricId",
                table: "MetricTrackingEntries");

            migrationBuilder.DropIndex(
                name: "IX_MetricTrackingEntries_MetricId",
                table: "MetricTrackingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ActiveMetrics_MetricId",
                table: "ActiveMetrics");
        }
    }
}
