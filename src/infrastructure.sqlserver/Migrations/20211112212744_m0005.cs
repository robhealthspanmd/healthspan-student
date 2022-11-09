using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    public partial class m0005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_MetricTypes_MetricTypeId",
                table: "Metrics");

            migrationBuilder.DropTable(
                name: "MetricTypes");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_MetricTypeId",
                table: "Metrics");

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "Metrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "Metrics",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinValue",
                table: "Metrics",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MetricsSelectItems",
                columns: table => new
                {
                    MetricSelectItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricId = table.Column<int>(type: "int", nullable: false),
                    ItemValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDisplayText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricsSelectItems", x => x.MetricSelectItemId);
                    table.ForeignKey(
                        name: "FK_MetricsSelectItems_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "MetricId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetricsSelectItems_MetricId",
                table: "MetricsSelectItems",
                column: "MetricId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricsSelectItems");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "Metrics");

            migrationBuilder.CreateTable(
                name: "MetricTypes",
                columns: table => new
                {
                    MetricTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricTypes", x => x.MetricTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_MetricTypeId",
                table: "Metrics",
                column: "MetricTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_MetricTypes_MetricTypeId",
                table: "Metrics",
                column: "MetricTypeId",
                principalTable: "MetricTypes",
                principalColumn: "MetricTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
