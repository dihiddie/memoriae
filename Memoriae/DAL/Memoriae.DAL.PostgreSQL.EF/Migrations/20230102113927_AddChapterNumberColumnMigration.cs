using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoriae.DAL.PostgreSQL.EF.Migrations
{
    public partial class AddChapterNumberColumnMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterNumber",
                schema: "memoriae",
                table: "Post",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterNumber",
                schema: "memoriae",
                table: "Post");
        }
    }
}
