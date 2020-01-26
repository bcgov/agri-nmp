using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class FeedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Name = table.Column<string>(nullable: true),
                    CPPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PhosphorousPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    PotassiumPercent = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    FeedForageTypeId1 = table.Column<int>(nullable: true),
                    FeedForageTypeStaticDataVersionId = table.Column<int>(nullable: true),
                    FeedForageTypeId = table.Column<int>(nullable: false)
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
                        name: "FK_Feeds_FeedForageTypes_FeedForageTypeId1_FeedForageTypeStati~",
                        columns: x => new { x.FeedForageTypeId1, x.FeedForageTypeStaticDataVersionId },
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
                name: "IX_Feeds_FeedForageTypeId1_FeedForageTypeStaticDataVersionId",
                table: "Feeds",
                columns: new[] { "FeedForageTypeId1", "FeedForageTypeStaticDataVersionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feeds");

            migrationBuilder.DropTable(
                name: "FeedForageTypes");
        }
    }
}
