using Microsoft.EntityFrameworkCore.Migrations;

namespace Glare.Migrations
{
    public partial class AddedSubTotalToCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "CartItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "CartItems");
        }
    }
}
