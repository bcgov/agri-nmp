using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class AddJourney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JourneyId",
                table: "MainMenus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Journey",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journey", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainMenus_JourneyId",
                table: "MainMenus",
                column: "JourneyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainMenus_Journey_JourneyId",
                table: "MainMenus",
                column: "JourneyId",
                principalTable: "Journey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainMenus_Journey_JourneyId",
                table: "MainMenus");

            migrationBuilder.DropTable(
                name: "Journey");

            migrationBuilder.DropIndex(
                name: "IX_MainMenus_JourneyId",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "MainMenus");
        }
    }
}
