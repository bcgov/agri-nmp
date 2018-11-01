using Microsoft.EntityFrameworkCore.Migrations;

namespace SERVERAPI.Migrations
{
    public partial class PrevCropTypeCorrection2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrevCropCd",
                table: "PrevCropType",
                newName: "PrevCropCode");

            migrationBuilder.RenameColumn(
                name: "PrevCropCd",
                table: "Crops",
                newName: "PrevCropCode");

            migrationBuilder.AddColumn<int>(
                name: "CropId",
                table: "PrevCropType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropId",
                table: "PrevCropType",
                column: "CropId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrevCropType_Crops_CropId",
                table: "PrevCropType",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrevCropType_Crops_CropId",
                table: "PrevCropType");

            migrationBuilder.DropIndex(
                name: "IX_PrevCropType_CropId",
                table: "PrevCropType");

            migrationBuilder.DropColumn(
                name: "CropId",
                table: "PrevCropType");

            migrationBuilder.RenameColumn(
                name: "PrevCropCode",
                table: "PrevCropType",
                newName: "PrevCropCd");

            migrationBuilder.RenameColumn(
                name: "PrevCropCode",
                table: "Crops",
                newName: "PrevCropCd");
        }
    }
}
