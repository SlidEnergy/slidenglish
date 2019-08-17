using Microsoft.EntityFrameworkCore.Migrations;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class WordSynonymCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordSinonym_Words_SinonymId",
                table: "WordSinonym");

            migrationBuilder.AddForeignKey(
                name: "FK_WordSinonym_Words_SinonymId",
                table: "WordSinonym",
                column: "SinonymId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordSinonym_Words_SinonymId",
                table: "WordSinonym");

            migrationBuilder.AddForeignKey(
                name: "FK_WordSinonym_Words_SinonymId",
                table: "WordSinonym",
                column: "SinonymId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
