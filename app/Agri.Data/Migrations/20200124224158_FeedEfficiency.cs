using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class FeedEfficiency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedEfficiencies",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    AnimalType = table.Column<string>(nullable: true),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedEfficiencies", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FeedEfficiencies_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedEfficiencies_StaticDataVersionId",
                table: "FeedEfficiencies",
                column: "StaticDataVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedEfficiencies");
        }
    }
}
