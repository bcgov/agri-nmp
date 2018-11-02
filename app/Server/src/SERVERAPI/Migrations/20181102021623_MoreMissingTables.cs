using Microsoft.EntityFrameworkCore.Migrations;

namespace SERVERAPI.Migrations
{
    public partial class MoreMissingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnit_DensityUnitId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizer_FertilizerId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_DM_DMId",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_NMineralization_NMineralizationId",
                table: "Manures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NMineralization",
                table: "NMineralization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fertilizer",
                table: "Fertilizer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DM",
                table: "DM");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DensityUnit",
                table: "DensityUnit");

            migrationBuilder.RenameTable(
                name: "NMineralization",
                newName: "NMineralizations");

            migrationBuilder.RenameTable(
                name: "Fertilizer",
                newName: "Fertilizers");

            migrationBuilder.RenameTable(
                name: "DM",
                newName: "DMs");

            migrationBuilder.RenameTable(
                name: "DensityUnit",
                newName: "DensityUnits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NMineralizations",
                table: "NMineralizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DMs",
                table: "DMs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId",
                table: "LiquidFertilizerDensities",
                column: "DensityUnitId",
                principalTable: "DensityUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId",
                table: "LiquidFertilizerDensities",
                column: "FertilizerId",
                principalTable: "Fertilizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_DMs_DMId",
                table: "Manures",
                column: "DMId",
                principalTable: "DMs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_NMineralizations_NMineralizationId",
                table: "Manures",
                column: "NMineralizationId",
                principalTable: "NMineralizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_DMs_DMId",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_NMineralizations_NMineralizationId",
                table: "Manures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NMineralizations",
                table: "NMineralizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DMs",
                table: "DMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits");

            migrationBuilder.RenameTable(
                name: "NMineralizations",
                newName: "NMineralization");

            migrationBuilder.RenameTable(
                name: "Fertilizers",
                newName: "Fertilizer");

            migrationBuilder.RenameTable(
                name: "DMs",
                newName: "DM");

            migrationBuilder.RenameTable(
                name: "DensityUnits",
                newName: "DensityUnit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NMineralization",
                table: "NMineralization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fertilizer",
                table: "Fertilizer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DM",
                table: "DM",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DensityUnit",
                table: "DensityUnit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnit_DensityUnitId",
                table: "LiquidFertilizerDensities",
                column: "DensityUnitId",
                principalTable: "DensityUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizer_FertilizerId",
                table: "LiquidFertilizerDensities",
                column: "FertilizerId",
                principalTable: "Fertilizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_DM_DMId",
                table: "Manures",
                column: "DMId",
                principalTable: "DM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_NMineralization_NMineralizationId",
                table: "Manures",
                column: "NMineralizationId",
                principalTable: "NMineralization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
