using Microsoft.EntityFrameworkCore.Migrations;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class AddWordIndexByText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Words_Text",
                table: "Words",
                column: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Words_Text",
                table: "Words");
        }
    }
}
