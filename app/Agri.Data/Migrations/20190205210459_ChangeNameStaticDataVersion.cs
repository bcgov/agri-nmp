using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class ChangeNameStaticDataVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "AmmoniaRetentions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaticDataVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticDataVersions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmmoniaRetentions_StaticDataVersionId",
                table: "AmmoniaRetentions",
                column: "StaticDataVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmmoniaRetentions_StaticDataVersions_StaticDataVersionId",
                table: "AmmoniaRetentions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmmoniaRetentions_StaticDataVersions_StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropTable(
                name: "StaticDataVersions");

            migrationBuilder.DropIndex(
                name: "IX_AmmoniaRetentions_StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    StaticDataVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });
        }
    }
}
