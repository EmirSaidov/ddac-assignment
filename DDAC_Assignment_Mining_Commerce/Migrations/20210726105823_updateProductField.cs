using Microsoft.EntityFrameworkCore.Migrations;

namespace DDAC_Assignment_Mining_Commerce.Migrations
{
    public partial class updateProductField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productQuantity",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "productDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "productMass",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productDescription",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "productMass",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "productQuantity",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
