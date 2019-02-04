using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class VersionsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Versions",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "AmmoniaRetentions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId_VersionId",
                table: "AmmoniaRetentions",
                columns: new[] { "DryMatter", "SeasonApplicationId", "VersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                columns: new[] { "SeasonApplicationId", "DryMatter", "VersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_AmmoniaRetentions_VersionId",
                table: "AmmoniaRetentions",
                column: "VersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmmoniaRetentions_Versions_VersionId",
                table: "AmmoniaRetentions",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmmoniaRetentions_Versions_VersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId_VersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.DropIndex(
                name: "IX_AmmoniaRetentions_VersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId",
                table: "AmmoniaRetentions",
                columns: new[] { "DryMatter", "SeasonApplicationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                columns: new[] { "SeasonApplicationId", "DryMatter" });
        }
    }
}
