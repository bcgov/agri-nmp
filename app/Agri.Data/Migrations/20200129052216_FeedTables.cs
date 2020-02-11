using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class FeedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "DailyFeedRequirements",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FeedForageTypes",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedForageTypes", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FeedForageTypes_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    FeedForageTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CPPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PhosphorousPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PotassiumPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    GetFeedForageTypeId = table.Column<int>(nullable: true),
                    GetFeedForageTypeStaticDataVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Feeds_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feeds_FeedForageTypes_GetFeedForageTypeId_GetFeedForageType~",
                        columns: x => new { x.GetFeedForageTypeId, x.GetFeedForageTypeStaticDataVersionId },
                        principalTable: "FeedForageTypes",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedForageTypes_StaticDataVersionId",
                table: "FeedForageTypes",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_StaticDataVersionId",
                table: "Feeds",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_GetFeedForageTypeId_GetFeedForageTypeStaticDataVersio~",
                table: "Feeds",
                columns: new[] { "GetFeedForageTypeId", "GetFeedForageTypeStaticDataVersionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feeds");

            migrationBuilder.DropTable(
                name: "FeedForageTypes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "DailyFeedRequirements",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
