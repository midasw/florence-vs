using Microsoft.EntityFrameworkCore.Migrations;

namespace BackOffice_ASP.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditorId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_EditorId",
                table: "Announcements",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Users_EditorId",
                table: "Announcements",
                column: "EditorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Users_EditorId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_EditorId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Announcements");
        }
    }
}
