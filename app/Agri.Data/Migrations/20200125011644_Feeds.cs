using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class Feeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CPPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PhosphorousPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PotassiumPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "FeedForageType",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FeedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedForageType", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FeedForageType_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedForageType_Feeds_FeedId_StaticDataVersionId",
                        columns: x => new { x.FeedId, x.StaticDataVersionId },
                        principalTable: "Feeds",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedForageType_StaticDataVersionId",
                table: "FeedForageType",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedForageType_FeedId_StaticDataVersionId",
                table: "FeedForageType",
                columns: new[] { "FeedId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_StaticDataVersionId",
                table: "Feeds",
                column: "StaticDataVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedForageType");

            migrationBuilder.DropTable(
                name: "Feeds");
        }
    }
}
