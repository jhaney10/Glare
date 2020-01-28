using Microsoft.EntityFrameworkCore.Migrations;

namespace Glare.Migrations
{
    public partial class CartTableUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingStatus",
                table: "CartItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingStatus",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
