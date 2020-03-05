using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class FeedConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedConsumptions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedConsumptions", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FeedConsumptions_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedConsumptions_StaticDataVersionId",
                table: "FeedConsumptions",
                column: "StaticDataVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedConsumptions");
        }
    }
}
