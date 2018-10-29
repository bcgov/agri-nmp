using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class PreviousManureTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrevManureApplicationYears",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevManureApplicationYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrevYearManureApplDefaultNitrogens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PrevYearManureAppFrequency = table.Column<string>(nullable: true),
                    DefaultNitrogenCredit = table.Column<int[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevYearManureApplDefaultNitrogens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrevManureApplicationYears");

            migrationBuilder.DropTable(
                name: "PrevYearManureApplDefaultNitrogens");
        }
    }
}
