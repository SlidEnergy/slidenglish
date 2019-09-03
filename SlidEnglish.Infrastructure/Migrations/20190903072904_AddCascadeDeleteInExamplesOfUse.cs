using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SlidEnglish.Infrastructure.Migrations
{
    public partial class AddCascadeDeleteInExamplesOfUse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"ExampleOfUse\" WHERE \"LexicalUnitId\" IS NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_ExampleOfUse_LexicalUnits_LexicalUnitId",
                table: "ExampleOfUse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExampleOfUse",
                table: "ExampleOfUse");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExampleOfUse");

            migrationBuilder.AlterColumn<int>(
                name: "LexicalUnitId",
                table: "ExampleOfUse",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExampleOfUse",
                table: "ExampleOfUse",
                columns: new[] { "Example", "LexicalUnitId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExampleOfUse_LexicalUnits_LexicalUnitId",
                table: "ExampleOfUse",
                column: "LexicalUnitId",
                principalTable: "LexicalUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExampleOfUse_LexicalUnits_LexicalUnitId",
                table: "ExampleOfUse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExampleOfUse",
                table: "ExampleOfUse");

            migrationBuilder.AlterColumn<int>(
                name: "LexicalUnitId",
                table: "ExampleOfUse",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExampleOfUse",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExampleOfUse",
                table: "ExampleOfUse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExampleOfUse_LexicalUnits_LexicalUnitId",
                table: "ExampleOfUse",
                column: "LexicalUnitId",
                principalTable: "LexicalUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
