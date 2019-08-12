using Microsoft.EntityFrameworkCore.Migrations;

namespace slovar.Migrations
{
    public partial class addTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Translation",
                table: "DictionaryEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Translation",
                table: "DictionaryEntries");
        }
    }
}
