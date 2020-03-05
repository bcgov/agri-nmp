using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class FixPreviousCropTypeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropId_CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropColumn(
                name: "CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.AlterColumn<int>(
                name: "CropId",
                table: "PreviousCropType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Crops_Id_PreviousCropCode_StaticDataVersionId",
                table: "Crops",
                columns: new[] { "Id", "PreviousCropCode", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId_PreviousCropCode_StaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "PreviousCropCode", "StaticDataVersionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_PreviousCropCode_StaticDataVe~",
                table: "PreviousCropType",
                columns: new[] { "CropId", "PreviousCropCode", "StaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "PreviousCropCode", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_PreviousCropCode_StaticDataVe~",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropId_PreviousCropCode_StaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Crops_Id_PreviousCropCode_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.AlterColumn<int>(
                name: "CropId",
                table: "PreviousCropType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CropStaticDataVersionId",
                table: "PreviousCropType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId_CropStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "CropStaticDataVersionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_CropStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "CropStaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
