using Microsoft.EntityFrameworkCore.Migrations;

namespace SERVERAPI.Migrations
{
    public partial class PrevCropTypeCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrevCropType_PrevCropType_CropTypeId1",
                table: "PrevCropType");

            migrationBuilder.DropIndex(
                name: "IX_PrevCropType_CropTypeId1",
                table: "PrevCropType");

            migrationBuilder.DropColumn(
                name: "CropTypeId1",
                table: "PrevCropType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CropTypeId1",
                table: "PrevCropType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropTypeId1",
                table: "PrevCropType",
                column: "CropTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PrevCropType_PrevCropType_CropTypeId1",
                table: "PrevCropType",
                column: "CropTypeId1",
                principalTable: "PrevCropType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
