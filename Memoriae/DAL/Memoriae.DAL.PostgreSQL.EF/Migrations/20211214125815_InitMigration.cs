using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Memoriae.DAL.PostgreSQL.EF.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "memoriae");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "Post",
                schema: "memoriae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Text = table.Column<string>(type: "character varying", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "memoriae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostTagLink",
                schema: "memoriae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTagLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostTagLink_Post_PostId",
                        column: x => x.PostId,
                        principalSchema: "memoriae",
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTagLink_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "memoriae",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTagLink_PostId",
                schema: "memoriae",
                table: "PostTagLink",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTagLink_TagId",
                schema: "memoriae",
                table: "PostTagLink",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTagLink",
                schema: "memoriae");

            migrationBuilder.DropTable(
                name: "Post",
                schema: "memoriae");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "memoriae");
        }
    }
}
