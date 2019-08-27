using Microsoft.EntityFrameworkCore.Migrations;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class AddWordAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attributes",
                table: "Words",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "Words");
        }
    }
}
