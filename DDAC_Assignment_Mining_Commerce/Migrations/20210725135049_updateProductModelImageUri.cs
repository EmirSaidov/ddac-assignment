using Microsoft.EntityFrameworkCore.Migrations;

namespace DDAC_Assignment_Mining_Commerce.Migrations
{
    public partial class updateProductModelImageUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUri",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageUri",
                table: "Product");
        }
    }
}
