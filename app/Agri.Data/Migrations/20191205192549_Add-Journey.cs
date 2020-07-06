using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class AddJourney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NextAction",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextController",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousAction",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousController",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseJavaScriptInterceptMethod",
                table: "SubMenu",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JourneyId",
                table: "MainMenus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NextAction",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextController",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousAction",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousController",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseJavaScriptInterceptMethod",
                table: "MainMenus",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.Sql(@"
                INSERT INTO public.""Journey"" (""Name"") VALUES
                ('Initial')
                , ('Dairy')
                , ('Ranch')
                , ('Poultry')
                , ('Crops')
                , ('Mixed');

                UPDATE public.""MainMenus"" SET ""JourneyId"" = 2;
                ; ");

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
                name: "NextAction",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "NextController",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "PreviousAction",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "PreviousController",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "UseJavaScriptInterceptMethod",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "NextAction",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "NextController",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "PreviousAction",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "PreviousController",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "UseJavaScriptInterceptMethod",
                table: "MainMenus");
        }
    }
}