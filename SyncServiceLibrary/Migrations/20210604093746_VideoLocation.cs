using Microsoft.EntityFrameworkCore.Migrations;

namespace SyncServiceLibrary.Migrations
{
    public partial class VideoLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoLocation",
                table: "VideoFiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUri",
                table: "VideoFiles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoLocation",
                table: "VideoFiles");

            migrationBuilder.DropColumn(
                name: "VideoUri",
                table: "VideoFiles");
        }
    }
}
