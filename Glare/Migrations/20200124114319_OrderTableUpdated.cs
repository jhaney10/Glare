using Microsoft.EntityFrameworkCore.Migrations;

namespace Glare.Migrations
{
    public partial class OrderTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingStatus",
                table: "CartItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingStatus",
                table: "CartItems");
        }
    }
}
