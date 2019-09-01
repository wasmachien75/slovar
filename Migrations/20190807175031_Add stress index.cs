using Microsoft.EntityFrameworkCore.Migrations;

namespace Slovar.Migrations
{
    public partial class Addstressindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StressIndex",
                table: "DictionaryEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StressIndex",
                table: "DictionaryEntries");
        }
    }
}
