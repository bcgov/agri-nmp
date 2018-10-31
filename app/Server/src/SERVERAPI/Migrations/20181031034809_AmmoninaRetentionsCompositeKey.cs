using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class AmmoninaRetentionsCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonApplicationId",
                table: "AmmoniaRetentions",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AmmoniaRetentions_DM_SeasonApplicationId",
                table: "AmmoniaRetentions",
                columns: new[] { "DM", "SeasonApplicationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                columns: new[] { "SeasonApplicationId", "DM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AmmoniaRetentions_DM_SeasonApplicationId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonApplicationId",
                table: "AmmoniaRetentions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                column: "SeasonApplicationId");
        }
    }
}
