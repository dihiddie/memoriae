using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoriae.DAL.PostgreSQL.EF.Migrations
{
    public partial class PreviewTextAddedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewText",
                schema: "memoriae",
                table: "Post",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewText",
                schema: "memoriae",
                table: "Post");
        }
    }
}
