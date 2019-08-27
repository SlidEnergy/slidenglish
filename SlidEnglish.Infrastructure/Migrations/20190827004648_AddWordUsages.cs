using Microsoft.EntityFrameworkCore.Migrations;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class AddWordUsages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Usages",
                table: "Words",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usages",
                table: "Words");
        }
    }
}
