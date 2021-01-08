using Microsoft.EntityFrameworkCore.Migrations;

namespace BackOffice_ASP.Migrations
{
    public partial class Migration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Announcements",
                newName: "IsPublished");

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Announcements",
                newName: "Published");
        }
    }
}
