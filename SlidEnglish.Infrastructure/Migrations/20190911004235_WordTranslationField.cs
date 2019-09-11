using Microsoft.EntityFrameworkCore.Migrations;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class WordTranslationField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Translation",
                table: "LexicalUnits",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Translation",
                table: "LexicalUnits");
        }
    }
}
