using Microsoft.EntityFrameworkCore.Migrations;

namespace slovar.Migrations
{
    public partial class addsearchstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LemmaForSearch",
                table: "DictionaryEntries",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE DictionaryEntries SET LemmaForSearch = replace(Lemma, 'ё', 'е')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LemmaForSearch",
                table: "DictionaryEntries");
        }
    }
}
