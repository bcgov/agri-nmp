using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class DailyFeedRequirement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyFeedRequirements",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyFeedRequirements", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_DailyFeedRequirements_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyFeedRequirements_StaticDataVersionId",
                table: "DailyFeedRequirements",
                column: "StaticDataVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyFeedRequirements");
        }
    }
}
