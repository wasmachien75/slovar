using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace slovar.Migrations
{
    public partial class fixdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update DictionaryEntries set Definition = substr(Definition, 4) where Definition like '). %'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }

    public class DoSomethingOperation : MigrationOperation
    {

    }
}
