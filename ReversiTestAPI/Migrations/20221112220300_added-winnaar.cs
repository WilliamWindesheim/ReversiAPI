using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiAPI.Migrations
{
    public partial class addedwinnaar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Winnaar",
                table: "Spellen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Winnaar",
                table: "Spellen");
        }
    }
}
