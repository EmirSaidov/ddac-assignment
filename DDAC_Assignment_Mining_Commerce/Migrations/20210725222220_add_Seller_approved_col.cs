using Microsoft.EntityFrameworkCore.Migrations;

namespace DDAC_Assignment_Mining_Commerce.Migrations
{
    public partial class add_Seller_approved_col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_approved",
                table: "Seller",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_approved",
                table: "Seller");
        }
    }
}
