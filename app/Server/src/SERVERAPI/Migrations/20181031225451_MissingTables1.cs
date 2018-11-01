using Microsoft.EntityFrameworkCore.Migrations;

namespace SERVERAPI.Migrations
{
    public partial class MissingTables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crops_CropType_CropTypeId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSTKRegionCd_Crops_CropId",
                table: "CropSTKRegionCd");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSTPRegionCd_Crops_CropId",
                table: "CropSTPRegionCd");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYield_Crops_CropId",
                table: "CropYield");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYield_Location_LocationId",
                table: "CropYield");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevCropType_CropType_CropTypeId",
                table: "PrevCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Location_LocationId",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Region",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropYield",
                table: "CropYield");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropType",
                table: "CropType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSTPRegionCd",
                table: "CropSTPRegionCd");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CropSTPRegionCd_CropId_SoilTestPhosphorousRegionCd",
                table: "CropSTPRegionCd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSTKRegionCd",
                table: "CropSTKRegionCd");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CropSTKRegionCd_CropId_SoilTestPotassiumRegionCd",
                table: "CropSTKRegionCd");

            migrationBuilder.RenameTable(
                name: "Region",
                newName: "Regions");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameTable(
                name: "CropYield",
                newName: "CropYields");

            migrationBuilder.RenameTable(
                name: "CropType",
                newName: "CropTypes");

            migrationBuilder.RenameTable(
                name: "CropSTPRegionCd",
                newName: "CropSTPRegionCds");

            migrationBuilder.RenameTable(
                name: "CropSTKRegionCd",
                newName: "CropSTKRegionCds");

            migrationBuilder.RenameIndex(
                name: "IX_Region_LocationId",
                table: "Regions",
                newName: "IX_Regions_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CropYield_LocationId",
                table: "CropYields",
                newName: "IX_CropYields_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields",
                columns: new[] { "CropId", "LocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSTPRegionCds",
                table: "CropSTPRegionCds",
                columns: new[] { "CropId", "PhosphorousCropGroupRegionCd" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CropSTPRegionCds_CropId_SoilTestPhosphorousRegionCd",
                table: "CropSTPRegionCds",
                columns: new[] { "CropId", "SoilTestPhosphorousRegionCd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSTKRegionCds",
                table: "CropSTKRegionCds",
                columns: new[] { "CropId", "PotassiumCropGroupRegionCd" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CropSTKRegionCds_CropId_SoilTestPotassiumRegionCd",
                table: "CropSTKRegionCds",
                columns: new[] { "CropId", "SoilTestPotassiumRegionCd" });

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops",
                column: "CropTypeId",
                principalTable: "CropTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSTKRegionCds_Crops_CropId",
                table: "CropSTKRegionCds",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSTPRegionCds_Crops_CropId",
                table: "CropSTPRegionCds",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYields_Crops_CropId",
                table: "CropYields",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYields_Locations_LocationId",
                table: "CropYields",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevCropType_CropTypes_CropTypeId",
                table: "PrevCropType",
                column: "CropTypeId",
                principalTable: "CropTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Locations_LocationId",
                table: "Regions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSTKRegionCds_Crops_CropId",
                table: "CropSTKRegionCds");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSTPRegionCds_Crops_CropId",
                table: "CropSTPRegionCds");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYields_Crops_CropId",
                table: "CropYields");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYields_Locations_LocationId",
                table: "CropYields");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevCropType_CropTypes_CropTypeId",
                table: "PrevCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Locations_LocationId",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSTPRegionCds",
                table: "CropSTPRegionCds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CropSTPRegionCds_CropId_SoilTestPhosphorousRegionCd",
                table: "CropSTPRegionCds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSTKRegionCds",
                table: "CropSTKRegionCds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CropSTKRegionCds_CropId_SoilTestPotassiumRegionCd",
                table: "CropSTKRegionCds");

            migrationBuilder.RenameTable(
                name: "Regions",
                newName: "Region");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameTable(
                name: "CropYields",
                newName: "CropYield");

            migrationBuilder.RenameTable(
                name: "CropTypes",
                newName: "CropType");

            migrationBuilder.RenameTable(
                name: "CropSTPRegionCds",
                newName: "CropSTPRegionCd");

            migrationBuilder.RenameTable(
                name: "CropSTKRegionCds",
                newName: "CropSTKRegionCd");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_LocationId",
                table: "Region",
                newName: "IX_Region_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CropYields_LocationId",
                table: "CropYield",
                newName: "IX_CropYield_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Region",
                table: "Region",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropYield",
                table: "CropYield",
                columns: new[] { "CropId", "LocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropType",
                table: "CropType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSTPRegionCd",
                table: "CropSTPRegionCd",
                columns: new[] { "CropId", "PhosphorousCropGroupRegionCd" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CropSTPRegionCd_CropId_SoilTestPhosphorousRegionCd",
                table: "CropSTPRegionCd",
                columns: new[] { "CropId", "SoilTestPhosphorousRegionCd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSTKRegionCd",
                table: "CropSTKRegionCd",
                columns: new[] { "CropId", "PotassiumCropGroupRegionCd" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CropSTKRegionCd_CropId_SoilTestPotassiumRegionCd",
                table: "CropSTKRegionCd",
                columns: new[] { "CropId", "SoilTestPotassiumRegionCd" });

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_CropType_CropTypeId",
                table: "Crops",
                column: "CropTypeId",
                principalTable: "CropType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSTKRegionCd_Crops_CropId",
                table: "CropSTKRegionCd",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSTPRegionCd_Crops_CropId",
                table: "CropSTPRegionCd",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYield_Crops_CropId",
                table: "CropYield",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYield_Location_LocationId",
                table: "CropYield",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevCropType_CropType_CropTypeId",
                table: "PrevCropType",
                column: "CropTypeId",
                principalTable: "CropType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Location_LocationId",
                table: "Region",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
