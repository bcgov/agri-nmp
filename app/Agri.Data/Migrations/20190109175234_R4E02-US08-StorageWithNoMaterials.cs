using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Agri.Data.Migrations
{
    public partial class R4E02US08StorageWithNoMaterials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("UserPrompts", 
                new string[] {"Id", "Name", "Text"}, 
                new string[] {"36", "NoMaterialsForStorage", "No materials of this type have been added.  Return to Manure generated or imported pages to add materials to store."});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("UserPrompts", "Id", "36");
        }
    }
}
